package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.StockConversionLookupsRepository;
import com.mst.repositories.StockConversionRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Stock Conversion — invfrmStockConversionProduction.cs, DocTypeId 66.
 *
 * Read and write. Save / Update run DAL InvStockConversion.SetData through
 * {@link StockConversionRepository#save}; Delete is InvPurchaseInvoice.RemoveByID for 66.
 *
 * Tenancy, financial year, CanViewAllRecord and EntryUser are all server-derived. Nothing that
 * decides which company's documents are returned comes from the request.
 */
@Service
public class StockConversionService {

    private static final Logger LOG = LoggerFactory.getLogger(StockConversionService.class);

    /** The desktop form name, for the per-screen grant lookup. */
    private static final String SCREEN_NAME = "invfrmStockConversionProduction";
    private static final String RIGHT_CAN_VIEW_ALL_RECORDS = "CanView AllRecord";
    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
          + "@CompanyId=?, @Activity=?";

    @Autowired private StockConversionRepository repo;
    @Autowired private StockConversionLookupsRepository lookups;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;

    // ==================================================================================== read

    /** The header with its three child grids, the way DAL 0275 GetData assembles them. */
    public Map<String, Object> load(int id) {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> head = repo.header(id);
        if (head == null) return null;

        /* ReadById filters on Id alone. The desktop only ever asks for ids its own history
           listed, so it never meets another company's row; a URL can. A header outside the
           signed-in organisation/company is answered exactly like a missing one. */
        if (asInt(ci(head, "OrganizationId")) != (u.getOrganizationId() == null ? 0 : u.getOrganizationId())
                || asInt(ci(head, "CompanyId")) != (u.getCompanyId() == null ? 0 : u.getCompanyId())) {
            return null;
        }

        /* DAL GetData: details are read with ReadByHeaderId for DocTypeId 66 and 808, with
           ReadByHeaderIdForTrading for 67, and NOT AT ALL for any other type. */
        int docTypeId = asInt(head.get("DocTypeId"));
        if (docTypeId == 0) docTypeId = StockConversionRepository.DOC_TYPE_ID;

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("header", head);
        res.put("details", (docTypeId == 66 || docTypeId == 808 || docTypeId == 67)
                ? repo.details(id, docTypeId) : new ArrayList<Map<String, Object>>());
        res.put("packings", repo.packings(id));
        res.put("expenses", repo.expenses(id));
        return res;
    }

    public int nextCode() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.nextCode(u, currentUserContext.currentFinancialYearId());
    }

    public int idByDocNo(int docSrNo) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.idByDocNo(u, docSrNo, currentUserContext.currentFinancialYearId());
    }

    /** Every filter is optional, and an unset one is OMITTED rather than sent as null. */
    public Map<String, Object> history(String fromDate, String toDate,
                                       String entryFromDate, String entryToDate,
                                       String modifyFromDate, String modifyToDate,
                                       String approvedDateFrom, String approvedDateTo,
                                       Integer docNoFrom, Integer docNoTo) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean canViewAll = canViewAllRecords(u);
        List<Map<String, Object>> rows = repo.history(
                u, currentUserContext.currentFinancialYearId(), canViewAll, u.getId(),
                fromDate, toDate, entryFromDate, entryToDate, modifyFromDate, modifyToDate,
                approvedDateFrom, approvedDateTo, docNoFrom, docNoTo);
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("canViewAllRecords", canViewAll);
        res.put("rows", rows);
        return res;
    }

    public List<Map<String, Object>> stockFilter(String activity, Integer itemCategoryId,
                                                 String docDateTo, Integer warehouseId,
                                                 String cropYear, Integer itemId, Integer jobLotId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (activity == null || activity.trim().isEmpty()) {
            throw new IllegalArgumentException("Select what to filter");
        }
        return repo.stockFilter(u, activity.trim(), itemCategoryId, docDateTo,
                                warehouseId, cropYear, itemId, jobLotId);
    }

    public List<Map<String, Object>> availableStock(Integer itemCategoryId, String dateTo,
                                                    Integer warehouseId, Integer itemId,
                                                    Integer jobLotId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.availableStock(u, itemCategoryId, dateTo, warehouseId, itemId, jobLotId);
    }

    public List<Map<String, Object>> storeAndPmItems() {
        return repo.storeAndPmItems(currentUserContext.requireAccountingUser());
    }

    /**
     * cmbEntryType — invfrmStockConversionProduction.CmbEntryTypeFill():840.
     *
     * The three captions are literals on the desktop, but row 1 "Issue" is conditional: it is
     * added only when the IssuanceByLoader configuration parses as false. Emitting it
     * unconditionally offered an entry type the desktop withholds, so the config is read here.
     */
    public List<Map<String, Object>> entryTypes() {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        /* CmbEntryTypeFill:801 -
             list = configrationsAllocation.Where(ConfigDescription == "IssuanceByLoader")
             if (list.Count > 0 && !bool.Parse(list[0].ConfigKey)) add (1, "Issue")
           Stock Conversion pass 2 correction: with NO such row the guard is false and "Issue" is
           NOT added. The previous code treated a missing row as "off" and offered Issue.
           bool.Parse accepts only True/False (trimmed, any case); anything else throws before the
           combo is bound, so the desktop shows the error and no entry types at all. */
        String key = lookups.configKey(u, "IssuanceByLoader");
        if (key != null) {
            String t = key.trim();
            if ("false".equalsIgnoreCase(t)) {
                out.add(row(1, "Issue"));
            } else if (!"true".equalsIgnoreCase(t)) {
                throw new IllegalStateException("The IssuanceByLoader configuration value '" + key
                        + "' is not True or False. The desktop's bool.Parse fails on it and binds no"
                        + " entry types, so none are offered here either.");
            }
        }
        out.add(row(2, "Recovery By Product"));
        out.add(row(3, "Recovery Head Rice"));
        return out;
    }

    // ============================================================== the thirteen dropdowns

    /** ERP feature that keeps Conversion Type 5 in the list (Load:576, BindProductionType:878). */
    private static final int ERP_FEATURE_STOCK_RELEASE_FROM_FUMIGATION = 24;

    /** The combo-fill methods Load calls (:749-760), in one read. See StockConversionLookupsRepository. */
    public Map<String, Object> lookups() {
        UserAccount u = currentUserContext.requireAccountingUser();
        int branchId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        boolean fumigation = lookups.erpFeature(u, ERP_FEATURE_STOCK_RELEASE_FROM_FUMIGATION);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("stockReleaseFromFumigation", fumigation);
        out.put("parentCategories", pick(lookups.parentCategories(), "Id", "InvParentCateDescription"));
        out.put("productionDepartments",
                pick(lookups.productionDepartments(u, branchId), "Id", "WareHouseName"));

        /* BindProductionType:878 - when feature 24 is OFF the row with Id 5 ("Stock Release From
           Fumigation") is deleted from the table before binding. */
        List<Map<String, Object>> types = new ArrayList<>();
        for (Map<String, Object> r : pick(lookups.conversionTypes(), "Id", "type")) {
            if (!fumigation && asInt(r.get("Id")) == 5) continue;
            types.add(r);
        }
        out.put("conversionTypes", types);

        out.put("warehouses", pick(lookups.warehousesForBranch(u, branchId), "Id", "WareHouseName"));
        /* The whole item cache; the page filters it on the parent category exactly as
           BindItemCombo does in memory. */
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : lookups.allItems(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("ItemName", ci(r, "ItemName"));
            m.put("InventoryParentCategoriesId", ci(r, "InventoryParentCategoriesId"));
            items.add(m);
        }
        out.put("items", items);
        out.put("jobLots", pick(lookups.jobLotsForBranch(u, branchId), "Id", "JobLotDescription"));
        out.put("cropYears", pick(lookups.cropYears(u), "Id", "CropYear"));
        out.put("packingTypes", pick(lookups.packingTypes(), "Id", "PackTypeDesc"));
        out.put("moistureSlabs", pick(lookups.moistureSlabs(u), "Id", "MoistureSlabDescription"));
        out.put("differenceAccounts",
                pick(lookups.differenceAccounts(u, currentUserContext.currentAppId()), "Id", "AccountTitle"));

        /* Load:572-602 - the switches that decide what the form shows. */
        out.put("issuanceByLoader", flag(u, "IssuanceByLoader"));
        out.put("contractWagesChargeToProduct", flag(u, "ContractWagesChargetoProductForStockConversion"));
        out.put("saleMinusAllowedAgainstFifo", lookups.erpFeature(u, 14));
        /* Formats: DecimalRateFormate and stringFormatsingle (CommonServices.GetDecimalConfiguration). */
        out.put("rateDecimals", decimals(u, "Default NoofDecimal Points For Rate", true));
        out.put("amountDecimals", decimals(u, "Default NoofDecimal Points For Amount", false));
        /* :593-596 - Save / Print / Update / Delete enabled from the grant rows. */
        Map<String, Object> rights = new LinkedHashMap<>();
        rights.put("save", right(u, "Save"));
        rights.put("update", right(u, "Update"));
        rights.put("print", right(u, "Print"));
        rights.put("delete", right(u, "Delete"));
        out.put("rights", rights);
        return out;
    }

    private boolean flag(UserAccount u, String name) {
        try {
            String v = lookups.configKey(u, name);
            return v != null && ("1".equals(v.trim()) || "true".equalsIgnoreCase(v.trim()));
        } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read; treating as off", name, e);
            return false;
        }
    }

    /** 1-4 decimals; the rate format falls back to 2 when the value is 0/missing, the amount
     *  format to none ("#,##0."), exactly as GetDecimalConfiguration builds the strings. */
    private int decimals(UserAccount u, String name, boolean rate) {
        int n = 0;
        try {
            String v = lookups.configKey(u, name);
            n = v == null || v.trim().isEmpty() ? 0 : (int) Math.floor(Double.parseDouble(v.trim()));
        } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read", name, e);
        }
        if (n >= 1 && n <= 4) return n;
        return (rate && n == 0) ? 2 : 0;
    }

    /** SetRightsValueInRightsObject:14738 - "Admin" is pre-granted Save/Update/Delete/Print; the
     *  grant rows are read for everyone else. */
    private boolean right(UserAccount u, String rightName) {
        String role = currentUserContext.currentRoleName();
        boolean admin = "Admin".equals(role);
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    SQL_USER_RIGHTS, u.getId(), SCREEN_NAME, role, u.getCompanyId(), "GetByUserId")) {
                Object name = ci(r, "RightName");
                if (name != null && rightName.equalsIgnoreCase(name.toString().trim())) {
                    /* a Delete row overrides even for Admin; the others stay granted */
                    return admin && !"Delete".equals(rightName) ? true : toBool(ci(r, "Value"));
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read the '{}' right for {}", rightName, SCREEN_NAME, e);
        }
        return admin;
    }

    /** btnVoucher_Click:6207 - CommonServices.VoucherHeadIdGet(RecId, 66). */
    public int voucherHeadId(int id) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (id <= 0) return 0;
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, "
              + "@DocumentTypeId=?, @DocumentTypeSrNo=?",
                "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                u.getOrganizationId(), u.getCompanyId(), 66, id);
        if (rows.isEmpty()) return 0;
        Object v = ci(rows.get(0), "Id");
        if (v == null) v = rows.get(0).values().iterator().next();
        return (v instanceof Number) ? ((Number) v).intValue() : 0;
    }

    /**
     * cmbUOM / cmbRateUom for one item - CommonServices.GetUomScheduleByItemId, which copies
     * exactly these five columns out of the procedure's result.
     */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        if (itemId <= 0) return out;
        for (Map<String, Object> r : lookups.uomsForItem(u, itemId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("UOMCode", ci(r, "UOMCode"));
            m.put("Equivalent", ci(r, "Equivalent"));
            m.put("QtyEquivalent", ci(r, "QtyEquivalent"));
            m.put("BaseRateUom", ci(r, "BaseRateUom"));
            out.add(m);
        }
        return out;
    }

    /** Keeps the value and display columns a combo binds, under their desktop names. */
    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows,
                                                  String valueCol, String textCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put(valueCol, ci(r, valueCol));
            m.put(textCol, ci(r, textCol));
            out.add(m);
        }
        return out;
    }

    private static Map<String, Object> row(int id, String name) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", id);
        m.put("EntryType", name);
        return m;
    }

    // ============================================================================ edit side: setup

    /** The configuration value as GlobalVariables_Helper.GetConfigValueFromGlobal returns it (null when absent). */
    private String cfg(UserAccount u, String name) {
        try { return lookups.configKey(u, name); }
        catch (Exception e) { LOG.warn("Configuration '{}' could not be read", name, e); return null; }
    }
    /** Conversion.ToBool(object) on the config value. */
    private boolean cfgBool(UserAccount u, String name) { return StockConversionRepository.toBoolNet(cfg(u, name)); }
    /** Conversion.ToDouble(object) on the config value: Convert.ToDouble, 0 on failure or infinity. */
    private double cfgDouble(UserAccount u, String name) {
        String v = cfg(u, name);
        if (v == null || v.trim().isEmpty()) return 0d;
        try {
            double d = Double.parseDouble(v.trim().replace(",", ""));
            return Double.isInfinite(d) ? 0d : d;
        } catch (NumberFormatException e) { return 0d; }
    }

    private int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }

    /**
     * Everything Load (:597-795) reads besides the thirteen combos, plus the grid value lists that do
     * not depend on the Conversion Type: the configuration switches, ERP features 5 and 11, the wages
     * status row for RefDocumentTypeId 66, the wages lists (only when
     * ContractWagesChargetoProductForStockConversion is on, as Load only fills them then), item
     * conditions without Id 4, the overhead accounts, the Charge To list and the racks for F1.
     */
    public Map<String, Object> editSetup() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        boolean multiBranch = lookups.erpFeature(u, 11);
        out.put("fifoCgs", lookups.erpFeature(u, 5));
        out.put("multiBranchFeature", multiBranch);
        out.put("saleMinusAllowedAgainstFifo", lookups.erpFeature(u, 14));
        out.put("stockReleaseFromFumigation", lookups.erpFeature(u, ERP_FEATURE_STOCK_RELEASE_FROM_FUMIGATION));
        out.put("wagesStatus", cfgBool(u, "WagesCompulsoryOnStockConversion"));
        out.put("pmCompulsoryForWarning", cfgBool(u, "PackingMaterialCompulsoryOnStockConversionForWarning"));
        out.put("pmCompulsoryForStop", cfgBool(u, "PackingMaterialCompulsoryOnStockConversionForStop"));
        out.put("gainLossTolerance", cfgDouble(u, "ToleranceForInPutMinusOutputForConversionInPercent"));
        out.put("percentageForRateAddLess", cfgDouble(u, "PercentageForRateAddLess"));
        boolean contractWages = cfgBool(u, "ContractWagesChargetoProductForStockConversion");
        out.put("contractWagesChargeToProduct", contractWages);
        out.put("enableAddLessOnWagesRegular", cfgBool(u, "EnableAddLessOnWagesRegular"));
        out.put("wagesAmountCalculateOnQty", cfgBool(u, "WagesAmountCalculateOnQty"));
        out.put("issuanceByLoader", cfgBool(u, "IssuanceByLoader"));
        out.put("stichingWagesCompulsory", cfgBool(u, "OtherWagesCompulsoryForStockConversion"));
        boolean wagesActive = false;
        for (Map<String, Object> r : repo.wagesRefDocuments()) {
            if (asInt(ci(r, "RefDocumentTypeId")) == StockConversionRepository.DOC_TYPE_ID) {
                wagesActive = toBool(ci(r, "IsActive"));
                break;
            }
        }
        out.put("wagesActive", wagesActive);

        /* cmbChargeTo():855 - two literal rows. */
        List<Map<String, Object>> chargeTo = new ArrayList<>();
        chargeTo.add(kv("Id", "1", "ChargeTo", "Recovery By Product"));
        chargeTo.add(kv("Id", "2", "ChargeTo", "Recovery Head Rice"));
        out.put("chargeTo", chargeTo);

        /* GridPmDropdownBind:2942 - globalItemConditions without Id 4, as Id / Description. */
        List<Map<String, Object>> conds = new ArrayList<>();
        for (Map<String, Object> r : repo.itemConditions()) {
            if (asInt(ci(r, "Id")) == 4) continue;
            conds.add(kv("Id", ci(r, "Id"), "Description", ci(r, "ConditionStatus")));
        }
        out.put("itemConditions", conds);
        out.put("overheadAccounts", pick(repo.overheadAccounts(u), "Id", "AccountTitle"));
        List<Map<String, Object>> racks = new ArrayList<>();
        for (Map<String, Object> r : repo.racksWithWarehouseAndItems(u, branch(u))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("RackName", ci(r, "RackName"));
            m.put("ItemId", ci(r, "ItemId"));
            m.put("WarehouseId", ci(r, "invWarehouseId"));
            m.put("WareHouseName", ci(r, "WareHouseName"));
            racks.add(m);
        }
        out.put("racks", racks);

        /* :635-640 and CmbConversionType_Leave:1592 - suppliercustomer() and accountName() only when the
           wages configuration is on; GridStichingSettings reads its own list with default "35". */
        if (contractWages) {
            out.put("contractors", pick(repo.contractors(u, multiBranch, branch(u)), "Id", "CompanyName"));
            String ids = null;
            for (Map<String, Object> r : repo.wagesTypeIdsAgainstDocumentType()) {
                if (asInt(ci(r, "DocumentTypeId")) == StockConversionRepository.DOC_TYPE_ID) {
                    Object v = ci(r, "WagesActivityIds");
                    ids = v == null ? "" : String.valueOf(v);
                    break;
                }
            }
            out.put("wagesAccounts", pick(repo.wagesAccounts(u, ids, 0, 1), "Id", "WagesAccountName"));
            out.put("stitchingAccounts", pick(repo.wagesAccounts(u, ids == null ? "35" : ids, 0, 1), "Id", "WagesAccountName"));
        } else {
            out.put("contractors", new ArrayList<>());
            out.put("wagesAccounts", new ArrayList<>());
            out.put("stitchingAccounts", new ArrayList<>());
        }
        return out;
    }

    /** GridPmDropdownBind:2924 - Item.GetItemByItemTypeId with "14,17" for Conversion Type 5, else "14". */
    public List<Map<String, Object>> pmItems(int conversionTypeId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return pick(repo.pmItems(u, conversionTypeId == 5 ? "14,17" : "14"), "Id", "ItemName");
    }

    /** ScheduleNoDbCall:2896 - pending export schedules for this record (RecId). */
    public List<Map<String, Object>> schedules(int recId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.pendingSchedules(u, recId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("ScheduleCode", ci(r, "ScheduleCode"));
            m.put("InvoiceId", ci(r, "InvoiceId"));
            out.add(m);
        }
        return out;
    }

    // ================================================================= edit side: per-row lookups

    private static LocalDateTime docDate(String yyyyMmDd) {
        if (yyyyMmDd == null || yyyyMmDd.trim().isEmpty()) return LocalDate.now().atTime(LocalTime.now());
        return LocalDate.parse(yyyyMmDd.trim().substring(0, 10)).atTime(LocalTime.now());
    }

    /** GetAvgRate:1162 - AvgRateOnlyForCGS(item, docDate, 66, RecId, lot, CmbCropyr.Value, null, godown). */
    public double avgRate(int itemId, String date, int recId, int jobLotId, int cropYearId, int warehouseId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.avgRateOnlyForCgs(u, itemId, docDate(date), StockConversionRepository.DOC_TYPE_ID, recId,
                jobLotId, cropYearId, null, warehouseId, 0);
    }

    /** grdPackingMaterial_CellUpdated:2864 - Math.Round(AvgRate, 3) of row 0, else 0; DocumentTypeId is
     *  66 only when RecId > 0. */
    public double pmRate(int itemId, String date, int itemConditionId, int recId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> r = repo.pmAvgRate(u, itemId, docDate(date), itemConditionId, recId,
                recId > 0 ? StockConversionRepository.DOC_TYPE_ID : 0);
        if (r.isEmpty()) return 0d;
        return new java.math.BigDecimal(StockConversionRepository.dbl(ci(r.get(0), "AvgRate")))
                .setScale(3, java.math.RoundingMode.HALF_EVEN).doubleValue();
    }

    /** CommonServices.GetWagesRate - {WagesRate, ScheduleId} or null. */
    public Map<String, Object> wagesRate(String date, double packSize, int wagesId, int contractorId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.wagesRate(u, docDate(date), packSize, wagesId, contractorId);
    }

    /** CommonServices.CheckItemsFreeofcostforWages(docDate, 66, item, wagesAccount), for a batch of rows. */
    public List<Boolean> wagesFree(String date, List<Map<String, Object>> rows) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Boolean> out = new ArrayList<>();
        LocalDateTime d = docDate(date);
        for (Map<String, Object> r : rows == null ? new ArrayList<Map<String, Object>>() : rows) {
            out.add(repo.wagesFreeOfCost(u, d, StockConversionRepository.DOC_TYPE_ID,
                    asInt(r.get("itemId")), asInt(r.get("wagesId"))));
        }
        return out;
    }

    /** WagesDetailReadbyId:5285 - the saved wages lines of a conversion, split by WagesTypeId (2 = other). */
    public List<Map<String, Object>> wagesDetail(int id) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (load(id) == null) return new ArrayList<>();   // tenancy guard, same as ReadById
        return repo.wagesDetailByRefDocument(u, currentUserContext.currentFinancialYearId(), id);
    }

    // ============================================================ LoadavailableTransactionsForIssuance

    /**
     * LoadInvoices_Load:128. Rights row "Rate" of screen LoadavailableTransactionsForIssuance: when rows
     * exist but none is "Rate", newList[0] throws and Load stops before the dates, combos and the first
     * search - reported as loadError. FoodProductionWithValues.ValuesShowRights is a static the desktop
     * only sets when screen 280 has been opened in the session; here it is read from screen 280's own
     * "Rate" grant (FoodProductionWithValues), the value it would hold once that screen was opened.
     */
    public Map<String, Object> loaderSetup() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        String role = currentUserContext.currentRoleName();
        try {
            List<Map<String, Object>> rights = repo.userRights(u.getId(), "LoadavailableTransactionsForIssuance",
                    role, u.getCompanyId());
            if (!rights.isEmpty()) {
                boolean hasRate = false;
                for (Map<String, Object> r : rights) if ("Rate".equals(String.valueOf(ci(r, "RightName")))) { hasRate = true; break; }
                if (!hasRate) {
                    out.put("loadError", "Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
                    return out;
                }
            }
        } catch (Exception e) {
            LOG.warn("Loader rights could not be read", e);
        }
        boolean valuesShow = false;
        try {
            for (Map<String, Object> r : repo.userRights(u.getId(), "FoodProductionWithValues", role, u.getCompanyId()))
                if ("Rate".equals(String.valueOf(ci(r, "RightName")))) valuesShow = toBool(ci(r, "Value"));
        } catch (Exception e) {
            LOG.warn("Screen 280 Rate right could not be read", e);
        }
        out.put("valuesShowRights", valuesShow);
        Object start = repo.financialYearStart(u, currentUserContext.currentFinancialYearId());
        out.put("fromDate", start == null ? null : String.valueOf(start).substring(0, Math.min(10, String.valueOf(start).length())));
        Map<String, List<Map<String, Object>>> lists = new LinkedHashMap<>();
        for (String k : new String[]{"ParentCategories", "ItemCategories", "ItemTypes", "JobLot", "CropYear",
                "Warehouse", "DocumentType", "Supplier_Customer", "Items"}) lists.put(k, new ArrayList<>());
        for (Map<String, Object> r : repo.issuanceDropDowns(u)) {
            String t = String.valueOf(ci(r, "ActivityType"));
            List<Map<String, Object>> l = lists.get(t);
            if (l != null) l.add(kv("Id", ci(r, "Id"), "name", ci(r, "name")));
        }
        out.put("lists", lists);
        return out;
    }

    /** PendingInventoryTransactionsForIssuanceLoad:319 with the dialog's filters (CropYear is the combo TEXT). */
    public List<Map<String, Object>> loaderSearch(String fromDate, String toDate, int parentCategoryId,
                                                  int itemCategoryId, int itemTypeId, int jobLotId,
                                                  String cropYear, int warehouseId, int refDocumentTypeId,
                                                  int supplierCustomerId, int itemId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        /* FromDate.Value = ActiveYr.Start_Period (midnight); Todate.Value keeps the time it was opened at. */
        LocalDateTime from = (fromDate == null || fromDate.trim().isEmpty()) ? null
                : LocalDate.parse(fromDate.trim().substring(0, 10)).atStartOfDay();
        for (Map<String, Object> r : repo.availableTransactionsForIssuance(u, from, docDate(toDate),
                parentCategoryId, itemCategoryId, itemTypeId, jobLotId, cropYear, warehouseId,
                refDocumentTypeId, supplierCustomerId, itemId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : new String[]{"RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "RefDocumentType",
                    "DocDate", "DocCodeNo", "ManualNo", "GrnNo", "SupplierCustomerId", "SupplierCustomerName",
                    "VehicleNo", "GpNo", "WarehouseId", "WareHouseCode", "RefWarehouse", "ItemId", "ItemName",
                    "ItemCode", "CropYearId", "CropBatch", "JobLotId", "JobLotCode", "InvPackingTypeId",
                    "PackingType", "ItemUom", "PackUom", "PackSize", "QtyIn", "QtyOut", "QtyBalance", "WeightIn",
                    "WeightOut", "WeightBalance", "ReserveWeight", "StockWeightOut", "AVgRate", "RateUom",
                    "Equivalent", "RateUomId", "ItemAmount", "BiltyNo"}) m.put(c, ci(r, c));
            /* :398 - GrnNo is 0 for RefDocumentTypeId 112 and 80. */
            int rt = asInt(ci(r, "RefDocumentTypeId"));
            if (rt == 112 || rt == 80) m.put("GrnNo", 0);
            m.put("Remarks", ci(r, "TranRemarks"));
            out.add(m);
        }
        return out;
    }

    // ============================================ LoadavailableTransactionsForStockReleaseFromFumigation

    /**
     * PendingInventoryTransactionsForIssuanceLoad (fumigation loader :325) with the dialog's filters.
     * Load (:133) is identical to the Issuance loader's, so the page reuses {@link #loaderSetup}.
     * Rows are the dtcol copy of :381-437: GrnNo 0 for RefDocumentTypeId 112 and 80, Remarks =
     * TranRemarks, IPMJobLot = JobLotDescription.
     */
    public List<Map<String, Object>> fumigationSearch(String fromDate, String toDate, int parentCategoryId,
                                                      int itemCategoryId, int itemTypeId, int jobLotId,
                                                      String cropYear, int warehouseId, int refDocumentTypeId,
                                                      int supplierCustomerId, int itemId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        LocalDateTime from = (fromDate == null || fromDate.trim().isEmpty()) ? null
                : LocalDate.parse(fromDate.trim().substring(0, 10)).atStartOfDay();
        for (Map<String, Object> r : repo.holdStockForFumigation(u, from, docDate(toDate),
                parentCategoryId, itemCategoryId, itemTypeId, jobLotId, cropYear, warehouseId,
                refDocumentTypeId, supplierCustomerId, itemId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : new String[]{"labIPmActivityLogId", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo",
                    "RefDocumentType", "DocDate", "DocCodeNo", "ManualNo", "GrnNo", "SupplierCustomerId",
                    "SupplierCustomerName", "VehicleNo", "GpNo", "GPDate", "WarehouseId", "WareHouseCode",
                    "RefWarehouse", "ItemId", "ItemName", "ItemCode", "CropYearId", "CropBatch", "JobLotId",
                    "JobLotCode", "InvPackingTypeId", "PackingType", "ItemUom", "PackUom", "PackSize", "QtyIn",
                    "QtyOut", "QtyBalance", "WeightIn", "WeightOut", "WeightBalance", "ReserveWeight",
                    "StockWeightOut", "AVgRate", "RateUom", "Equivalent", "RateUomId", "ItemAmount", "BiltyNo"})
                m.put(c, ci(r, c));
            int rt = asInt(ci(r, "RefDocumentTypeId"));
            if (rt == 112 || rt == 80) m.put("GrnNo", 0);
            m.put("Remarks", ci(r, "TranRemarks"));
            m.put("IPMJobLot", ci(r, "JobLotDescription"));
            for (String c : new String[]{"StepDescription", "qcActivityDate", "StatusDescription", "nextActivityPlanDate",
                    "fumigatedBy", "checkBy", "verifiedBy", "ReleaseHoldStatus", "qcActivityDescription"})
                m.put(c, ci(r, c));
            out.add(m);
        }
        return out;
    }

    // ===================================================================== frmLoadStockShortFallForSales

    /** StockComboFill (:125) - the same DropDownAndLists call; five of its lists are bound. */
    public Map<String, Object> shortfallSetup() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, List<Map<String, Object>>> lists = new LinkedHashMap<>();
        for (String k : new String[]{"ParentCategories", "JobLot", "CropYear", "Warehouse", "Items"}) lists.put(k, new ArrayList<>());
        for (Map<String, Object> r : repo.issuanceDropDowns(u)) {
            List<Map<String, Object>> l = lists.get(String.valueOf(ci(r, "ActivityType")));
            if (l != null) l.add(kv("Id", ci(r, "Id"), "name", ci(r, "name")));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("lists", lists);
        return out;
    }

    /**
     * PendingOrderLoad (:224). DocDate is the loader's DateTime.Now taken when it was constructed; the
     * page passes the moment the dialog opened (yyyy-MM-ddTHH:mm:ss). CropYear is the combo TEXT.
     * The rows are returned as the procedure gives them (dtHistory); the grid copy is the page's.
     */
    public List<Map<String, Object>> shortfallSearch(String docDate, int parentCategoryId, int itemId,
                                                     int warehouseId, int jobLotId, String cropYear) {
        UserAccount u = currentUserContext.requireAccountingUser();
        LocalDateTime d;
        if (docDate == null || docDate.trim().isEmpty()) d = LocalDateTime.now();
        else {
            String s = docDate.trim();
            d = s.length() > 10 ? LocalDateTime.parse(s.length() > 19 ? s.substring(0, 19) : s)
                                : LocalDate.parse(s).atTime(LocalTime.now());
        }
        return repo.balanceSalesForStock(u, d, warehouseId, parentCategoryId, itemId, jobLotId,
                cropYear == null ? "" : cropYear);
    }

    // =================================================================================== write

    private static Object v(Map<String, Object> m, String k) { return m == null ? null : m.get(k); }
    private static int i(Map<String, Object> m, String k) { return asInt(v(m, k)); }
    private static double d(Map<String, Object> m, String k) { return StockConversionRepository.dbl(v(m, k)); }
    private static String s(Map<String, Object> m, String k) { Object o = v(m, k); return o == null ? "" : String.valueOf(o); }
    private static boolean b(Map<String, Object> m, String k) { return toBool(v(m, k)); }
    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Map<String, Object> m, String k) {
        Object o = v(m, k);
        return o instanceof List ? (List<Map<String, Object>>) o : new ArrayList<>();
    }

    /**
     * Insert():4365 posts the model the form built; BLL InvStockConversion.Save -> DAL SetData. The page
     * runs every form-side check and confirmation first (they are the form's, with its messages) and
     * sends the model lists; everything that decides WHOSE document it is - organisation, company,
     * branch, financial year, users, dates, DocTypeId 66, ActionId - is set here and never read from
     * the request.
     */
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = currentUserContext.requireAccountingUser();
        int recId = i(body, "id");
        /* btnsave (DoHaveSaveRight) / btnUpdate (DoHaveUpdateRights) are disabled without the grant. */
        if (recId == 0 && !right(u, "Save")) throw new IllegalStateException("You don't have the Save right on this screen.");
        if (recId > 0 && !right(u, "Update")) throw new IllegalStateException("You don't have the Update right on this screen.");
        if (recId > 0 && load(recId) == null) throw new IllegalArgumentException("RecId not found");

        LocalDateTime now = LocalDateTime.now();
        LocalDateTime date = docDate(s(body, "docDate"));
        int userId = u.getId() == null ? 0 : u.getId();
        int fy = currentUserContext.currentFinancialYearId();

        StockConversionRepository.SaveModel m = new StockConversionRepository.SaveModel();
        Map<String, Object> h = m.header;
        h.put("Id", recId);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchedId", branch(u));
        h.put("FinancialYearId", fy);
        h.put("DocTypeId", StockConversionRepository.DOC_TYPE_ID);
        h.put("EntryUser", userId);
        h.put("ModifyUser", userId);
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("PostDate", now);
        h.put("PostUser", 0);
        h.put("PostState", false);
        h.put("ProjectsId", 0);
        h.put("ConversionTypeId", i(body, "conversionTypeId"));
        h.put("DocSrNo", i(body, "docSrNo"));
        h.put("DocDate", date);
        h.put("ProductionNo", s(body, "productionNo"));
        h.put("parentCategoryId", i(body, "parentCategoryId"));
        h.put("EBDepartmentId", i(body, "ebDepartmentId"));
        h.put("DocManualRef", "");
        h.put("Remarks", s(body, "remarks"));
        h.put("GainLossId", i(body, "gainLossId"));
        h.put("DifferenceAccountId", i(body, "differenceAccountId"));
        /* BLL Save: Id == 0 -> ActionId 1, ModifyUser 0; otherwise ActionId 2, EntryUser 0. */
        if (recId == 0) { h.put("ActionId", 1); h.put("ModifyUser", 0); }
        else { h.put("ActionId", 2); h.put("EntryUser", 0); }
        String removeIds = s(body, "inputDetailRowsRemoveIds");
        m.inputDetailRowsRemoveIds = removeIds.isEmpty() ? null : removeIds;

        for (Map<String, Object> r : list(body, "details")) {
            Map<String, Object> dd = new LinkedHashMap<>();
            dd.put("Id", i(r, "Id"));
            dd.put("InvStockConversionId", 0);
            dd.put("EntryType", s(r, "EntryType"));
            dd.put("WarehouseId", i(r, "WarehouseId"));
            dd.put("ItemId", i(r, "ItemId"));
            dd.put("ItemUomId", i(r, "ItemUomId"));
            dd.put("CropBatch", s(r, "CropBatch"));
            dd.put("JobLotId", i(r, "JobLotId"));
            dd.put("PackingtypeId", i(r, "PackingtypeId"));
            dd.put("Qty", d(r, "Qty"));
            dd.put("PackUnit", 0);
            dd.put("Weight", d(r, "Weight"));
            dd.put("Rate", d(r, "Rate"));
            dd.put("RateUOMId", i(r, "RateUOMId"));
            dd.put("Amount", d(r, "Amount"));
            dd.put("ProjectId", 0);
            dd.put("VoucherHeadId", 0);
            dd.put("Remarks", s(r, "Remarks"));
            dd.put("ExpenseAmount", d(r, "ExpenseAmount"));
            dd.put("PackingMaterialAmount", d(r, "PackingMaterialAmount"));
            dd.put("Moisture", d(r, "Moisture"));
            dd.put("MoistureSlabId", i(r, "MoistureSlabId"));
            dd.put("RefDocumentTypeId", i(r, "RefDocumentTypeId"));
            dd.put("RefDocNoId", i(r, "RefDocNoId"));
            dd.put("RefDocSubId", i(r, "RefDocSubId"));
            dd.put("LineId", i(r, "LineId"));
            dd.put("WagesAmount", d(r, "WagesAmount"));
            dd.put("ItemPmCost", d(r, "ItemPmCost"));
            dd.put("ItemOhCost", d(r, "ItemOhCost"));
            dd.put("ItemConditionId", 0);
            dd.put("RackId", 0);
            dd.put("SortNo", 0);
            dd.put("labIPmActivityLogId", i(r, "labIPmActivityLogId"));
            dd.put("IsOnHold", b(r, "IsOnHold"));
            m.details.add(dd);
        }
        for (Map<String, Object> r : list(body, "packings")) {
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("Id", 0);
            p.put("InvStockConversionId", 0);
            p.put("ItemId", i(r, "ItemId"));
            p.put("ItemSchUOM", 0);
            p.put("ItemQty", d(r, "ItemQty"));
            p.put("ItemRate", d(r, "ItemRate"));
            p.put("ItemAmount", d(r, "ItemAmount"));
            p.put("ChargeTo", s(r, "ChargeTo"));
            p.put("WarehouseId", i(r, "WarehouseId"));
            p.put("LineId", 0);
            p.put("BrandItemId", i(r, "BrandItemId"));
            p.put("BrandItemUomId", i(r, "BrandItemUomId"));
            p.put("ItemConditionId", i(r, "ItemConditionId"));
            p.put("ContractScheduleId", i(r, "ContractScheduleId"));
            p.put("ExImInvoiceId", i(r, "ExImInvoiceId"));
            p.put("RackId", i(r, "RackId"));
            m.packings.add(p);
        }
        for (Map<String, Object> r : list(body, "expenses")) {
            Map<String, Object> e = new LinkedHashMap<>();
            e.put("Id", 0);
            e.put("InvStockConversionId", 0);
            e.put("ChartOfAccountId", i(r, "ChartOfAccountId"));
            e.put("LedgerRemarks", s(r, "LedgerRemarks"));
            e.put("ExpAmount", d(r, "ExpAmount"));
            e.put("ChargeTo", s(r, "ChargeTo"));
            e.put("BrandItemId", i(r, "BrandItemId"));
            e.put("BrandItemUomId", i(r, "BrandItemUomId"));
            m.expenses.add(e);
        }
        /* AddWagesListInInsert:5008 - the header fields the form sets; tenancy and users from here. */
        for (Map<String, Object> r : list(body, "wagesBills")) {
            StockConversionRepository.WagesBill wb = new StockConversionRepository.WagesBill();
            Map<String, Object> wh = wb.header;
            wh.put("Id", 0);
            wh.put("CompanyId", u.getCompanyId());
            wh.put("OrganizationId", u.getOrganizationId());
            wh.put("BranchesId", branch(u));
            wh.put("DocNo", i(body, "docSrNo"));
            wh.put("DocDate", date);
            wh.put("DocumentTypeId", 101);
            wh.put("RefDocumentTypeId", StockConversionRepository.DOC_TYPE_ID);
            wh.put("RefDocNoId", recId);
            wh.put("RefDocNo", i(body, "docSrNo"));
            wh.put("OtherRemarks", s(body, "remarks"));
            wh.put("EntryUser", userId);
            wh.put("ModifyUser", userId);
            wh.put("ScaleSlipNo", 0);
            wh.put("RefDocument", s(r, "RefDocument"));
            wh.put("ModifyDate", now);
            wh.put("EntryDate", now);
            wh.put("FinancialYearId", fy);
            wh.put("IsAproved", false);
            wh.put("ApprovedUserId", 0);
            wh.put("ProjectsId", 0);
            wh.put("JobOrderId", 0);
            wh.put("QtyTotal", d(r, "QtyTotal"));
            wh.put("WeightTotal", d(r, "WeightTotal"));
            for (Map<String, Object> l : list(r, "lines")) {
                Map<String, Object> wl = new LinkedHashMap<>();
                wl.put("Id", 0);
                wl.put("InvContractorWagesBillHeaderId", 0);
                wl.put("ContractorId", i(l, "ContractorId"));
                wl.put("ItemId", i(l, "ItemId"));
                wl.put("ItemName", s(l, "ItemName"));
                wl.put("WagesAccountName", s(l, "WagesAccountName"));
                wl.put("Crop", s(l, "Crop"));
                wl.put("JobLotId", i(l, "JobLotId"));
                wl.put("InvPackingTypeId", i(l, "InvPackingTypeId"));
                wl.put("WbTransactionsIdDt", 0);
                wl.put("InvConractorWagesAccountsId", i(l, "InvConractorWagesAccountsId"));
                wl.put("Weight", d(l, "Weight"));
                wl.put("PackSize", d(l, "PackSize"));
                wl.put("Qty", d(l, "Qty"));
                wl.put("WageRate", d(l, "WageRate"));
                wl.put("WagesAmount", d(l, "WagesAmount"));
                wl.put("WareHouseFromId", i(l, "WareHouseFromId"));
                wl.put("WareHouseToId", i(l, "WareHouseToId"));
                wl.put("BillQty", d(l, "BillQty"));
                wl.put("WeightCut", d(l, "WeightCut"));
                wl.put("BillWeight", d(l, "BillWeight"));
                String rd = s(l, "RefDocDate");
                wl.put("RefDocDate", rd.isEmpty() ? LocalDate.of(1900, 1, 1).atStartOfDay()
                        : LocalDate.parse(rd.substring(0, 10)).atTime(LocalTime.now()));
                wl.put("WagesTypeId", i(l, "WagesTypeId"));
                wl.put("FreeOfCost", b(l, "FreeOfCost"));
                wl.put("RefDocumentTypeId", 0);
                wl.put("JobOrderId", 0);
                wl.put("IsCompany", false);
                wl.put("RateAddLess", d(l, "RateAddLess"));
                wl.put("RefDocQty", d(l, "RefDocQty"));
                wl.put("RefDocWeight", d(l, "RefDocWeight"));
                wl.put("RefLineId", i(l, "RefLineId"));
                wl.put("InvContractorWagesScheduleId", i(l, "InvContractorWagesScheduleId"));
                wb.lines.add(wl);
            }
            m.wagesBills.add(wb);
        }

        int code = repo.save(m);
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", code);
        res.put("docSrNo", h.get("DocSrNo"));
        res.put("message", (recId == 0 ? "Record Save Successfully " : "Record Update Successfully ") + h.get("DocSrNo"));
        return res;
    }

    /** btnDelete_Click:4967. */
    public Map<String, Object> delete(int id) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (!right(u, "Delete")) throw new IllegalStateException("Record cannot be delete because you don't have right....");
        if (id <= 0) throw new IllegalArgumentException("RecordId Not Found.....");
        if (load(id) == null) throw new IllegalArgumentException("RecordId Not Found.....");
        repo.delete(u, id, u.getId() == null ? 0 : u.getId());
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", true);
        r.put("message", "Delete Record Successfully");
        return r;
    }

    private static Map<String, Object> kv(String k1, Object v1, String k2, Object v2) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put(k1, v1);
        m.put(k2, v2);
        return m;
    }

    // ========================================================================== authorization

    /**
     * CommonServices.SetRightsValueInRightsObject, the same way the rest of this port reads it:
     * Admin/Administrator short-circuits, otherwise the "CanView AllRecord" ROW of
     * Sp_tblUserRights_GetAllMethod for THIS screen. @RightName is the user's ROLE, which picks
     * the procedure's Admin branch; the right being asked about is not a parameter.
     */
    private boolean canViewAllRecords(UserAccount u) {
        String role = currentUserContext.currentRoleName();
        /* SetRightsValueInRightsObject:14738 - only the role spelled exactly "Admin" is pre-granted
           CanView AllRecord (and a grant row cannot take it away); every other role, including
           "Administrator", reads its row. */
        if ("Admin".equals(role)) return true;
        try {
            List<Map<String, Object>> rights = jdbcTemplate.queryForList(
                    SQL_USER_RIGHTS, u.getId(), SCREEN_NAME, role, u.getCompanyId(), "GetByUserId");
            for (Map<String, Object> r : rights) {
                Object name = ci(r, "RightName");
                if (name != null
                        && RIGHT_CAN_VIEW_ALL_RECORDS.equalsIgnoreCase(name.toString().trim())) {
                    return toBool(ci(r, "Value"));
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read CanView AllRecord for {}; restricting to own records",
                     SCREEN_NAME, e);
        }
        return false;
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static boolean toBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }
}
