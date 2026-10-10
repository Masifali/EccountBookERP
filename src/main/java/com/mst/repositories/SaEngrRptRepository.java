package com.mst.repositories;

import com.mst.models.UserAccount;
import java.math.BigDecimal;
import java.sql.Date;
import java.sql.ResultSet;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.ColumnMapRowMapper;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

/**
 * Group E2 (Sale Engr reports, modules 86 / 135): the report screens the desktop ships for the Engr lineage, each on the desktop's own
 * procedure and parameter set (the BLL call is quoted per method). Every guarded parameter (the desktop's "if (obj.X != 0)") is OMITTED
 * when unset, never sent as NULL. A procedure's result is its FIRST result set (SqlDataAdapter.Fill(DataTable)).
 *   555 frmGDNHistory_Engr             dbo.USP_GdnRegister_Eng                       InvGdn.GdnRegister_Eng
 *                                      dbo.USP_GetDataForDropDownFromGdn             InvGdn.GetDataForDropDownFromGdn
 *   554 frmSaleOrderHistory_Engr       dbo.USP_SaleOrderDetailRegister_Eng           SaleOrder.SaleOrderDetailRegister_Eng
 *                                      dbo.USP_SaleOrderSummaryRegister              SaleOrder.SaleOrderSummaryRegister
 *                                      dbo.USP_GetDataForDropDownFromSaleOrder       SaleOrder.GetDataForDropDownFromSaleOrder
 *                                      Sp_SaleOrder_GetAllMethod 'UpdateStatusandIsApprovedbyOrderId'
 *   848 Mfg frmSaleOrderHistory_Engr   [Mfg].[USP_SaleOrderDetailRegister_Eng]     SaleOrder.SaleOrderDetailRegister_Mfg
 *                                      dbo.USP_SaleOrderEng_SummaryRegister          SaleOrder.SaleOrderEng_SummaryRegister
 *                                      dbo.USP_GetBranchsAllocatedToUserFromSaleOrder SaleOrder.GetBranchesAllocatedToUserFromSaleOrder
 *   556 SaleReportWithActivities_Engr  dbo.USP_Sales_EvaulationDetailReports_Engr    InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
 *                                      dbo.USP_GetDataFromInventoryStocksEvaluationsForSales
 * Tenancy (Organization / Company / User / Branch / Financial Year) always comes from the caller (CurrentUserContext).
 */
@Repository
public class SaEngrRptRepository {
    private final JdbcTemplate jdbc;

    public SaEngrRptRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------ plumbing
    private static final class Sql {
        final StringBuilder text;
        final List<Object> args = new ArrayList<>();
        boolean any;
        Sql(String proc) { text = new StringBuilder("EXEC ").append(proc.indexOf('.') < 0 ? "dbo." + proc : proc).append(' '); }
        Sql p(String name, Object v) { text.append(any ? ", " : "").append('@').append(name).append("=?"); any = true; args.add(v); return this; }
        Sql nz(String name, int v) { if (v != 0) p(name, v); return this; }
        Sql ne(String name, String v) { if (v != null && !v.isEmpty()) p(name, v); return this; }
        Sql date(String name, LocalDate v) { if (v != null) p(name, Date.valueOf(v)); return this; }
    }

    private List<Map<String, Object>> first(Sql s) {
        List<List<Map<String, Object>>> all = sets(s);
        return all.isEmpty() ? new ArrayList<>() : all.get(0);
    }

    private List<List<Map<String, Object>>> sets(Sql s) {
        return jdbc.execute(s.text.toString(), (PreparedStatementCallback<List<List<Map<String, Object>>>>) st -> {
            for (int i = 0; i < s.args.size(); i++) st.setObject(i + 1, s.args.get(i));
            List<List<Map<String, Object>>> out = new ArrayList<>();
            boolean result = st.execute();
            while (true) {
                if (result) {
                    try (ResultSet rs = st.getResultSet()) {
                        List<Map<String, Object>> rows = new ArrayList<>();
                        ColumnMapRowMapper mapper = new ColumnMapRowMapper();
                        while (rs.next()) rows.add(mapper.mapRow(rs, rows.size()));
                        out.add(norm(rows));
                    }
                } else if (st.getUpdateCount() == -1) return out;
                result = st.getMoreResults();
            }
        });
    }

