package com.mst.services;

import com.mst.repositories.ProductionPackingMaterialRepository;
import com.mst.repositories.ProductionPackingMaterialRepository.PmRow;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.NestedExceptionUtils;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ProductionPackingMaterialRepository.ci;
import static com.mst.repositories.ProductionPackingMaterialRepository.toInt;

/**
 * Screen 280, tab "PackingMaterial" - frmProductionPackingMaterial.cs (DocumentTypeId 111).
 *
 * Tenancy (organization, company, branch, financial year) and the user id come from
 * CurrentUserContext only; nothing the browser sends decides whose data is read or written.
 * Line numbers are src280/frmProductionPackingMaterial.cs.
 */
@Service
public class ProductionPackingMaterialService {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionPackingMaterialService.class);

    /** frmFoodFood_Load:279-286 - the shell's name when the user has any view-rights row for it,
     *  otherwise the form's own base.Name. */
    public static final String SHELL_SCREEN_NAME = "FoodProductionWithValues";
    public static final String FORM_SCREEN_NAME  = "frmProductionPackingMaterial";

    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?";

    @Autowired private ProductionPackingMaterialRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private JdbcTemplate jdbc;
    @Autowired(required = false) private DashboardModuleService dashboard;

    private int org()    { return ctx.currentOrganizationId(); }
    private int comp()   { return ctx.currentCompanyId(); }
    private int branch() { return ctx.currentBranchId(); }
    private int fy()     { return ctx.currentFinancialYearId(); }
    private int user()   { return ctx.currentUserId(); }

    // =============================================================================== rights

    /**
     * :279 `clsGlobalVariables.ScreenViewReights.Any(dd => dd.ScreenName == "FoodProductionWithValues")`
     * - ScreenViewReights is USP_GetUserRightsForViewbyUserId, read here through
     * DashboardModuleService.viewRights(), the port's existing reader of that list. The test is
     * ordinal and ignores Value, exactly like the lambda.
     */
    public String rightsScreenName() {
        try {
            if (dashboard != null) {
                for (Map<String, Object> r : dashboard.viewRights()) {
                    Object n = ci(r, "ScreenName");
                    if (n != null && SHELL_SCREEN_NAME.equals(String.valueOf(n))) return SHELL_SCREEN_NAME;
                }
            }
        } catch (Exception e) {
            LOG.warn("View-rights list could not be read; falling back to {}", FORM_SCREEN_NAME, e);
        }
        return FORM_SCREEN_NAME;
    }

    /**
     * CommonServices.SetRightsValueInRightsObject(screen): role "Admin" (ordinal) starts with Save,
     * Update and Print; the grant rows then apply (Admin keeps Save/Update/Print). "Rate" ->
     * DoHaveCanRateandAmount comes from the rows only. An unreadable grid grants nothing extra.
     */
    public Map<String, Object> rights() {
        String screen = rightsScreenName();
        String role = ctx.currentRoleName();
        boolean admin = "Admin".equals(role);
        Map<String, Object> r = new LinkedHashMap<>();
        boolean save = admin, update = admin, print = admin, rate = false;
        try {
            for (Map<String, Object> row : jdbc.queryForList(SQL_USER_RIGHTS, user(), screen,
                    role == null ? "" : role, comp(), "GetByUserId")) {
                String name = String.valueOf(ci(row, "RightName") == null ? "" : ci(row, "RightName"));
                boolean v = toBool(ci(row, "Value"));
                switch (name) {
                    case "Save":   save = admin || v; break;
                    case "Update": update = admin || v; break;
                    case "Print":  print = admin || v; break;
                    case "Rate":   rate = v; break;
                    default: break;
                }
            }
        } catch (Exception e) {
            LOG.warn("Rights for {} could not be read", screen, e);
        }
        r.put("screenName", screen);
        r.put("save", save);
        r.put("update", update);
        r.put("print", print);
        r.put("rateAndAmount", rate);
        return r;
    }

    // ================================================================================ reads

    /** Load:272-346 in one read: rights, doc no, the four pickers Load binds, the history job
     *  orders (:338), the pending schedules (:323, RecId 0), formats and the default warehouse. */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights());
        out.put("docNo", repo.generateCode(org(), comp(), fy(), branch()));
        out.putAll(refresh(0));
        out.put("historyJobOrders", historyJobOrders());
        out.put("rateDecimals", decimals("Default NoofDecimal Points For Rate", true));
        out.put("amountDecimals", decimals("Default NoofDecimal Points For Amount", false));
        String wh = safeConfig("PackingMaterialDefaultWarehouse");
        out.put("defaultWarehouseId", toInt(wh));
        return out;
    }

    /** btnRefreshPackingMaterial_Click:1000 - JobOrderNoFill, ItemFillPackingMaterial,
     *  ItemConditionBindFromGlobal, ScheduleNoDbCall(RecpackingMaterial). */
    public Map<String, Object> refresh(int recId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("jobOrders", repo.jobOrders(org(), comp(), branch()));
        out.put("items", pmItems());
        out.put("itemConditions", itemConditions());
        out.put("schedules", repo.pendingSchedules(org(), comp(), recId));
        out.put("scheduleRecId", recId);
        return out;
    }

    /** ItemFillPackingMaterial:694 - global items with ItemTypeOfTypeId == 14. */
    private List<Map<String, Object>> pmItems() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.allItems(org(), comp())) {
            if (toInt(ci(r, "ItemTypeOfTypeId")) != 14) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("ItemName", ci(r, "ItemName"));
            m.put("ItemCode", ci(r, "ItemCode"));
            out.add(m);
        }
        return out;
    }

    /** ItemConditionBindFromGlobal:651 - the global list without Id 4. */
    private List<Map<String, Object>> itemConditions() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.itemConditions()) {
            if (toInt(ci(r, "Id")) == 4) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("Description", ci(r, "ConditionStatus"));
            out.add(m);
        }
        return out;
    }

    /** HistoryComboDBCall:2080 - Activity 'ProductionJobOrder'. */
    public List<Map<String, Object>> historyJobOrders() {
        return repo.historyDropDown(org(), comp(), fy(), "ProductionJobOrder");
    }

    public int nextCode() { return repo.generateCode(org(), comp(), fy(), branch()); }

    /** GetPlantFeeder:391. */
    public List<Map<String, Object>> plants(int jobOrderId) {
        return repo.plants(org(), comp(), fy(), branch(), jobOrderId);
    }

    /** GetTotalInPutOutOnPM:834. */
    public List<Map<String, Object>> totals(int jobOrderId, int plantId) {
        return repo.totals(org(), comp(), jobOrderId, plantId);
    }

    /** GetBrandItemsAndUomForPackMaterialAgainstPmItem - the three callers differ only in Id. */
    public List<Map<String, Object>> brandItems(int jobOrderId, int itemId, int pmDocId) {
        return repo.brandItems(org(), comp(), jobOrderId, itemId, pmDocId);
    }

    /** racksWithWarehouseAndItems filtered to one item, as BindWarehouseDropdown:715 and
     *  RackBindFromGlobalRacksByItemId:749 filter the global list. The rows keep the global
     *  list's order; the page does the grouping. */
    public List<Map<String, Object>> racks(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.racksWithWarehouseAndItems(org(), comp(), branch())) {
            if (toInt(ci(r, "ItemId")) != itemId) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("RackName", ci(r, "RackName"));
            m.put("WarehouseId", toInt(ci(r, "invWarehouseId")));
            m.put("WareHouseName", ci(r, "WareHouseName"));
            m.put("BaseRackId", toInt(ci(r, "BaseRackId")));
            out.add(m);
        }
        return out;
    }

    /**
     * GetAvgRateByItemAndWarehouseForPackingMAterial:1832 / AverageCalculate:1964, non-FIFO
     * branch: CommonServices.GetAvgRateQtyAndStockInHand(item, docDate, itemCondition, Rec,
     * Rec > 0 ? 111 : 0) and Math.Round(AvgRate, 3). FIFOCGSFlag is never assigned in this form,
     * so the FIFO branch is unreachable - see the page script.
     */
    public double avgRate(int itemId, String docDateTime, int itemConditionId, int recId) {
        List<Map<String, Object>> rows = repo.avgRate(org(), comp(), itemId, parseTs(docDateTime),
                itemConditionId, recId, recId > 0 ? ProductionPackingMaterialRepository.DOC_TYPE_ID : 0);
        if (rows.isEmpty()) return 0.0;
        return Math.rint(toDouble(ci(rows.get(0), "AvgRate")) * 1000.0) / 1000.0;
    }

    /** ReadByIdPackingMaterial:1253 - the row, the tenancy check the desktop never needed, and
     *  GetVoucherHeadId(Rec, 111):1314. */
    public Map<String, Object> readById(int id) {
        Map<String, Object> row = repo.readById(id);
        Map<String, Object> out = new LinkedHashMap<>();
        if (row == null) {
            out.put("found", false);
            return out;
        }
        if (toInt(ci(row, "OrganizationId")) != org() || toInt(ci(row, "CompanyId")) != comp()) {
            out.put("found", false);
            return out;
        }
        out.put("found", true);
        out.put("row", localDates(row));
        out.put("voucherHeadId", repo.voucherHeadId(org(), comp(), ProductionPackingMaterialRepository.DOC_TYPE_ID, id));
        return out;
    }

    /** BindHistoryPMGrid:1324. Dates arrive as "yyyy-MM-ddTHH:mm:ss" (the picker keeps its time
     *  of day) or empty when the picker is unticked. */
    public List<Map<String, Object>> history(boolean pendingForRates, String dateMode,
                                             String from, String to,
                                             String docNoFrom, String docNoTo, int jobOrderId) {
        Timestamp f = parseTs(from), t = parseTs(to);
        Timestamp df = null, dt = null, ef = null, et = null, mf = null, mt = null, af = null, at = null;
        if ("doc".equals(dateMode))           { df = f; dt = t; }
        else if ("entry".equals(dateMode))    { ef = f; et = t; }
        else if ("modify".equals(dateMode))   { mf = f; mt = t; }
        else if ("approved".equals(dateMode)) { af = f; at = t; }
        List<Map<String, Object>> rows = repo.history(org(), comp(), fy(), branch(), pendingForRates ? 1 : 0,
                df, dt, ef, et, mf, mt, af, at,
                toIntText(docNoFrom), toIntText(docNoTo), jobOrderId);
        for (Map<String, Object> r : rows) localDates(r);
        return rows;
    }

    /** btnPackingMaterialSlip_Click:1492 - does the report have rows ("Record Not Found For DisPlay"). */
    public int report606Rows(int jobOrderId) {
        return repo.report606(org(), comp(), jobOrderId, "PackingMaterial").size();
    }

    // ================================================================================ save

    /** One row of tablePackingMaterial as the page holds it (Load:292-315). */
    public static class GridRow {
        public Integer id;
        public String docDate;
        public Integer docNo;
        public Integer plantId;
        public Integer packingItemId;
        public Integer warehouseId;
        public Integer rackId;
        public String rackName;
        public Integer itemConditionId;
        public String itemCondition;
        public Integer brandId;
        public Integer brandUomId;
        public Double itemQty;
        public Double itemRate;
        public Double amount;
        public String remarks;
        public Integer contractScheduleId;
        public String contractScheduleNo;
    }

    public static class SaveRequest {
        /** RecpackingMaterial: 0 from Save (btnsavepackingmaterial_Click zeroes it), else Update. */
        public int recId;
        /** cmbJobOrderPackingMaterial.Value - every row gets it (:1134). */
        public int jobOrderId;
        /** the RecId dtScheduleData was last read with (Load: 0; Refresh: RecpackingMaterial). */
        public int scheduleRecId;
        public List<GridRow> rows = new ArrayList<>();
    }

    /**
     * PackingMaterialInsert:1096 from the grid loop onward (FormValidation and the Yes/No prompt
     * run in the page first, as on the desktop). Row checks and messages verbatim; a message that
     * the desktop shows and then `return`s from, and one it throws, both stop the save here.
     */
    public Map<String, Object> save(SaveRequest req) {
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> rights = rights();
        boolean isUpdate = req.recId > 0;
        if (isUpdate ? !Boolean.TRUE.equals(rights.get("update")) : !Boolean.TRUE.equals(rights.get("save"))) {
            return fail(out, isUpdate ? "You do not have the Update right on this screen."
                                      : "You do not have the Save right on this screen.");
        }
        boolean rateRight = Boolean.TRUE.equals(rights.get("rateAndAmount"));

        try {
            /* SelectedRow.Cells["DocumentTypeId"] of the header job order (:1135). */
            Map<String, Object> jobOrder = null;
            List<PmRow> list = new ArrayList<>();
            if (req.rows != null && !req.rows.isEmpty()) {
                for (Map<String, Object> jo : repo.jobOrders(org(), comp(), branch())) {
                    if (toInt(ci(jo, "Id")) == req.jobOrderId) { jobOrder = jo; break; }
                }
                List<Map<String, Object>> schedules = repo.pendingSchedules(org(), comp(), req.scheduleRecId);
                LocalDateTime now = LocalDateTime.now();

                for (int i = 0; i < req.rows.size(); i++) {
                    GridRow r = req.rows.get(i);
                    /* :1128 - an int cell's text is never empty, so this fires only on a missing value. */
                    if (r.plantId == null) return fail(out, "Plant Field Required in Detail");
                    PmRow m = new PmRow();
                    m.plantId = r.plantId;
                    m.invProductionJobOrderId = req.jobOrderId;
                    if (jobOrder == null) return fail(out, "Object reference not set to an instance of an object.");
                    if (toInt(ci(jobOrder, "DocumentTypeId")) == 401) m.baseDocumentTypeId = 2;
                    m.documentTypeId = ProductionPackingMaterialRepository.DOC_TYPE_ID;
                    m.entryDate = now;
                    m.entryUser = user();
                    m.modifyDate = now;
                    m.modifyUser = user();
                    m.organizationId = org();
                    m.companyId = comp();
                    m.branchesId = branch();
                    m.financialYearId = fy();
                    m.id = nz(r.id);
                    m.docNo = nz(r.docNo);
                    m.docDate = parseDate(r.docDate);
                    m.itemId = nz(r.packingItemId);
                    m.warehouseId = nz(r.warehouseId);
                    m.rackId = nz(r.rackId);
                    if (m.rackId == 0) return fail(out, "Rack Name field required in grid at row#" + (i + 1));
                    m.itemConditionId = nz(r.itemConditionId);
                    if (m.itemConditionId == 0) return fail(out, "Item Condition field required in grid at row#" + (i + 1));
                    m.brandId = nz(r.brandId);
                    m.brandUomId = nz(r.brandUomId);
                    /* FormHelper.ValidateField(ItemId, "Packing Item", RowIndex) */
                    if (m.itemId == 0) return fail(out, "Packing Item is required in Detail Grid at row No: " + (i + 1));
                    /* :1170 - RowIndex, NOT RowIndex + 1: the desktop's message is one row low. */
                    if (m.brandId == 0 && m.brandUomId != 0) return fail(out, "OutPut Item Field Require in Detail grid at row#" + i);
                    if (m.brandUomId == 0 && m.brandId != 0) return fail(out, "OutPut Uom Field Require in Detail");
                    double qty = nzd(r.itemQty), rate = nzd(r.itemRate), amount = nzd(r.amount);
                    /* :1178 - the ItemRate cell's text is never empty, so a zero qty always stops here. */
                    if (qty == 0.0) return fail(out, "ItemQty Required in Detail");
                    m.qty = qty;
                    if (rate == 0.0 && rateRight) return fail(out, "ItemRate Required in Detail");
                    m.rate = rate;
                    if (amount == 0.0 && rateRight) return fail(out, "Amount Required in Detail");
                    m.amount = amount;
                    m.contractScheduleId = nz(r.contractScheduleId);
                    m.pmRemarks = r.remarks == null ? "" : r.remarks;
                    for (Map<String, Object> s : schedules) {
                        if (toInt(ci(s, "Id")) == m.contractScheduleId) {
                            m.exImInvoiceId = toInt(ci(s, "InvoiceId"));
                            break;
                        }
                    }
                    /* A row read by id belongs to this company, or it is not written. */
                    if (m.id > 0) {
                        Map<String, Object> existing = repo.readById(m.id);
                        if (existing == null || toInt(ci(existing, "OrganizationId")) != org()
                                || toInt(ci(existing, "CompanyId")) != comp()) {
                            return fail(out, "Record not found.");
                        }
                    }
                    list.add(m);
                }
                if (list.isEmpty()) return fail(out, "Grid Record not found please check!");
            }

            /* BLL Save always reaches SetData; an empty list commits nothing and the form still
               says "Save Successfully". */
            if (!list.isEmpty()) {
                boolean finOff = toBool(safeConfig("InventoryFinancialsEffectsInActive"));
                repo.save(list, finOff);
            }
            out.put("success", true);
            out.put("message", isUpdate ? "Update Successfully" : "Save Successfully");
            return out;
        } catch (Exception e) {
            LOG.warn("Packing material save failed", e);
            return fail(out, rootMessage(e));
        }
    }

    // ============================================================================== helpers

    /** Dates leave as the server's local "yyyy-MM-ddTHH:mm:ss", never as a UTC instant: a
     *  midnight DocDate serialised in UTC would show the previous day east of Greenwich. */
    private static Map<String, Object> localDates(Map<String, Object> row) {
        for (Map.Entry<String, Object> e : row.entrySet()) {
            Object v = e.getValue();
            if (v instanceof Timestamp) e.setValue(((Timestamp) v).toLocalDateTime().withNano(0).toString());
            else if (v instanceof java.sql.Date) e.setValue(((java.sql.Date) v).toLocalDate().toString());
            else if (v instanceof java.util.Date) e.setValue(new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString());
            else if (v instanceof LocalDateTime) e.setValue(((LocalDateTime) v).withNano(0).toString());
        }
        return row;
    }

    private static Map<String, Object> fail(Map<String, Object> out, String message) {
        out.put("success", false);
        out.put("message", message);
        return out;
    }

    public static String rootMessage(Throwable e) {
        Throwable c = NestedExceptionUtils.getMostSpecificCause(e);
        String m = c == null ? null : c.getMessage();
        if (m == null || m.isEmpty()) m = e.getMessage();
        return m == null ? e.getClass().getSimpleName() : m;
    }

    private String safeConfig(String name) {
        try { return repo.config(org(), comp(), name); } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read", name, e);
            return null;
        }
    }

    /** CommonServices.GetDecimalConfiguration, as StockConversionService reads it. */
    private int decimals(String name, boolean rate) {
        int n = 0;
        try {
            String v = repo.config(org(), comp(), name);
            n = v == null || v.trim().isEmpty() ? 0 : (int) Math.floor(Double.parseDouble(v.trim()));
        } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read", name, e);
        }
        if (n >= 1 && n <= 4) return n;
        return (rate && n == 0) ? 2 : 0;
    }

    private static int nz(Integer v) { return v == null ? 0 : v; }
    private static double nzd(Double v) { return v == null ? 0.0 : v; }

    private static double toDouble(Object o) {
        if (o == null) return 0.0;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0.0; }
    }

    /** Conversion.ToInt(text) of a doc-no box: blank or non-numeric is 0. */
    private static double toIntText(String s) {
        if (s == null || s.trim().isEmpty()) return 0.0;
        try { return Integer.parseInt(s.trim()); } catch (Exception e) { return 0.0; }
    }

    private static boolean toBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim().toLowerCase();
        return "1".equals(s) || "true".equals(s) || "yes".equals(s);
    }

    /** "yyyy-MM-dd" or "yyyy-MM-ddTHH:mm[:ss]" -> Timestamp; blank -> null (the parameter is omitted). */
    private static Timestamp parseTs(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        String v = s.trim();
        if (v.length() == 10) return Timestamp.valueOf(LocalDate.parse(v).atStartOfDay());
        return Timestamp.valueOf(LocalDateTime.parse(v.length() == 16 ? v + ":00" : v.substring(0, 19)));
    }

    /** A row's DocDate is ToShortDateString() of the picker - the date at midnight (:1621). */
    private static LocalDateTime parseDate(String s) {
        if (s == null || s.trim().isEmpty()) return LocalDate.now().atStartOfDay();
        return LocalDate.parse(s.trim().substring(0, 10)).atStartOfDay();
    }
}
