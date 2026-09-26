package com.mst.repositories;

import com.mst.models.UserAccount;
import java.sql.Date;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * Sale Order (screen 81) data calls that SaleOrder.cs makes and services/sale/SaleOrderService does not.
 * Every procedure, parameter, guard and in-memory rule below is read from the desktop form, its BLL
 * (Architecture.BLL.Inventory.SaleOrder / InvOrderCategory / Item / GetAvgRatesAndStockInHand) and the
 * procedure bodies in procdure.sql - not inferred from names.
 */
@Repository
public class SaleOrderHistoryLookupsRepository {
    public static final int SALE_ORDER_DOCUMENT_TYPE_ID = 81;
    /** base.Name of the desktop form (SaleOrder.cs:10630) - the screen the grant grid is keyed on. */
    public static final String DESKTOP_SCREEN_NAME = "SaleOrder";

    private final JdbcTemplate jdbc;
    public SaleOrderHistoryLookupsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------ history branch combo

    /**
     * HistoryBranchComboFill (SaleOrder.cs:3968-4005). Config "SaleOrderBranchWise" (BranchImplemented, :622) on ->
     * only the signed-in user's own branch. Off -> SaleOrder.GetBranchesAllocatedToUserFromSaleOrder(org, comp,
     * UserId, 81) = USP_GetBranchsAllocatedToUserFromSaleOrder @OrganizationId,@CompanyId,@UserId,@DocumentTypeId.
     * Rows are shaped as the form's dtBranch: Id, BranchName. The combo's text starts as the user's BranchName (:4003),
     * i.e. the user's branch is the one ticked by default.
     */
    public List<Map<String, Object>> historyBranches(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (truthy(config(u, "SaleOrderBranchWise"))) {
            for (Map<String, Object> r : jdbc.queryForList(
                    "EXEC dbo.Sp_Branches_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='GetAll'",
                    u.getOrganizationId(), u.getCompanyId())) {
                if (num(r.get("Id")) == u.getBranchesId()) out.add(branchRow(r.get("Id"), r.get("BranchName"), true));
            }
            return out;
        }
        for (Map<String, Object> r : jdbc.queryForList(
                "EXEC dbo.USP_GetBranchsAllocatedToUserFromSaleOrder @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId(), SALE_ORDER_DOCUMENT_TYPE_ID)) {
            out.add(branchRow(r.get("BranchId"), r.get("BranchName"), num(r.get("BranchId")) == u.getBranchesId()));
        }
        return out;
    }

    private static Map<String, Object> branchRow(Object id, Object name, boolean userBranch) {
        Map<String, Object> x = new LinkedHashMap<>();
        x.put("Id", id); x.put("BranchName", name); x.put("UserBranch", userBranch);
        return x;
    }

    /**
     * The desktop builds @BranchesIds as ",id,id" from the ticked names of ITS OWN combo, so it can only ever send
     * branches that combo offered. The web receives ids from the browser, so the same restriction is enforced here:
     * any id not in historyBranches() is refused. Returns the ",id,id" string, or null when nothing valid is left.
     */
    public String allowedBranchIds(UserAccount u, String requested) {
        Set<Integer> allowed = new HashSet<>();
        for (Map<String, Object> b : historyBranches(u)) allowed.add(num(b.get("Id")));
        StringBuilder s = new StringBuilder();
        if (requested != null) {
            for (String part : requested.split(",")) {
                String t = part.trim();
                if (t.isEmpty()) continue;
                int id;
                try { id = Integer.parseInt(t); } catch (NumberFormatException e) { continue; }
                if (!allowed.contains(id)) throw new IllegalArgumentException("Branch " + id + " is not available for Sale Order history");
                s.append(',').append(id);
            }
        }
        return s.length() == 0 ? null : s.toString();
    }

    // ------------------------------------------------------------------ history party / booking-person combos

    /**
     * HistoryCombosFill (SaleOrder.cs:4012-4062): SaleOrder.GetDataForDropDownFromSaleOrder (BLL 0596:1512) =
     * USP_GetDataForDropDownFromSaleOrder @OrganizationId,@CompanyId,@AppId,@UserId,@DocumentTypeIds='81',
     * @BranchesIds (only when not empty). One result set split on Activity: 'Customer' -> CmbCustomerHistory,
     * 'BookingPerson' -> CmbBookingPersonHistory, each Id / ReferenceName. The procedure raises 'UserId not found'
     * when @UserId is 0, so the signed-in user's id is always sent.
     */
    public Map<String, List<Map<String, Object>>> historyLookups(UserAccount u, String branchesIds) {
        List<Map<String, Object>> customers = new ArrayList<>(), bookingPersons = new ArrayList<>();
        List<Map<String, Object>> rows = branchesIds != null
                ? jdbc.queryForList("EXEC dbo.USP_GetDataForDropDownFromSaleOrder @OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?, @DocumentTypeIds='81', @BranchesIds=?",
                        u.getOrganizationId(), u.getCompanyId(), u.getAppId(), u.getId(), branchesIds)
                : jdbc.queryForList("EXEC dbo.USP_GetDataForDropDownFromSaleOrder @OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?, @DocumentTypeIds='81'",
                        u.getOrganizationId(), u.getCompanyId(), u.getAppId(), u.getId());
        for (Map<String, Object> r : rows) {
            String activity = String.valueOf(r.get("Activity"));
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", r.get("Id"));
            x.put("ReferenceName", r.get("ReferenceName"));
            if ("Customer".equals(activity)) customers.add(x);
            else if ("BookingPerson".equals(activity)) bookingPersons.add(x);
        }
        Map<String, List<Map<String, Object>>> out = new LinkedHashMap<>();
        out.put("customers", customers);
        out.put("bookingPersons", bookingPersons);
        return out;
    }

    /** User's own branch as ",id" - the default the desktop combo starts on. */
    public String userBranchIds(UserAccount u) {
        return u.getBranchesId() != null && u.getBranchesId() > 0 ? "," + u.getBranchesId() : null;
    }

    // ------------------------------------------------------------------ history grid

    /**
     * gridhistoryfill (SaleOrder.cs:4122-4240) -> SaleOrder.GetHisoty (BLL 0596) = Sp_SaleOrder_GetAllMethod
     * @Activity='SaleOrderFormHistory' with the BLL's guards: FinancialYearId when non-zero; exactly one date pair
     * chosen by the date-mode radio (doc / entry / modify / approved), each end only when supplied; @PoSrFrom/@PoSrTo
     * when non-zero; @SupplierCustomerId / @BookingPersonId when non-zero; @CanViewAllRecord from the user's right,
     * and @EntryUser ONLY when that right is false; @BranchesIds when not empty. The desktop refuses to search without
     * a branch ("Select branch first", :4246).
     */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, boolean canViewAll, String dateMode,
                                             Date from, Date to, int fromDocNo, int toDocNo, int customerId,
                                             int bookingPersonId, String branchesIds) {
        if (branchesIds == null) throw new IllegalArgumentException("Select branch first");
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        add(names, args, "@OrganizationId", u.getOrganizationId());
        add(names, args, "@CompanyId", u.getCompanyId());
        add(names, args, "@DocumentTypeId", SALE_ORDER_DOCUMENT_TYPE_ID);
        if (financialYearId != 0) add(names, args, "@FinancialYearId", financialYearId);
        String fromName, toName;
        switch (dateMode == null ? "doc" : dateMode) {
            case "entry":    fromName = "@EntryFromDate";    toName = "@EntryToDate";    break;
            case "modify":   fromName = "@ModifyFromDate";   toName = "@ModifyToDate";   break;
            case "approved": fromName = "@ApprovedFromDate"; toName = "@ApprovedToDate"; break;
            default:         fromName = "@DocDateFrom";      toName = "@DocDateTo";
        }
        if (from != null) add(names, args, fromName, from);
        if (to != null) add(names, args, toName, to);
        if (fromDocNo != 0) add(names, args, "@PoSrFrom", fromDocNo);
        if (toDocNo != 0) add(names, args, "@PoSrTo", toDocNo);
        if (customerId != 0) add(names, args, "@SupplierCustomerId", customerId);
        add(names, args, "@CanViewAllRecord", canViewAll ? 1 : 0);
        if (!canViewAll) add(names, args, "@EntryUser", u.getId());
        add(names, args, "@BranchesIds", branchesIds);
        if (bookingPersonId != 0) add(names, args, "@BookingPersonId", bookingPersonId);
        add(names, args, "@Activity", "SaleOrderFormHistory");
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_SaleOrder_GetAllMethod ");
        for (int i = 0; i < names.size(); i++) sql.append(i == 0 ? "" : ", ").append(names.get(i)).append("=?");
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    private static void add(List<String> n, List<Object> a, String name, Object v) { n.add(name); a.add(v); }

    /**
     * formright.DoHaveCanViewAllRecordRights for this screen, read the way InvoiceHistoryService and
     * SaleOrderCmagtService read it: Admin / Administrator short-circuit to true, otherwise the "CanView AllRecord"
     * row of Sp_tblUserRights_GetAllMethod @Activity='GetByUserId'. Any failure answers false - the restrictive
     * choice, which makes the procedure filter to the user's own entries.
     */
    public boolean canViewAllRecords(int userId, int companyId, String roleName) {
        if ("Admin".equalsIgnoreCase(roleName) || "Administrator".equalsIgnoreCase(roleName)) return true;
        try {
            for (Map<String, Object> r : jdbc.queryForList(
                    "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?",
                    userId, DESKTOP_SCREEN_NAME, roleName == null ? "" : roleName, companyId, "GetByUserId")) {
                Object name = ci(r, "RightName");
                if (name != null && "CanView AllRecord".equalsIgnoreCase(name.toString().trim())) return truthyObj(ci(r, "Value"));
            }
        } catch (Exception ignored) {
            // restrictive answer below
        }
        return false;
    }

    // ------------------------------------------------------------------ header / line helpers

    /**
     * GenerateOrderCategoryNo (SaleOrder.cs:1012) -> InvOrderCategory.GenerateSaleOrderCategoryCodebyId =
     * Sp_InvOrderCategory_GetAllMethod @OrganizationId,@CompanyId,@OrderCatagoryId,@FinancialYearId (when non-zero),
     * @Activity='GenerateSaleOrderCategoryCodeById' -> CatagorySrNo (MAX+1 for that category and year). The form
     * writes it into txtcatsr only when it is > 0.
     */
    public int nextCategorySrNo(UserAccount u, int financialYearId, int categoryId) {
        List<Map<String, Object>> rows = financialYearId != 0
                ? jdbc.queryForList("EXEC dbo.Sp_InvOrderCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @OrderCatagoryId=?, @FinancialYearId=?, @Activity='GenerateSaleOrderCategoryCodeById'",
                        u.getOrganizationId(), u.getCompanyId(), categoryId, financialYearId)
                : jdbc.queryForList("EXEC dbo.Sp_InvOrderCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @OrderCatagoryId=?, @Activity='GenerateSaleOrderCategoryCodeById'",
                        u.getOrganizationId(), u.getCompanyId(), categoryId);
        return rows.isEmpty() ? 0 : num(rows.get(0).get("CatagorySrNo"));
    }

    /**
     * combitem_ValueChanged / combitem_Leave (SaleOrder.cs:1631, :1669): CommonServices.CheckCommissionOnSaleByItem ->
     * Item.CheckCommissionOnByItem = Sp_Item_GetAllMethod @OrganizationId,@CompanyId,@ItemId,
     * @Activity='ItemCommissionScheduleByItemId' (IL Inventory.txt:81102) -> first row's OnSale, false when none.
     */
    public boolean commissionOnSale(UserAccount u, int itemId) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity='ItemCommissionScheduleByItemId'",
                u.getOrganizationId(), u.getCompanyId(), itemId);
        return !rows.isEmpty() && truthyObj(rows.get(0).get("OnSale"));
    }

    /**
     * combitem_ValueChanged (SaleOrder.cs:1670): CommonServices.GetAvailableStockFromTransactionOrEvaluation(item,
     * DocDate, 0, 0, null, 0, 0, actionId 2, WithoutStockLogic 1).BalWeight -> GetAvgRatesAndStockInHand
     * .GetAvailableStockByTransactionOrEvaluation (IL GetAvgRatesAndStockInHand.txt:1100) = usp_getAvailableStock
     * @OrganizationId,@CompanyId,@ItemId,@DateTo, then @WithouStockLogic=1 and @ActionId=2 (both non-zero, so sent);
     * warehouse, job lot, crop year, packing type and pack UOM are zero/null here and so are not sent.
     */
    public double availableStockWeight(UserAccount u, int itemId, Date docDate) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.usp_getAvailableStock @OrganizationId=?, @CompanyId=?, @ItemId=?, @DateTo=?, @WithouStockLogic=1, @ActionId=2",
                u.getOrganizationId(), u.getCompanyId(), itemId, docDate);
        if (rows.isEmpty()) return 0;
        Object w = rows.get(0).get("BalWeight");
        return w instanceof Number ? ((Number) w).doubleValue() : 0;
    }

    /**
     * Insert() (SaleOrder.cs:3142-3144) -> SaleOrder.CheckOrderExist (BLL 0596:1961) = USP_SaleOrderValidations per
     * detail row, stopping at the first row that returns a row. The procedure always ends in
     * SELECT @WarningMessage, so in practice only the FIRST detail line is ever checked - reproduced as-is.
     * Parameters and guards: @OrganizationId, @Id (when non-zero), @CompanyId, @DocDate, @OrderSupCustId (non-zero),
     * @OrderItemId (non-zero), @OrderItemQty, @OrderItemRate, @Amount. Returns the warning text, "" when none.
     */
    public String orderExistsWarning(UserAccount u, int id, Date docDate, int customerId,
                                     int itemId, Double qty, Double rate, Double amount) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        add(names, args, "@OrganizationId", u.getOrganizationId());
        if (id != 0) add(names, args, "@Id", id);
        add(names, args, "@CompanyId", u.getCompanyId());
        if (docDate != null) add(names, args, "@DocDate", docDate);
        if (customerId != 0) add(names, args, "@OrderSupCustId", customerId);
        if (itemId != 0) add(names, args, "@OrderItemId", itemId);
        add(names, args, "@OrderItemQty", qty);
        add(names, args, "@OrderItemRate", rate);
        add(names, args, "@Amount", amount);
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_SaleOrderValidations ");
        for (int i = 0; i < names.size(); i++) sql.append(i == 0 ? "" : ", ").append(names.get(i)).append("=?");
        List<Map<String, Object>> rows = jdbc.queryForList(sql.toString(), args.toArray());
        if (rows.isEmpty() || rows.get(0).get("WarningMessage") == null) return "";
        return String.valueOf(rows.get(0).get("WarningMessage"));
    }

    // ------------------------------------------------------------------ configuration defaults

    /**
     * Values SaleOrder.cs reads when it opens:
     *  - ConfigurationDefault (:786-826): City Area, Job/Lot, Paking Type, Default Crop Year, Warehouse (Conversion.ToInt).
     *  - PurchsaeOrder_Load (:620-623): CheckStockAndGiveWarningMessageOnSaleOrder, ItemSearchByCode, SaleOrderBranchWise.
     *  - :751-754: DefaultDaysToLessFromHistoryFromDate - history From date = today minus that many days, else minus 3.
     *  - :596-597: ERP features 4 and 6.
     */
    public Map<String, Object> configDefaults(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        String[][] ints = {{"cityAreaId", "City Area"}, {"jobLotId", "Job/Lot"}, {"packingTypeId", "Paking Type"},
                {"cropYearId", "Default Crop Year"}, {"warehouseId", "Warehouse"},
                {"defaultDaysToLessFromHistoryFromDate", "DefaultDaysToLessFromHistoryFromDate"}};
        for (String[] k : ints) {
            String v = config(u, k[1]);
            if (v.isEmpty()) continue;
            int n;
            try { n = (int) Double.parseDouble(v); } catch (NumberFormatException e) { n = 0; }
            out.put(k[0], n);
        }
        out.put("checkStockAndGiveWarningMessageOnSaleOrder", truthy(config(u, "CheckStockAndGiveWarningMessageOnSaleOrder")));
        out.put("itemSearchByCode", truthy(config(u, "ItemSearchByCode")));
        out.put("saleOrderBranchWise", truthy(config(u, "SaleOrderBranchWise")));
        Set<Integer> features = new HashSet<>();
        for (Map<String, Object> r : jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId())) features.add(num(r.get("Id")));
        out.put("subsidiaryAccountAllownOnVouchers", features.contains(4));
        out.put("hasMultiCurrencyFeature", features.contains(6));
        /* clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount (CommonServices.cs:5440) - FCY rounding. */
        String fcy = config(u, "DefaultNoOfDecimalPointsForFcyAmount");
        int fcyDecimals;
        try { fcyDecimals = fcy.isEmpty() ? 0 : (int) Double.parseDouble(fcy); } catch (NumberFormatException e) { fcyDecimals = 0; }
        out.put("fcyDecimals", fcyDecimals);
        return out;
    }

    /**
     * CurrencyFill (SaleOrder.cs:3462): MultiCurrency.GetAll = Sp_MultiCurrency_GetAllMethod @organizationId,@CompanyId,
     * @Activity='ReadAll', bound Id / CurrencyCode with a placeholder row (BindDDLNew ZeroIndex true). Shown only with
     * ERP feature 6 (:597-608).
     */
    public List<Map<String, Object>> currencies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbc.queryForList(
                "EXEC dbo.Sp_MultiCurrency_GetAllMethod @organizationId=?, @CompanyId=?, @Activity='ReadAll'",
                u.getOrganizationId(), u.getCompanyId())) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", r.get("Id")); x.put("CurrencyCode", r.get("CurrencyCode")); x.put("CurrencyName", r.get("CurrencyName"));
            out.add(x);
        }
        return out;
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal - the ConfigKey text for this org/company, "" when absent. */
    private String config(UserAccount u, String description) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @DefinitionIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), description, null, "GetConfigurationByOrgCompandConfigDescription");
        Object v = rows.isEmpty() ? null : rows.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    private static boolean truthy(String v) { return "true".equalsIgnoreCase(v) || "1".equals(v); }
    private static boolean truthyObj(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        return o != null && truthy(String.valueOf(o).trim());
    }
    private static int num(Object o) { return o instanceof Number ? ((Number) o).intValue() : 0; }
    private static Object ci(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }
}
