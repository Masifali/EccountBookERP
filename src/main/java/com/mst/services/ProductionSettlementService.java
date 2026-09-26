package com.mst.services;

import com.mst.repositories.ProductionSettlementRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import javax.servlet.http.HttpSession;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.ProductionSettlementRepository.col;
import static com.mst.repositories.ProductionSettlementRepository.p;
import static com.mst.repositories.ProductionSettlementRepository.toInt;

/**
 * Screen 280, Settlement tab - frmProductionSettlement.cs.
 *
 * The page does the grid arithmetic (the desktop does it in the grid too); this service does every
 * database step, and re-checks every desktop refusal on the server, in the desktop's order and
 * wording, so a crafted request cannot skip one. Tenancy, financial year, branch and user always
 * come from {@link CurrentUserContext}, never from the request.
 *
 * Two kinds of refusal exist on the desktop and they steer the caller differently, so they are kept
 * apart here:
 *   - a MessageBox followed by `return false` (UpdateSettlement's WareHouse / Item / JobLot / approval
 *     checks) -> {@link Refusal}; btnUpdate still runs ResetSettlement afterwards on two of its paths.
 *   - `throw new Exception(...)` -> IllegalArgumentException; btnUpdate's catch shows it and nothing
 *     after it runs.
 */
@Service
public class ProductionSettlementService {

    /** GetERPFeatureById(5) -> FIFOCGSFlag (Load:274). */
    private static final int ERP_FEATURE_FIFO_CGS = 5;
    /** stockAccoutnFill:304 - CoaAllocationAccountTitleByAccountTypeIds("4,12"). */
    private static final String DIFFERENCE_ACCOUNT_TYPES = "4,12";
    private static final String SESSION_KEY = "p280.settlement.generated";

    @Autowired private ProductionSettlementRepository repo;
    @Autowired private CurrentUserContext ctx;

    /** A desktop "MessageBox then return false" - not an exception on the desktop. */
    public static class Refusal extends RuntimeException {
        public final String caption;
        public final String icon;
        public Refusal(String message, String caption, String icon) {
            super(message);
            this.caption = caption;
            this.icon = icon;
        }
    }

    /* ================================================================================= Load */

    /**
     * frmFoodProduction_Load:262 - the two configurations, the FIFO feature, the rights object,
     * the Difference Account list and the pending job orders (JobOrderNoFillForSettlement(1)).
     */
    public Map<String, Object> load() {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("screenName", ProductionSettlementRepository.DESKTOP_SCREEN_NAME);
        out.put("jobOrderCreatewithoutRates", toBool(safeConfig(org, comp, "JobOrderCreatewithoutRates")));
        out.put("saleCostingJobOrderWise", toBool(safeConfig(org, comp, "SaleCostingJobOrderWise")));
        boolean fifo;
        try { fifo = repo.erpFeature(org, comp, ERP_FEATURE_FIFO_CGS); } catch (Exception e) { fifo = false; }
        out.put("fifoCgs", fifo);
        /* formright is read (:273) and then never consulted anywhere in the form. It is returned
           for transparency only; no control on the page is gated by it, exactly as on the desktop. */
        out.put("rights", rights());
        out.put("differenceAccounts", differenceAccounts());
        out.put("jobOrders", jobOrders(1));
        return out;
    }

