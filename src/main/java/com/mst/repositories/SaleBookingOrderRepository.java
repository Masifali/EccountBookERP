package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.CallableStatement;
import java.sql.ResultSet;
import java.sql.Timestamp;
import java.sql.Types;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

/**
 * Screen 140 "Booking Order" - Architecture.WinApp.Sale.BookingOrder, DocumentTypeId 127 (Sale Order table).
 * Persistence is BLL 0596 SaleOrder.Save -> DAL 0449 SetData: Sp_SaleOrder_Insert / Sp_SaleOrder_Update for the header (model
 * properties mapped to the procedure parameters by name), Sp_SaleOrderDetail_Insert for every detail (deleted rows carry
 * ActionTypeId 3), then [DAW].[USp_DocumentApprovalDetail_Insert].
 * Every procedure is called through {@link #run}, which sends only the parameters the procedure declares (sys.parameters),
 * so a value the desktop does not send for a given procedure is never sent here either.
 */
@Repository("saleBookingOrderRepository")
public class SaleBookingOrderRepository extends SaleGdnPurchaseReturnRepository {
    public static final int DOC = 127;
    public static final int PRICE_TYPE = 6;
    public static final String SCREEN = "BookingOrder";

    private final Map<String, List<String>> names = new ConcurrentHashMap<>();
    private final Map<String, List<Integer>> kinds = new ConcurrentHashMap<>();

    public SaleBookingOrderRepository(JdbcTemplate j) { super(j); }

    @Override protected int documentTypeId() { return DOC; }
    @Override protected String screenName() { return SCREEN; }
    @Override protected String recordLabel() { return "Booking Order"; }

