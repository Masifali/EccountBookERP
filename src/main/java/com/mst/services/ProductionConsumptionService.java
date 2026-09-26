package com.mst.services;

import com.mst.repositories.ProductionAgainstJobOrderRepository;
import com.mst.repositories.ProductionConsumptionRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ProductionConsumptionRepository.DOC_TYPE_CONSUMPTION;
import static com.mst.repositories.ProductionConsumptionRepository.bool;
import static com.mst.repositories.ProductionConsumptionRepository.col;
import static com.mst.repositories.ProductionConsumptionRepository.dbl;
import static com.mst.repositories.ProductionConsumptionRepository.intOf;
import static com.mst.repositories.ProductionConsumptionRepository.netG;
import static com.mst.repositories.ProductionConsumptionRepository.str;

/**
 * Screen 280 - the parts FoodProductionWithValues.cs implements itself: the Consumption tab page
 * (Form + History) and the Transaction History tab page, and the two loader forms the Consumption
 * page opens.
 *
 * Tenancy (organization, company, branch, financial year, user) always comes from the session.
 * The browser sends only what the operator typed or picked.
 */
@Service
public class ProductionConsumptionService {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionConsumptionService.class);

    /** LoadConsumptionPendingforRates.JobOrderFill:127 - DocumentTypeId = 403. */
    private static final int PENDING_JOB_ORDER_DOC_TYPE = 403;
    /** LoadavailableTransactionsForIssuanceOnConsumption_Load:532 - the rights screen it reads. */
    private static final String LOADER_RIGHTS_SCREEN = "LoadavailableTransactionsForIssuance";

    @Autowired private ProductionConsumptionRepository repo;
    @Autowired private ProductionAgainstJobOrderRepository shellRepo;
    @Autowired private ProductionAgainstJobOrderService shell;
    @Autowired private CurrentUserContext cu;
    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private PlatformTransactionManager txManager;

    private int org()  { return cu.currentOrganizationId(); }
    private int comp() { return cu.currentCompanyId(); }
    private int branch() { return cu.currentBranchId(); }
    private int year() { return cu.currentFinancialYearId(); }
    private int user() { return cu.currentUserId(); }

    // ========================================================================================= init

    /**
     * What the page needs once: the two decimal configurations the desktop reads at login
     * (CommonServices.GetDecimalConfiguration - clsGlobalVariables.stringFormatsingle and
     * DecimalRateFormate) and ActiveYr.Start_Period (the loaders' From date).
     */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        int amountDec = 0, rateDec = 2;
        try { amountDec = intOf(repo.configFromAllocation(org(), comp(), "Default NoofDecimal Points For Amount")); }
        catch (Exception e) { LOG.warn("amount decimals", e); }
        try {
            int r = intOf(repo.configFromAllocation(org(), comp(), "Default NoofDecimal Points For Rate"));
            rateDec = r > 0 ? r : 2;    /* "case 0: text2 = 00" - 0 and absent both mean two decimals */
        } catch (Exception e) { LOG.warn("rate decimals", e); }
        /* stringFormatsingle = "#,##0." + (1..4 zeros, otherwise nothing): 0 or >4 -> "#,##0." (no decimals). */
        out.put("amountDecimals", (amountDec >= 1 && amountDec <= 4) ? amountDec : 0);
        /* DecimalRateFormate = "#,#0." + zeros; 0 -> "00"; anything else outside 1..4 -> no decimals. */
        out.put("rateDecimals", rateDec >= 1 && rateDec <= 4 ? rateDec : 0);
        out.put("financialYearStart", financialYearStart());
        out.put("now", LocalDateTime.now().withNano(0).toString());
        return out;
    }

    private String financialYearStart() {
        try {
            List<Map<String, Object>> years = jdbcTemplate.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    org(), comp());
            Map<String, Object> row = null;
            for (Map<String, Object> r : years) if (intOf(col(r, "Id")) == year()) { row = r; break; }
            if (row == null && !years.isEmpty()) row = years.get(0);
            Object v = row == null ? null : col(row, "Start_Period");
            if (v == null) return null;
            String s = String.valueOf(v);
            return s.length() >= 10 ? s.substring(0, 10) : s;
        } catch (Exception e) {
            LOG.warn("Could not read ActiveYr.Start_Period", e);
            return null;
        }
    }

    // ================================================================================ Consumption

    /** GenerateDocConsumption:1300 - DocumentTypeId 181, BranchId, FinancialYearId. */
    public Map<String, Object> serial() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", repo.serialNumber(org(), comp(), DOC_TYPE_CONSUMPTION, year(), branch()));
        return m;
    }

    /**
     * ConsumptionDetailComboBind:1330 - one call, split on ActivityType into the five detail
     * pickers. Rows are returned raw; the page splits them exactly as the desktop does.
     */
    public List<Map<String, Object>> detailCombos() {
        return repo.dropDownForConsumptionForm(org(), comp(), String.valueOf(branch()));
    }

    /** GetAvailableStockForConsumption:2546. */
    public List<Map<String, Object>> stock(int warehouseId, int itemId, int jobLotId, String cropYear, String toDate) {
        return repo.weightCurrStockByItem(org(), comp(), itemId, dt(toDate), warehouseId, jobLotId,
                cropYear == null ? "" : cropYear);
    }

    /**
     * CommonServices.AvgRateOnlyForCGS - the page passes exactly the arguments each desktop caller
     * passes (GetAvgRateByItemAndJoblotForConsumption:2632 or AvgRateUpdateOnDocDateChange:3422).
     */
    public Map<String, Object> cgsRate(int itemId, String docDate, int documentTypeId, int recId, int jobLotId,
                                       int cropYearId, String cropYear, int warehouseId) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rate", repo.avgRateOnlyForCgs(org(), comp(), itemId, dt(docDate), documentTypeId, recId, jobLotId,
                cropYearId, cropYear, warehouseId, 0));
        return m;
    }

    /**
     * CommonServices.GetAvgRateFromFIFOMethod (WinApp :2386) - USP_GetStockByFifoMethod through the
     * BLL, then the desktop's own weighted walk over BalWeight/BalQty. Returns the per-kg rate
     * (callers multiply by 40 themselves, as the desktop does).
     */
    public Map<String, Object> fifoRate(String docDate, int itemId, int stockUom, int warehouseId, int jobLotId,
                                        int packingTypeId, String cropYear, double itemQty, double netWeight,
                                        int documentTypeId, int id) {
        List<Map<String, Object>> list = repo.stockByFifo(org(), comp(), itemId, dt(docDate), stockUom, warehouseId,
                0, jobLotId, packingTypeId, cropYear, documentTypeId, id, null, true);
        double result = 0d;
        if (!list.isEmpty()) {
            double totalW = 0d;
            for (Map<String, Object> r : list) totalW += dbl(col(r, "BalWeight"));
            double num2 = 0d;
            if (netWeight <= totalW) {
                double num3 = netWeight, num4 = itemQty, num6 = 0d, num8 = 0d;
                for (Map<String, Object> r : list) {
                    double num7 = dbl(col(r, "BalWeight"));
                    double num5 = dbl(col(r, "BalQty"));
                    double avg = dbl(col(r, "AvgRate"));
                    if (num7 <= num3 - num8 && num5 <= num4 - num6) {
                        num8 += num7;
                        num6 += num5;
                        num2 += num7 * avg;
                    } else if (num7 >= num3 - num8 && num5 >= num4 - num6) {
                        double num9 = num4 - num6;
                        double num10 = num3 - num8;
                        num2 += (num3 - num8) * avg;
                        num6 += num9;
                        num8 += num10;
                    }
                    if (netWeight == num8 && itemQty == num6) break;
                }
                if (num2 > 0d && netWeight > 0d) result = num2 / netWeight;
            }
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rate", result);
        return m;
    }

    /**
     * ReadByIdConsumption:2341 - InvFoodProduction.GetByID. The page applies the desktop's checks
     * (EntryType, IsApproved, PlanStatus) and builds the grid rows; this returns the model.
     */
    public Map<String, Object> readById(int id) {
        Map<String, Object> sc = repo.getById(id);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("found", sc != null);
        if (sc == null) return m;
        m.put("header", header(sc));
        m.put("consumption", sc.get("InvFoodProductionConsumptionDetailslist"));
        m.put("details", sc.get("invFoodProductionDetails"));
        return m;
    }

    private Map<String, Object> header(Map<String, Object> sc) {
        Map<String, Object> h = new LinkedHashMap<>();
        for (String k : new String[]{"Id", "DocDate", "DocumentTypeId", "DocCode", "InvJobOrderId", "InvJobOrderNo",
                "MainRemarks", "IsApproved", "EntryType", "PlantId", "EBDepartmentId", "WIPAccountId", "WIPItemId",
                "BaseDocumentTypeId", "PlanStatus"}) {
            h.put(k, col(sc, k));
        }
        return h;
    }

    /** CommonServices.VoucherHeadIdGet(RecIdConsumption, 181) - btnPrintVoucherConsumption_Click:2430. */
    public Map<String, Object> voucherHeadId(int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("voucherHeadId", shellRepo.voucherHeadId(org(), comp(), DOC_TYPE_CONSUMPTION, id));
        return m;
    }

    // ------------------------------------------------------------------------ Consumption history

    /** jobOrderConsumptionHCombobind:2858 - DocumentTypeIds "181", Activity "JobOrder". */
    public List<Map<String, Object>> historyJobOrders() {
        return repo.dropDownFromFoodProduction(org(), comp(), year(), "JobOrder", "181", String.valueOf(branch()));
    }

    /**
     * GrdHistoryConsumptionMain:2928 - ProductionFormHistory with both dates always sent (the two
     * pickers have no check box here), then only rows whose DocumentTypeId is 181 are kept.
     */
    public List<Map<String, Object>> history(String from, String to, String docFrom, String docTo, int jobOrderId) {
        List<Map<String, Object>> all = repo.formHistory(org(), comp(), year(), branch(), dt(from), dt(to),
                intOf(docFrom), intOf(docTo), jobOrderId, 0);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : all) if (intOf(col(r, "DocumentTypeId")) == DOC_TYPE_CONSUMPTION) out.add(r);
        return out;
    }

    // ------------------------------------------------------------------------ Transaction History

    /** jobOrderTransactionHistoryCombobind:931 - DocumentTypeIds "80,112", no Activity. */
    public List<Map<String, Object>> transactionHistoryCombos() {
        return repo.dropDownFromFoodProduction(org(), comp(), year(), null, "80,112", String.valueOf(branch()));
    }

    /**
     * GridHistory:998 - the two dates are sent only when their check boxes are ticked (the
     * page sends nothing otherwise); every other filter is guarded by the BLL.
     */
    public List<Map<String, Object>> transactionHistory(String from, String to, String docFrom, String docTo,
                                                        int jobOrderId, int plantId) {
        return repo.formHistory(org(), comp(), year(), branch(), dt(from), dt(to), intOf(docFrom), intOf(docTo),
                jobOrderId, plantId);
    }

    // --------------------------------------------------------------------------------- loaders

    /** LoadConsumptionPendingforRates.JobOrderFill:113. */
    public List<Map<String, Object>> pendingJobOrders() {
        return repo.jobOrderNoForInvFoodProduction(org(), comp(), PENDING_JOB_ORDER_DOC_TYPE, year());
    }

    /** LoadConsumptionPendingforRates.OutputGridHistory:189 - rows raw; the page groups by Id. */
    public List<Map<String, Object>> pendingForRates(String from, String to, int jobOrderId) {
        return repo.consumptionPendingForRates(org(), comp(), branch(), jobOrderId, dt(from), dt(to));
    }

    /**
     * LoadavailableTransactionsForIssuanceOnConsumption_Load:520 - tblUserRights.GetByUserId for
     * "LoadavailableTransactionsForIssuance" with RightName = the role name. When rows come back and
     * none is named "Rate", `newList[0]` throws on the desktop and Load stops before any combo is
     * bound; that failure is reported as `loadError`, with the .NET message, and nothing else runs.
     */
    public Map<String, Object> loaderInit() {
        Map<String, Object> m = new LinkedHashMap<>();
        try {
            List<Map<String, Object>> rights = shellRepo.userRightsForScreen(user(), LOADER_RIGHTS_SCREEN,
                    cu.currentRoleName(), comp());
            if (!rights.isEmpty()) {
                boolean hasRate = false;
                for (Map<String, Object> r : rights) if ("Rate".equals(str(col(r, "RightName")))) { hasRate = true; break; }
                if (!hasRate) {
                    m.put("loadError", "Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
                    return m;
                }
            }
        } catch (Exception e) {
            m.put("loadError", e.getMessage());
            return m;
        }
        m.put("combos", repo.dropDownForConsumptionForm(org(), comp(), null));
        return m;
    }

    /** BtnRefreshConsumption_Click on the loader - ConsumptionDetailComboBind again. */
    public List<Map<String, Object>> loaderCombos() {
        return repo.dropDownForConsumptionForm(org(), comp(), null);
    }

    public List<Map<String, Object>> loaderSearch(String from, String to, int parentCategoryId, int itemCategoryId,
                                                  int itemTypeId, int jobLotId, String cropYear, int warehouseId,
                                                  int refDocumentTypeId, int supplierCustomerId, int itemId) {
        return repo.availableTransactionsForIssuance(org(), comp(), dt(from), dt(to), supplierCustomerId,
                warehouseId, itemId, refDocumentTypeId, jobLotId, parentCategoryId, itemCategoryId, itemTypeId,
                cropYear);
    }

    // ======================================================================================= save

    /**
     * InsertConsumption:2174 -> InvFoodProduction.Save (BLL :4994) -> DAL InvFoodProduction.SetData.
     *
     * The page has already run FormValidationConsumption, the confirmation and the per-row checks
     * (with their exact messages) and sends the rows that survived the "Quantity > 0 and Weight > 0"
     * filter together with their grid row index. The rights, the row checks and the rate rules are
     * applied again here because the browser is not trusted with them.
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> save(Map<String, Object> in) {
        Map<String, Boolean> rights = shell.rights();
        boolean rateRight = Boolean.TRUE.equals(rights.get("rateAndAmount"));
        int recId = intOf(in.get("id"));
        if (recId > 0 && !Boolean.TRUE.equals(rights.get("update")))
            throw new IllegalStateException("You do not have the Update right on this screen.");
        if (recId <= 0 && !Boolean.TRUE.equals(rights.get("save")))
            throw new IllegalStateException("You do not have the Save right on this screen.");

        final int org = org(), comp = comp(), branch = branch(), year = year(), user = user();

        /* ---------------------------------------------------------------- the model (:2186-2221) */
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", recId > 0 ? recId : 0);
        h.put("DocumentTypeId", DOC_TYPE_CONSUMPTION);
        h.put("DocCode", intOf(in.get("docCode")));
        LocalDateTime docDate = dt(str(in.get("docDate")));
        if (docDate == null) throw new IllegalArgumentException("Doc Date is required.");
        h.put("DocDate", docDate);
        h.put("MainRemarks", str(in.get("mainRemarks")));
        h.put("EntryType", "Consumption");
        h.put("InvFoodProductionPlanId", intOf(in.get("plantId")));
        h.put("WIPAccountId", intOf(in.get("wipAccountId")));
        h.put("WIPItemId", intOf(in.get("wipItemId")));
        h.put("EBDepartmentId", intOf(in.get("departmentId")));
        h.put("InvJobOrderId", intOf(in.get("jobOrderId")));
        h.put("InvJobOrderNo", str(in.get("jobOrderNo")).trim());
        h.put("BaseDocumentTypeId", intOf(in.get("baseDocumentTypeId")));
        h.put("PlantId", intOf(in.get("plantId")));
        h.put("OrganizationId", org);
        h.put("CompanyId", comp);
        h.put("BranchId", branch);
        LocalDateTime now = LocalDateTime.now().withNano(0);
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("EntryUser", user);
        h.put("ModifyUser", user);
        h.put("FinancialYearId", year);

        /* FormValidationConsumption:1663 - repeated server side, same messages. */
        if (intOf(h.get("DocCode")) == 0) throw new IllegalArgumentException("document Number Field Required");
        if (intOf(h.get("InvJobOrderId")) == 0) throw new IllegalArgumentException("Job Order Number Field Required");
        if (intOf(h.get("PlantId")) == 0) throw new IllegalArgumentException("Plant Name Field Required");
        if (intOf(h.get("WIPAccountId")) == 0) throw new IllegalArgumentException("WorkInProcess Ac Field Required");
        if (intOf(h.get("WIPItemId")) == 0) throw new IllegalArgumentException("WIP Item Field Required");

        /* --------------------------------------------------------------- the rows (:2222-2308) */
        List<Map<String, Object>> rowsIn = in.get("rows") instanceof List ? (List<Map<String, Object>>) in.get("rows")
                : new ArrayList<>();
        if (rowsIn.isEmpty()) throw new IllegalArgumentException("Enter Detail First ...");
        List<Map<String, Object>> details = new ArrayList<>();
        double grossWeight = 0d;
        for (Map<String, Object> r : rowsIn) {
            /* Conversion.ToDouble(r.Cells["Quantity"].Text) > 0 && ...Weight.Text > 0 - the TEXT of a
               "#,##0.##" column, so a value that shows as "0" is skipped. */
            if (!(fmt2(dbl(r.get("quantity"))) > 0d) || !(fmt2(dbl(r.get("weight"))) > 0d)) continue;
            int rowNo = intOf(r.get("rowIndex")) + 1;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("EntryType", "Consumption");
            d.put("Id", intOf(r.get("id")));
            d.put("RefDocNoId", intOf(r.get("refDocIdNo")));
            d.put("RefDocumentTypeId", intOf(r.get("refDocumentTypeId")));
            d.put("RefDocSubIdNo", intOf(r.get("refDocSubIdNo")));
            String wh = str(r.get("warehouseId")).trim();
            if (wh.isEmpty() || "0".equals(wh)) throw new IllegalArgumentException("WareHouse Field Require in Consumption Detail");
            d.put("WarehouseId", intOf(wh));
            String it = str(r.get("itemId")).trim();
            if (it.isEmpty() || "0".equals(it)) throw new IllegalArgumentException("Item Field Require in Consumption Detail");
            d.put("ItemId", intOf(it));
            d.put("ItemUomId", intOf(r.get("itemUomId")));
            String crop = str(r.get("cropYear"));
            if (crop.isEmpty() || "0".equals(crop)) throw new IllegalArgumentException("CropYear Field Require in Consumption Detail");
            d.put("CropBatch", crop);
            if (str(r.get("jobLotId")).isEmpty()) throw new IllegalArgumentException("JobLot Field Require in Consumption Detail");
            d.put("JobLotId", intOf(r.get("jobLotId")));
            d.put("PackingtypeId", intOf(r.get("packingTypeId")));
            double qty = dbl(r.get("quantity"));
            d.put("Qty", qty);
            if (qty == 0d) throw new IllegalArgumentException("ItemQty Field Required");
            double weight = dbl(r.get("weight"));
            d.put("Weight", weight);
            grossWeight += weight;
            if (weight == 0d) throw new IllegalArgumentException("ItemWeight Field Required");
            d.put("InvProductionJobOrderId", intOf(h.get("InvJobOrderId")));
            if (!rateRight && intOf(d.get("RefDocumentTypeId")) == 0 && intOf(d.get("RefDocNoId")) == 0) {
                d.put("Rate", 0d);
                d.put("Amount", 0d);
            } else {
                d.put("Rate", dbl(r.get("rate")));
                /* Conversion.ToInt(ItemAmount) - Convert.ToInt32 of a decimal: rounded half to even. */
                d.put("Amount", (double) new BigDecimal(dbl(r.get("itemAmount"))).setScale(0, RoundingMode.HALF_EVEN).intValue());
            }
            d.put("RateUOMId", intOf(r.get("rateUomId")));
            double rateUom = dbl(r.get("rateUom"));
            if (rateUom != 0d) {
                List<Map<String, Object>> uoms = repo.uomScheduleByItemId(org, comp, intOf(it));
                if (!uoms.isEmpty()) {
                    int want = (int) new BigDecimal(rateUom).setScale(0, RoundingMode.HALF_EVEN).intValue();
                    Map<String, Object> hit = null;
                    for (Map<String, Object> u : uoms) {
                        /* dt.Select("Equivalent='40'") on a double column: numeric equality. */
                        if (dbl(col(u, "Equivalent")) == want) { hit = u; break; }
                    }
                    if (hit == null) throw new IllegalArgumentException("RateUOM On Row No " + rowNo + " is not define please check");
                    d.put("RateUOMId", intOf(col(hit, "Id")));
                }
            }
            d.put("Remarks", str(r.get("remarks")));
            /* carried for the posting engine only (not procedure parameters) */
            d.put("_RateEquivalent", dbl(r.get("rateUomForCheck")));
            details.add(d);
        }
        if (details.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        final double gross = grossWeight;
        TransactionTemplate tx = new TransactionTemplate(txManager);
        Integer id = tx.execute(status -> post(h, details, org, comp));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("message", (recId > 0 ? "Record Update Successfully" : "Record Save Successfully") + intOf(h.get("DocCode")));
        out.put("grossWeight", gross);
        /* :2318-2324 - WagesStatus && WagesActiveOrInActiveForConsumption: WagesDeleteByReferenceIds(181, id),
           then the page opens frmwagesBillHeader (RefDocTypeId 181, RefDocId, GrossWeightTotal). */
        Map<String, Object> st = shell.shellState();
        boolean wages = Boolean.TRUE.equals(st.get("wagesCompulsoryOnProduction"))
                     && Boolean.TRUE.equals(st.get("wagesActiveForConsumption"));
        out.put("openWages", wages);
        if (wages && id != null) repo.contractorWagesRemoveByReferenceIds(org, comp, 181, id);
        return out;
    }

    /** "#,##0.##" then Conversion.ToDouble of that text. */
    private static double fmt2(double v) { return ProductionConsumptionRepository.round(v, 2); }

    /**
     * BLL InvFoodProduction.Save + MakeVoucher + DAL SetData, DocumentTypeId 181 only.
     * Runs inside the transaction; any exception rolls every write back (the DAL's Rollback).
     */
    private Integer post(Map<String, Object> h, List<Map<String, Object>> details, int org, int comp) {
        /* ---- BLL Save (:4994) */
        boolean jobOrderCreatewithoutRates = bool(repo.configFromAllocation(org, comp, "JobOrderCreatewithoutRates"));
        int refDocumentTypeId = 0;
        if (bool(repo.configFromAllocation(org, comp, "InventoryFinancialsEffectsInActive"))
                || (jobOrderCreatewithoutRates && intOf(h.get("DocumentTypeId")) == 112)) {
            refDocumentTypeId = 1;
        }
        if (details.isEmpty()) throw new IllegalStateException("InvFoodProductionConsumptionDetailslist List Not Found.");
        Map<String, Object> voucher = refDocumentTypeId == 1 ? null : makeVoucher(h, details, org, comp);
        boolean isInsert = intOf(h.get("Id")) == 0;
        int actionId = isInsert ? 1 : 2;
        h.put("ActionId", actionId);
        if (isInsert) h.put("ModifyUser", 0); else h.put("EntryUser", 0);

        /* ---- DAL SetData */
        boolean l7 = jobOrderCreatewithoutRates && intOf(h.get("DocumentTypeId")) == 112;   /* false for 181 */
        int l0 = repo.setProc(isInsert ? "Sp_InvFoodProduction_Insert" : "Sp_InvFoodProduction_Update", headerParams(h));
        if (l0 > 0) h.put("Id", l0); else l0 = intOf(h.get("Id"));
        int headerId = intOf(h.get("Id"));

        /* invFoodProductionDetails is an empty list for a consumption - nothing to insert. */
        int lineId = 0;
        boolean l11 = false;
        for (Map<String, Object> d : details) {
            if (intOf(d.get("RefDocumentTypeId")) > 0 && intOf(d.get("RefDocNoId")) > 0 && intOf(d.get("RefDocSubIdNo")) > 0) l11 = true;
            lineId++;
            d.put("LineId", lineId);
            d.put("InvFoodProductionId", headerId);
            int did = repo.setProc("USP_InvFoodProductionConsumptionDetail_Insert", consumptionDetailParams(d));
            d.put("Id", did);
        }

        boolean fifo = repo.erpFeature(org, comp, 5);
        int modifyUser = intOf(h.get("ModifyUser"));
        if (fifo && !l11) {
            List<Map<String, Object>> itemGl = repo.itemGlIdsAndItemName(org, comp);
            List<Map<String, Object>> jobLotGl = repo.jobLotGlIdsAndName(org, comp);
            List<Map<String, Object>> evaluations = new ArrayList<>();
            for (Map<String, Object> item : new ArrayList<>(details)) {
                Map<String, Object> gl = firstById(itemGl, intOf(item.get("ItemId")));
                if (gl == null) continue;
                int rpDocType = 0, rpId = 0;
                if (modifyUser > 0) { rpDocType = intOf(h.get("DocumentTypeId")); rpId = headerId; }
                Map<String, Object> jl = firstById(jobLotGl, intOf(item.get("JobLotId")));
                boolean jobLotHasAccount = jl != null && intOf(col(jl, "AccountId")) > 0;
                List<Map<String, Object>> taken = fifoImplementation(org, comp, item, (LocalDateTime) h.get("DocDate"),
                        rpDocType, rpId, evaluations);
                for (Map<String, Object> se : taken) {
                    if (!jobLotHasAccount) {
                        if (voucher == null) throw new NullPointerException("Object reference not set to an instance of an object.");
                        String comments = "ItemQty: " + netG(dbl(se.get("QtyOut"))) + "   " + str(col(gl, "ItemName"))
                                + "  Net Weight: " + netG(dbl(se.get("StockWeightOut"))) + "  CGS Rate:" + netG(dbl(se.get("CgsRate")));
                        @SuppressWarnings("unchecked")
                        List<Map<String, Object>> vds = (List<Map<String, Object>>) voucher.get("voucherDetailList");
                        vds.add(fifoVoucherLine(item, gl, se, h, comments, true));
                        vds.add(fifoVoucherLine(item, gl, se, h, comments, false));
                    }
                    evaluations.add(se);
                }
            }
            if (!evaluations.isEmpty()) {
                if (modifyUser > 0) {
                    Map<String, Object> p = ProductionConsumptionRepository.params();
                    p.put("@OrganizationId", org);
                    p.put("@CompanyId", comp);
                    p.put("@RefDocumentTypeId", h.get("DocumentTypeId"));
                    p.put("@RefDocIdNo", headerId);
                    repo.command("[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", p);
                }
                for (Map<String, Object> se : evaluations) {
                    se.put("OrganizationId", org);
                    se.put("CompanyId", comp);
                    se.put("DocDate", h.get("DocDate"));
                    se.put("DocCodeNo", h.get("DocCode"));
                    se.put("SupplierCustomerId", 0);
                    se.put("BranchesId", h.get("BranchId"));
                    se.put("RefDocumentTypeId", h.get("DocumentTypeId"));
                    se.put("EntryUser", h.get("EntryUser"));
                    se.put("ModifyUser", h.get("ModifyUser"));
                    se.put("CalcType", "Weight");
                    /* <SetData>b__5 - the consumption row the evaluation was taken for (same LineId). */
                    Map<String, Object> d = null;
                    for (Map<String, Object> x : details) if (intOf(x.get("LineId")) == intOf(se.get("LineId"))) { d = x; break; }
                    if (d != null) {
                        double eq = repo.equivalentByItemAndSchedule(org, comp, intOf(se.get("ItemId")), intOf(d.get("RateUOMId")));
                        if (dbl(se.get("BillWeightOut")) > 0d && dbl(d.get("Rate")) > 0d && eq > 0d) {
                            se.put("AmountOut", dbl(se.get("BillWeightOut")) / eq * dbl(d.get("Rate")));
                        }
                        se.put("ItemRate", dbl(d.get("Rate")));
                        se.put("RefDocIdNo", d.get("InvFoodProductionId"));
                        se.put("RefDocSubIdNo", d.get("Id"));
                    }
                    repo.setProc("USP_InventoryStockEvalautionDetail_Insert", stockEvaluationInsertParams(se));
                }
            }
        } else if (actionId == 1) {
            Map<String, Object> se = stockEvaluationDefaults();
            se.put("OrganizationId", org);
            se.put("CompanyId", comp);
            se.put("RefDocumentTypeId", h.get("DocumentTypeId"));
            se.put("RefDocIdNo", headerId);
            repo.setProc("Sp_InventoryStockEvalautionDetail_Update", stockEvaluationUpdateParams(se));
        }

        if (!l7) {
            repo.setProc("Sp_InventoryTransactions_GetALLMethod", inventoryTransactionParams(org, comp,
                    intOf(h.get("DocumentTypeId")), l0));
        }

        if (actionId == 2) {
            Map<String, Object> p = ProductionConsumptionRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@DocumentTypeId", h.get("DocumentTypeId"));
            p.put("@InvProductionJobOrderId", h.get("InvJobOrderId"));
            p.put("@PlantId", h.get("PlantId"));
            repo.command("USP_ProductionInPutAndOutPutWeightValidation", p);
        }
        for (Map<String, Object> d : details) {
            Map<String, Object> p = ProductionConsumptionRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@DocumentTypeId", h.get("DocumentTypeId"));
            p.put("@DocDate", h.get("DocDate"));
            p.put("@ItemId", d.get("ItemId"));
            p.put("@WarehouseId", d.get("WarehouseId"));
            p.put("@JobLotId", d.get("JobLotId"));
            p.put("@CropYear", d.get("CropBatch"));
            p.put("@InvPackingTypeId", d.get("PackingtypeId"));
            p.put("@PackUomId", d.get("ItemUomId"));
            p.put("@RefDocumentTypeId", d.get("RefDocumentTypeId"));
            p.put("@RefDocNoId", d.get("RefDocNoId"));
            p.put("@RefDocSubIdNo", d.get("RefDocSubIdNo"));
            p.put("@NetWeight", d.get("Weight"));
            repo.command("USP_InventoryValidation", p);
        }
        /* OutputDetailRowsRemoveIds is never set by the consumption form - nothing to delete. */

        if (refDocumentTypeId != 1 && voucher != null) {
            int existing = repo.voucherHeadIdForDocument(org, comp, intOf(h.get("DocumentTypeId")), headerId);
            if (existing != 0) voucher.put("Id", existing);
            voucher.put("DocumentTypeSrNo", headerId);
            int l2 = repo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", voucherHeadParams(voucher));
            if (l2 > 0) voucher.put("Id", l2);
            @SuppressWarnings("unchecked")
            List<Map<String, Object>> vds = (List<Map<String, Object>>) voucher.get("voucherDetailList");
            if (!vds.isEmpty()) {
                for (Map<String, Object> vd : vds) {
                    vd.put("VoucherHeadId", voucher.get("Id"));
                    vd.put("BranchesId", h.get("BranchId"));
                    /* <SetData>b__6 searches invFoodProductionDetails - empty for 181, never found. */
                    repo.setProc("Sp_VoucherDetail_Insert", voucherDetailParams(vd));
                }
                if (intOf(voucher.get("Id")) > 0) {
                    Map<String, Object> p = ProductionConsumptionRepository.params();
                    p.put("@OrganizationId", org);
                    p.put("@CompanyId", comp);
                    p.put("@Id", voucher.get("Id"));
                    repo.command("USP_VoucherBalanceCheck", p);
                }
            }
        }
        return l0;
    }

    private static Map<String, Object> firstById(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (intOf(col(r, "Id")) == id) return r;
        return null;
    }

    /**
     * BLL InvFoodProduction.MakeVoucher (:4080), the consumption half. A row posts when the FIFO
     * feature is off, or when it came from the loader (RefDocumentTypeId and RefDocNoId set).
     */
    private Map<String, Object> makeVoucher(Map<String, Object> h, List<Map<String, Object>> details, int org, int comp) {
        Map<String, Object> vh = voucherHeadDefaults();
        vh.put("DocumentTypeSrNo", h.get("Id"));
        vh.put("VoucherDate", h.get("DocDate"));
        vh.put("VoucherCode", h.get("DocCode"));
        vh.put("DocumentTypeId", h.get("DocumentTypeId"));
        vh.put("BaseDocumentTypeId", h.get("BaseDocumentTypeId"));
        vh.put("RefAccountId", h.get("WIPAccountId"));
        /* AgainstAccountId = WIPItemId - an item id in an account column, as the BLL writes it. */
        vh.put("AgainstAccountId", h.get("WIPItemId"));
        LocalDateTime now = LocalDateTime.now().withNano(0);
        vh.put("EntryDate", now);
        vh.put("ModifyDate", now);
        vh.put("EntryUser", h.get("EntryUser"));
        vh.put("ModifyUser", h.get("ModifyUser"));
        vh.put("OrganizationId", h.get("OrganizationId"));
        vh.put("CompanyId", h.get("CompanyId"));
        vh.put("FinancialYearId", h.get("FinancialYearId"));
        vh.put("BranchId", h.get("BranchId"));
        List<Map<String, Object>> vds = new ArrayList<>();
        vh.put("voucherDetailList", vds);
        List<Map<String, Object>> itemGl = repo.itemGlIdsAndItemName(org, comp);
        boolean fifo = repo.erpFeature(org, comp, 5);
        double total = 0d;
        for (Map<String, Object> item : details) {
            boolean post = (!fifo) || (intOf(item.get("RefDocumentTypeId")) > 0 && intOf(item.get("RefDocNoId")) > 0);
            if (!post) continue;
            if (itemGl.isEmpty()) throw new IllegalStateException("Item GL Account Not Found");
            Map<String, Object> gl = firstById(itemGl, intOf(item.get("ItemId")));
            if (gl == null) continue;
            String remarks = str(item.get("Remarks"));
            String comments = str(h.get("InvJobOrderNo")) + " " + (remarks.trim().isEmpty() ? "" : remarks + " ")
                    + " ItemName: " + str(col(gl, "ItemName")) + " Qty " + netG(dbl(item.get("Qty")))
                    + " Weight " + netG(dbl(item.get("Weight"))) + " Rate " + netG(dbl(item.get("Rate")));
            double amount = dbl(item.get("Amount"));
            Map<String, Object> dr = voucherDetailDefaults();
            dr.put("AccountId", intOf(h.get("WIPAccountId")));
            dr.put("AgainstAccountId", intOf(col(gl, "PurchaseGLAC")));
            dr.put("Comments", comments);
            dr.put("DebitAmount", amount);
            dr.put("QtyIn", dbl(item.get("Qty")));
            dr.put("WeightIn", dbl(item.get("Weight")));
            dr.put("ItemAmount", amount);
            dr.put("OrderNo", intOf(item.get("InvProductionJobOrderId")));
            dr.put("BranchesId", h.get("BranchId"));
            total += amount;
            vds.add(dr);
            Map<String, Object> cr = voucherDetailDefaults();
            cr.put("AccountId", intOf(col(gl, "PurchaseGLAC")));
            cr.put("AgainstAccountId", intOf(h.get("WIPAccountId")));
            cr.put("Comments", comments);
            cr.put("CreditAmount", amount);
            cr.put("QtyOut", dbl(item.get("Qty")));
            cr.put("WeightOut", dbl(item.get("Weight")));
            cr.put("ItemId", intOf(item.get("ItemId")));
            cr.put("ItemAmount", amount);
            cr.put("OrderNo", intOf(item.get("InvProductionJobOrderId")));
            cr.put("BranchesId", h.get("BranchId"));
            vds.add(cr);
        }
        vh.put("VoucherAmount", total);
        return vh;
    }

    /** The two FIFO cost lines DAL SetData adds per evaluation (IL_0e32 - IL_11a0). */
    private static Map<String, Object> fifoVoucherLine(Map<String, Object> item, Map<String, Object> gl,
                                                       Map<String, Object> se, Map<String, Object> h,
                                                       String comments, boolean debit) {
        Map<String, Object> v = voucherDetailDefaults();
        v.put("LineId", item.get("LineId"));
        if (debit) {
            v.put("AccountId", intOf(h.get("WIPAccountId")));
            v.put("AgainstAccountId", intOf(col(gl, "PurchaseGLAC")));
            v.put("DebitAmount", dbl(se.get("CgsAmount")));
        } else {
            v.put("AccountId", intOf(col(gl, "PurchaseGLAC")));
            v.put("AgainstAccountId", intOf(h.get("WIPAccountId")));
            v.put("CreditAmount", dbl(se.get("CgsAmount")));
        }
        v.put("Comments", comments);
        v.put("DocumentTypeIdRef", intOf(item.get("RefDocumentTypeId")));
        v.put("InvoiceNoRefId", intOf(item.get("RefDocNoId")));
        v.put("RefInvoiceNo", String.valueOf(intOf(item.get("RefDocSubIdNo"))));
        v.put("ItemId", intOf(item.get("ItemId")));
        v.put("QtyOut", dbl(se.get("QtyOut")));
        v.put("WeightOut", dbl(se.get("StockWeightOut")));
        v.put("ItemCgsRate", dbl(se.get("CgsRate")));
        v.put("RateCut", 0d);
        v.put("RateCutAmount", 0d);
        v.put("ItemAmount", dbl(se.get("CgsAmount")));
        v.put("JobLotId", intOf(item.get("JobLotId")));
        v.put("BranchesId", h.get("BranchId"));
        v.put("OrderNo", item.get("InvProductionJobOrderId"));
        return v;
    }

    /**
     * DAL CommonServices.FIFOImplemention (:1759) for one consumption row: the reservations already
     * taken by earlier rows go as @FIFOXML, USP_GetStockByFifoMethod answers, and the row's weight is
     * taken from the oldest layers first.
     */
    private List<Map<String, Object>> fifoImplementation(int org, int comp, Map<String, Object> item,
                                                         LocalDateTime docDate, int docType, int id,
                                                         List<Map<String, Object>> reserved) {
        String xml = null;
        if (!reserved.isEmpty()) {
            StringBuilder sb = new StringBuilder("<ArrayOfFIFOStockEvaluation xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            for (Map<String, Object> r : reserved) {
                sb.append("<FIFOStockEvaluation>")
                  .append("<RefDocumentTypeId>").append(intOf(r.get("RefRefDocumentTypeId"))).append("</RefDocumentTypeId>")
                  .append("<RefDocIdNo>").append(intOf(r.get("RefRefDocIdNo"))).append("</RefDocIdNo>")
                  .append("<RefDocSubIdNo>").append(intOf(r.get("RefRefDocSubIdNo"))).append("</RefDocSubIdNo>")
                  .append("<ReserveQty>").append(xmlDouble(dbl(r.get("QtyOut")))).append("</ReserveQty>")
                  .append("<ReserveWeight>").append(xmlDouble(dbl(r.get("StockWeightOut")))).append("</ReserveWeight>")
                  .append("</FIFOStockEvaluation>");
            }
            xml = sb.append("</ArrayOfFIFOStockEvaluation>").toString();
        }
        String itemName = "";   /* ReportsParameters.ItemName is never set by SetData - null in every message */
        List<Map<String, Object>> rows = repo.stockByFifo(org, comp, intOf(item.get("ItemId")), docDate,
                intOf(item.get("ItemUomId")), intOf(item.get("WarehouseId")), 0, intOf(item.get("JobLotId")),
                intOf(item.get("PackingtypeId")), str(item.get("CropBatch")), docType, id, xml, false);
        if (rows.isEmpty()) throw new IllegalStateException("Stock Not Found this Item " + itemName + " against FIFO Method .....");
        double available = 0d;
        for (Map<String, Object> r : rows) available += dbl(col(r, "NetBalWeight"));
        double itemQty = dbl(item.get("Qty")), netWeight = dbl(item.get("Weight"));
        if (netWeight > ProductionConsumptionRepository.round(available, 2)) {
            throw new IllegalStateException("Weight available is " + netG(available) + " and row Weight is "
                    + netG(netWeight) + " this item " + itemName + " against FIFO....");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        double l12 = netWeight, l13 = itemQty, l15 = 0d, l17 = 0d;
        for (Map<String, Object> r : rows) {
            if (!(dbl(col(r, "AvgRate")) > 0d)) throw new IllegalStateException("Rate Not Found this Item " + itemName + " against FIFO Method");
            int rateUomId = intOf(col(r, "RateUomId"));
            if (rateUomId == 0) throw new IllegalStateException("RateUomId not found  this " + itemName + " against FIFO Method");
            double l16 = dbl(col(r, "NetBalWeight"));
            double l14 = dbl(col(r, "NetBalQty"));
            double eq = repo.equivalentByItemAndSchedule(org, comp, intOf(item.get("ItemId")), rateUomId);
            if (eq == 0d) throw new IllegalStateException("RateUom Not Found");
            Map<String, Object> se = stockEvaluationDefaults();
            if (!(l16 > l12 - l17)) {
                l17 += l16;
                l15 += l14;
                fillEvaluation(se, r, item, rateUomId);
                se.put("QtyOut", l14);
                se.put("BillWeightOut", l16);
                se.put("StockWeightOut", l16);
                se.put("CgsRate", dbl(col(r, "AvgRate")) * eq);
                se.put("CgsAmount", dbl(se.get("BillWeightOut")) / eq * dbl(se.get("CgsRate")));
                out.add(se);
            } else if (!(l16 < l12 - l17)) {
                fillEvaluation(se, r, item, rateUomId);
                se.put("QtyOut", l13 - l15);
                se.put("BillWeightOut", l12 - l17);
                se.put("StockWeightOut", l12 - l17);
                se.put("CgsRate", dbl(col(r, "AvgRate")) * eq);
                se.put("CgsAmount", dbl(se.get("BillWeightOut")) / eq * dbl(se.get("CgsRate")));
                l15 += dbl(se.get("QtyOut"));
                l17 += dbl(se.get("BillWeightOut"));
                out.add(se);
            }
            if (netWeight == l17) break;
        }
        return out;
    }

    private static void fillEvaluation(Map<String, Object> se, Map<String, Object> r, Map<String, Object> item, int rateUomId) {
        se.put("Id", intOf(col(r, "Id")));
        se.put("LineId", item.get("LineId"));
        se.put("ItemId", item.get("ItemId"));
        se.put("WarehouseId", item.get("WarehouseId"));
        se.put("RateUom", rateUomId);
        se.put("JobLotId", intOf(item.get("JobLotId")));
        se.put("InvPackingTypeId", intOf(item.get("PackingtypeId")));
        se.put("ItemUom", intOf(item.get("ItemUomId")));
        se.put("CropBatch", str(item.get("CropBatch")));
        se.put("RefRefDocumentTypeId", intOf(col(r, "RefDocumentTypeId")));
        se.put("RefRefDocIdNo", intOf(col(r, "RefDocIdNo")));
        se.put("RefRefDocSubIdNo", intOf(col(r, "RefDocSubIdNo")));
    }

    private static String xmlDouble(double d) {
        /* XmlSerializer writes doubles in the "R" round-trip form. */
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }

    // ------------------------------------------------------------------ SetProc parameter sets

    /** Sp_InvFoodProduction_Insert / _Update parameters that are InvFoodProduction properties. */
    private static Map<String, Object> headerParams(Map<String, Object> h) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Id", intOf(h.get("Id")));
        p.put("@DocDate", h.get("DocDate"));
        p.put("@DocCode", intOf(h.get("DocCode")));
        p.put("@DocumentTypeId", intOf(h.get("DocumentTypeId")));
        p.put("@WIPAccountId", intOf(h.get("WIPAccountId")));
        p.put("@WIPItemId", intOf(h.get("WIPItemId")));
        p.put("@MainRemarks", h.get("MainRemarks"));
        p.put("@IsApproved", false);
        p.put("@EntryDate", h.get("EntryDate"));
        p.put("@EntryUser", intOf(h.get("EntryUser")));
        p.put("@ModifyDate", h.get("ModifyDate"));
        p.put("@ModifyUser", intOf(h.get("ModifyUser")));
        p.put("@OrganizationId", intOf(h.get("OrganizationId")));
        p.put("@CompanyId", intOf(h.get("CompanyId")));
        p.put("@InvFoodProductionPlanId", intOf(h.get("InvFoodProductionPlanId")));
        p.put("@InvJobOrderId", intOf(h.get("InvJobOrderId")));
        p.put("@InvJobOrderNo", h.get("InvJobOrderNo"));
        p.put("@EntryType", h.get("EntryType"));
        p.put("@BranchId", intOf(h.get("BranchId")));
        p.put("@ProjectId", 0);
        p.put("@ActionId", intOf(h.get("ActionId")));
        p.put("@FinancialYearId", intOf(h.get("FinancialYearId")));
        p.put("@PlantId", intOf(h.get("PlantId")));
        p.put("@EBDepartmentId", intOf(h.get("EBDepartmentId")));
        p.put("@ContractScheduleId", 0);
        p.put("@WipWareHouseId", 0);
        p.put("@BaseDocumentTypeId", intOf(h.get("BaseDocumentTypeId")));
        p.put("@StockHoldForLabApproval", false);
        return p;
    }

    /** USP_InvFoodProductionConsumptionDetail_Insert - every declared parameter is a model property. */
    private static Map<String, Object> consumptionDetailParams(Map<String, Object> d) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Amount", dbl(d.get("Amount")));
        p.put("@Qty", dbl(d.get("Qty")));
        p.put("@Rate", dbl(d.get("Rate")));
        p.put("@Weight", dbl(d.get("Weight")));
        p.put("@Id", intOf(d.get("Id")));
        p.put("@InvFoodProductionId", intOf(d.get("InvFoodProductionId")));
        p.put("@InvProductionJobOrderId", intOf(d.get("InvProductionJobOrderId")));
        p.put("@InvProductionJobOrderNo", 0);
        p.put("@ItemId", intOf(d.get("ItemId")));
        p.put("@ItemUomId", intOf(d.get("ItemUomId")));
        p.put("@JobLotId", intOf(d.get("JobLotId")));
        p.put("@PackingtypeId", intOf(d.get("PackingtypeId")));
        p.put("@RateUOMId", intOf(d.get("RateUOMId")));
        p.put("@RefDocumentTypeId", intOf(d.get("RefDocumentTypeId")));
        p.put("@RefDocNoId", intOf(d.get("RefDocNoId")));
        p.put("@RefDocSubIdNo", intOf(d.get("RefDocSubIdNo")));
        p.put("@WarehouseId", intOf(d.get("WarehouseId")));
        p.put("@LineId", intOf(d.get("LineId")));
        p.put("@CropBatch", d.get("CropBatch"));
        p.put("@EntryType", d.get("EntryType"));
        p.put("@Remarks", d.get("Remarks"));
        return p;
    }

    private static Map<String, Object> stockEvaluationDefaults() {
        Map<String, Object> s = new LinkedHashMap<>();
        for (String k : new String[]{"AmountIn", "AmountOut", "BillWeightIn", "BillWeightOut", "CgsRate", "ExpenseAmountIn",
                "ItemRate", "QtyIn", "QtyOut", "StockWeightIn", "StockWeightOut", "CgsAmount"}) s.put(k, 0d);
        for (String k : new String[]{"BranchesId", "CompanyId", "DocCodeNo", "GpNoDcNo", "Id", "InvPackingTypeId", "ItemId",
                "ItemUom", "JobLotId", "OrderNo", "OrganizationId", "PrdJobOrderNo", "ProjectsId", "RateUom", "RefDocIdNo",
                "RefDocSubIdNo", "RefDocumentTypeId", "OtherDocumentTypeId", "OtherDocNoId", "OtherSubDocNoId",
                "SupplierCustomerId", "WarehouseId", "RefWarehouseId", "CityId", "LineId", "RefRefDocumentTypeId",
                "RefRefDocIdNo", "RefRefDocSubIdNo", "EntryUser", "ModifyUser", "InvoiceId", "InvoiceDetailId",
                "VarientId", "ItemConditionId"}) s.put(k, 0);
        s.put("IsApproved", false);
        s.put("DocDate", null);           /* DateTime? - not sent when null */
        return s;
    }

    private static final String[] SE_INSERT = {"Id", "RefDocumentTypeId", "RefDocIdNo", "RefRefDocumentTypeId",
            "RefRefDocIdNo", "DocCodeNo", "DocDate", "SupplierCustomerId", "RefRefDocSubIdNo", "RefDocSubIdNo", "OrderNo",
            "WarehouseId", "PrdJobOrderNo", "VehicleNo", "GpNoDcNo", "TranRemarks", "ItemId", "ItemUom", "CropBatch",
            "JobLotId", "InvPackingTypeId", "QtyIn", "QtyOut", "BillWeightIn", "BillWeightOut", "StockWeightIn",
            "StockWeightOut", "CalcType", "ItemRate", "RateUom", "AmountIn", "ExpenseAmountIn", "AmountOut", "CgsRate",
            "CgsAmount", "IsApproved", "OrganizationId", "CompanyId", "BranchesId", "ProjectsId", "EntryUser",
            "ModifyUser", "LineId", "CityId", "OtherDocumentTypeId", "OtherDocNoId", "OtherSubDocNoId", "InvoiceId",
            "InvoiceDetailId", "VarientId", "RefWarehouseId", "BiltyNo", "ItemConditionId"};

    private static final String[] SE_UPDATE = {"Id", "RefDocumentTypeId", "RefDocIdNo", "DocCodeNo", "DocDate",
            "SupplierCustomerId", "RefDocSubIdNo", "OrderNo", "WarehouseId", "PrdJobOrderNo", "VehicleNo", "GpNoDcNo",
            "TranRemarks", "ItemId", "ItemUom", "CropBatch", "JobLotId", "InvPackingTypeId", "QtyIn", "QtyOut",
            "BillWeightIn", "BillWeightOut", "StockWeightIn", "StockWeightOut", "CalcType", "ItemRate", "RateUom",
            "AmountIn", "ExpenseAmountIn", "AmountOut", "CgsRate", "IsApproved", "OrganizationId", "CompanyId",
            "BranchesId", "ProjectsId", "CityId", "LineId", "RefRefDocumentTypeId", "RefRefDocIdNo", "RefRefDocSubIdNo",
            "EntryUser", "ModifyUser", "CgsAmount", "OtherDocumentTypeId", "OtherDocNoId", "OtherSubDocNoId", "InvoiceId",
            "InvoiceDetailId", "VarientId", "RefWarehouseId", "BiltyNo", "ItemConditionId"};

    private static Map<String, Object> pick(Map<String, Object> src, String[] names) {
        Map<String, Object> p = new LinkedHashMap<>();
        for (String n : names) p.put("@" + n, src.get(n));
        return p;
    }

    /** USP_InventoryStockEvalautionDetail_Insert parameters that are InventoryStockEvalautionDetail properties. */
    private static Map<String, Object> stockEvaluationInsertParams(Map<String, Object> se) { return pick(se, SE_INSERT); }

    /** Sp_InventoryStockEvalautionDetail_Update parameters that are model properties (@hId is not). */
    private static Map<String, Object> stockEvaluationUpdateParams(Map<String, Object> se) { return pick(se, SE_UPDATE); }

    /**
     * Sp_InventoryTransactions_GetALLMethod with a new InventoryTransactions carrying only
     * OrganizationId, CompanyId, RefDocumentTypeId and RefDocIdNo (= the header proc's result);
     * every other model property goes with its type default and @Activity is not sent (SetProc
     * receives "" for it).
     */
    private static Map<String, Object> inventoryTransactionParams(int org, int comp, int refDocTypeId, int refDocIdNo) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Id", 0);
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@BranchesId", 0);
        p.put("@ProjectsId", 0);
        p.put("@RefDocumentTypeId", refDocTypeId);
        p.put("@RefDocIdNo", refDocIdNo);
        p.put("@DocCodeNo", 0);
        p.put("@SupplierCustomerId", 0);
        p.put("@IsApproved", false);
        p.put("@RefDocSubIdNo", 0);
        p.put("@WarehouseId", 0);
        p.put("@JobLotId", 0);
        p.put("@ItemId", 0);
        p.put("@ItemUom", 0);
        p.put("@InvPackingTypeId", 0);
        p.put("@QtyIn", 0d);
        p.put("@QtyOut", 0d);
        p.put("@BillWeightIn", 0d);
        p.put("@BillWeightOut", 0d);
        p.put("@StockWeightIn", 0d);
        p.put("@StockWeightOut", 0d);
        p.put("@ItemRate", 0d);
        p.put("@RateUom", 0);
        p.put("@AmountIn", 0d);
        p.put("@ExpenseAmountIn", 0d);
        p.put("@AmountOut", 0d);
        return p;
    }

    private static Map<String, Object> voucherHeadDefaults() {
        Map<String, Object> v = new LinkedHashMap<>();
        for (String k : new String[]{"IncludeWHT", "IsApproved", "PostState", "InclusiveTax", "IsUploaded", "CustomAccounts"}) v.put(k, false);
        for (String k : new String[]{"BillAmount", "ExchangeCurrencyRate", "FcAmount", "VoucherAmount", "CostCenterAmount",
                "AdvanceTaxAmount", "OtherChargesAmount"}) v.put(k, 0d);
        for (String k : new String[]{"AgainstAccountId", "BranchId", "CheqId", "ChequePrintId", "CompanyId", "DocumentTypeId",
                "DocumentTypeSrNo", "DueDays", "EntryUser", "FinancialYearId", "Id", "ModifyUser", "MultiCurrencyId",
                "OrganizationId", "PostUser", "ProjectId", "RefAccountId", "RefDocNoId", "VoucherCode", "ActionId",
                "RefDocumentTypeId", "FixedAssetEntryTypeId", "BaseDocumentTypeId", "AdvanceTaxAccountId",
                "OtherChargesAccountId"}) v.put(k, 0);
        return v;
    }

    private static final String[] VH = {"Id", "DocumentTypeId", "DocumentTypeSrNo", "RefDocNoId", "VoucherCode",
            "VoucherDate", "Remarks", "RemarksOtherLingo", "VoucherAmount", "FinancialYearId", "RefAccountId",
            "AgainstAccountId", "MultiCurrencyId", "ConversionFormula", "ExchangeCurrencyRate", "FcAmount", "CheqId",
            "ChequeNo", "ChequeDate", "PayTitle", "BankBranch", "ChequePrintId", "Source", "DrCrNoteType", "IsApproved",
            "EntryDate", "EntryUser", "ModifyDate", "ModifyUser", "PostDate", "PostUser", "PostState", "OrganizationId",
            "CompanyId", "IncludeWHT", "BranchId", "ProjectId", "ManualBillNo", "BillAmount", "DueDate", "DueDays",
            "ActionId", "CostCenterAmount", "AttachmentsValues", "RefDocumentTypeId", "CustomAttachmentsValues",
            "CustomAccounts", "BaseDocumentTypeId", "AdvanceTaxAccountId", "AdvanceTaxAmount", "OtherChargesAccountId",
            "OtherChargesAmount", "IsUploaded", "InclusiveTax", "FixedAssetEntryTypeId"};

    /** Sp_VoucherHead_Insert / _Update parameters that are VoucherHead properties. */
    private static Map<String, Object> voucherHeadParams(Map<String, Object> v) { return pick(v, VH); }

    private static Map<String, Object> voucherDetailDefaults() {
        Map<String, Object> v = new LinkedHashMap<>();
        for (String k : new String[]{"Adjustment", "AdvanceAmount", "Commission", "CreditAmount", "DCurrencyAmount",
                "DebitAmount", "TaxAmount", "DExchangeCurrencyRate", "Expenses", "ExTax", "Freight", "ItemAmount", "ItemRate",
                "Journal", "QtyIn", "QtyOut", "RateCut", "RateCutAmount", "SaleTax", "TaxesTotalAmount", "TaxPrcnt",
                "WeightIn", "WeightOut", "WhtHolding", "ItemCgsRate", "TotalDebitAmount", "TotalCreditAmount",
                "ThirdCurrencyFcyExchangeRate", "ThirdCurrencyHcyExchangeRate", "ThirdCurrencyAmount",
                "ThirdCurrencyReceiverExchangeRate", "ThirdCurrencyReceiverFcyAmount", "SBRTaxAmount", "DiscountPercent",
                "DiscountAmount", "BaseFcyExchangeRate", "BaseFcyAmount"}) v.put(k, 0d);
        for (String k : new String[]{"ThirdCurrencyId", "AccountId", "AgainstAccountId", "DMultiCurrencyId", "DocumentTypeIdRef",
                "GpNo", "Id", "InvoiceNoRefId", "ItemId", "JobLotId", "OrderNo", "SupplierCustomerId", "EmployeeId",
                "SubsidiaryTypeId", "SubsidiaryAccountId", "SubsidiaryAgainstTypeId", "SubsidiaryAgainstAccountId",
                "TaxTypeId", "ActionId", "VoucherHeadId", "RefDocumentTypeId", "RefDocNoId", "RefDocNoDetailId",
                "RefDocSubIdNo", "LineId", "InstrumentTypeId", "SubNo", "SortNo", "IsCGS", "PaymentTypeId", "ChequeTypeId",
                "BranchesId", "CostCenterId", "ReferenceAccountId", "LocationTypeId", "BaseFcyId"}) v.put(k, 0);
        return v;
    }

    private static final String[] VD = {"Id", "VoucherHeadId", "AccountId", "AgainstAccountId", "Comments",
            "CommentsOtherLingo", "DebitAmount", "CreditAmount", "JobLotId", "RefInvoiceNo", "TaxesTotalAmount",
            "TaxesRemarks", "IsTaxable", "TaxTypeId", "TaxPrcnt", "DCheqDate", "CheqNoDetail", "DocumentTypeIdRef",
            "InvoiceNoRefId", "ItemId", "OrderNo", "GpNo", "VehicleNo", "GpDate", "QtyIn", "QtyOut", "WeightIn",
            "WeightOut", "SupplierCustomerId", "ItemRate", "RateCut", "RateCutAmount", "ItemAmount", "Expenses", "Freight",
            "Journal", "Commission", "DMultiCurrencyId", "DConversionFormula", "DExchangeCurrencyRate", "DCurrencyAmount",
            "PaymentType", "AdvanceAmount", "WhtHolding", "SaleTax", "ExTax", "Adjustment", "ActionId", "LineId",
            "RefDocumentTypeId", "RefDocNoId", "RefDocNoDetailId", "RefDocSubIdNo", "ItemCgsRate", "TotalCreditAmount",
            "TotalDebitAmount", "SubNo", "PayeeTitle", "SubsidiaryTypeId", "EmployeeId", "SubsidiaryAccountId", "IsCGS",
            "SubsidiaryAgainstTypeId", "SubsidiaryAgainstAccountId", "ThirdCurrencyId", "ThirdCurrencyFcyExchangeRate",
            "ThirdCurrencyHcyExchangeRate", "ThirdCurrencyAmount", "ThirdCurrencyReceiverExchangeRate",
            "ThirdCurrencyReceiverFcyAmount", "SortNo", "InstrumentTypeId", "ChequeTypeId", "BranchesId", "CostCenterId",
            "SBRTaxAmount", "DiscountPercent", "DiscountAmount", "ReferenceAccountId", "LocationTypeId", "PaymentTypeId",
            "TaxAmount", "BaseFcyId", "BaseFcyExchangeRate", "BaseFcyAmount"};

    /** Sp_VoucherDetail_Insert parameters that are VoucherDetail properties. */
    private static Map<String, Object> voucherDetailParams(Map<String, Object> v) { return pick(v, VD); }

    // ===================================================================================== helpers

    /** "yyyy-MM-dd" or "yyyy-MM-ddTHH:mm[:ss]" from the page; empty means "not set". */
    static LocalDateTime dt(String s) {
        if (s == null) return null;
        s = s.trim();
        if (s.isEmpty()) return null;
        try {
            if (s.length() <= 10) return LocalDate.parse(s).atTime(LocalTime.MIDNIGHT);
            return LocalDateTime.parse(s.replace(' ', 'T'));
        } catch (Exception e) {
            throw new IllegalArgumentException("Invalid date: " + s);
        }
    }
}
