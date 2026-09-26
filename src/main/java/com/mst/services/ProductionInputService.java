package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ProductionInputRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;

import java.math.BigDecimal;
import java.math.MathContext;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ProductionInputRepository.ci;
import static com.mst.repositories.ProductionInputRepository.toDouble;
import static com.mst.repositories.ProductionInputRepository.toInt;

/**
 * Screen 280, INPUT tab - frmProductionInput.cs (DocumentTypeId 80), ported line by line.
 *
 * Tenancy, branch, financial year and user always come from {@link CurrentUserContext}; nothing
 * the browser sends can choose them. Rights are re-read here for every write.
 *
 * Save reproduces three desktop layers in order:
 *   1. frmProductionInput.InsertInput:1385 - the per-row loop that builds InvFoodProductionDetail,
 *      with its early returns and exact messages (the header checks of FormValidation run in the
 *      page first and again here);
 *   2. BLL InvFoodProduction.Save (:4994) - the two configuration reads, MakeVoucher, ActionId and
 *      the Entry/Modify user blanking;
 *   3. DAL InvFoodProduction.SetData (:2072) - one transaction: header, details, stock evaluation
 *      rebuild, inventory transactions, removed rows, four guard procedures, voucher head/lines
 *      and the balance check.
 *
 * The DAL's FIFO branch (ERP feature 5 ON and no row loaded from the issuance loader) is ported in
 * {@link #fifoBranch}: CommonServices.FIFOImplemention per row (USP_GetStockByFifoMethod with the
 * @FIFOXML reservations of earlier rows), two voucher lines per allocated layer, then
 * USP_InventoryQtyReverseAndDeleteByReferenceId (on update) and USP_InventoryStockEvalautionDetail_Insert
 * per layer - all inside the save's single transaction.
 */