    public static LinkedHashMap<String, Object> m(Object... kv) {
        LinkedHashMap<String, Object> x = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) x.put(String.valueOf(kv[i]), kv[i + 1]);
        return x;
    }

    private void load(String proc) {
        if (names.containsKey(proc)) return;
        List<String> n = new ArrayList<>(); List<Integer> k = new ArrayList<>();
        for (var r : q("SELECT p.name,t.system_type_id FROM sys.parameters p JOIN sys.types t ON p.user_type_id=t.user_type_id WHERE p.object_id=OBJECT_ID(?) ORDER BY p.parameter_id", proc)) {
            n.add(String.valueOf(r.get("name")).substring(1)); k.add(jdbcType(number(r.get("system_type_id"))));
        }
        names.put(proc, n); kinds.put(proc, k);
    }

    private static Object pick(Map<String, Object> v, String key) {
        for (var e : v.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }
    private static boolean has(Map<String, Object> v, String key) {
        for (var e : v.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return true;
        return false;
    }

    /** EXEC proc with the declared parameters that appear in {@code v} (case-insensitive); Activity falls back to MethodType. */
    public List<Map<String, Object>> run(String proc, Map<String, Object> v) {
        load(proc);
        List<String> declared = names.get(proc);
        StringBuilder sql = new StringBuilder("EXEC ").append(proc.contains(".") ? proc : "dbo." + proc);
        List<Object> a = new ArrayList<>();
        boolean first = true;
        for (String p : declared) {
            String key = p;
            if (!has(v, key) && p.equalsIgnoreCase("MethodType") && has(v, "Activity")) key = "Activity";
            if (!has(v, key)) continue;
            sql.append(first ? " @" : ",@").append(p).append("=?"); first = false;
            a.add(pick(v, key));
        }
        return q(sql.toString(), a.toArray());
    }

    /** Header/detail save call: every declared parameter is sent (missing ones as 0/false/null), the first scalar is returned. */
    private int call(String proc, Map<String, Object> values) {
        load(proc);
        List<String> ps = names.get(proc); List<Integer> ts = kinds.get(proc);
        String marks = String.join(",", Collections.nCopies(ps.size(), "?"));
        String full = proc.contains(".") ? proc : "dbo." + proc;
        return jdbc.execute((ConnectionCallback<Integer>) c -> {
            try (CallableStatement st = c.prepareCall("{call " + full + "(" + marks + ")}")) {
                for (int i = 0; i < ps.size(); i++) {
                    int t = ts.get(i);
                    Object value = pick(values, ps.get(i));
                    if (value == null) value = defaultValue(t);
                    if (value instanceof String text && (t == Types.DATE || t == Types.TIMESTAMP))
                        value = text.isBlank() ? null : (t == Types.DATE ? java.sql.Date.valueOf(text.substring(0, 10))
                                : Timestamp.valueOf(text.length() == 10 ? text + " 00:00:00" : text.replace('T', ' ')));
                    st.setObject(i + 1, value, t);
                }
                boolean result = st.execute(); int found = 0;
                while (true) {
                    if (result) { try (ResultSet rs = st.getResultSet()) { if (rs.next() && found == 0) { Object f = rs.getObject(1); if (f instanceof Number) found = ((Number) f).intValue(); } } }
                    else if (st.getUpdateCount() == -1) break;
                    result = st.getMoreResults();
                }
                return found;
            }
        });
    }
    private static Object defaultValue(int type) {
        return switch (type) { case Types.BIT, Types.BOOLEAN -> false; case Types.TINYINT, Types.SMALLINT, Types.INTEGER -> 0; case Types.BIGINT -> 0L;
            case Types.FLOAT, Types.REAL, Types.DOUBLE, Types.NUMERIC, Types.DECIMAL -> 0d; default -> null; };
    }
    private static int jdbcType(int sql) {
        return switch (sql) { case 40 -> Types.DATE; case 42, 43, 58, 61 -> Types.TIMESTAMP; case 104 -> Types.BIT; case 48 -> Types.TINYINT; case 52 -> Types.SMALLINT;
            case 56 -> Types.INTEGER; case 127 -> Types.BIGINT; case 59 -> Types.REAL; case 62 -> Types.DOUBLE; case 106, 108 -> Types.DECIMAL; case 165 -> Types.VARBINARY; default -> Types.NVARCHAR; };
    }

    private LinkedHashMap<String, Object> base(UserAccount u) { return m("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }

    // ---------------------------------------------------------------------------------------------- generators

    /** GenerateSaleOrderCodeByDocId (column DocNo). */
    public Object docNo(UserAccount u, int year) {
        var v = base(u); v.put("DocumentTypeId", DOC); v.put("FinancialYearId", year); v.put("Activity", "GenerateSaleOrderCodeByDocId");
        var r = run("Sp_SaleOrder_GetAllMethod", v);
        return r.isEmpty() ? 1 : r.get(0).get("DocNo");
    }
    /** GenerateSaleOrderBranchCodeByDocId (column BranchSrNo). */
    public Object branchSr(UserAccount u, int year) {
        var v = base(u); v.put("DocumentTypeId", DOC); v.put("FinancialYearId", year); v.put("BranchesId", u.getBranchesId()); v.put("Activity", "GenerateSaleOrderBranchCodeByDocId");
        var r = run("Sp_SaleOrder_GetAllMethod", v);
        return r.isEmpty() ? 1 : r.get(0).get("BranchSrNo");
    }
    /** GenerateSaleOrderCategoryCodeById (column CatagorySrNo). */
    public Object categorySr(UserAccount u, int year, int categoryId) {
        var v = base(u); v.put("OrderCatagoryId", categoryId); v.put("FinancialYearId", year); v.put("Activity", "GenerateSaleOrderCategoryCodeById");
        var r = run("Sp_SaleOrder_GetAllMethod", v);
        return r.isEmpty() ? 1 : r.get(0).get("CatagorySrNo");
    }

    // ---------------------------------------------------------------------------------------------- form lists

    public Map<String, Object> initial(UserAccount u, int year) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("nextNo", docNo(u, year));
        o.put("branchSr", branchSr(u, year));
        o.put("orderCategories", q("EXEC dbo.Sp_InvOrderCategory_GetAllMethod @Activity='GetAll'"));
        boolean sub = feature(u, 4);
        o.put("customers", sub
                ? q("EXEC dbo.USP_GetVendorsAndCustomers @OrganizationId=?,@CompanyId=?,@PartyTypeId=2", u.getOrganizationId(), u.getCompanyId())
                : q("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadByOrganizationIdCompanyIdForBinding'", u.getOrganizationId(), u.getCompanyId()));
        o.put("items", q("EXEC dbo.Sp_ItemPricingSchedule_GetAllMethod @OrganizationId=?,@CompanyId=?,@PriceTypeId=?,@Activity='ReadByItemIdAndPriceLookUpId'", u.getOrganizationId(), u.getCompanyId(), PRICE_TYPE));
        o.put("packingTypes", q("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity='ReadAll'"));
        o.put("cities", q("EXEC dbo.SP_City_GetAllMethod @OrganizationId=?,@CompanyId=?,@MethodType='GetAll'", u.getOrganizationId(), u.getCompanyId()));
        o.put("referenceParties", q("EXEC dbo.Sp_ReferenceParties_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'", u.getOrganizationId(), u.getCompanyId()));
        Map<String, Object> cfg = new LinkedHashMap<>();
        cfg.put("roundingForItemPricing", config(u, "RoundingForItemPricing"));
        cfg.put("itemSearchByCode", truthy(config(u, "ItemSearchByCode")));
        cfg.put("branchWise", truthy(config(u, "SaleOrderBranchWise")));
        cfg.put("cityArea", config(u, "City Area"));
        String days = config(u, "DefaultDaysToLessFromHistoryFromDate");
        int d = 0; try { d = (int) Double.parseDouble(days); } catch (Exception ignored) { /* not a number */ }
        cfg.put("historyDays", d);
        o.put("config", cfg);
        return o;
    }

    /** UOMSchedule.SearchByObject for the item: Id, ScheduleUnitId, Equivalent (Sp_UOMSchedule_GetAllMethod 'ReadByItemID'). */
    public List<Map<String, Object>> uoms(UserAccount u, int itemId) {
        return q("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@Activity='ReadByItemID'", u.getOrganizationId(), u.getCompanyId(), itemId);
    }

    /** usp_getCropYearFromPricingScheduleByItemId. */
    public List<Map<String, Object>> cropYears(UserAccount u, int itemId, Timestamp date) {
        var v = base(u); v.put("ItemId", itemId); v.put("PriceTypeId", PRICE_TYPE); v.put("EffectedDate", date);
        return run("usp_getCropYearFromPricingScheduleByItemId", v);
    }
    /** Sp_ItemPricingSchedule_GetAllMethod 'GetItemRateAndUomIdByEffectedDate' - Id, RateUomId, ItemPrice, PackingUom. */
    public List<Map<String, Object>> rate(UserAccount u, int itemId, int cropYearId, Timestamp date) {
        var v = base(u); v.put("ItemId", itemId); v.put("PriceTypeId", PRICE_TYPE); v.put("CropYearId", cropYearId); v.put("EffectedDate", date); v.put("Activity", "GetItemRateAndUomIdByEffectedDate");
        return run("Sp_ItemPricingSchedule_GetAllMethod", v);
    }
    /** USP_GetRegularItemDiscount (MasterDiscountTypeId 19) - DiscountValue, else 0. */
    public double itemDiscount(UserAccount u, int itemId, Timestamp date) {
        var v = base(u); v.put("ItemId", itemId); v.put("PriceTypeId", PRICE_TYPE); v.put("MasterDiscountTypeId", 19); v.put("CurrentDate", date);
        var r = run("USP_GetRegularItemDiscount", v);
        return r.isEmpty() ? 0d : dbl(r.get(0).get("DiscountValue"));
    }
    /** Sp_CustomerDiscountPolicy_GetAllMethod 'GetCustomerDiscountByEffectiveDate' - Rows[0][0], else 0 (the procedure's own typo @OrgnaizationId is honoured). */
    public double customerDiscount(UserAccount u, int customerId, int itemId, Timestamp date) {
        var v = base(u); v.put("OrgnaizationId", u.getOrganizationId()); v.put("CustomerId", customerId); v.put("SupplierCustomerId", customerId); v.put("ItemId", itemId);
        v.put("EffectiveDate", date); v.put("EffectedDate", date); v.put("Activity", "GetCustomerDiscountByEffectiveDate");
        var r = run("Sp_CustomerDiscountPolicy_GetAllMethod", v);
        return r.isEmpty() ? 0d : dbl(r.get(0).values().iterator().next());
    }
    /** Usp_InvPackingChangePriceScheduleGetPackingAddLess - PackingAddLess, else 0. */
    public double packingAddLess(UserAccount u, int uomId, int customerId, Timestamp date) {
        var v = base(u); v.put("UomId", uomId); v.put("ItemUomId", uomId); v.put("SupplierCustomerId", customerId); v.put("DocDate", date); v.put("EffectedDate", date);
        var r = run("Usp_InvPackingChangePriceScheduleGetPackingAddLess", v);
        return r.isEmpty() ? 0d : dbl(r.get(0).get("PackingAddLess"));
    }
    /** Sp_Item_GetAllMethod 'ItemCommissionScheduleByItemId' - OnSale truthy. */
    public boolean commOnSale(UserAccount u, int itemId) {
        var v = base(u); v.put("ItemId", itemId); v.put("Activity", "ItemCommissionScheduleByItemId");
        var r = run("Sp_Item_GetAllMethod", v);
        if (r.isEmpty()) return false;
        Object o = r.get(0).get("OnSale");
        return o instanceof Boolean ? (Boolean) o : o != null && truthy(String.valueOf(o));
    }

    // ---------------------------------------------------------------------------------------------- read

    /** SaleOrder.GetByID: header 'ReadById', details 'ReadBySaleOrderHeaderId'. */
    @Override
    public Map<String, Object> record(UserAccount u, int id) {
        var v = base(u); v.put("Id", id); v.put("Activity", "ReadById");
        var h = run("Sp_SaleOrder_GetAllMethod", v);
        if (h.isEmpty() || number(h.get(0).get("DocumentTypeId")) != DOC && h.get(0).containsKey("DocumentTypeId"))
            throw new NoSuchElementException("Record not found...");
        Map<String, Object> out = new LinkedHashMap<>(h.get(0));
        var d = base(u); d.put("Id", id); d.put("Activity", "ReadBySaleOrderHeaderId");
        out.put("details", run("Sp_SaleOrder_GetAllMethod", d));
        return out;
    }

    // ---------------------------------------------------------------------------------------------- history

    public List<Map<String, Object>> historyBranches(UserAccount u, boolean branchWise, String ownBranchName) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (branchWise) { out.add(m("Id", u.getBranchesId(), "BranchName", ownBranchName)); return out; }
        for (var r : q("EXEC dbo.USP_GetBranchsAllocatedToUserFromSaleOrder @OrganizationId=?,@CompanyId=?,@UserId=?,@DocumentTypeId=?", u.getOrganizationId(), u.getCompanyId(), u.getId(), DOC))
            out.add(m("Id", r.get("BranchId"), "BranchName", r.get("BranchName")));
        return out;
    }
    /** USP_GetDataForDropDownFromSaleOrder (DocumentTypeIds '127'), the 'Customer' rows. */
    public List<Map<String, Object>> historyCustomers(UserAccount u, String branchIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : q("EXEC dbo.USP_GetDataForDropDownFromSaleOrder @OrganizationId=?,@CompanyId=?,@AppId=?,@UserId=?,@DocumentTypeIds='127',@BranchesIds=?",
                u.getOrganizationId(), u.getCompanyId(), u.getAppId(), u.getId(), branchIds == null ? "" : branchIds)) {
            if ("Customer".equals(String.valueOf(r.get("Activity")))) out.add(m("Id", r.get("Id"), "Customer", r.get("ReferenceName")));
        }
        return out;
    }
    /** BLL 0596 GetHistory 'SaleOrderFormHistory': each parameter only when the form sends it. */
    public List<Map<String, Object>> history(UserAccount u, int year, boolean canViewAll, String mode, Timestamp from, Timestamp to,
                                             double fromNo, double toNo, int customerId, String branchIds) {
        var v = base(u); v.put("DocumentTypeId", DOC);
        if (year != 0) v.put("FinancialYearId", year);
        String fk, tk;
        switch (mode == null ? "" : mode) {
            case "entry": fk = "EntryFromDate"; tk = "EntryToDate"; break;
            case "modify": fk = "ModifyFromDate"; tk = "ModifyToDate"; break;
            case "approved": fk = "ApprovedFromDate"; tk = "ApprovedToDate"; break;
            default: fk = "DocDateFrom"; tk = "DocDateTo"; break;
        }
        if (from != null) v.put(fk, from);
        if (to != null) v.put(tk, to);
        if (fromNo != 0) v.put("PoSrFrom", fromNo);
        if (toNo != 0) v.put("PoSrTo", toNo);
        if (customerId != 0) v.put("SupplierCustomerId", customerId);
        v.put("CanViewAllRecord", canViewAll);
        if (!canViewAll) v.put("EntryUser", u.getId());
        if (branchIds != null && !branchIds.isEmpty()) v.put("BranchesIds", branchIds);
        v.put("Activity", "SaleOrderFormHistory");
        return run("Sp_SaleOrder_GetAllMethod", v);
    }

    // ---------------------------------------------------------------------------------------------- pre-save checks

    /** USP_SaleOrderValidations for one detail row - the WarningMessage of the first row, else null. */
    public String rowWarning(UserAccount u, Object id, Timestamp docDate, int customerId, Map<String, Object> d) {
        var v = base(u); v.put("Id", id); v.put("DocDate", docDate); v.put("OrderSupCustId", customerId);
        v.put("OrderItemId", d.get("OrderItemId")); v.put("OrderItemQty", d.get("OrderItemQty")); v.put("OrderItemRate", d.get("OrderItemRate")); v.put("Amount", d.get("Amount"));
        for (var r : run("USP_SaleOrderValidations", v)) {
            Object w = r.get("WarningMessage");
            if (w != null && !String.valueOf(w).isBlank()) return String.valueOf(w);
        }
        return null;
    }
    /** SP_CHECKSUPPLIERCUSTOMERLIMITS (@ActionId 2) - AvailableLimit. */
    public Double availableLimit(UserAccount u, int customerId, double amount) {
        var v = base(u); v.put("SupplierCustomerId", customerId); v.put("Amount", amount); v.put("ActionId", 2);
        var r = run("SP_CHECKSUPPLIERCUSTOMERLIMITS", v);
        if (r.isEmpty()) return null;
        Object o = r.get(0).get("AvailableLimit");
        return o == null ? null : dbl(o);
    }

    // ---------------------------------------------------------------------------------------------- save

    public int saveHeader(boolean insert, Map<String, Object> header) { return call(insert ? "Sp_SaleOrder_Insert" : "Sp_SaleOrder_Update", header); }
    public int saveDetail(Map<String, Object> detail) { return call("Sp_SaleOrderDetail_Insert", detail); }
    public void approval(UserAccount u, int id, double limitAmount) {
        call("[DAW].[USp_DocumentApprovalDetail_Insert]", m("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC, "Id", id, "LimitAmount", limitAmount));
    }

    private static double dbl(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o)); } catch (Exception e) { return 0d; }
    }
}
