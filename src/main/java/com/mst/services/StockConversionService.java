package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.StockConversionDto;
import com.mst.repositories.StockConversionLookupsRepository;
import com.mst.repositories.StockConversionRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Stock Conversion — invfrmStockConversionProduction.cs, DocTypeId 66.
 *
 * Read side complete; Save refuses. See {@link StockConversionRepository#save} for exactly what is
 * missing and why a partial write would be worse than none.
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

    // =================================================================================== write

    /**
     * Refuses. The desktop's Save is a posting engine — see
     * {@link StockConversionRepository#save}. Wired now so the screen and its contract exist, and
     * so that turning it on later is a change in one place rather than a new code path.
     */
    public Map<String, Object> save(StockConversionDto dto) {
        currentUserContext.requireAccountingUser();
        return repo.save(dto) > 0 ? null : null;   // repo.save always throws
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