    /** stockAccoutnFill - (Id, AccountTitle) only, as BindDDLNew rebuilds the table. */
    public List<Map<String, Object>> differenceAccounts() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.accountTitlesByAccountTypeIds(ctx.currentOrganizationId(),
                ctx.currentCompanyId(), ctx.currentAppId(), ctx.currentUserId(), DIFFERENCE_ACCOUNT_TYPES)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", col(r, "Id"));
            m.put("AccountTitle", col(r, "AccountTitle"));
            out.add(m);
        }
        return out;
    }

    /** JobOrderNoFillForSettlement(ActionId) - financial year from the session. */
    public List<Map<String, Object>> jobOrders(int actionId) {
        return repo.jobOrdersAll(ctx.currentOrganizationId(), ctx.currentCompanyId(),
                                 ctx.currentFinancialYearId(), actionId);
    }

    /** SetRightsValueInRightsObject("frmProductionSettlement") - same rules as the shell's port. */
    public Map<String, Boolean> rights() {
        Map<String, Boolean> r = new LinkedHashMap<>();
        String role = ctx.currentRoleName();
        boolean admin = "Admin".equals(role);
        r.put("save", admin);
        r.put("update", admin);
        r.put("delete", admin);
        r.put("print", admin);
        try {
            for (Map<String, Object> row : repo.userRightsForScreen(ctx.currentUserId(), role,
                                                                    ctx.currentCompanyId())) {
                String name = str(col(row, "RightName"));
                boolean v = toBool(col(row, "Value"));
                switch (name) {
                    case "Save":   r.put("save", admin || v); break;
                    case "Update": r.put("update", admin || v); break;
                    case "Print":  r.put("print", admin || v); break;
                    case "Delete": r.put("delete", v); break;
                    default: break;
                }
            }
        } catch (Exception ignored) { /* defaults stay */ }
        return r;
    }

    /* ============================================================================= Generate */

    /**
     * btnGeneralSettlement_Click:376 - the three reads in the desktop's order inside one try: if
     * the first throws, the other two never run.
     */
    public Map<String, Object> generate(int jobOrderId, HttpSession session) {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> io = repo.settlementData(org, comp, jobOrderId, "InPutOutPut");
        out.put("inputOutput", io);
        out.put("overHeads", repo.settlementData(org, comp, jobOrderId, "OverHeads"));
        out.put("packingMaterial", repo.settlementData(org, comp, jobOrderId, "PackingMaterial"));

        /* Remember which production rows this tenant was shown for this job order, so an update
           can only touch rows the procedure itself returned. */
        Set<Integer> ids = new HashSet<>();
        for (Map<String, Object> r : io) {
            ids.add(toInt(col(r, "DetailId")));
        }
        if (session != null) {
            @SuppressWarnings("unchecked")
            Map<String, Object> m = (Map<String, Object>) session.getAttribute(SESSION_KEY);
            if (m == null) m = new HashMap<>();
            m.put(org + ":" + comp + ":" + jobOrderId, ids);
            session.setAttribute(SESSION_KEY, m);
        }
        return out;
    }

    /** CommonServices.GetUomScheduleByItemId (ProporationFinishGoodsSettlement:1210). */
    public List<Map<String, Object>> uomByItem(int itemId) {
        return repo.uomScheduleByItemId(ctx.currentOrganizationId(), ctx.currentCompanyId(), itemId);
    }

    /* ======================================================================= UpdateSettlement */

    /**
     * UpdateSettlement():1327, rule for rule, over the rows the output grid shows (GetRows).
     * Rows with Weight <= 0 are skipped. The first failing row stops everything before any write.
     */
    public void updateOutput(int jobOrderId, List<Map<String, Object>> rows, HttpSession session) {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        List<LinkedHashMap<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (dbl(r.get("Weight")) > 0.0) {
                double qty = dbl(hashText(r.get("Quantity")));     /* Cells["Quantity"].Text, format "#,#" */
                double weight = dbl(r.get("Weight"));
                if ("UnApproved".equals(str(r.get("ProductionApprovalStatus")))) {
                    String itemName = str(r.get("ItemName"));
                    int docNo = toInt(r.get("DocNo"));
                    throw new Refusal("Settlement cannot be processed because the Production Output transaction is still pending approval.\n\nItem: "
                            + itemName + "\nDocument No: " + docNo + "\nQty: " + net(qty) + "\nWeight: " + net(weight),
                            "Approval Pending", "error");
                }
                LinkedHashMap<String, Object> d = ProductionSettlementRepository.detailParams();
                d.put("@Qty", qty);
                d.put("@Weight", weight);
                d.put("@Id", toInt(r.get("DetailId")));
                d.put("@InvFoodProductionId", toInt(r.get("Id")));
                String entryType = str(r.get("EntryType"));
                d.put("@EntryType", entryType);
                d.put("@RefDocNoId", 0);
                if (str(r.get("WareHouseId")).isEmpty() || toInt(r.get("WareHouseId")) == 0) {
                    throw new Refusal("WareHouse Field Require in Output Detail", null, null);
                }
                d.put("@WarehouseId", toInt(r.get("WareHouseId")));
                if (str(r.get("ItemId")).isEmpty() || toInt(r.get("ItemId")) == 0) {
                    throw new Refusal("Item Field Require in Output Detail", null, null);
                }
                d.put("@ItemId", toInt(r.get("ItemId")));
                d.put("@ItemUomId", toInt(r.get("ItemUOMId")));
                d.put("@CropBatch", str(r.get("CropYear")));
                String jobLotText = str(r.get("JobLotId"));
                if (jobLotText.isEmpty() || "0".equals(jobLotText)) {
                    throw new Refusal("JobLot Field Require in Output Detail", null, null);
                }
                d.put("@JobLotId", toInt(r.get("JobLotId")));
                d.put("@PackingtypeId", toInt(r.get("PackingTypeId")));
                if (weight == 0.0) throw new IllegalArgumentException("Weight Field Required");
                double rate = dbl(r.get("Rate"));
                boolean fgUsed = "FinishGoods".equals(entryType) && "Used".equals(str(r.get("Status")));
                double netRate = fgUsed ? rate : dbl(r.get("NetRate"));
                int rateUomId = toInt(r.get("RateUOMId"));
                double amount = dbl(r.get("ItemAmount"));
                rate = rate > 0.0 ? rate : dbl(r.get("RateWithoutExp"));
                amount = amount > 0.0 ? amount : dbl(r.get("AmountWithoutExp"));
                double totalAmount = fgUsed ? amount : dbl(r.get("TotalAmount"));
                d.put("@Rate", rate);
                d.put("@NetRate", netRate);
                d.put("@RateUOMId", rateUomId);
                d.put("@InvProductionJobOrderId", jobOrderId);
                d.put("@Amount", amount);
                d.put("@ItemPmCost", dbl(r.get("ItemPmCost")));
                d.put("@GeneralPmCost", dbl(r.get("GeneralPmCost")));
                d.put("@ItemOhCost", dbl(r.get("ItemOhCost")));
                d.put("@GeneralOhCost", dbl(r.get("GeneralOhCost")));
                d.put("@TotalAmount", totalAmount);
                if (rate <= 0.0) throw new IllegalArgumentException("Rate Field Required");
                if (netRate <= 0.0) throw new IllegalArgumentException("NetRate Field Required");
                if (rateUomId == 0) throw new IllegalArgumentException("RateUom Field Required");
                if (amount <= 0.0) throw new IllegalArgumentException("ItemAmount must be greater than zero Please Check");
                if (totalAmount <= 0.0) throw new IllegalArgumentException("TotalAmount must be greater than zero Please Check");
                d.put("@VoucherHeadId", toInt(r.get("WIPAccountId")));
                d.put("@Remarks", str(r.get("Remarks")));
                details.add(d);
            }
        }
        requireGenerated(session, org, comp, jobOrderId, details);
        repo.updateSettlement(org, comp, jobOrderId, details);
    }

    /** InPutUpdateSettlement():1431 over every input row. */
    public void updateInput(int jobOrderId, List<Map<String, Object>> rows, HttpSession session) {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        Set<Integer> matchedIds = new HashSet<>();
        for (Map<String, Object> r : rows) {
            int refType = toInt(r.get("RefDocumentTypeId"));
            if ((refType == 112 && toInt(r.get("RefJobOrderId")) > 0) || refType == 56 || refType == 57 || refType == 40) {
                matchedIds.add(toInt(r.get("Id")));
            }
        }
        List<LinkedHashMap<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            int currentId = toInt(r.get("Id"));
            if (!matchedIds.contains(currentId)) continue;
            LinkedHashMap<String, Object> d = ProductionSettlementRepository.detailParams();
            d.put("@Id", toInt(r.get("DetailId")));
            d.put("@InvFoodProductionId", currentId);
            d.put("@EntryType", str(r.get("EntryType")));
            d.put("@RefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
            d.put("@RefDocNoId", toInt(r.get("RefDocIdNo")));
            d.put("@RefDocSubIdNo", toInt(r.get("RefDocSubIdNo")));
            d.put("@WarehouseId", toInt(r.get("WareHouseId")));
            d.put("@ItemId", toInt(r.get("ItemId")));
            d.put("@ItemUomId", toInt(r.get("ItemUOMId")));
            d.put("@CropBatch", str(r.get("CropYear")));
            d.put("@JobLotId", toInt(r.get("JobLotId")));
            d.put("@PackingtypeId", toInt(r.get("PackingTypeId")));
            d.put("@Qty", dbl(hashText(r.get("Quantity"))));
            d.put("@Weight", dbl(r.get("Weight")));
            double rate = dbl(r.get("Rate"));
            if (rate == 0.0) throw new IllegalArgumentException("Rate Field Required");
            d.put("@Rate", rate);
            d.put("@NetRate", rate);
            int rateUomId = toInt(r.get("RateUOMId"));
            if (rateUomId == 0) throw new IllegalArgumentException("RateUom Field Required");
            d.put("@RateUOMId", rateUomId);
            d.put("@InvProductionJobOrderId", jobOrderId);
            double amount = dbl(r.get("ItemAmount"));
            if (amount == 0.0) throw new IllegalArgumentException("ItemAmount must be greater than zero Please Check");
            d.put("@Amount", amount);
            d.put("@TotalAmount", amount);
            d.put("@VoucherHeadId", toInt(r.get("WIPAccountId")));
            d.put("@Remarks", str(r.get("Remarks")));
            details.add(d);
        }
        requireGenerated(session, org, comp, jobOrderId, details);
        repo.inputUpdateSettlement(details);
    }

    /* =================================================================== SettlementFinancials */

    /**
     * SettlementFinancials():1505 up to the point it writes: the WIP account, the same-account
     * check, the weight validation and the special-approval state. Returns
     *   {action:"proceed"} | {action:"confirm", message, remarks} | throws IllegalArgumentException.
     * The two "Field is Required" checks before it are the page's (they focus a control); they are
     * repeated in {@link #saveFinancials}.
     */
    public Map<String, Object> financialsGate(int jobOrderId, int differenceAccountId) {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        requireJobOrder(jobOrderId);
        List<Map<String, Object>> gl = repo.glAccountsByJobOrderId(org, comp, jobOrderId);
        if (gl == null || gl.isEmpty()) throw new IllegalArgumentException("WIP Account Not Found");
        int wip = toInt(col(gl.get(0), "WorkInProccessAcId"));
        if (wip == differenceAccountId) {
            throw new IllegalArgumentException("WIP Account and Difference Account are the same. Please verify.");
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("action", "proceed");
        out.put("wipAccountId", wip);
        List<Map<String, Object>> v = repo.weightValidations(org, comp, jobOrderId);
        if (v != null && !v.isEmpty()) {
            Map<String, Object> r = v.get(0);
            double in = dbl(col(r, "InPutWeight"));
            double outW = dbl(col(r, "OutPutWeight"));
            double diffW = dbl(col(r, "DiffWeight"));
            double diffP = dbl(col(r, "DiffPercentage"));
            int status = toInt(col(r, "EntryStatusId"));
            String message = str(col(r, "ErrorMessage"));
            if (status != 1) throw new IllegalArgumentException(message);
            List<Map<String, Object>> sa = repo.specialApproval(org, comp, jobOrderId);
            if (sa != null && !sa.isEmpty()) {
                int saId = toInt(col(sa.get(0), "SpecialApprovalId"));
                int saUser = toInt(col(sa.get(0), "SpecialApprovalUserId"));
                if (saId == 0 && saUser == 0) {
                    out.put("action", "confirm");
                    out.put("message", message + "\nYou need to reduce this weight difference or request special approval for this job order. Do you want to request special approval for this job order???");
                    out.put("remarks", message);
                    return out;
                }
                if (saId == 1 && saUser == 0) {
                    throw new IllegalArgumentException("This Job Order is currently in Special Approval. You cannot settle it until the special approval is granted");
                }
            } else if (diffP > 50.0) {
                throw new IllegalArgumentException("The difference between input weight (" + net(in) + ") and output weight (" + net(outW) + ") "
                        + "is greater than 50%.\n" + "Difference weight: " + net(diffW) + " and Difference Percentage: " + net(diffP) + "%\n"
                        + "The entry is in " + net(100.0 - diffP) + "% loss, which cannot be settled even after special approval.");
            }
        }
        return out;
    }

    /** The Yes branch at :1627 - request, then the desktop throws "Job Order Sent for special approval." */
    public void requestSpecialApproval(int jobOrderId, int differenceAccountId) {
        Map<String, Object> gate = financialsGate(jobOrderId, differenceAccountId);
        if (!"confirm".equals(gate.get("action"))) {
            throw new IllegalArgumentException("Special approval is not pending a request for this job order.");
        }
        repo.requestSpecialApproval(ctx.currentOrganizationId(), ctx.currentCompanyId(), jobOrderId,
                                    ctx.currentUserId(), str(gate.get("remarks")));
    }

    /**
     * The voucher (:1551-1602) and InvFoodProduction.SetDataForSettlementFinancials (:1642).
     * The gate is re-run first; only a "proceed" answer writes.
     *
     * @return the DAL's return value; the page says "Record Save Successfully" when > 0,
     *         otherwise "Record Update Successfully".
     */
    public int saveFinancials(Map<String, Object> body) {
        int jobOrderId = toInt(body.get("jobOrderId"));
        int diffAccountId = toInt(body.get("differenceAccountId"));
        if (jobOrderId == 0) throw new IllegalArgumentException("JobOrder Settlement Field is Required");
        if (diffAccountId == 0) throw new IllegalArgumentException("Difference Account Field is Required");
        String diffText = str(body.get("differenceAmountText"));
        double differenceAmount = dbl(diffText);
        if (differenceAmount == 0.0) throw new IllegalArgumentException("Difference Amount is zero - there is no settlement voucher to post.");

        Map<String, Object> gate = financialsGate(jobOrderId, diffAccountId);
        if (!"proceed".equals(gate.get("action"))) {
            throw new IllegalArgumentException(str(gate.get("remarks")));
        }
        int wip = toInt(gate.get("wipAccountId"));
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        int user = ctx.currentUserId();
        int branch = ctx.currentBranchId();
        String jobOrderText = str(body.get("jobOrderText"));

        int voucherId = repo.voucherHeadId(org, comp, ProductionSettlementRepository.SETTLEMENT_DOCUMENT_TYPE_ID, jobOrderId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        LinkedHashMap<String, Object> head = ProductionSettlementRepository.voucherHeadDefaults();
        head.put("@Id", voucherId);
        head.put("@DocumentTypeSrNo", jobOrderId);
        head.put("@ManualBillNo", jobOrderText);                  /* CmbJobOrderSettlement.Text */
        head.put("@VoucherDate", settlementDate(body.get("settlementDate")));
        /* obj.VoucherCode = obj.VoucherCode - never set: 0 */
        head.put("@DocumentTypeId", ProductionSettlementRepository.SETTLEMENT_DOCUMENT_TYPE_ID);
        head.put("@RefAccountId", wip);
        head.put("@AgainstAccountId", diffAccountId);
        head.put("@EntryDate", now);
        head.put("@ModifyDate", now);
        head.put("@EntryUser", user);
        head.put("@ModifyUser", user);
        head.put("@OrganizationId", org);
        head.put("@CompanyId", comp);
        head.put("@FinancialYearId", ctx.currentFinancialYearId());
        head.put("@BranchId", branch);
        String remarks = "Finish Goods ActualAmount " + str(body.get("actualFinishGoodsText"))
                + " Finish Goods Amount against JobOrder " + jobOrderText.trim() + "  "
                + str(body.get("finishAmountAgainstJobOrderText")) + " Difference Amount " + diffText;
        head.put("@VoucherAmount", Math.abs(differenceAmount));

        List<LinkedHashMap<String, Object>> lines = new ArrayList<>();
        LinkedHashMap<String, Object> vd = ProductionSettlementRepository.voucherDetailDefaults();
        vd.put("@AccountId", diffAccountId);
        vd.put("@AgainstAccountId", wip);
        vd.put("@Comments", remarks);
        /* A negative difference goes in as a NEGATIVE CreditAmount / DebitAmount - desktop as is. */
        if (differenceAmount > 0.0) vd.put("@DebitAmount", differenceAmount);
        else vd.put("@CreditAmount", differenceAmount);
        vd.put("@OrderNo", jobOrderId);
        vd.put("@BranchesId", branch);
        lines.add(vd);
        LinkedHashMap<String, Object> vd2 = ProductionSettlementRepository.voucherDetailDefaults();
        vd2.put("@AccountId", wip);
        vd2.put("@AgainstAccountId", diffAccountId);
        vd2.put("@Comments", remarks);
        if (differenceAmount > 0.0) vd2.put("@CreditAmount", differenceAmount);
        else vd2.put("@DebitAmount", differenceAmount);
        vd2.put("@OrderNo", jobOrderId);
        vd2.put("@BranchesId", branch);
        lines.add(vd2);
        return repo.settlementFinancials(head, lines);
    }

    /* ================================================================================ Approve */

    /** btnApproveSettlement_Click:1952 - JobOrderApproveList with this one job order. */
    public void approve(int jobOrderId) {
        requireJobOrder(jobOrderId);
        repo.jobOrderApprove(ctx.currentOrganizationId(), ctx.currentCompanyId(), jobOrderId, ctx.currentUserId());
    }

    /* ================================================================================ Voucher */

    /** btnSettlementVoucher_Click:1975 - VoucherHeadIdGet(JobOrderId, 142). */
    public int voucherHeadId(int jobOrderId) {
        return repo.voucherHeadId(ctx.currentOrganizationId(), ctx.currentCompanyId(),
                ProductionSettlementRepository.SETTLEMENT_DOCUMENT_TYPE_ID, jobOrderId);
    }

    /* ================================================================================= Prints */

    /**
     * The row count each print button checks before opening the viewer
     * ("Record Not Found For DisPlay" / "No Record Found For Display"). The data source and guards
     * are the ones the matching ReportRegistry entry must carry.
     */
    public int printRowCount(String report, int jobOrderId, int actionId, int voucherHeadId) {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        LinkedHashMap<String, Object> m = p("@OrganizationId", org, "@CompanyId", comp);
        String proc;
        switch (report) {
            case "613":  proc = "dbo.Sp_InvFoodProduction_Summery2_Rpt"; break;
            case "613A": proc = "dbo.usp_ProductionBeforSettlement_Summery2_Rpt"; break;
            case "615":  proc = "dbo.Sp_InvFoodProduction_Summery3_Rpt"; break;
            case "615A": proc = "dbo.usp_ProductionBeforSettlement_Summery3_Rpt"; break;
            case "602A": proc = "dbo.Sp_InvFoodProduction_Summery_Rpt"; break;
            case "613B": proc = "[dbo].[USP_ProductionSettlement_WithReferenceDocumentDetailReport]"; break;
            case "103":
                /* VoucherSlipForInventoryReport: BranchesId/ProjectsId 0, dates unset, doc nos 0 and
                   ApprovedFilter "All" are all omitted; @Id and @DocumentTypeId are non-zero. */
                if (voucherHeadId != 0) m.put("@Id", voucherHeadId);
                m.put("@DocumentTypeId", ProductionSettlementRepository.SETTLEMENT_DOCUMENT_TYPE_ID);
                return repo.reportRows("dbo.Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt", m).size();
            default: throw new IllegalArgumentException("Unknown report " + report);
        }
        m.put("@JobOrderId", jobOrderId);
        /* Every one of these BLLs adds @ActionId only when it is non-zero; 602A's also carries
           @PlantId/@FromDate/@ToDate (unset: omitted) and @BranchesIds (null value: not supplied). */
        if (actionId != 0) m.put("@ActionId", actionId);
        return repo.reportRows(proc, m).size();
    }

    /* ================================================================================ helpers */

    /** Only rows the Generate call returned to this tenant for this job order may be written. */
    private void requireGenerated(HttpSession session, int org, int comp, int jobOrderId,
                                  List<LinkedHashMap<String, Object>> details) {
        if (details.isEmpty()) return;
        Set<?> ids = null;
        if (session != null) {
            Object m = session.getAttribute(SESSION_KEY);
            if (m instanceof Map) ids = (Set<?>) ((Map<?, ?>) m).get(org + ":" + comp + ":" + jobOrderId);
        }
        if (ids == null) throw new IllegalStateException("Generate the settlement for this job order first.");
        for (Map<String, Object> d : details) {
            if (!ids.contains(toInt(d.get("@Id")))) {
                throw new IllegalStateException("A row does not belong to the generated settlement of this job order.");
            }
        }
    }

    /** A job order id must be one this company's GetJobOrderNoAll lists (all, @ActionId omitted). */
    private void requireJobOrder(int jobOrderId) {
        for (Map<String, Object> r : jobOrders(0)) {
            if (toInt(col(r, "Id")) == jobOrderId) return;
        }
        throw new IllegalArgumentException("JobOrder Settlement Field is Required");
    }

    private String safeConfig(int org, int comp, String name) {
        try { return repo.configValue(org, comp, name); } catch (Exception e) { return ""; }
    }

    /** txtSettlementDate.Value - the page sends yyyy-MM-ddTHH:mm[:ss]; the time of day is kept. */
    private static Timestamp settlementDate(Object o) {
        String s = str(o);
        try {
            if (s.length() == 10) return Timestamp.valueOf(s + " 00:00:00");
            return Timestamp.valueOf(LocalDateTime.parse(s.length() == 16 ? s + ":00" : s));
        } catch (Exception e) {
            return Timestamp.valueOf(LocalDateTime.now());
        }
    }

    /** Conversion.ToBool: Convert.ToBoolean, falling back to Convert.ToInt32 != 0. */
    static boolean toBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).doubleValue() != 0;
        String s = String.valueOf(o).trim();
        if (s.isEmpty()) return false;
        if ("true".equalsIgnoreCase(s)) return true;
        if ("false".equalsIgnoreCase(s)) return false;
        try { return Integer.parseInt(s) != 0; } catch (NumberFormatException e) { return false; }
    }

    static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    /** Conversion.ToDouble on text: thousands separators allowed, anything unparsable is 0. */
    static double dbl(Object o) {
        if (o == null) return 0.0;
        if (o instanceof Number) {
            double d = ((Number) o).doubleValue();
            return Double.isInfinite(d) ? 0.0 : d;
        }
        String s = String.valueOf(o).trim().replace(",", "");
        if (s.isEmpty()) return 0.0;
        try {
            double d = Double.parseDouble(s);
            return Double.isInfinite(d) ? 0.0 : d;
        } catch (NumberFormatException e) {
            return 0.0;
        }
    }

    /** GridEX cell.Text of a "#,#" column: rounded half away from zero, "" for zero. */
    static String hashText(Object v) {
        double d = dbl(v);
        if (Double.isNaN(d)) return "NaN";
        long r = new BigDecimal(d).setScale(0, RoundingMode.HALF_UP).longValue();
        return r == 0 ? "" : String.valueOf(r);
    }

    /** .NET double interpolation ("{x}"), 15 significant digits. */
    static String net(double d) {
        if (Double.isNaN(d)) return "NaN";
        if (Double.isInfinite(d)) return d > 0 ? "Infinity" : "-Infinity";
        BigDecimal b = new BigDecimal(d).round(new java.math.MathContext(15)).stripTrailingZeros();
        String s = b.abs().compareTo(BigDecimal.ONE.movePointRight(15)) >= 0 ? b.toString() : b.toPlainString();
        return "-0".equals(s) ? "0" : s;
    }
}