@Service
public class ProductionInputService {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionInputService.class);

    public static final int DOC_TYPE_ID = ProductionInputRepository.DOC_TYPE_INPUT;   // 80
    /** Load:369 - rights are read under the shell's name when the user has it, else base.Name. */
    public static final String SHELL_SCREEN = "FoodProductionWithValues";
    public static final String FORM_SCREEN = "frmProductionInput";
    public static final String LOADER_SCREEN = "LoadavailableTransactionsForIssuance";

    private static final int ERP_FEATURE_FIFO_CGS = 5;
    private static final int MOVE_ORDER_DOCUMENT_TYPE_ID = 74;

    @Autowired private ProductionInputRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private PlatformTransactionManager transactionManager;

    // =========================================================================== context helpers

    private UserAccount user() { return currentUserContext.requireAccountingUser(); }
    private int branchId(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private int fy() { return currentUserContext.currentFinancialYearId(); }

    /** Conversion.ToBool: Convert.ToBoolean, falling back to Convert.ToInt32 != 0; anything else false. */
    static boolean toBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).doubleValue() != 0;
        String s = o.toString().trim();
        if ("true".equalsIgnoreCase(s)) return true;
        if ("false".equalsIgnoreCase(s) || s.isEmpty()) return false;
        try { return Integer.parseInt(s) != 0; } catch (NumberFormatException e) { return false; }
    }

    private boolean flag(UserAccount u, String name) {
        try { return toBool(repo.config(u, name)); }
        catch (Exception e) { LOG.warn("Configuration '{}' could not be read; treated as off", name, e); return false; }
    }

    private boolean fifo(UserAccount u) {
        try { return repo.erpFeature(u, ERP_FEATURE_FIFO_CGS); }
        catch (Exception e) { LOG.warn("ERP feature 5 could not be read; treated as off", e); return false; }
    }

    /** Decimal places from "Default NoofDecimal Points For Rate/Amount" (GetDecimalConfiguration). */
    private int decimals(UserAccount u, String name, boolean rate) {
        int n = 0;
        try {
            String v = repo.config(u, name);
            n = v == null || v.trim().isEmpty() ? 0 : (int) Math.floor(Double.parseDouble(v.trim()));
        } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read", name, e);
        }
        if (n >= 1 && n <= 4) return n;
        return (rate && n == 0) ? 2 : 0;
    }

    // ==================================================================================== rights

    /**
     * CommonServices.SetRightsValueInRightsObject(screen) (CommonServices.cs:14738), the members this
     * form reads (Load:377-390): Save, Update, Print, Rate, Grid Print, Grid Export. Only the role
     * spelled "Admin" is pre-granted Save/Update/Print; Rate and the grid rights come only from rows;
     * SaveLayout and group collapse/expand start true.
     *
     * Load:369 chooses the screen: "FoodProductionWithValues" when the user's screen-view list
     * contains it, otherwise base.Name ("frmProductionInput"). The web has no ScreenViewReights list,
     * so "the user has grant rows under the shell's name" stands in for it.
     */
    public Map<String, Object> rights() {
        UserAccount u = user();
        String role = currentUserContext.currentRoleName();
        List<Map<String, Object>> rows = rightRows(u, role, SHELL_SCREEN);
        String screen = SHELL_SCREEN;
        if (rows.isEmpty()) { rows = rightRows(u, role, FORM_SCREEN); screen = FORM_SCREEN; }
        Map<String, Object> r = rightsFrom(rows, "Admin".equals(role));
        r.put("screenName", screen);
        return r;
    }

    private List<Map<String, Object>> rightRows(UserAccount u, String role, String screen) {
        try {
            return repo.userRights(u.getId(), screen, role, u.getCompanyId());
        } catch (Exception e) {
            LOG.warn("Rights for {} could not be read", screen, e);
            return new ArrayList<>();
        }
    }

    private static Map<String, Object> rightsFrom(List<Map<String, Object>> rows, boolean admin) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("save", admin);
        r.put("update", admin);
        r.put("print", admin);
        r.put("rateAndAmount", false);
        r.put("saveLayout", true);
        r.put("gridPrint", false);
        r.put("gridExport", false);
        r.put("groupCollapse", true);
        r.put("groupExpand", true);
        for (Map<String, Object> row : rows) {
            Object n = ci(row, "RightName");
            if (n == null) continue;
            String name = n.toString();
            boolean v = toBool(ci(row, "Value"));
            switch (name) {
                case "Save":        r.put("save", admin || v); break;
                case "Update":      r.put("update", admin || v); break;
                case "Print":       r.put("print", admin || v); break;
                case "Rate":        r.put("rateAndAmount", v); break;
                case "Grid Print":  r.put("gridPrint", v); break;
                case "Grid Export": r.put("gridExport", v); break;
                default: break;
            }
        }
        return r;
    }

    /** FoodProductionWithValues.ValuesShowRights - the shell's own "Rate" row (read by the loader's grid). */
    private boolean shellRateRight(UserAccount u) {
        String role = currentUserContext.currentRoleName();
        for (Map<String, Object> row : rightRows(u, role, SHELL_SCREEN)) {
            Object n = ci(row, "RightName");
            if (n != null && "Rate".equals(n.toString())) return toBool(ci(row, "Value"));
        }
        return false;
    }

    // ============================================================================== Load (:351)

    public Map<String, Object> state() {
        UserAccount u = user();
        int branch = branchId(u);
        int year = fy();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("documentTypeId", DOC_TYPE_ID);

        /* :358 - WagesRefDocumentsStatusList.Find(RefDocumentTypeId == 80) */
        out.put("wagesActiveForInput", wagesActiveForInput());
        /* :360-364 */
        out.put("saleCostingJobOrderWise", flag(u, "SaleCostingJobOrderWise"));
        out.put("wagesCompulsoryOnProduction", flag(u, "WagesCompulsoryOnProduction"));
        out.put("issuanceByLoader", flag(u, "IssuanceByLoader"));
        out.put("jobOrderCreateWithoutRates", flag(u, "JobOrderCreatewithoutRates"));
        out.put("isManualEntryOnInputNotAllowed", flag(u, "IsManualEntryOnInputNotAllowed"));
        out.put("rights", rights());
        out.put("shellRateRight", shellRateRight(u));
        /* :391 */
        out.put("fifoCgs", fifo(u));
        out.put("rateDecimals", decimals(u, "Default NoofDecimal Points For Rate", true));
        out.put("amountDecimals", decimals(u, "Default NoofDecimal Points For Amount", false));
        /* :443 GenerateDocNumberInput */
        out.put("docNumber", serial());
        Object start = null;
        try { start = repo.financialYearStart(u, year); } catch (Exception e) { LOG.warn("Active year start", e); }
        out.put("financialYearStart", start == null ? null : start.toString());

        /* :445-451 the pickers */
        out.put("jobLots", project(repo.jobLotsForBranch(u, branch), "Id", "JobLotDescription"));
        out.put("cropYears", project(repo.cropYears(u), "Id", "CropYear"));
        out.put("packingTypes", project(repo.packingTypes(), "Id", "PackTypeDesc"));
        out.put("jobOrders", project(repo.jobOrdersForProduction(u, branch), "Id", "PlanCode", "DocumentTypeId"));
        out.put("warehouses", project(repo.warehousesForBranch(u, branch), "Id", "WareHouseName"));
        out.put("departments", project(repo.productionDepartments(u), "Id", "WareHouseName"));
        out.put("uomSchedules", project(repo.uomSchedules(u), "Id", "ItemId", "UOMCode", "Equivalent"));
        /* :468 jobOrderInputHCombobind */
        out.put("historyJobOrders", historyJobOrders());
        return out;
    }

    private boolean wagesActiveForInput() {
        try {
            for (Map<String, Object> r : repo.wagesRefDocuments()) {
                if (toInt(ci(r, "RefDocumentTypeId")) == DOC_TYPE_ID) return toBool(ci(r, "IsActive"));
            }
        } catch (Exception e) {
            LOG.warn("USP_GetRefDocumentsForWages failed; wages treated as inactive", e);
        }
        return false;
    }

    /** btnRefresh_Click:1735 - wages status, job orders, crop years and job lots again. */
    public Map<String, Object> refresh() {
        UserAccount u = user();
        int branch = branchId(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("wagesActiveForInput", wagesActiveForInput());
        out.put("jobOrders", project(repo.jobOrdersForProduction(u, branch), "Id", "PlanCode", "DocumentTypeId"));
        out.put("cropYears", project(repo.cropYears(u), "Id", "CropYear"));
        out.put("jobLots", project(repo.jobLotsForBranch(u, branch), "Id", "JobLotDescription"));
        return out;
    }

    public int serial() {
        UserAccount u = user();
        return repo.serialNumber(u, fy(), branchId(u));
    }

    public List<Map<String, Object>> historyJobOrders() {
        UserAccount u = user();
        return project(repo.historyJobOrders(u, fy(), String.valueOf(branchId(u))), "Id", "ReferenceName");
    }

    /** CmbJobOrderNo_Leave:1130 - WIP combos, then GetPlantFeeder, then getJobOrderItems. */
    public Map<String, Object> jobOrder(int jobOrderId) {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("glAccounts", project(repo.glAccountsByJobOrder(u, jobOrderId),
                "WorkInProccessAcId", "WorkInProcessAc", "WipItemId", "ItemName", "WipWareHouseId", "WareHouseName"));
        out.put("plants", project(repo.plantsForJobOrder(u, fy(), branchId(u), jobOrderId), "PlantId", "PlantName"));
        /* getJobOrderItems:800 returns at once unless JobOrderCreatewithoutRates */
        if (flag(u, "JobOrderCreatewithoutRates")) {
            out.put("jobOrderItems", project(repo.jobOrderItems(jobOrderId, 1), "ItemId", "ItemName"));
        }
        return out;
    }

    public List<Map<String, Object>> itemsByWarehouse(int warehouseId) {
        return project(repo.itemsByWarehouse(user(), warehouseId), "ItemId", "ItemName");
    }

    /** GetAvailableStockForInput:899 - AvailableQty / AvailableStock of row 0, else nothing. */
    public Map<String, Object> stock(int warehouseId, int itemId, int jobLotId, String cropYear, String docDate) {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = repo.weightCurrStock(u, itemId, dateTime(docDate), warehouseId, jobLotId,
                cropYear == null ? "" : cropYear);
        if (!rows.isEmpty()) {
            out.put("availableQty", toDouble(ci(rows.get(0), "AvailableQty")));
            out.put("availableStock", toDouble(ci(rows.get(0), "AvailableStock")));
        }
        return out;
    }

    /**
     * GetAvgRateByItemAndJoblot:1194 - only with the Rate right. FIFO (feature 5) -> the FIFO
     * weighted rate; otherwise AvgRateOnlyForCGS(item, date, 80, RecId, jobLot, cropYearId, null,
     * warehouse). Returns the raw per-kg rate; the page multiplies by 40 as the form does.
     */
    public Map<String, Object> avgRate(int itemId, String docDate, int recId, int jobLotId, int cropYearId,
                                       String cropYear, int warehouseId, int stockUom, int packingTypeId,
                                       double qty, double netWeight) {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        if (!Boolean.TRUE.equals(rights().get("rateAndAmount"))) { out.put("skipped", true); return out; }
        LocalDateTime d = dateTime(docDate);
        double rate;
        if (fifo(u)) {
            rate = fifoRate(u, d, warehouseId, itemId, stockUom, jobLotId, packingTypeId, cropYear, qty, netWeight, 0, 0);
        } else {
            rate = repo.avgRateOnlyForCgs(u, itemId, d, DOC_TYPE_ID, recId, jobLotId, cropYearId, null, warehouseId, 0);
        }
        out.put("rate", rate);
        return out;
    }

    /** CellUpdated FIFO branch (:1810, :1844) - GetAvgRateFromFIFOMethod without stockUOM / Id. */
    public Map<String, Object> fifoRateForRow(String docDate, int warehouseId, int itemId, int jobLotId,
                                              int packingTypeId, String cropYear, double qty, double netWeight) {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rate", fifoRate(u, dateTime(docDate), warehouseId, itemId, 0, jobLotId, packingTypeId, cropYear,
                qty, netWeight, 0, 0));
        return out;
    }

    /**
     * BtnGenerateRates_Click -> AvgRateUpdateOnDocDateChange:2836, one row at a time. FIFO:
     * Math.Round(FIFO rate, 3) * 40 with DocumentTypeId/Id only when RecId > 0; otherwise
     * AvgRateOnlyForCGS(item, date, 80, RecId, jobLot, 0, cropYear text, warehouse) * 40.
     */
    public Map<String, Object> generateRate(String docDate, int recId, int itemId, int itemUomId, int packingTypeId,
                                            int warehouseId, String cropYear, int jobLotId, double qty, double weight) {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        LocalDateTime d = dateTime(docDate);
        boolean f = fifo(u);
        out.put("fifo", f);
        double rate;
        if (f) {
            double r = fifoRate(u, d, warehouseId, itemId, itemUomId, jobLotId, packingTypeId, cropYear, qty, weight,
                    recId > 0 ? DOC_TYPE_ID : 0, recId > 0 ? recId : 0);
            rate = BigDecimal.valueOf(r).setScale(3, java.math.RoundingMode.HALF_EVEN).doubleValue() * 40.0;
        } else {
            rate = repo.avgRateOnlyForCgs(u, itemId, d, DOC_TYPE_ID, recId, jobLotId, 0, cropYear, warehouseId, 0) * 40.0;
        }
        out.put("rate", rate);
        return out;
    }

    /** CommonServices.GetAvgRateFromFIFOMethod (CommonServices.cs:2386), statement for statement. */
    private double fifoRate(UserAccount u, LocalDateTime docDate, int warehouseId, int itemId, int stockUom,
                            int jobLotId, int packingTypeId, String cropYear, double itemQty, double netWeight,
                            int documentTypeId, int id) {
        List<Map<String, Object>> rows = repo.stockByFifo(u, itemId, stockUom, docDate, warehouseId, jobLotId,
                packingTypeId, 0, cropYear, documentTypeId, id);
        double result = 0.0;
        if (rows.isEmpty()) return result;
        double total = 0;
        for (Map<String, Object> r : rows) total += toDouble(ci(r, "BalWeight"));
        double sum = 0.0;
        if (netWeight <= total) {
            double num3 = netWeight, num4 = itemQty, num6 = 0, num8 = 0;
            for (Map<String, Object> r : rows) {
                double num7 = toDouble(ci(r, "BalWeight"));
                double num5 = toDouble(ci(r, "BalQty"));
                double avg = toDouble(ci(r, "AvgRate"));
                if (num7 <= num3 - num8 && num5 <= num4 - num6) {
                    num8 += num7;
                    num6 += num5;
                    sum += num7 * avg;
                } else if (num7 >= num3 - num8 && num5 >= num4 - num6) {
                    double num9 = num4 - num6;
                    double num10 = num3 - num8;
                    sum += (num3 - num8) * avg;
                    num6 += num9;
                    num8 += num10;
                }
                if (netWeight == num8 && itemQty == num6) break;
            }
            if (sum > 0.0 && netWeight > 0.0) result = sum / netWeight;
        }
        return result;
    }

    // ======================================================================= read one / history

    /**
     * ReadByIdInput:1575 / InputDetailByHeaderId:2609 - InvFoodProduction.GetByID. The page applies
     * the desktop's checks (IsApproved, PlanStatus) in its own order with the exact messages.
     */
    public Map<String, Object> read(int id) {
        UserAccount u = user();
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", h);
        out.put("details", repo.details(id));
        out.put("voucherHeadId", repo.voucherHeadId(u, DOC_TYPE_ID, id));
        return out;
    }

    public List<Map<String, Object>> history(String fromDate, String toDate, int docNoFrom, int docNoTo, int jobOrderId) {
        UserAccount u = user();
        return repo.history(u, fy(), branchId(u), dateTimeOrNull(fromDate), dateTimeOrNull(toDate),
                docNoFrom, docNoTo, jobOrderId);
    }

    public int voucherHeadId(int id) {
        return id <= 0 ? 0 : repo.voucherHeadId(user(), DOC_TYPE_ID, id);
    }

    // ============================================================ LoadavailableTransactionsForIssuance

    /**
     * LoadInvoices_Load:128 - the rights read under "LoadavailableTransactionsForIssuance"
     * (lstRights.Where(RightName=="Rate").ToList()[0] throws when rows exist but none is "Rate",
     * which aborts the rest of Load - reported as rateRowMissing), then StockComboFill.
     */
    public Map<String, Object> loaderLookups(String jobOrderItemsJson) {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        String role = currentUserContext.currentRoleName();
        List<Map<String, Object>> rows = rightRows(u, role, LOADER_SCREEN);
        boolean rateRowMissing = false;
        if (!rows.isEmpty()) {
            rateRowMissing = true;
            for (Map<String, Object> r : rows) {
                Object n = ci(r, "RightName");
                if (n != null && "Rate".equals(n.toString())) { rateRowMissing = false; break; }
            }
        }
        out.put("rateRowMissing", rateRowMissing);
        out.put("shellRateRight", shellRateRight(u));
        Object start = null;
        try { start = repo.financialYearStart(u, fy()); } catch (Exception e) { LOG.warn("Active year start", e); }
        out.put("financialYearStart", start == null ? null : start.toString());
        Map<String, List<Map<String, Object>>> lists = new LinkedHashMap<>();
        for (String k : new String[]{"ParentCategories", "ItemCategories", "ItemTypes", "JobLot", "CropYear",
                "Warehouse", "DocumentType", "Supplier_Customer", "Items", "Stock_Account"}) lists.put(k, new ArrayList<>());
        for (Map<String, Object> r : repo.issuanceDropDowns(u, String.valueOf(branchId(u)))) {
            Object t = ci(r, "ActivityType");
            List<Map<String, Object>> l = t == null ? null : lists.get(t.toString());
            if (l == null) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("name", ci(r, "name"));
            l.add(m);
        }
        out.put("lists", lists);
        return out;
    }

    public List<Map<String, Object>> loaderSearch(String fromDate, String toDate, int parentCategory, int itemCategory,
                                                  int itemType, int jobLotId, String cropYear, int warehouseId,
                                                  int refDocumentTypeId, int supplierId, int itemId, String itemIds) {
        UserAccount u = user();
        return repo.availableTransactionsForIssuance(u, branchId(u), dateTimeOrNull(fromDate), dateTimeOrNull(toDate),
                supplierId, warehouseId, itemId, refDocumentTypeId, jobLotId, parentCategory, itemCategory, itemType,
                cropYear, itemIds);
    }

    // ===================================================================== LoadOutPutPendingforRates

    public List<Map<String, Object>> pendingRateJobOrders() {
        return project(repo.pendingRateJobOrders(user(), fy()), "Id", "PlanCode");
    }

    public List<Map<String, Object>> pendingRates(String fromDate, String toDate, int jobOrderId, String entryType) {
        UserAccount u = user();
        return repo.pendingRatesData(u, branchId(u), dateTimeOrNull(fromDate), dateTimeOrNull(toDate), jobOrderId,
                entryType);
    }

    // ================================================================= frmPendingMoveOrderDocuments

    public List<Map<String, Object>> moveOrders(String fromDate, String toDate, int fromDocNo, int toDocNo,
                                                String vehicleNo, String biltyNo) {
        UserAccount u = user();
        return repo.moveOrderPending(u, fy(), String.valueOf(branchId(u)), dateTimeOrNull(fromDate),
                dateTimeOrNull(toDate), fromDocNo, toDocNo, vehicleNo, biltyNo);
    }

    // ================================================================================= Save

    /** Thrown for the form's own MessageBox-and-return cases: nothing has been written. */
    public static class InputMessage extends RuntimeException {
        public InputMessage(String m) { super(m); }
    }

    /**
     * btnsave_Click / btnUpdate_Click -> InsertInput:1385.
     *
     * Body: { id, docNumber, docDate, remarks, wipAccountId, wipItemId, wipWarehouseId, departmentId,
     *         jobOrderId, jobOrderText, jobOrderDocumentTypeId, plantId, moveOrderId,
     *         inputDetailRowsRemoveIds, rows:[ grid row as the form's DataTable holds it ] }
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = user();
        int recId = toInt(body.get("id"));
        Map<String, Object> rights = rights();
        boolean rateRight = Boolean.TRUE.equals(rights.get("rateAndAmount"));
        if (recId > 0 ? !Boolean.TRUE.equals(rights.get("update")) : !Boolean.TRUE.equals(rights.get("save"))) {
            /* the button is disabled on the desktop; the web refuses the call the same way */
            throw new InputMessage(recId > 0 ? "Update is not allowed for this user." : "Save is not allowed for this user.");
        }

        /* ---- FormValidation:1244 (the page runs it first; repeated so a direct call cannot skip it) */
        String docNumber = str(body.get("docNumber")).trim();
        if (docNumber.isEmpty() || "0".equals(docNumber)) throw new InputMessage("document Number Field Required");
        int jobOrderId = toInt(body.get("jobOrderId"));
        if (jobOrderId == 0) throw new InputMessage("Job Order Number Field Required");
        if (toInt(body.get("plantId")) == 0) throw new InputMessage("Plant Name Field Required");
        if (toInt(body.get("wipAccountId")) == 0) throw new InputMessage("WorkInProcess Field Required");
        if (toInt(body.get("wipItemId")) == 0) throw new InputMessage("WIP Item Field Required");
        if (toInt(body.get("wipWarehouseId")) == 0) throw new InputMessage("WIP WareHouse Field Required");

        List<Map<String, Object>> rows = body.get("rows") instanceof List ? (List<Map<String, Object>>) body.get("rows") : new ArrayList<>();
        if (rows.isEmpty()) throw new InputMessage("Enter Detail First ...");

        LocalDateTime now = LocalDateTime.now();
        int moveOrderId = toInt(body.get("moveOrderId"));

        /* ---- the header the form fills (:1411-1439) */
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", recId);
        h.put("DocDate", dateTime(str(body.get("docDate"))));
        h.put("DocCode", toInt(docNumber));
        h.put("DocumentTypeId", DOC_TYPE_ID);
        h.put("WIPAccountId", toInt(body.get("wipAccountId")));
        h.put("WIPItemId", toInt(body.get("wipItemId")));
        h.put("MainRemarks", str(body.get("remarks")));
        h.put("IsApproved", false);
        h.put("EntryDate", now);
        h.put("EntryUser", u.getId());
        h.put("ModifyDate", now);
        h.put("ModifyUser", u.getId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("InvFoodProductionPlanId", 0);
        h.put("InvJobOrderId", jobOrderId);
        h.put("InvJobOrderNo", str(body.get("jobOrderText")).trim());
        h.put("EntryType", "Input");
        h.put("BranchId", branchId(u));
        h.put("ProjectId", 0);
        h.put("FinancialYearId", fy());
        h.put("PlantId", toInt(body.get("plantId")));
        h.put("EBDepartmentId", toInt(body.get("departmentId")));
        h.put("ContractScheduleId", 0);
        h.put("WipWareHouseId", toInt(body.get("wipWarehouseId")));
        /* :1423 - BaseDocumentTypeId = 2 only when the job order row's DocumentTypeId is 401 */
        h.put("BaseDocumentTypeId", toInt(body.get("jobOrderDocumentTypeId")) == 401 ? 2 : 0);
        h.put("StockHoldForLabApproval", false);
        String removeIds = str(body.get("inputDetailRowsRemoveIds"));

        /* ---- the row loop (:1446-1538) */
        List<Map<String, Object>> details = new ArrayList<>();
        double grossWeightTotal = 0.0;   // named GrossWeight on the desktop but sums the NET Weight
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            if (!(toDouble(r.get("Weight")) > 0.0)) continue;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("EntryType", "Issue");
            d.put("Id", toInt(r.get("Id")));
            d.put("RefDocNoId", toInt(r.get("RefDocIdNo")));
            d.put("RefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
            d.put("RefDocSubIdNo", toInt(r.get("RefDocSubIdNo")));
            String wh = str(r.get("WareHouseId"));
            if (wh.isEmpty() || "0".equals(wh)) throw new InputMessage("WareHouse Field Require in Input Detail");
            d.put("WarehouseId", toInt(r.get("WareHouseId")));
            String item = str(r.get("ItemId"));
            if (item.isEmpty() || "0".equals(item)) throw new InputMessage("Item Field Require in Input Detail");
            d.put("ItemId", toInt(r.get("ItemId")));
            d.put("ItemUomId", toInt(r.get("ItemUOMId")));
            String crop = str(r.get("CropYear"));
            if (crop.isEmpty() || "0".equals(crop)) throw new InputMessage("CropYear Field Require in Input Detail");
            d.put("CropBatch", crop);
            if (str(r.get("JobLotId")).isEmpty()) throw new InputMessage("JobLot Field Require in Input Detail");
            d.put("JobLotId", toInt(r.get("JobLotId")));
            d.put("PackingtypeId", toInt(r.get("PackingTypeId")));
            /* :1485 - Conversion.ToInt of the pack UOM's TEXT: 0 unless the code is a whole number */
            d.put("PackUnit", toInt(r.get("ItemUOM")));
            double qty = toDouble(r.get("Quantity"));
            double weight = toDouble(r.get("Weight"));
            d.put("Qty", qty);
            d.put("Weight", weight);
            grossWeightTotal += weight;
            if (weight == 0.0) throw new InputMessage("ItemWeight Field Required");
            d.put("InvProductionJobOrderId", jobOrderId);
            int refType = toInt(d.get("RefDocumentTypeId"));
            int refNo = toInt(d.get("RefDocNoId"));
            if (!rateRight && refType == 0 && refNo == 0) {
                d.put("Rate", 0.0);
                d.put("NetRate", 0.0);
                d.put("Amount", 0.0);
                d.put("TotalAmount", 0.0);
            } else {
                double rate = toDouble(r.get("Rate"));
                d.put("Rate", rate);
                d.put("NetRate", rate);
                /* :1507 - Conversion.ToInt(ItemAmount): the amount is saved rounded to a whole number */
                double amount = toInt(r.get("ItemAmount"));
                d.put("Amount", amount);
                d.put("TotalAmount", amount);
            }
            d.put("RateUOMId", toInt(r.get("RateUOMId")));
            if (toDouble(r.get("RateUOMForCheck")) != 0.0) {
                List<Map<String, Object>> uoms = repo.uomsForItem(u, toInt(r.get("ItemId")));
                if (!uoms.isEmpty()) {
                    int eq = toInt(r.get("RateUOMForCheck"));
                    Map<String, Object> hit = null;
                    for (Map<String, Object> x : uoms) {
                        if (toDouble(ci(x, "Equivalent")) == eq) { hit = x; break; }
                    }
                    if (hit == null) throw new InputMessage("RateUOM On Row No " + (i + 1) + " is not define please check");
                    d.put("RateUOMId", toInt(ci(hit, "Id")));
                }
            }
            d.put("VoucherHeadId", 0);
            d.put("Remarks", str(r.get("Remarks")));
            d.put("MoveOrderDocumentTypeId", MOVE_ORDER_DOCUMENT_TYPE_ID);
            d.put("MoveOrderDocId", moveOrderId);
            d.put("EbUnit", toDouble(r.get("EbUnit")));
            d.put("EbTotal", toDouble(r.get("EbTotal")));
            double gross = toDouble(r.get("GrossWeight"));
            d.put("GrossWeight", gross);
            if (gross == 0.0) throw new InputMessage("GrossWeight Field Required Input Detail");
            /* model value-type defaults the form never sets */
            d.put("ItemPmCost", 0.0);
            d.put("GeneralPmCost", 0.0);
            d.put("ItemOhCost", 0.0);
            d.put("GeneralOhCost", 0.0);
            d.put("StockAcId", 0);
            d.put("JobOrderInPutId", 0);
            d.put("OutPutIdFromJobOrder", 0);
            d.put("DeleteFlag", 0);
            d.put("JobOrderScheduleId", 0);
            details.add(d);
        }
        if (details.isEmpty()) throw new InputMessage("Grid Record Not Found");

        /* ---- BLL InvFoodProduction.Save (:4994) */
        boolean financialsInactive = flag(u, "InventoryFinancialsEffectsInActive");
        int headerRefDocumentTypeId = financialsInactive ? 1 : 0;   // JobOrderCreatewithoutRates only matters for 112
        boolean fifo = fifo(u);
        Map<String, Object> voucher = null;
        List<Map<String, Object>> voucherLines = new ArrayList<>();
        if (headerRefDocumentTypeId != 1) {
            voucher = makeVoucher(u, h, details, fifo, voucherLines);
        }
        boolean insert = recId == 0;
        h.put("ActionId", insert ? 1 : 2);
        if (insert) h.put("ModifyUser", 0); else h.put("EntryUser", 0);

        final Map<String, Object> voucherF = voucher;
        final int refDocF = headerRefDocumentTypeId;
        TransactionTemplate tx = new TransactionTemplate(transactionManager);
        Integer savedId = tx.execute(status -> setData(u, h, details, removeIds, refDocF, voucherF, voucherLines));
        int id = savedId == null ? 0 : savedId;

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        /* :1546/:1550 - no space before the number, as the desktop shows it */
        out.put("message", (recId > 0 ? "Record Update Successfully" : "Record Save Successfully") + toInt(docNumber));
        out.put("grossWeightTotal", grossWeightTotal);

        /* :1552 - when wages are compulsory and active for 80: remove the old bills, then the page
           opens frmwagesBillHeader (RefDocTypeId 80, RefDocId, GrossWeightTotal). */
        boolean wages = flag(u, "WagesCompulsoryOnProduction") && wagesActiveForInput();
        out.put("openWages", wages);
        /* :1554 WagesDeleteByReferenceIds(80, id), then the page opens frmwagesBillHeader
           (/production/wages-bill) as a modal, exactly as the desktop's ShowDialog. */
        if (wages) {
            try {
                repo.wagesRemoveByReference(u, DOC_TYPE_ID, id);
            } catch (RuntimeException e) {
                out.put("wagesMessage", rootMessage(e));
            }
        }
        return out;
    }

    /**
     * BLL InvFoodProduction.MakeVoucher (:4080), the DocumentTypeId 80 branch. A detail gets its
     * two lines when (FIFO is off) or (it came from the loader: RefDocumentTypeId > 0 and
     * RefDocNoId > 0) - and only if its item is in the GL list. LineId counts pairs, not details.
     */
    private Map<String, Object> makeVoucher(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details,
                                            boolean fifo, List<Map<String, Object>> lines) {
        List<Map<String, Object>> items = repo.itemGlAccounts(u);
        Map<String, Object> vh = ProductionInputRepository.voucherHeadDefaults();
        vh.put("DocumentTypeSrNo", h.get("Id"));
        vh.put("VoucherDate", h.get("DocDate"));
        vh.put("VoucherCode", h.get("DocCode"));
        vh.put("DocumentTypeId", h.get("DocumentTypeId"));
        vh.put("BaseDocumentTypeId", h.get("BaseDocumentTypeId"));
        vh.put("RefAccountId", h.get("WIPAccountId"));
        /* the WIP ITEM id is written into AgainstAccountId - as the desktop does */
        vh.put("AgainstAccountId", h.get("WIPItemId"));
        vh.put("EntryDate", LocalDateTime.now());
        vh.put("ModifyDate", LocalDateTime.now());
        vh.put("EntryUser", h.get("EntryUser"));
        vh.put("ModifyUser", h.get("ModifyUser"));
        vh.put("OrganizationId", h.get("OrganizationId"));
        vh.put("CompanyId", h.get("CompanyId"));
        vh.put("FinancialYearId", h.get("FinancialYearId"));
        vh.put("BranchId", h.get("BranchId"));
        double total = 0.0;
        int lineId = 1;
        int wip = toInt(h.get("WIPAccountId"));
        String jobNo = str(h.get("InvJobOrderNo"));
        for (Map<String, Object> d : details) {
            boolean eligible = !fifo || (toInt(d.get("RefDocumentTypeId")) > 0 && toInt(d.get("RefDocNoId")) > 0);
            if (!eligible) continue;
            if (items.isEmpty()) throw new InputMessage("Item GL Account Not Found");
            Map<String, Object> item = null;
            for (Map<String, Object> it : items) {
                if (toInt(ci(it, "Id")) == toInt(d.get("ItemId"))) { item = it; break; }
            }
            if (item == null) continue;
            String remarks = str(d.get("Remarks"));
            String comment = jobNo + " " + (remarks.trim().isEmpty() ? "" : remarks + " ") + " ItemName: "
                    + str(ci(item, "ItemName")) + " Qty " + net(toDouble(d.get("Qty"))) + " Weight "
                    + net(toDouble(d.get("Weight"))) + " Rate " + net(toDouble(d.get("Rate")));
            int purchaseGl = toInt(ci(item, "PurchaseGLAC"));
            double amount = toDouble(d.get("Amount"));

            Map<String, Object> dr = ProductionInputRepository.voucherDetailDefaults();
            dr.put("LineId", lineId);
            dr.put("AccountId", wip);
            dr.put("AgainstAccountId", purchaseGl);
            dr.put("Comments", comment);
            dr.put("DebitAmount", amount);
            dr.put("QtyIn", toDouble(d.get("Qty")));
            dr.put("WeightIn", toDouble(d.get("Weight")));
            dr.put("ItemAmount", amount);
            dr.put("OrderNo", toInt(d.get("InvProductionJobOrderId")));
            dr.put("BranchesId", h.get("BranchId"));
            total += amount;
            lines.add(dr);

            Map<String, Object> cr = ProductionInputRepository.voucherDetailDefaults();
            cr.put("LineId", lineId);
            cr.put("AccountId", purchaseGl);
            cr.put("AgainstAccountId", wip);
            cr.put("Comments", comment);
            cr.put("CreditAmount", amount);
            cr.put("QtyOut", toDouble(d.get("Qty")));
            cr.put("WeightOut", toDouble(d.get("Weight")));
            cr.put("ItemId", toInt(d.get("ItemId")));
            cr.put("ItemAmount", amount);
            cr.put("OrderNo", toInt(d.get("InvProductionJobOrderId")));
            cr.put("BranchesId", h.get("BranchId"));
            lines.add(cr);
            lineId++;
        }
        vh.put("VoucherAmount", total);
        return vh;
    }

    /** DAL InvFoodProduction.SetData (:2072) for DocumentTypeId 80, inside one transaction. */
    private int setData(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details, String removeIds,
                        int headerRefDocumentTypeId, Map<String, Object> voucher, List<Map<String, Object>> lines) {
        int org = toInt(h.get("OrganizationId")), comp = toInt(h.get("CompanyId"));
        boolean insert = toInt(h.get("Id")) == 0;

        /* header - Sp_InvFoodProduction_Insert / _Update (BLL chooses by Id == 0) */
        int result = repo.setProc(insert ? "Sp_InvFoodProduction_Insert" : "Sp_InvFoodProduction_Update",
                h, ProductionInputRepository.P_HEADER);
        int id;
        if (result > 0) { h.put("Id", result); id = result; } else { id = toInt(h.get("Id")); }

        /* details - LineId 1..n, InvFoodProductionId, Sp_InvFoodProductionDetail_Insert (upserts on Id) */
        int line = 0;
        for (Map<String, Object> d : details) {
            line++;
            d.put("LineId", line);
            d.put("InvFoodProductionId", id);
            int did = repo.setProc("Sp_InvFoodProductionDetail_Insert", d, ProductionInputRepository.P_DETAIL);
            d.put("Id", did);
        }

        /* IL_01b1 - DocumentTypeId 80: ERP feature 5 (read again by the DAL) and no detail with
           RefDocumentTypeId, RefDocNoId and RefDocSubIdNo all > 0 (local 9, set in the detail loop
           above) -> the FIFO branch; otherwise (IL_0b05) on insert only (ActionId == 1) the stock
           evaluation is rebuilt. */
        boolean loaderRow = false;
        for (Map<String, Object> d : details) {
            if (toInt(d.get("RefDocumentTypeId")) > 0 && toInt(d.get("RefDocNoId")) > 0 && toInt(d.get("RefDocSubIdNo")) > 0) {
                loaderRow = true;
            }
        }
        if (repo.erpFeature(u, ERP_FEATURE_FIFO_CGS) && !loaderRow) {
            fifoBranch(u, h, details, voucher, lines);
        } else if (toInt(h.get("ActionId")) == 1) {
            Map<String, Object> se = ProductionInputRepository.stockEvaluationDefaults();
            se.put("OrganizationId", org);
            se.put("CompanyId", comp);
            se.put("RefDocumentTypeId", DOC_TYPE_ID);
            se.put("RefDocIdNo", id);
            repo.setProc("Sp_InventoryStockEvalautionDetail_Update", se, ProductionInputRepository.P_STOCK_EVALUATION_UPDATE);
        }

        /* inventory transactions - the procedure deletes and rebuilds from the saved rows */
        Map<String, Object> it = ProductionInputRepository.inventoryTransactionsDefaults();
        it.put("OrganizationId", org);
        it.put("CompanyId", comp);
        it.put("RefDocumentTypeId", DOC_TYPE_ID);
        it.put("RefDocIdNo", id);
        repo.setProc("Sp_InventoryTransactions_GetALLMethod", it, ProductionInputRepository.P_INV_TRANSACTIONS);

        /* removed rows */
        if (removeIds != null && !removeIds.isEmpty()) {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@DocumentTypeId", DOC_TYPE_ID);
            p.put("@Id", id);
            p.put("@DetailIds", removeIds);
            repo.command("dbo.USP_InvFoodProductionDetailRowsDeleteByIds", p);
        }

        /* USP_InventoryValidation per detail (ExecuteScalar, result ignored - a RAISERROR guard) */
        for (Map<String, Object> d : details) {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@DocumentTypeId", DOC_TYPE_ID);
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
            repo.command("dbo.USP_InventoryValidation", p);
        }

        /* 80 and ActionId == 2 -> input/output weight validation */
        if (toInt(h.get("ActionId")) == 2) {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@DocumentTypeId", DOC_TYPE_ID);
            p.put("@InvProductionJobOrderId", h.get("InvJobOrderId"));
            p.put("@PlantId", h.get("PlantId"));
            repo.command("dbo.USP_ProductionInPutAndOutPutWeightValidation", p);
        }

        /* 80 -> per detail, the job-order weight comparison (@RefId = JobOrderInPutId) */
        for (Map<String, Object> d : details) {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@DocumentTypeId", DOC_TYPE_ID);
            p.put("@JobOrderId", h.get("InvJobOrderId"));
            p.put("@RefId", d.get("JobOrderInPutId"));
            p.put("@NetWeight", d.get("Weight"));
            repo.command("dbo.usp_ProductionWeightCompareFromJobOrderWeight", p);
        }

        /* voucher - unless the header's RefDocumentTypeId is 1 */
        if (headerRefDocumentTypeId != 1 && voucher != null) {
            int existing = repo.voucherHeadId(u, DOC_TYPE_ID, id);
            if (existing > 0) voucher.put("Id", existing);
            voucher.put("DocumentTypeSrNo", id);
            int vr = repo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update",
                    voucher, ProductionInputRepository.P_VOUCHER_HEAD);
            if (vr > 0) voucher.put("Id", vr);
            int voucherId = toInt(voucher.get("Id"));
            if (!lines.isEmpty()) {
                for (Map<String, Object> l : lines) {
                    l.put("VoucherHeadId", voucherId);
                    l.put("BranchesId", h.get("BranchId"));
                    int lid = toInt(l.get("LineId"));
                    for (Map<String, Object> d : details) {
                        if (toInt(d.get("LineId")) == lid && lid > 0) { l.put("RefDocSubIdNo", d.get("Id")); break; }
                    }
                    repo.setProc("Sp_VoucherDetail_Insert", l, ProductionInputRepository.P_VOUCHER_DETAIL);
                }
                if (voucherId > 0) {
                    LinkedHashMap<String, Object> p = ProductionInputRepository.params();
                    p.put("@OrganizationId", org);
                    p.put("@CompanyId", comp);
                    p.put("@Id", voucherId);
                    repo.command("dbo.USP_VoucherBalanceCheck", p);
                }
            }
        }
        return id;
    }

    // =============================================================== DAL SetData - FIFO branch

    /**
     * DAL InvFoodProduction.SetData IL_01dc - IL_0af5 (DocumentTypeId 80, ERP feature 5 on, no loader
     * row). Runs right after the detail inserts, on the save's transaction.
     *
     * Per detail (in grid order): the item's GL row (GetItemGlIdsandItemName, Where Id == ItemId -
     * a detail whose item has no GL row is skipped entirely); the job lot's GL row
     * (GetJobLotGlIdsandName, Where Id == JobLotId - "has an account" when the first match has
     * AccountId > 0); FIFOImplemention with the evaluations already taken by earlier details as
     * reservations. For every layer taken: when the job lot has NO account, two voucher lines are
     * appended to the voucher MakeVoucher built (a NullReferenceException when there is none, i.e.
     * InventoryFinancialsEffectsInActive); the layer is kept in any case.
     *
     * Then, when at least one layer was taken: on update (ModifyUser > 0) the old stock-out rows
     * are reversed and deleted, and every layer is inserted as a stock-evaluation row.
     *
     * The lambdas <SetData>b__0 / b__1 / b__2 are compiler-generated and not in the disassembly;
     * they are read as ItemId, JobLotId and LineId matches (the only fields that fit their
     * inputs, the same reading the Consumption port uses).
     */
    private void fifoBranch(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details,
                            Map<String, Object> voucher, List<Map<String, Object>> lines) {
        int headerId = toInt(h.get("Id"));
        int modifyUser = toInt(h.get("ModifyUser"));
        LocalDateTime docDate = (LocalDateTime) h.get("DocDate");
        List<Map<String, Object>> itemGl = repo.itemGlAccounts(u);
        List<Map<String, Object>> jobLotGl = repo.jobLotGlIdsAndName(u);
        List<Map<String, Object>> evaluations = new ArrayList<>();   // InventoryStockEvalautionDetailslist

        for (Map<String, Object> item : new ArrayList<>(details)) {
            Map<String, Object> gl = firstById(itemGl, toInt(item.get("ItemId")));
            if (gl == null) continue;
            /* IL_0306 - DocumentTypeId / Id go to the FIFO query only when ModifyUser > 0 (update) */
            int rpDocType = 0, rpId = 0;
            if (modifyUser > 0) { rpDocType = toInt(h.get("DocumentTypeId")); rpId = headerId; }
            Map<String, Object> jl = firstById(jobLotGl, toInt(item.get("JobLotId")));
            boolean jobLotHasAccount = jl != null && toInt(ci(jl, "AccountId")) > 0;

            for (Map<String, Object> se : fifoImplementation(u, item, docDate, rpDocType, rpId, evaluations)) {
                if (!jobLotHasAccount) {
                    /* IL_03fe - no rounding, .NET double.ToString() */
                    String comments = "ItemQty: " + net(toDouble(se.get("QtyOut"))) + "   " + str(ci(gl, "ItemName"))
                            + "  Net Weight: " + net(toDouble(se.get("StockWeightOut")))
                            + "  CGS Rate:" + net(toDouble(se.get("CgsRate")));
                    Map<String, Object> dr = fifoVoucherLine(h, item, gl, se, comments, true);
                    if (voucher == null) throw new InputMessage("Object reference not set to an instance of an object.");
                    lines.add(dr);
                    lines.add(fifoVoucherLine(h, item, gl, se, comments, false));
                }
                evaluations.add(se);
            }
        }

        if (evaluations.isEmpty()) return;
        int org = toInt(h.get("OrganizationId")), comp = toInt(h.get("CompanyId"));
        if (modifyUser > 0) {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@RefDocumentTypeId", h.get("DocumentTypeId"));
            p.put("@RefDocIdNo", headerId);
            repo.command("[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", p);
        }
        for (Map<String, Object> se : evaluations) {
            se.put("OrganizationId", org);
            se.put("CompanyId", comp);
            se.put("DocDate", docDate);
            se.put("DocCodeNo", toInt(h.get("DocCode")));
            se.put("SupplierCustomerId", 0);
            se.put("BranchesId", h.get("BranchId"));
            se.put("RefDocumentTypeId", h.get("DocumentTypeId"));
            se.put("EntryUser", h.get("EntryUser"));
            se.put("ModifyUser", h.get("ModifyUser"));
            se.put("CalcType", "Weight");
            /* IL_09d5 - invFoodProductionDetails.Find(<SetData>b__2): the detail with the same LineId */
            Map<String, Object> d = null;
            for (Map<String, Object> x : details) {
                if (toInt(x.get("LineId")) == toInt(se.get("LineId"))) { d = x; break; }
            }
            if (d != null) {
                double eq = repo.equivalentByItemAndSchedule(u, toInt(se.get("ItemId")), toInt(d.get("RateUOMId")));
                double rate = toDouble(d.get("Rate"));
                if (toDouble(se.get("BillWeightOut")) > 0.0 && rate > 0.0 && eq > 0.0) {
                    se.put("AmountOut", toDouble(se.get("BillWeightOut")) / eq * rate);
                }
                se.put("ItemRate", rate);
                se.put("RefDocIdNo", d.get("InvFoodProductionId"));
                se.put("RefDocSubIdNo", d.get("Id"));
            }
            /* no detail found -> RefDocIdNo stays 0 and the procedure raises
               "RefDocId Not Found in Stock Evalaution" - as on the desktop */
            repo.setProc("USP_InventoryStockEvalautionDetail_Insert", se, ProductionInputRepository.P_STOCK_EVALUATION_INSERT);
        }
    }

    /** The two FIFO cost lines (IL_0479 debit WIP / IL_0635 credit the item's purchase GL). */
    private static Map<String, Object> fifoVoucherLine(Map<String, Object> h, Map<String, Object> item,
                                                       Map<String, Object> gl, Map<String, Object> se,
                                                       String comments, boolean debit) {
        Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
        int wip = toInt(h.get("WIPAccountId"));
        int purchaseGl = toInt(ci(gl, "PurchaseGLAC"));
        double cgsAmount = toDouble(se.get("CgsAmount"));
        v.put("LineId", toInt(item.get("LineId")));
        v.put("AccountId", debit ? wip : purchaseGl);
        v.put("AgainstAccountId", debit ? purchaseGl : wip);
        v.put("Comments", comments);
        v.put(debit ? "DebitAmount" : "CreditAmount", cgsAmount);
        v.put("DocumentTypeIdRef", toInt(item.get("RefDocumentTypeId")));
        v.put("InvoiceNoRefId", toInt(item.get("RefDocNoId")));
        v.put("RefInvoiceNo", String.valueOf(toInt(item.get("RefDocSubIdNo"))));
        v.put("ItemId", toInt(item.get("ItemId")));
        v.put("QtyOut", toDouble(se.get("QtyOut")));
        v.put("WeightOut", toDouble(se.get("StockWeightOut")));
        v.put("ItemCgsRate", toDouble(se.get("CgsRate")));
        v.put("RateCut", 0.0);
        v.put("RateCutAmount", 0.0);
        v.put("ItemAmount", cgsAmount);
        v.put("JobLotId", toInt(item.get("JobLotId")));
        v.put("BranchesId", h.get("BranchId"));
        v.put("OrderNo", toInt(item.get("InvProductionJobOrderId")));
        return v;
    }

    /**
     * DAL CommonServices.FIFOImplemention (Architecture.DAL.Common :1759) for one detail, statement for
     * statement. ReportsParameters.ItemName is never set by SetData, so every message carries an
     * empty name (String.Concat of null) - reproduced.
     */
    private List<Map<String, Object>> fifoImplementation(UserAccount u, Map<String, Object> item, LocalDateTime docDate,
                                                         int docType, int id, List<Map<String, Object>> reserved) {
        String itemName = "";
        String xml = reserved.isEmpty() ? null : fifoXml(reserved);
        List<Map<String, Object>> rows = repo.stockByFifoForSave(u, toInt(item.get("ItemId")), toInt(item.get("ItemUomId")),
                docDate, toInt(item.get("WarehouseId")), toInt(item.get("JobLotId")), toInt(item.get("PackingtypeId")),
                (String) item.get("CropBatch"), docType, id, xml);
        if (rows.isEmpty()) throw new InputMessage("Stock Not Found this Item " + itemName + " against FIFO Method .....");
        double available = 0.0;                       // <FIFOImplemention>b__21_0 - Sum(NetBalWeight)
        for (Map<String, Object> r : rows) available += toDouble(ci(r, "NetBalWeight"));
        double itemQty = toDouble(item.get("Qty"));
        double netWeight = toDouble(item.get("Weight"));
        if (!(netWeight <= netRound2(available))) {  // bgt.un
            throw new InputMessage("Weight available is " + net(available) + " and row Weight is " + net(netWeight)
                    + " this item " + itemName + " against FIFO....");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        double l12 = netWeight, l13 = itemQty, l15 = 0.0, l17 = 0.0;
        for (Map<String, Object> r : rows) {
            double avgRate = toDouble(ci(r, "AvgRate"));
            if (!(avgRate > 0.0)) throw new InputMessage("Rate Not Found this Item " + itemName + " against FIFO Method");
            int rateUomId = toInt(ci(r, "RateUomId"));
            if (rateUomId == 0) throw new InputMessage("RateUomId not found  this " + itemName + " against FIFO Method");
            double l16 = toDouble(ci(r, "NetBalWeight"));
            double l14 = toDouble(ci(r, "NetBalQty"));
            double eq = repo.equivalentByItemAndSchedule(u, toInt(item.get("ItemId")), rateUomId);
            if (eq == 0.0) throw new InputMessage("RateUom Not Found");
            Map<String, Object> se = ProductionInputRepository.stockEvaluationDefaults();
            if (!(l16 > l12 - l17)) {                 // the whole layer
                l17 += l16;
                l15 += l14;
                fillEvaluation(se, r, item, rateUomId);
                se.put("QtyOut", l14);
                se.put("BillWeightOut", l16);
                se.put("StockWeightOut", l16);
                se.put("CgsRate", avgRate * eq);
                se.put("CgsAmount", l16 / eq * (avgRate * eq));
                out.add(se);
            } else if (!(l16 < l12 - l17)) {          // the rest of the row from this layer
                fillEvaluation(se, r, item, rateUomId);
                double q = l13 - l15, w = l12 - l17;
                se.put("QtyOut", q);
                se.put("BillWeightOut", w);
                se.put("StockWeightOut", w);
                se.put("CgsRate", avgRate * eq);
                se.put("CgsAmount", w / eq * (avgRate * eq));
                l15 += q;
                l17 += w;
                out.add(se);
            }
            if (netWeight == l17) break;
        }
        return out;
    }

    private static void fillEvaluation(Map<String, Object> se, Map<String, Object> r, Map<String, Object> item, int rateUomId) {
        se.put("Id", toInt(ci(r, "Id")));
        se.put("LineId", toInt(item.get("LineId")));
        se.put("ItemId", toInt(item.get("ItemId")));
        se.put("WarehouseId", toInt(item.get("WarehouseId")));
        se.put("RateUom", rateUomId);
        se.put("JobLotId", toInt(item.get("JobLotId")));
        se.put("InvPackingTypeId", toInt(item.get("PackingtypeId")));
        se.put("ItemUom", toInt(item.get("ItemUomId")));
        se.put("CropBatch", str(item.get("CropBatch")));
        se.put("RefRefDocumentTypeId", toInt(ci(r, "RefDocumentTypeId")));
        se.put("RefRefDocIdNo", toInt(ci(r, "RefDocIdNo")));
        se.put("RefRefDocSubIdNo", toInt(ci(r, "RefDocSubIdNo")));
    }

    /**
     * The List&lt;FIFOStockEvaluation&gt; FIFOImplemention serialises into @FIFOXML - one element per
     * reservation (RefRef* ids, QtyOut, StockWeightOut). The procedure reads exactly these five
     * child elements through .nodes('//ArrayOfFIFOStockEvaluation/FIFOStockEvaluation'); the
     * serializer's other (default-valued) elements are not read by it and are not written here.
     */
    private static String fifoXml(List<Map<String, Object>> reserved) {
        StringBuilder sb = new StringBuilder("<ArrayOfFIFOStockEvaluation xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                + "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
        for (Map<String, Object> r : reserved) {
            sb.append("<FIFOStockEvaluation>")
              .append("<RefDocumentTypeId>").append(toInt(r.get("RefRefDocumentTypeId"))).append("</RefDocumentTypeId>")
              .append("<RefDocIdNo>").append(toInt(r.get("RefRefDocIdNo"))).append("</RefDocIdNo>")
              .append("<RefDocSubIdNo>").append(toInt(r.get("RefRefDocSubIdNo"))).append("</RefDocSubIdNo>")
              .append("<ReserveQty>").append(xmlDouble(toDouble(r.get("QtyOut")))).append("</ReserveQty>")
              .append("<ReserveWeight>").append(xmlDouble(toDouble(r.get("StockWeightOut")))).append("</ReserveWeight>")
              .append("</FIFOStockEvaluation>");
        }
        return sb.append("</ArrayOfFIFOStockEvaluation>").toString();
    }

    /** XmlConvert.ToString(double) - the "R" round-trip form. */
    private static String xmlDouble(double d) {
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }

    /** .NET Framework Math.Round(value, 2): scale, round half to even, unscale (|value| < 1e16). */
    private static double netRound2(double v) {
        if (Math.abs(v) >= 1e16) return v;
        return Math.rint(v * 100.0) / 100.0;
    }

    private static Map<String, Object> firstById(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(ci(r, "Id")) == id) return r;
        return null;
    }

    // ================================================================================= helpers

    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    /** .NET Framework double.ToString(): up to 15 significant digits, no trailing zeros. */
    static String net(double d) {
        if (Double.isNaN(d)) return "NaN";
        if (Double.isInfinite(d)) return d > 0 ? "Infinity" : "-Infinity";
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return new BigDecimal(d).round(new MathContext(15)).stripTrailingZeros().toPlainString();
    }

    /**
     * A DateTimePicker value: "yyyy-MM-dd" or "yyyy-MM-ddTHH:mm[:ss]". A date-only value takes the
     * current time of day, which is what an untouched DateTimePicker carries on the desktop.
     */
    static LocalDateTime dateTime(String s) {
        LocalDateTime v = dateTimeOrNull(s);
        return v == null ? LocalDateTime.now() : v;
    }

    static LocalDateTime dateTimeOrNull(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        String t = s.trim();
        try {
            if (t.length() <= 10) return LocalDate.parse(t).atTime(LocalTime.now().withNano(0));
            return LocalDateTime.parse(t.length() == 16 ? t + ":00" : t.substring(0, Math.min(19, t.length())));
        } catch (Exception e) {
            throw new InputMessage("Invalid date: " + s);
        }
    }

    public static String rootMessage(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.isEmpty()) m = e.getMessage();
        return m == null ? e.getClass().getSimpleName() : m;
    }

    /** Keeps the named columns (case-insensitive read), under the names given. */
    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : cols) m.put(c, ci(r, c));
            out.add(m);
        }
        return out;
    }
}
