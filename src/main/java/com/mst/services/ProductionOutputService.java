package com.mst.services;

import com.mst.repositories.ProductionOutputRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ProductionOutputRepository.b;
import static com.mst.repositories.ProductionOutputRepository.col;
import static com.mst.repositories.ProductionOutputRepository.d;
import static com.mst.repositories.ProductionOutputRepository.i;
import static com.mst.repositories.ProductionOutputRepository.s;

/**
 * Screen 280, tab "Output" - frmProductionOutput.cs (DocumentTypeId 112).
 *
 * The page runs the form's events; this service owns what must not come from the browser:
 * tenancy, the rights object, the configuration switches, the job order's WIP account / WIP item /
 * WIP warehouse, and the whole save chain (OutPutInsert's model build and row loop -> BLL Save ->
 * MakeVoucher -> DAL SetData). Line numbers are frmProductionOutput.cs's.
 */
@Service
public class ProductionOutputService {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionOutputService.class);

    public static final int DOC_TYPE = ProductionOutputRepository.DOC_TYPE_OUTPUT;
    /** frmFoodProduction_Load:428-435 - the shell's screen name when the user can view it, else base.Name. */
    public static final String SCREEN_SHELL = "FoodProductionWithValues";
    public static final String SCREEN_FORM = "frmProductionOutput";

    @Autowired private ProductionOutputRepository repo;
    @Autowired private CurrentUserContext user;

    private int org()  { return user.currentOrganizationId(); }
    private int comp() { return user.currentCompanyId(); }
    private int branch() { return user.currentBranchId(); }
    private int year() { return user.currentFinancialYearId(); }

    // ============================================================================ Load

    /** frmFoodProduction_Load:399 - the switches, the rights object and every list Load binds. */
    public Map<String, Object> load() {
        Map<String, Object> out = new LinkedHashMap<>();
        List<String> errors = new ArrayList<>();
        LOAD_ERRORS.set(errors);
        Map<String, Object> cfg = switches();
        out.put("config", cfg);
        out.put("rights", rights(b(cfg.get("jobOrderCreatewithoutRates"))));
        out.put("docNo", safeSerial());
        out.put("financialYearStart", safe(() -> repo.financialYearStart(org(), comp(), year()), null));
        out.put("jobLots", safeRows(() -> repo.jobLots(org(), comp(), branch())));
        out.put("cropYears", safeRows(() -> repo.cropYears(org(), comp())));
        out.put("uoms", safeRows(() -> repo.uomSchedules(org(), comp())));
        out.put("packingTypes", safeRows(repo::packingTypes));
        out.put("warehouses", safeRows(() -> repo.warehouses(org(), comp(), branch())));
        out.put("jobOrders", safeRows(() -> repo.jobOrdersForProduction(org(), comp(), branch())));
        /* :500 - the all-items list only when items are NOT scheduled per job order. */
        if (!b(cfg.get("outputItemsByJobOrderRateSchedule"))) {
            out.put("allItems", safeRows(() -> repo.allItems(org(), comp())));
        }
        out.put("historyJobOrders", safeRows(() -> repo.historyJobOrders(org(), comp(), year(), branch())));
        /* Each fill on the desktop has its own try/catch -> MessageBox; one failing list does not
           stop the others, and its message is still shown. */
        out.put("errors", errors);
        LOAD_ERRORS.remove();
        return out;
    }

    /** The configuration switches Load (:406-451) and btnRefreshOutPut_Click (:940-943) read. */
    public Map<String, Object> switches() {
        Map<String, Object> c = new LinkedHashMap<>();
        boolean wagesOutput = false;
        try {
            for (Map<String, Object> r : repo.wagesRefDocumentStatuses()) {
                if (i(col(r, "RefDocumentTypeId")) == DOC_TYPE) { wagesOutput = b(col(r, "IsActive")); break; }
            }
        } catch (Exception e) { LOG.warn("wages status list", e); }
        c.put("wagesActiveForOutput", wagesOutput);
        c.put("saleCostingJobOrderWise", b(config("SaleCostingJobOrderWise")));
        c.put("outputItemsByJobOrderRateSchedule", b(config("OutputItemsByJobOrderRateSchedule")));
        c.put("jobOrderCreatewithoutRates", b(config("JobOrderCreatewithoutRates")));
        c.put("stockHoldForLabApprovalAutoChecked", b(config("StockHoldForLabApprovalAutoChecked")));
        /* :88 ProductionOutputLabApprovalRequired is declared and never assigned - always false,
           so the check box stays hidden and unchecked (:412, designer Visible=false). */
        c.put("productionOutputLabApprovalRequired", false);
        boolean wagesStatus = false, byProductRateEditable = false;
        try {
            for (Map<String, Object> r : repo.multipleConfigurations(org(), comp(),
                    "WagesCompulsoryOnProduction,ByProductRateEditableIsAllow")) {
                String n = s(col(r, "ConfigDescription"));
                if ("WagesCompulsoryOnProduction".equals(n)) wagesStatus = b(col(r, "ConfigKey"));
                if ("ByProductRateEditableIsAllow".equals(n)) byProductRateEditable = b(col(r, "ConfigKey"));
            }
        } catch (Exception e) { LOG.warn("multiple configurations", e); }
        c.put("wagesCompulsoryOnProduction", wagesStatus);
        c.put("byProductRateEditable", byProductRateEditable);
        boolean fifo = false;
        try { fifo = repo.erpFeature(org(), comp(), 5); } catch (Exception e) { LOG.warn("erp feature 5", e); }
        c.put("fifoCgs", fifo);
        /* clsGlobalVariables.DecimalRateFormate (CommonServices :4937): "#,#0." + n zeros, where
           0 means two and anything above four means none. */
        int n = i(config("Default NoofDecimal Points For Rate"));
        c.put("rateDecimals", n == 0 ? 2 : (n >= 1 && n <= 4 ? n : 0));
        return c;
    }

    /**
     * CommonServices.SetRightsValueInRightsObject (CommonServices.cs) for the screen Load picks.
     * Admin starts with Save/Update/Print; grant rows set the rest. Rate comes ONLY from a row,
     * and Load:445 then ANDs it with !JobOrderCreatewithoutRates.
     */
    public Map<String, Object> rights(boolean jobOrderCreatewithoutRates) {
        String role = user.currentRoleName();
        boolean admin = "Admin".equals(role);
        String screen = SCREEN_FORM;
        List<Map<String, Object>> rows = new ArrayList<>();
        try {
            List<Map<String, Object>> shell = repo.userRightsForScreen(user.currentUserId(), SCREEN_SHELL, role, comp());
            boolean canViewShell = admin;
            for (Map<String, Object> r : shell) {
                if ("View".equals(s(col(r, "RightName"))) && b(col(r, "Value"))) { canViewShell = true; break; }
            }
            if (canViewShell) { screen = SCREEN_SHELL; rows = shell; }
            else rows = repo.userRightsForScreen(user.currentUserId(), SCREEN_FORM, role, comp());
        } catch (Exception e) { LOG.warn("rights", e); }

        boolean save = admin, update = admin, print = admin, rate = false, gridPrint = false, gridExport = false;
        for (Map<String, Object> r : rows) {
            String n = s(col(r, "RightName"));
            boolean v = b(col(r, "Value"));
            switch (n) {
                case "Save":   save = admin || v; break;
                case "Update": update = admin || v; break;
                case "Print":  print = admin || v; break;
                case "Rate":   rate = v; break;
                case "Grid Print":  gridPrint = v; break;
                case "Grid Export": gridExport = v; break;
                default: break;
            }
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("screenName", screen);
        m.put("save", save);
        m.put("update", update);
        m.put("print", print);
        m.put("rateAndAmount", !jobOrderCreatewithoutRates && rate);
        m.put("gridPrint", gridPrint);
        m.put("gridExport", gridExport);
        m.put("saveLayout", true);
        return m;
    }

    private String config(String name) {
        try { return repo.configValue(org(), comp(), name); } catch (Exception e) { return ""; }
    }

    private int safeSerial() {
        try { return repo.serialNumber(org(), comp(), year(), branch()); } catch (Exception e) { LOG.warn("serial", e); return 0; }
    }

    private interface Q<T> { T get() throws Exception; }
    private static <T> T safe(Q<T> q, T dflt) { try { return q.get(); } catch (Exception e) { LOG.warn("lookup", e); return dflt; } }
    private static final ThreadLocal<List<String>> LOAD_ERRORS = new ThreadLocal<>();
    private static List<Map<String, Object>> safeRows(Q<List<Map<String, Object>>> q) {
        try { return q.get(); } catch (Exception e) {
            LOG.warn("load list", e);
            List<String> errs = LOAD_ERRORS.get();
            if (errs != null) errs.add(rootMessage(e));
            return new ArrayList<>();
        }
    }
    private static String rootMessage(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? e.toString() : t.getMessage();
    }

    // ============================================================================ lookups

    public int docNo() { return repo.serialNumber(org(), comp(), year(), branch()); }
    public List<Map<String, Object>> jobOrders() { return repo.jobOrdersForProduction(org(), comp(), branch()); }
    public List<Map<String, Object>> jobLots() { return repo.jobLots(org(), comp(), branch()); }
    public List<Map<String, Object>> cropYears() { return repo.cropYears(org(), comp()); }
    public List<Map<String, Object>> uoms() { return repo.uomSchedules(org(), comp()); }
    public List<Map<String, Object>> packingTypes() { return repo.packingTypes(); }
    public List<Map<String, Object>> warehouses() { return repo.warehouses(org(), comp(), branch()); }
    public List<Map<String, Object>> allItems() { return repo.allItems(org(), comp()); }
    public List<Map<String, Object>> itemsByJobOrder(int jobOrderId) { return repo.itemsByJobOrder(org(), comp(), jobOrderId); }
    public List<Map<String, Object>> plants(int jobOrderId) { return repo.plantsFromProductionInput(jobOrderId, branch()); }
    public List<Map<String, Object>> totals(int jobOrderId, int plantId) { return repo.inputTotals(org(), comp(), jobOrderId, plantId); }
    public List<Map<String, Object>> returnToGodown(int jobOrderId, int plantId) {
        return repo.productionInputByJobAndPlant(org(), comp(), jobOrderId, plantId);
    }
    public List<Map<String, Object>> exportContracts(int jobOrderId, int recId) {
        return repo.exportContracts(org(), comp(), year(), jobOrderId, recId);
    }
    public List<Map<String, Object>> lastItemRate(int jobOrderId, int typeId, int itemId, String docDate) {
        return repo.lastItemRate(org(), comp(), jobOrderId, itemId, typeId, dateTime(docDate));
    }
    public List<Map<String, Object>> avgRateReturnToGodown(int jobOrderId, int plantId, int itemId, String cropYear) {
        return repo.avgRateForReturnToGodown(org(), comp(), jobOrderId, plantId, itemId, cropYear);
    }
    public List<Map<String, Object>> historyJobOrders() { return repo.historyJobOrders(org(), comp(), year(), branch()); }
    public List<Map<String, Object>> summaryValues(int jobOrderId, double fgWeight) {
        return repo.summaryValues(org(), comp(), jobOrderId, fgWeight);
    }

    /** OutputGridHistory:2575 - only DocumentTypeId 112 rows are kept (:2630). */
    public List<Map<String, Object>> history(String from, String to, String docFrom, String docTo, int jobOrderId) {
        List<Map<String, Object>> all = repo.history(org(), comp(), year(), branch(), dateOnly(from), dateOnly(to),
                intText(docFrom), intText(docTo), jobOrderId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : all) if (i(col(r, "DocumentTypeId")) == DOC_TYPE) out.add(r);
        return out;
    }

    /** InvFoodProduction.GetByID + VoucherHeadIdGet(id, 112) - ReadByIdOutPut:2113 / OutPutDetailByHeaderId:2925. */
    public Map<String, Object> read(int id) {
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> h = repo.header(id);
        out.put("header", h);
        out.put("details", h == null ? new ArrayList<>() : repo.details(id));
        int vh = 0;
        try {
            List<Map<String, Object>> v = repo.voucherHeadRows(org(), comp(), DOC_TYPE, id);
            if (!v.isEmpty()) vh = i(col(v.get(0), "Id"));
        } catch (Exception e) { LOG.warn("voucher head id", e); }
        out.put("voucherHeadId", vh);
        return out;
    }

    // ---- LoadOutPutPendingforRates

    public List<Map<String, Object>> pendingRatesJobOrders() { return repo.pendingRatesJobOrders(org(), comp(), year()); }
    public List<Map<String, Object>> pendingRates(String from, String to, int jobOrderId, String entryType) {
        return repo.pendingRates(org(), comp(), branch(), dateOnly(from), dateOnly(to), jobOrderId, entryType);
    }

    // ---- frmPendingMoveOrderDocuments

    public List<Map<String, Object>> moveOrders(String from, String to, String docFrom, String docTo,
                                                String vehicleNo, String biltyNo) {
        return repo.moveOrdersPending(org(), comp(), year(), branch(), dateOnly(from), dateOnly(to),
                intText(docFrom), intText(docTo), vehicleNo, biltyNo);
    }

    // ============================================================================ SAVE

    /**
     * btnSaveOutPut_Click / btnUpdateOutPut_Click -> OutPutInsert:2212 -> InvFoodProduction.Save.
     *
     * The page has already run FormValidationOutPut, ValidateMoveOrder and the Yes/No question,
     * in that order, as the desktop does. The header checks are repeated here against the job
     * order the SERVER reads, because WIP account, WIP item, WIP warehouse and the job order's
     * document type are taken from that row, never from the request.
     */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = i(body.get("recId"));
        Map<String, Object> cfg = switches();
        Map<String, Object> rights = rights(b(cfg.get("jobOrderCreatewithoutRates")));
        if (recId > 0 ? !b(rights.get("update")) : !b(rights.get("save"))) {
            throw new IllegalStateException(recId > 0 ? "Update right is not allowed on this screen"
                                                     : "Save right is not allowed on this screen");
        }
        boolean rateRight = b(rights.get("rateAndAmount"));
        boolean fifo = b(cfg.get("fifoCgs"));

        /* FormValidationOutPut:975. */
        String docCode = s(body.get("docCode")).trim();
        if (docCode.isEmpty() || "0".equals(docCode)) throw new IllegalArgumentException("DocNo Field Required");
        int jobOrderId = i(body.get("jobOrderId"));
        Map<String, Object> job = null;
        for (Map<String, Object> r : repo.jobOrdersForProduction(org(), comp(), branch())) {
            if (i(col(r, "Id")) == jobOrderId && jobOrderId != 0) { job = r; break; }
        }
        if (job == null) throw new IllegalArgumentException("Job Order Number Field Required");
        int plantId = i(body.get("plantId"));
        if (plantId == 0) throw new IllegalArgumentException("Plant Name Field Is Required");
        if (i(col(job, "WorkInProcessAcId")) == 0)
            throw new IllegalArgumentException("WorkInProcess account not bind against selected job order");
        if (i(col(job, "WipItemId")) == 0)
            throw new IllegalArgumentException("WIP Item not bind against selected job order");

        /* :2238-2267 - the header model. */
        int userId = user.currentUserId();
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("@Id", recId);
        h.put("@DocDate", docDateTime(s(body.get("docDate"))));
        h.put("@DocCode", i(docCode));
        h.put("@DocumentTypeId", DOC_TYPE);
        h.put("@WIPAccountId", i(col(job, "WorkInProcessAcId")));
        h.put("@WIPItemId", i(col(job, "WipItemId")));
        h.put("@MainRemarks", s(body.get("remarks")));
        h.put("@IsApproved", false);
        h.put("@EntryDate", now);
        h.put("@EntryUser", userId);
        h.put("@ModifyDate", now);
        h.put("@ModifyUser", userId);
        h.put("@OrganizationId", org());
        h.put("@CompanyId", comp());
        h.put("@InvFoodProductionPlanId", 0);
        h.put("@InvJobOrderId", jobOrderId);
        h.put("@InvJobOrderNo", s(col(job, "PlanCode")).trim());
        h.put("@EntryType", "Output");
        h.put("@BranchId", branch());
        h.put("@ProjectId", 0);
        h.put("@ActionId", 0);
        h.put("@FinancialYearId", year());
        h.put("@PlantId", plantId);
        h.put("@EBDepartmentId", 0);
        h.put("@ContractScheduleId", i(body.get("contractScheduleId")));
        h.put("@WipWareHouseId", i(col(job, "WipWareHouseId")));
        /* :2247 - a job order of document type 401 makes this a base-document-2 production. */
        h.put("@BaseDocumentTypeId", i(col(job, "DocumentTypeId")) == 401 ? 2 : 0);
        h.put("@StockHoldForLabApproval", b(body.get("stockHold")));
        if (i(h.get("@WipWareHouseId")) == 0) throw new IllegalArgumentException("Please Add WareHouse In JobOrder");

        /* :2270 - the grid loop. */
        Object rawRows = body.get("rows");
        List<?> rows = rawRows instanceof List ? (List<?>) rawRows : new ArrayList<>();
        if (rows.isEmpty()) throw new IllegalArgumentException("Enter Detail First ...");
        int moveOrderDocId = i(body.get("moveOrderDocId"));
        double grossWeightTotal = 0;
        List<Map<String, Object>> details = new ArrayList<>();
        List<String> itemNames = new ArrayList<>();
        for (Object o : rows) {
            @SuppressWarnings("unchecked") Map<String, Object> r = (Map<String, Object>) o;
            double qtyValue = d(r.get("quantity"));
            double weight = d(r.get("weight"));
            /* Conversion.ToInt(double) is Convert.ToInt32: banker's rounding. */
            if (!(Math.rint(qtyValue) > 0 && weight > 0)) continue;
            String entryType = s(r.get("entryType")).trim();
            if (entryType.isEmpty()) throw new IllegalArgumentException("EntryType Field Require in Output Detail");
            if (i(r.get("wareHouseId")) == 0) throw new IllegalArgumentException("WareHouse Field Require in Output Detail");
            if (i(r.get("itemId")) == 0) throw new IllegalArgumentException("Item Field Require in Output Detail");
            String jobLotText = s(r.get("jobLotId")).trim();
            if (jobLotText.isEmpty() || "0".equals(jobLotText)) throw new IllegalArgumentException("JobLot Field Require in Output Detail");
            /* :2307 Qty = ToDouble(cell.Text): the "#,##0.##" display, i.e. rounded to two places. */
            double qty = roundAway(qtyValue, 2);
            double rate = d(r.get("rate"));
            if (rateRight && fifo && rate == 0.0) throw new IllegalArgumentException("Rate not found");
            double rateEq = d(r.get("rateEquivalent"));
            if (rateRight && rateEq == 0.0) throw new IllegalArgumentException("Rate Equivalent not found");
            double amount = weight / rateEq * rate;
            if (rateRight && fifo && amount == 0.0) throw new IllegalArgumentException("Amount not Found in detail Grid");
            grossWeightTotal += weight;

            Map<String, Object> det = new LinkedHashMap<>();
            det.put("@Id", i(r.get("id")));
            det.put("@InvFoodProductionId", 0);
            det.put("@RefDocumentTypeId", 0);
            det.put("@RefDocNoId", 0);
            det.put("@InvProductionJobOrderId", jobOrderId);
            det.put("@InvProductionJobOrderNo", 0);
            det.put("@EntryType", entryType);
            det.put("@WarehouseId", i(r.get("wareHouseId")));
            det.put("@ItemId", i(r.get("itemId")));
            det.put("@ItemUomId", i(r.get("itemUomId")));
            det.put("@CropBatch", s(r.get("cropYear")));
            det.put("@JobLotId", i(r.get("jobLotId")));
            det.put("@PackingtypeId", i(r.get("packingTypeId")));
            det.put("@Qty", qty);
            det.put("@PackUnit", 0);
            det.put("@Weight", weight);
            det.put("@Rate", rate);
            det.put("@RateUOMId", i(r.get("rateUomId")));
            det.put("@Amount", amount);
            det.put("@ItemPmCost", 0.0);
            det.put("@GeneralPmCost", 0.0);
            det.put("@ItemOhCost", 0.0);
            det.put("@GeneralOhCost", 0.0);
            det.put("@NetRate", rate);
            det.put("@TotalAmount", amount);
            det.put("@Remarks", s(r.get("remarks")));
            det.put("@VoucherHeadId", 0);
            det.put("@StockAcId", i(r.get("stockAcId")));
            det.put("@RefDocSubIdNo", 0);
            det.put("@JobOrderInPutId", 0);
            det.put("@OutPutIdFromJobOrder", 0);
            det.put("@DeleteFlag", 0);
            det.put("@LineId", 0);
            det.put("@JobOrderScheduleId", i(r.get("scheduleId")));
            det.put("@GrossWeight", d(r.get("grossWeight")));
            det.put("@EbUnit", d(r.get("ebUnit")));
            det.put("@EbTotal", d(r.get("ebTotal")));
            det.put("@MoveOrderDocumentTypeId", 74);
            det.put("@MoveOrderDocId", moveOrderDocId);
            details.add(det);
            itemNames.add(s(r.get("item")));
        }
        if (details.isEmpty()) throw new IllegalArgumentException("grid Record not found please check!");
        String removeIds = s(body.get("removeIds"));

        /* BLL Save:0000 - the two switches read before anything else. */
        boolean withoutRates = b(config("JobOrderCreatewithoutRates"));
        boolean financialsInactive = b(config("InventoryFinancialsEffectsInActive"));
        boolean refDocTypeOne = financialsInactive || withoutRates;   // DocumentTypeId is always 112 here

        Map<String, Object> voucher = null;
        List<Map<String, Object>> voucherLines = null;
        if (!refDocTypeOne) {
            voucherLines = new ArrayList<>();
            voucher = makeVoucher(h, details, voucherLines);
        }
        /* BLL Save:00c9 - ActionId 1 insert / 2 update; the other audit user zeroed. */
        boolean insert = recId == 0;
        h.put("@ActionId", insert ? 1 : 2);
        if (insert) h.put("@ModifyUser", 0); else h.put("@EntryUser", 0);

        int id = repo.save(h, details, voucher, voucherLines, withoutRates, removeIds.isEmpty() ? null : removeIds);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        /* :2353 / :2357 - no space before the number, and it is the form's DocCode. */
        out.put("message", (recId > 0 ? "Record Update Successfully" : "Record Save Successfully") + i(docCode));
        /* :2359 - WagesDeleteByReferenceIds then the frmwagesBillHeader dialog. */
        boolean wages = b(cfg.get("wagesCompulsoryOnProduction")) && b(cfg.get("wagesActiveForOutput"));
        out.put("wages", wages);
        if (wages) {
            /* :2361 WagesDeleteByReferenceIds(112, id); the page then opens frmwagesBillHeader. */
            repo.wagesRemoveByReference(org(), comp(), DOC_TYPE, id);
            out.put("wagesRefDocTypeId", DOC_TYPE);
            out.put("wagesRefDocId", id);
            out.put("grossWeightTotal", grossWeightTotal);
        }
        return out;
    }

    /**
     * BLL InvFoodProduction.MakeVoucher, the DocumentTypeId 112 branch (:0499-:081d).
     *
     * One debit / credit pair per detail whose item is in the GL list; the stock side is the
     * row's StockAcId when set, else the item's PurchaseGLAC. Reproduced as written, including:
     *  - the credit line carries QtyIn / WeightIn (not Out) and no ItemId;
     *  - VoucherAmount accumulates the CREDIT line's DebitAmount, which is always 0;
     *  - AgainstAccountId on the head is the WIP ITEM id (:0077);
     *  - LineId counts vouchered details only, while DAL matches it against the detail LineId,
     *    which counts all details.
     */
    private Map<String, Object> makeVoucher(Map<String, Object> h, List<Map<String, Object>> details,
                                            List<Map<String, Object>> lines) {
        List<Map<String, Object>> items = repo.itemGlAccounts(org(), comp());
        Map<String, Object> v = ProductionOutputRepository.voucherHeadDefaults();
        v.put("@DocumentTypeSrNo", i(h.get("@Id")));
        v.put("@VoucherDate", h.get("@DocDate"));
        v.put("@VoucherCode", h.get("@DocCode"));
        v.put("@DocumentTypeId", DOC_TYPE);
        v.put("@BaseDocumentTypeId", h.get("@BaseDocumentTypeId"));
        v.put("@RefAccountId", h.get("@WIPAccountId"));
        v.put("@AgainstAccountId", h.get("@WIPItemId"));
        v.put("@EntryDate", LocalDateTime.now());
        v.put("@ModifyDate", LocalDateTime.now());
        v.put("@EntryUser", h.get("@EntryUser"));
        v.put("@ModifyUser", h.get("@ModifyUser"));
        v.put("@OrganizationId", h.get("@OrganizationId"));
        v.put("@CompanyId", h.get("@CompanyId"));
        v.put("@FinancialYearId", h.get("@FinancialYearId"));
        v.put("@BranchId", h.get("@BranchId"));
        double total = 0;
        int ln = 1;
        String jobNo = s(h.get("@InvJobOrderNo"));
        int wip = i(h.get("@WIPAccountId"));
        for (Map<String, Object> det : details) {
            if (items.isEmpty()) throw new IllegalArgumentException("Item GL Account Not Found");
            Map<String, Object> gl = null;
            for (Map<String, Object> it : items) if (i(col(it, "Id")) == i(det.get("@ItemId"))) { gl = it; break; }
            if (gl == null) continue;
            String remarks = s(det.get("@Remarks"));
            double qty = d(det.get("@Qty")), weight = d(det.get("@Weight")), rate = d(det.get("@Rate"));
            double amount = d(det.get("@Amount"));
            String comments = jobNo + " " + (remarks.trim().isEmpty() ? "" : remarks + " ") + " ItemName: "
                    + s(col(gl, "ItemName")) + " Qty " + ProductionOutputRepository.csDouble(qty)
                    + " Weight " + ProductionOutputRepository.csDouble(weight)
                    + " Rate " + ProductionOutputRepository.csDouble(rate);
            int stockAc = i(det.get("@StockAcId")) > 0 ? i(det.get("@StockAcId")) : i(col(gl, "PurchaseGLAC"));

            Map<String, Object> a = ProductionOutputRepository.voucherDetailDefaults();
            a.put("@AccountId", stockAc);
            a.put("@LineId", ln);
            a.put("@AgainstAccountId", wip);
            a.put("@Comments", comments);
            a.put("@DebitAmount", amount);
            a.put("@QtyIn", qty);
            a.put("@WeightIn", weight);
            a.put("@ItemAmount", amount);
            a.put("@ItemId", i(det.get("@ItemId")));
            a.put("@OrderNo", i(det.get("@InvProductionJobOrderId")));
            a.put("@BranchesId", h.get("@BranchId"));
            lines.add(a);

            Map<String, Object> c = ProductionOutputRepository.voucherDetailDefaults();
            c.put("@AccountId", wip);
            c.put("@AgainstAccountId", stockAc);
            c.put("@LineId", ln);
            c.put("@Comments", comments);
            c.put("@CreditAmount", amount);
            c.put("@QtyIn", qty);
            c.put("@WeightIn", weight);
            c.put("@ItemAmount", amount);
            c.put("@OrderNo", i(det.get("@InvProductionJobOrderId")));
            c.put("@BranchesId", h.get("@BranchId"));
            total += d(c.get("@DebitAmount"));
            lines.add(c);
            ln++;
        }
        v.put("@VoucherAmount", total);
        return v;
    }

    // ============================================================================ helpers

    /** .NET "#,##0.##" rounds half away from zero. */
    private static double roundAway(double v, int dec) {
        return new java.math.BigDecimal(Double.toString(v)).setScale(dec, java.math.RoundingMode.HALF_UP).doubleValue();
    }

    private static LocalDateTime dateOnly(String v) {
        if (v == null || v.trim().isEmpty()) return null;
        try { return LocalDate.parse(v.trim().substring(0, 10)).atStartOfDay(); } catch (Exception e) { return null; }
    }

    private static LocalDateTime dateTime(String v) {
        LocalDateTime d = dateOnly(v);
        return d == null ? LocalDateTime.now() : d;
    }

    /** A DateTimePicker's Value carries the time of day it was created with. */
    private static LocalDateTime docDateTime(String v) {
        LocalDateTime d = dateOnly(v);
        if (d == null) throw new IllegalArgumentException("Doc Date is required");
        return LocalDateTime.of(d.toLocalDate(), LocalTime.now().withNano(0));
    }

    /** Conversion.ToInt(TextBox.Text) - Convert.ToInt32(string): anything not an integer is 0. */
    private static double intText(String v) {
        if (v == null) return 0;
        try { return Integer.parseInt(v.trim()); } catch (Exception e) { return 0; }
    }
}