    /** Numbers as doubles, dates as local ISO text (a Timestamp would serialize as a UTC instant and can shift a day). */
    private static List<Map<String, Object>> norm(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof BigDecimal) v = ((BigDecimal) v).doubleValue();
                else if (v instanceof Timestamp) v = ((Timestamp) v).toLocalDateTime().toString();
                else if (v instanceof Date) v = ((Date) v).toLocalDate().toString();
                else if (v instanceof java.util.Date) v = new Date(((java.util.Date) v).getTime()).toLocalDate().toString();
                else if (v instanceof byte[]) v = null;
                m.put(e.getKey(), v);
            }
            out.add(m);
        }
        return out;
    }

    static Object ci(Map<String, Object> row, String key) {
        if (row == null) return null;
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    /** Conversion.ToInt: any failure / null -> 0 (banker's rounding like Convert.ToInt32). */
    public static int i(Object v) {
        if (v == null) return 0;
        try {
            double d = v instanceof Number ? ((Number) v).doubleValue() : Double.parseDouble(v.toString().trim());
            return Double.isNaN(d) || Double.isInfinite(d) ? 0 : (int) Math.rint(d);
        } catch (Exception e) { return 0; }
    }

    public static String str(Object v) { return v == null ? "" : v.toString(); }

    public static LocalDate day(Object v) {
        if (v == null) return null;
        String s = v.toString().trim();
        if (s.length() < 10) return null;
        try { return LocalDate.parse(s.substring(0, 10)); } catch (Exception e) { return null; }
    }

    private static Map<String, List<Map<String, Object>>> byActivity(List<Map<String, Object>> rows) {
        Map<String, List<Map<String, Object>>> out = new LinkedHashMap<>();
        for (Map<String, Object> r : rows) out.computeIfAbsent(str(ci(r, "Activity")), k -> new ArrayList<>()).add(r);
        return out;
    }

    // ------------------------------------------------------------------ shared lookups
    /** GlobalVariables_Helper.GetConfigValueFromGlobal: Sp_ConfigrationsAllocation_GetAllMethod, the ConfigKey text (null when not configured). */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), description, "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty()) return null;
        Object v = ci(rows.get(0), "ConfigKey");
        return v == null ? null : v.toString();
    }

    /** CommonServices.GetDecimalConfiguration: stringFormatsingle zeros (1..4 else none). */
    public int amountDecimals(UserAccount u) {
        int n = i(config(u, "Default NoofDecimal Points For Amount"));
        return n >= 1 && n <= 4 ? n : 0;
    }

    /** CommonServices.GetDecimalConfiguration: DecimalRateFormate = "#,#0." + zeros (1..4 = n, 0 = 2, else none). */
    public int rateDecimals(UserAccount u) {
        int n = i(config(u, "Default NoofDecimal Points For Rate"));
        if (n >= 1 && n <= 4) return n;
        return n == 0 ? 2 : 0;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period of the session's financial year, as local yyyy-MM-dd (null when unknown). */
    public String yearStart(UserAccount u, int yearId) {
        List<Map<String, Object>> years = jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId());
        Map<String, Object> pick = null;
        for (Map<String, Object> y : years) {
            Object id = ci(y, "Id");
            if (id instanceof Number && ((Number) id).intValue() == yearId) { pick = y; break; }
        }
        if (pick == null && !years.isEmpty() && yearId <= 0) pick = years.get(0);
        if (pick == null) return null;
        Object start = ci(pick, "Start_Period");
        if (start instanceof Timestamp) return ((Timestamp) start).toLocalDateTime().toLocalDate().toString();
        if (start instanceof java.util.Date) return new Date(((java.util.Date) start).getTime()).toLocalDate().toString();
        String t = start == null ? null : String.valueOf(start);
        return t == null ? null : t.substring(0, Math.min(10, t.length()));
    }

    /** What every screen needs to format its grid and to answer the date-type "Financial Year" choice. */
    public Map<String, Object> basics(UserAccount u, int yearId) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("amountDecimals", amountDecimals(u));
        d.put("rateDecimals", rateDecimals(u));
        d.put("yearStart", yearStart(u, yearId));
        d.put("userBranchId", u.getBranchesId());
        return d;
    }

    /** CommonServices.GetGlAccountIdBySupplierCustomerId: Sp_SupplierCustomer_GetAllMethod 'GetGlAccountIdBySupplierCustomerId' (first column). */
    public int glAccountIdOfParty(UserAccount u, int supplierCustomerId) {
        List<Map<String, Object>> rows = first(new Sql("Sp_SupplierCustomer_GetAllMethod").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("SupplierCustomerId", supplierCustomerId).p("Activity", "GetGlAccountIdBySupplierCustomerId"));
        if (rows.isEmpty() || rows.get(0).isEmpty()) return 0;
        return i(rows.get(0).values().iterator().next());
    }

    /** DMSAttachments.ReadAttachmentsbyRefDocumentTypeId(Id, RefDocumentTypeId): Sp_DMSAttachments_GetAllMethod 'ReadAttachmentsbyRefDocumentTypeId'. */
    public List<Map<String, Object>> attachmentsByRefDocumentType(UserAccount u, int id, int refDocumentTypeId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : first(new Sql("Sp_DMSAttachments_GetAllMethod").p("RefDocumentTypeId", refDocumentTypeId).p("Id", id)
                .p("Activity", "ReadAttachmentsbyRefDocumentTypeId"))) {
            Object org = ci(r, "OrganizationId"), co = ci(r, "CompanyId");
            if (org != null && i(org) != u.getOrganizationId()) continue;
            if (co != null && i(co) != u.getCompanyId()) continue;
            out.add(r);
        }
        return out;
    }

    /** frmApprovalCommentory.GridFill: Dashboard.ApprovalCommentory = USp_ApprovalCommentory @RefDocumentTypeId, @RefDocNoId (no tenant parameters). */
    public List<Map<String, Object>> approvalCommentory(int documentTypeId, int id) {
        return first(new Sql("USp_ApprovalCommentory").p("RefDocumentTypeId", documentTypeId).p("RefDocNoId", id));
    }

    /** CommonServices.SetRightsValueInRightsObject: the user's rights rows of the form's own screen (Sp_tblUserRights_GetAllMethod 'GetByUserId'). */
    public List<Map<String, Object>> userRights(UserAccount u, String screenName, String roleName) {
        return first(new Sql("Sp_tblUserRights_GetAllMethod").p("UserId", u.getId()).p("ScreenName", screenName).p("RightName", roleName)
                .p("CompanyId", u.getCompanyId()).p("Activity", "GetByUserId"));
    }

    // ------------------------------------------------------------------ 555 frmGDNHistory_Engr
    /**
     * ComboFill: InvGdn.GetDataForDropDownFromGdn(OrganizationId, CompanyId, DocumentTypeIds "1612") = dbo.USP_GetDataForDropDownFromGdn
     * (Activity not set, so not sent), split by its Activity column (ParentCategories, ItemCategories, ItemTypes, JobLot, Supplier,
     * ItemClassGroup, Warehouse, Item, RequestedBy, DeliveryType; text = ReferenceName).
     */
    public Map<String, Object> gdnLookups(UserAccount u, int yearId) {
        Map<String, Object> d = basics(u, yearId);
        List<Map<String, Object>> rows = first(new Sql("USP_GetDataForDropDownFromGdn").p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId()).p("DocumentTypeIds", "1612"));
        d.put("combos", byActivity(rows));
        return d;
    }

    /**
     * gridHisory: InvGdn.GdnRegister_Eng = dbo.USP_GdnRegister_Eng. Always: OrganizationId, CompanyId, BranchesId (UserAccount.BranchesId),
     * FinancialYearId, DocumentTypeId 1612, FromDate, ToDate; the rest only when non-zero. referred: 1 Reffered, 2 Not Reffered (@IsRefered).
     */
    public List<Map<String, Object>> gdnRegisterEng(UserAccount u, int yearId, Map<String, String> q) {
        Sql s = new Sql("USP_GdnRegister_Eng").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId()).p("FinancialYearId", yearId);
        s.p("DocumentTypeId", 1612);
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        s.nz("FromDocNo", i(q.get("fromDocNo"))).nz("ToDocNo", i(q.get("toDocNo")));
        s.nz("SupplierCustomerId", i(q.get("supplierCustomerId")));
        s.nz("InventoryParentCategoriesIds", i(q.get("parentCategoryId"))).nz("ItemTypeId", i(q.get("itemTypeId")));
        s.nz("ItemClassGroupId", i(q.get("itemClassId"))).nz("ItemCategoryId", i(q.get("categoryId"))).nz("ItemId", i(q.get("itemId")));
        s.nz("DeliveryTypeId", i(q.get("deliveryTypeId"))).nz("RequestedById", i(q.get("requestedById")));
        s.nz("JobLotId", i(q.get("jobLotId"))).nz("WarehouseId", i(q.get("warehouseId")));
        s.nz("OrderNoFrom", i(q.get("orderNoFrom"))).nz("OrderNoTo", i(q.get("orderNoTo")));
        s.nz("IsRefered", i(q.get("referred")));
        return first(s);
    }

    // ------------------------------------------------------------------ 556 SaleReportWithActivities_Engr
    /**
     * AllComboBind: InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales = dbo.USP_GetDataFromInventoryStocksEvaluationsForSales
     * (OrganizationId, CompanyId, UserId, AppId, DocType "Sale"; Activity is null so it is not sent), split by the Activity column
     * (GetParentCategory, GetItemType, GetItemCategory, GetSupplierCustomer, GetItems, GetCity, GetJobLot, GetWarehouse; text = RefName).
     */
    public Map<String, Object> evaluationLookups(UserAccount u, int yearId, int appId) {
        Map<String, Object> d = basics(u, yearId);
        List<Map<String, Object>> rows = first(new Sql("USP_GetDataFromInventoryStocksEvaluationsForSales").p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId()).p("UserId", u.getId()).p("AppId", appId).p("DocType", "Sale"));
        d.put("combos", byActivity(rows));
        return d;
    }

    /**
     * GridFill: Sales_EvaulationDetailReports_Engr = dbo.USP_Sales_EvaulationDetailReports_Engr: OrganizationId, CompanyId, FromDate, ToDate,
     * then only when non-zero DocNoFrom, DocNoTo, InventoryParentCategories, ItemCategoryId, ItemTypeId, ItemClassGroupId, ItemId, PackUom,
     * JobLotId, SupplierCustomerId, WarehouseId, CityId, and @ActivityName = the activity text (when not empty).
     */
    public List<Map<String, Object>> salesActivity(UserAccount u, Map<String, String> q) {
        Sql s = new Sql("USP_Sales_EvaulationDetailReports_Engr").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId());
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        s.nz("DocNoFrom", i(q.get("fromDocNo"))).nz("DocNoTo", i(q.get("toDocNo")));
        s.nz("InventoryParentCategories", i(q.get("parentCategoryId"))).nz("ItemCategoryId", i(q.get("categoryId")));
        s.nz("ItemTypeId", i(q.get("itemTypeId"))).nz("ItemClassGroupId", i(q.get("itemClassId"))).nz("ItemId", i(q.get("itemId")));
        s.nz("PackUom", i(q.get("packUom"))).nz("JobLotId", i(q.get("jobLotId"))).nz("SupplierCustomerId", i(q.get("supplierCustomerId")));
        s.nz("WarehouseId", i(q.get("warehouseId"))).nz("CityId", i(q.get("cityId")));
        s.ne("ActivityName", q.get("activity"));
        return first(s);
    }

    // ------------------------------------------------------------------ 554 frmSaleOrderHistory_Engr
    /**
     * AllDropDownBind: SaleOrder.GetDataForDropDownFromSaleOrder (Architecture.BLL.Inventory) = dbo.USP_GetDataForDropDownFromSaleOrder with
     * OrganizationId, CompanyId, AppId, UserId only (no cost centre / customer / document types / branches / Activity), split by Activity
     * (Customer, Item, JobLot, City, ParentCategory, ItemCategory, ItemType; text = ReferenceName).
     */
    public Map<String, Object> orderLookups(UserAccount u, int yearId, int appId) {
        Map<String, Object> d = basics(u, yearId);
        List<Map<String, Object>> rows = first(new Sql("USP_GetDataForDropDownFromSaleOrder").p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId()).p("AppId", appId).p("UserId", u.getId()));
        d.put("combos", byActivity(rows));
        return d;
    }

    /**
     * gridHisory: SaleOrder.SaleOrderDetailRegister_Eng = dbo.USP_SaleOrderDetailRegister_Eng. The form never sets BranchesId (CLR 0, sent) and
     * DocumentTypeId (0, not sent). statusText = the Status combo text (Open / Cancel / Complete); approval: Approve -> IsApproved true,
     * UnApporve -> false, All -> ApprovedFilter "All" (IsApproved not sent); any other text sends false like the unset field.
     */
    public List<Map<String, Object>> orderDetail(UserAccount u, Map<String, String> q) {
        Sql s = new Sql("USP_SaleOrderDetailRegister_Eng").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()).p("BranchesId", 0);
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        s.nz("FromDocNo", i(q.get("fromDocNo"))).nz("ToDocNo", i(q.get("toDocNo")));
        s.nz("SupplierCustomerId", i(q.get("supplierCustomerId"))).nz("ItemId", i(q.get("itemId")));
        s.nz("InventoryParentCategoriesId", i(q.get("parentCategoryId"))).nz("ItemCategoryId", i(q.get("categoryId"))).nz("ItemTypeId", i(q.get("itemTypeId")));
        s.nz("ActionId", i(q.get("actionId"))).nz("SkipZero", i(q.get("skipZero")));
        s.ne("Status", q.get("status"));
        String approval = str(q.get("approval"));
        if (!"All".equals(approval)) s.p("IsApproved", "Approve".equals(approval));
        return first(s);
    }

    /**
     * GridSummaryFill: SaleOrder.SaleOrderSummaryRegister (Inventory) = dbo.USP_SaleOrderSummaryRegister: OrganizationId, CompanyId, UserId, AppId always,
     * dates, DocNoFrom/To, OrderItemId, InventoryParentCategories (ParentCategoryId), ItemCategoryId, ItemTypeId, OrderSupCustId, JobLotId, CityId,
     * ActionId, ActivityName (the report type text), SkipZero, IsOnQty 1 and @IsApproved = false (the form never sets IsApproved nor ApprovedFilter).
     */
    public List<Map<String, Object>> orderSummary(UserAccount u, int appId, Map<String, String> q) {
        Sql s = new Sql("USP_SaleOrderSummaryRegister").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("UserId", u.getId()).p("AppId", appId);
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        s.nz("DocNoFrom", i(q.get("fromDocNo"))).nz("DocNoTo", i(q.get("toDocNo")));
        s.nz("OrderItemId", i(q.get("itemId"))).nz("InventoryParentCategories", i(q.get("parentCategoryId")));
        s.nz("ItemCategoryId", i(q.get("categoryId"))).nz("ItemTypeId", i(q.get("itemTypeId"))).nz("OrderSupCustId", i(q.get("supplierCustomerId")));
        s.nz("JobLotId", i(q.get("jobLotId"))).nz("CityId", i(q.get("cityId"))).nz("ActionId", i(q.get("actionId")));
        s.ne("ActivityName", q.get("activity"));
        s.nz("SkipZero", i(q.get("skipZero"))).p("IsOnQty", 1).p("IsApproved", false);
        return first(s);
    }

    /**
     * SaleOrder.UpdateStatusandIsApprovedbyOrderId(ApprovalList with one item): Sp_SaleOrder_GetAllMethod @ReqType (Approve | Status | Cancel |
     * UpdateExpiryDate), @Id, @OrganizationId, @CompanyId, @PostUser (= the user), @DocDate (only for UpdateExpiryDate), @Activity.
     * The form sets no Comments, so @OrderStatusRemarks is not sent.
     */
    public void updateOrderStatus(UserAccount u, int id, String reqType, LocalDate postDate) {
        Sql s = new Sql("Sp_SaleOrder_GetAllMethod").ne("ReqType", reqType).nz("Id", id).p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId()).p("PostUser", u.getId());
        s.date("DocDate", postDate);
        s.p("Activity", "UpdateStatusandIsApprovedbyOrderId");
        first(s);
    }

    // ------------------------------------------------------------------ 848 Mfg frmSaleOrderHistory_Engr (module 135)
    /**
     * Architecture.WinApp.Mfg.Reports.frmSaleOrderHistory_Engr. BranchesFill: SaleOrder.GetBranchesAllocatedToUserFromSaleOrder(Org, Company, User, 1656) =
     * dbo.USP_GetBranchsAllocatedToUserFromSaleOrder (BranchId, BranchName). AllDropDownBind: SaleOrder.GetDataForDropDownFromSaleOrder with OrganizationId,
     * CompanyId, AppId, UserId, DocumentTypeIds "1656" and BranchesIds (the comma led ids of the checked branches, only when any) = dbo.USP_GetDataForDropDownFromSaleOrder;
     * split by Activity: Customer, Item, ParentCategory, ItemCategory, ItemType, City (ZeroIndex true).
     */
    public List<Map<String, Object>> mfgBranches(UserAccount u) {
        return first(new Sql("USP_GetBranchsAllocatedToUserFromSaleOrder").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("UserId", u.getId()).p("DocumentTypeId", 1656));
    }

    public Map<String, Object> mfgOrderLookups(UserAccount u, int yearId, int appId, String branchIds, boolean withBranches) {
        Map<String, Object> d = basics(u, yearId);
        if (withBranches) d.put("branches", mfgBranches(u));
        Sql s = new Sql("USP_GetDataForDropDownFromSaleOrder").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("AppId", appId).p("UserId", u.getId()).p("DocumentTypeIds", "1656");
        s.ne("BranchesIds", branchIds);
        d.put("combos", byActivity(first(s)));
        return d;
    }

    /**
     * gridHisory (Mfg): SaleOrder.SaleOrderDetailRegister_Mfg = [Mfg].[USP_SaleOrderDetailRegister_Eng]. The form fills obj.BranchesIds but the BLL only sends
     * BranchesId, which the form never sets (0, sent); DocumentTypeId 1656; the rest as the Inventory form (approval: Approve -> true, UnApprove -> false, All -> not sent).
     */
    public List<Map<String, Object>> mfgOrderDetail(UserAccount u, Map<String, String> q) {
        Sql s = new Sql("Mfg.USP_SaleOrderDetailRegister_Eng").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()).p("BranchesId", 0);
        s.p("DocumentTypeId", 1656);
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        s.nz("FromDocNo", i(q.get("fromDocNo"))).nz("ToDocNo", i(q.get("toDocNo")));
        s.nz("SupplierCustomerId", i(q.get("supplierCustomerId"))).nz("ItemId", i(q.get("itemId")));
        s.nz("InventoryParentCategoriesId", i(q.get("parentCategoryId"))).nz("ItemCategoryId", i(q.get("categoryId"))).nz("ItemTypeId", i(q.get("itemTypeId")));
        s.nz("ActionId", i(q.get("actionId"))).nz("SkipZero", i(q.get("skipZero")));
        s.ne("Status", q.get("status"));
        String approval = str(q.get("approval"));
        if (!"All".equals(approval)) s.p("IsApproved", "Approve".equals(approval));
        return first(s);
    }

    /**
     * GridSummaryFill (Mfg): SaleOrder.SaleOrderEng_SummaryRegister = dbo.USP_SaleOrderEng_SummaryRegister: OrganizationId, CompanyId, UserId, AppId always, dates, DocNoFrom/To,
     * OrderItemId, InventoryParentCategories, ItemCategoryId, ItemTypeId, OrderSupCustId, CityId, ActivityName (the report type text), SkipZero, ActionId; ApprovedFilter is
     * "All" so @IsApproved is not sent; the form sets neither BranchesIds nor Status here.
     */
    public List<Map<String, Object>> mfgOrderSummary(UserAccount u, int appId, Map<String, String> q) {
        Sql s = new Sql("USP_SaleOrderEng_SummaryRegister").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("UserId", u.getId()).p("AppId", appId);
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        s.nz("DocNoFrom", i(q.get("fromDocNo"))).nz("DocNoTo", i(q.get("toDocNo")));
        s.nz("OrderItemId", i(q.get("itemId"))).nz("InventoryParentCategories", i(q.get("parentCategoryId")));
        s.nz("ItemCategoryId", i(q.get("categoryId"))).nz("ItemTypeId", i(q.get("itemTypeId"))).nz("OrderSupCustId", i(q.get("supplierCustomerId")));
        s.nz("CityId", i(q.get("cityId")));
        s.ne("ActivityName", q.get("activity"));
        s.nz("SkipZero", i(q.get("skipZero"))).nz("ActionId", i(q.get("actionId")));
        return first(s);
    }
}
