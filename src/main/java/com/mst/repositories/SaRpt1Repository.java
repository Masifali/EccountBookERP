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
 * Group R1 (Sales Reports, module 53): the ten report screens that had no web page, each on the desktop's own procedure
 * and parameter set (the BLL calls are quoted per method). Every guarded parameter (the desktop's "if (obj.X != 0)") is
 * OMITTED when unset, never sent as NULL. A procedure's result is its FIRST result set (SqlDataAdapter.Fill(DataTable)),
 * except 908 and 489 which read the DataSet (Table0 / Table1) as the desktop does.
 *   832 GdnRegister                    [pcc].[USP_GdnRegister]                    InvGdn.GdnRegister
 *   840 SaleInvoiceRegister            [pcc].[USP_SaleInvoiceSummaryRegister]     InvSaleInvoice.SaleInvoiceSummaryRegister
 *   908 SaleInvoiceStockRateUpdate     usp_getSaleDataForCgsUpdate / usp_getEXportDataForCgsUpdate / usp_StockRateUpdateFromJobOrder
 *   490 OrdersWithLedgerBalance        USP_GetOutstandingOrdersWithLedgerBalance  SaleOrder.GetOutstandingOrdersWithLedgerBalance
 *   485 frmSaleInvoiceHistory          Sp_InvSaleInvoice_Rpt                      InvSaleInvoiceReports.InvSaleInvoiceRegister
 *   487 frmSaleInvoiceDirectRegister   sp_InvSaleInvoiceDirectRegister            InvSaleInvoiceReports.InvSaleInvoiceDirectRegister
 *   491 SalePriceListWithDiscount      USP_GetItemSalePriceListWithDiscounts
 *   486 frmSaleInvoiceReturnRegister   Sp_InvPurchaseInvoice_SaleReturnRegister
 *   488 frmPurchaseAndSaleDetailByJobLot usp_PurchaseAndSaleDetailByJobLot
 *   489 DeliveryOrderHistory           Sp_InvDeliveryOrderForApproval_Rpt         DeliverySchedule.GetDeliveryOrderForApproval
 * Tenancy (Organization / Company / User / Branch / Financial Year) always comes from the caller (CurrentUserContext).
 */
@Repository
public class SaRpt1Repository {
    private final JdbcTemplate jdbc;

    public SaRpt1Repository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------ plumbing
    private static final class Sql {
        final StringBuilder text;
        final List<Object> args = new ArrayList<>();
        boolean any;
        Sql(String proc) { text = new StringBuilder("EXEC ").append(proc.indexOf('.') < 0 ? "dbo." + proc : proc).append(' '); }
        Sql p(String name, Object v) { text.append(any ? ", " : "").append('@').append(name).append("=?"); any = true; args.add(v); return this; }
        Sql nz(String name, int v) { if (v != 0) p(name, v); return this; }
        Sql nzD(String name, double v) { if (v != 0.0) p(name, v); return this; }
        Sql ne(String name, String v) { if (v != null && !v.isEmpty()) p(name, v); return this; }
        Sql date(String name, LocalDate v) { if (v != null) p(name, Date.valueOf(v)); return this; }
    }

    /** The first result set of the procedure (SqlDataAdapter.Fill), skipping update counts. */
    private List<Map<String, Object>> first(Sql s) {
        List<List<Map<String, Object>>> all = sets(s);
        return all.isEmpty() ? new ArrayList<>() : all.get(0);
    }

    /** Every result set of the procedure (SqlDataAdapter.Fill(DataSet)). */
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

    /** Numbers as doubles (Conversion.ToDouble), dates as local ISO text (a Timestamp would serialize as a UTC instant and can shift a day). */
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
                else if (v instanceof byte[]) v = null;          // company logo blobs are only for the Crystal templates
                m.put(e.getKey(), v);
            }
            out.add(m);
        }
        return out;
    }

    private static Object ci(Map<String, Object> row, String key) {
        if (row == null) return null;
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    /** Conversion.ToInt: any failure / null -> 0. */
    public static int i(Object v) {
        if (v == null) return 0;
        try {
            double d = v instanceof Number ? ((Number) v).doubleValue() : Double.parseDouble(v.toString().trim());
            return Double.isNaN(d) || Double.isInfinite(d) ? 0 : (int) Math.rint(d);
        } catch (Exception e) { return 0; }
    }

    public static double dbl(Object v) {
        if (v == null) return 0.0;
        try { return v instanceof Number ? ((Number) v).doubleValue() : Double.parseDouble(v.toString().trim()); } catch (Exception e) { return 0.0; }
    }

    public static String str(Object v) { return v == null ? "" : v.toString(); }

    public static LocalDate day(Object v) {
        if (v == null) return null;
        String s = v.toString().trim();
        if (s.length() < 10) return null;
        try { return LocalDate.parse(s.substring(0, 10)); } catch (Exception e) { return null; }
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

    /** CommonServices.GetDecimalConfiguration: stringFormatsingle / stringFormatboth zeros (1..4 else none). */
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

    /** InvSaleInvoice.GetBranchsAllocatedToUserFromSaleInvoice(org, company, user, documentTypeId): USP_GetBranchsAllocatedToUserFromSaleInvoice. */
    public List<Map<String, Object>> branchesOfSaleInvoice(UserAccount u, int documentTypeId) {
        Sql s = new Sql("USP_GetBranchsAllocatedToUserFromSaleInvoice").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("UserId", u.getId()).nz("DocumentTypeId", documentTypeId);
        return first(s);
    }

    /** Branches.GetAll: Sp_Branches_GetAllMethod 'GetAll' (frmSaleInvoiceReturnRegister.BranchesFill). */
    public List<Map<String, Object>> branchesAll(UserAccount u) {
        return first(new Sql("Sp_Branches_GetAllMethod").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()).p("Activity", "GetAll"));
    }

    /** USP_GetBranchsAllocatedToUser (frmPurchaseAndSaleDetailByJobLot.BranchesFill). */
    public List<Map<String, Object>> branchesOfUser(UserAccount u) {
        return first(new Sql("USP_GetBranchsAllocatedToUser").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()).p("UserId", u.getId()));
    }

    /** Rows of a lookup procedure grouped by their Activity / ActivityType column, order kept. */
    private static Map<String, List<Map<String, Object>>> byActivity(List<Map<String, Object>> rows, String column) {
        Map<String, List<Map<String, Object>>> out = new LinkedHashMap<>();
        for (Map<String, Object> r : rows) { Object a = ci(r, column); if (a == null) a = ci(r, "Activity"); if (a == null) a = ci(r, "ActivityType"); out.computeIfAbsent(str(a), k -> new ArrayList<>()).add(r); }
        return out;
    }

    // ------------------------------------------------------------------ 832 GdnRegister
    /** GdnRegister.ComboFill: [pcc].[USP_DropDownFillFromSaleOrder] @OrganizationId, @CompanyId (no Activity). */
    public Map<String, Object> gdnLookups(UserAccount u, int yearId) {
        Map<String, Object> d = basics(u, yearId);
        List<Map<String, Object>> rows = first(new Sql("pcc.USP_DropDownFillFromSaleOrder").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()));
        d.put("combos", byActivity(rows, "Activity"));
        return d;
    }

    /**
     * FillDetailDataGrid (mode "detail") / FillMainGrid (mode "main") -> InvGdn.GdnRegister -> [pcc].[USP_GdnRegister].
     * Always: OrganizationId, CompanyId, BranchesId (UserAccount.BranchesId), FinancialYearId, DocumentTypeId 1855.
     * The detail grid also sends the item / category / type / parent category; both send dates, doc numbers, customer,
     * IsApproved (unless the status text is "All") and IsFOC (1 FOC, 2 Not FOC). The procedure itself compares
     * h.CompanyId = @BranchesId AND h.BranchesId = @CompanyId (swapped), so the call is replicated as the desktop makes it.
     * statusId: 1 UnApproved, 2 Approve, 3 All, 0 = empty status text (IsApproved false is sent, as the desktop does).
     */
    public List<Map<String, Object>> gdnRegister(UserAccount u, int yearId, boolean main, Map<String, String> q) {
        Sql s = new Sql("pcc.USP_GdnRegister").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId()).p("FinancialYearId", yearId);
        s.p("DocumentTypeId", 1855);
        s.date("FromDate", day(q.get("fromDate"))).date("Todate", day(q.get("toDate")));
        s.nz("FromDocNo", i(q.get("fromDocNo"))).nz("ToDocNo", i(q.get("toDocNo")));
        s.nz("SupplierCustomerId", i(q.get("supplierCustomerId")));
        if (!main) {
            s.nz("ItemId", i(q.get("itemId")));
        }
        int status = i(q.get("statusId"));
        if (status != 3) s.p("IsApproved", status == 2);
        if (!main) {
            s.nz("ItemCategoryId", i(q.get("itemCategoryId"))).nz("InventoryParentCategories", i(q.get("parentCategoryId"))).nz("ItemTypeId", i(q.get("itemTypeId")));
        }
        s.nz("IsFOC", i(q.get("foc")));
        return first(s);
    }

    /** GdnRegister.GenerateGdnSlip data: [pcc].[USP_InvGdn_Slip] (the slip itself prints through /reports/print/1855-inv-gdn-slip). */
    public List<Map<String, Object>> gdnSlip(UserAccount u, int id) {
        return first(new Sql("pcc.USP_InvGdn_Slip").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()).p("Id", id));
    }

    // ------------------------------------------------------------------ 840 SaleInvoiceRegister
    /** SaleInvoiceRegister.ComboFill: InvSaleInvoice.DropDownFillFromInvSaleInvoice = [pcc].[USP_DropDownFillFromInvSaleInvoice] (org, company only). */
    public Map<String, Object> saleInvoiceRegisterLookups(UserAccount u, int yearId) {
        Map<String, Object> d = basics(u, yearId);
        List<Map<String, Object>> rows = first(new Sql("pcc.USP_DropDownFillFromInvSaleInvoice").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()));
        d.put("combos", byActivity(rows, "Activity"));
        return d;
    }

    /**
     * GridSummaryFill -> InvSaleInvoice.SaleInvoiceSummaryRegister -> [pcc].[USP_SaleInvoiceSummaryRegister].
     * ParentCategoryId is NOT sent: the form fills obj.ParentCategoryId but the BLL reads obj.InventoryParentCategories (desktop behaviour).
     * statusId: 1 UnApproved, 2 Approve, 3 All, 0 = empty status text (IsApproved false is sent).
     */
    public List<Map<String, Object>> saleInvoiceRegister(UserAccount u, int yearId, Map<String, String> q) {
        Sql s = new Sql("pcc.USP_SaleInvoiceSummaryRegister").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId()).p("FinancialYearId", yearId);
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        s.nz("DocNoFrom", i(q.get("fromDocNo"))).nz("DocNoTo", i(q.get("toDocNo")));
        s.nz("ItemId", i(q.get("itemId")));
        s.nz("ItemCategoryId", i(q.get("itemCategoryId"))).nz("ItemTypeId", i(q.get("itemTypeId")));
        s.nz("CustomerGroupId", i(q.get("customerGroupId"))).nz("SupplierCustomerId", i(q.get("supplierCustomerId")));
        s.ne("ReferencePartyName", q.get("referencePartyName")).ne("ReferencePartyIds", q.get("refPartyIds"));
        s.nz("JobLotId", i(q.get("jobLotId"))).nz("CityId", i(q.get("cityId"))).nz("DistrictId", i(q.get("districtId"))).nz("RefSalesManId", i(q.get("refSaleManId")));
        s.nz("IsCityAdd", i(q.get("cityAdd"))).nz("IsVarientAdd", i(q.get("varientAdd")));
        s.nzD("VarientAvgRate", dbl(q.get("avgRate")));
        int status = i(q.get("statusId"));
        if (status != 3) s.p("IsApproved", status == 2);
        s.ne("ActivityName", q.get("reportType"));
        return first(s);
    }

    // ------------------------------------------------------------------ 908 SaleInvoiceStockRateUpdate
    /**
     * btnshow: usp_getSaleDataForCgsUpdate (Sale Invoice) or usp_getEXportDataForCgsUpdate (Export Invoice):
     * OrganizationId, CompanyId, FinancialYearId, FromDate, ToDate and ActionId = 1 only for "Settled". Table0 = invoices, Table1 = details.
     */
    public Map<String, Object> cgsData(UserAccount u, int yearId, boolean sale, boolean settled, LocalDate from, LocalDate to) {
        Sql s = new Sql(sale ? "usp_getSaleDataForCgsUpdate" : "usp_getEXportDataForCgsUpdate").p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId()).p("FinancialYearId", yearId).date("FromDate", from).date("ToDate", to);
        if (settled) s.p("ActionId", 1);
        List<List<Map<String, Object>>> ds = sets(s);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("main", ds.size() > 0 ? ds.get(0) : new ArrayList<>());
        out.put("detail", ds.size() > 1 ? ds.get(1) : new ArrayList<>());
        return out;
    }

    /**
     * InvSaleInvoice.StockRateUpdateFromJobOrder(list): per item usp_StockRateUpdateFromJobOrder @OrganizationId, @CompanyId,
     * @RefDocumentTypeId = DocumentTypeId, @RefDocIdNo = Id [, @ActionId when != 0]. The desktop's GenericProvider runs every call on its own
     * connection, so there is no transaction across the invoices; neither is there here.
     */
    public void stockRateUpdate(UserAccount u, int documentTypeId, int id, int actionId) {
        Sql s = new Sql("usp_StockRateUpdateFromJobOrder").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("RefDocumentTypeId", documentTypeId).p("RefDocIdNo", id).nz("ActionId", actionId);
        first(s);
    }

    // ------------------------------------------------------------------ 490 OrdersWithLedgerBalance
    /** OrdersWithLedgerBalance.suppliercustomer: SupplierCustomer.SupplierCustomerAgainstSaleOrder = Usp_SupplierCustomerAgainstSaleOrder. */
    public Map<String, Object> ledgerBalanceLookups(UserAccount u, int yearId) {
        Map<String, Object> d = basics(u, yearId);
        d.put("customers", first(new Sql("Usp_SupplierCustomerAgainstSaleOrder").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())));
        return d;
    }

    /** SaleOrder.GetOutstandingOrdersWithLedgerBalance: actionId 1 date wise, 2 current ledger, 3 party wise; the dates only when the date type is not 6 (All). */
    public List<Map<String, Object>> ledgerBalance(UserAccount u, int yearId, int actionId, int supplierCustomerId, LocalDate from, LocalDate to) {
        Sql s = new Sql("USP_GetOutstandingOrdersWithLedgerBalance").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("FinancialYearId", yearId).p("ActionId", actionId).date("FromDate", from).date("ToDate", to).nz("SupplierCustomerId", supplierCustomerId);
        return first(s);
    }

    // ------------------------------------------------------------------ 485 / 487 sale invoice registers
    /**
     * AllComboBindAgainstSale(Invoice): InvSaleInvoice.AllComboBindAgainstSaleInvoice = Usp_AllComboAgainstSaleInvoice with
     * OrganizationId, CompanyId, AppId, UserId, [DocumentTypeIds], [BranchesIds]; the rows are split by Activity.
     */
    public Map<String, Object> saleInvoiceCombos(UserAccount u, int appId, String documentTypeIds, String branchesIds) {
        Sql s = new Sql("Usp_AllComboAgainstSaleInvoice").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("AppId", appId).p("UserId", u.getId()).ne("DocumentTypeIds", documentTypeIds).ne("BranchesIds", branchesIds);
        return new LinkedHashMap<>(byActivity(first(s), "Activity"));
    }

    /** InvSaleInvoiceReports.InvSaleInvoiceRegister (485) = Sp_InvSaleInvoice_Rpt. */
    public List<Map<String, Object>> saleInvoiceHistory(UserAccount u, Map<String, String> q) {
        Sql s = new Sql("Sp_InvSaleInvoice_Rpt").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId());
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        s.nz("DocNoFrom", i(q.get("fromDocNo"))).nz("DocNoTo", i(q.get("toDocNo")));
        s.nz("ItemId", i(q.get("itemId"))).nz("SupplierCustomerId", i(q.get("supplierCustomerId")));
        s.nz("SaleOrderFrom", i(q.get("saleOrderFrom"))).nz("SaleOrderTo", i(q.get("saleOrderTo")));
        s.nzD("RateFrom", dbl(q.get("rateFrom"))).nzD("RateTo", dbl(q.get("rateTo")));
        s.nz("WarehouseId", i(q.get("warehouseId"))).nz("JobLotId", i(q.get("jobLotId")));
        s.ne("BranchesIds", q.get("branchesIds"));
        return first(s);
    }

    /** InvSaleInvoiceReports.InvSaleInvoiceDirectRegister (487) = sp_InvSaleInvoiceDirectRegister. */
    public List<Map<String, Object>> saleInvoiceDirect(UserAccount u, Map<String, String> q) {
        Sql s = new Sql("sp_InvSaleInvoiceDirectRegister").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId());
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        s.nz("ItemId", i(q.get("itemId"))).nz("ItemStockAccountId", i(q.get("stockAccountId"))).nz("SupplierCustomerId", i(q.get("supplierCustomerId")));
        s.nz("JobLotId", i(q.get("jobLotId"))).nz("WarehouseId", i(q.get("warehouseId"))).nz("DocumentTypeId", i(q.get("documentTypeId")));
        s.ne("Activity", q.get("activity")).ne("BranchesIds", q.get("branchesIds"));
        return first(s);
    }

    /** 487 btnRefresh_Click: CommonServices.WareHouseGetAllService = Sp_InvWareHouse_GetAllMethod 'ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> allWarehouses(UserAccount u) {
        return first(new Sql("Sp_InvWareHouse_GetAllMethod").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()).p("Activity", "ReadByOrganizationCompanyId"));
    }

    /** 487 btnRefresh_Click: CommonServices.JobLotGetAllService = SP_JobLot_ReadMethod 'GetAll'. */
    public List<Map<String, Object>> allJobLots(UserAccount u) {
        return first(new Sql("SP_JobLot_ReadMethod").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()).p("Activity", "GetAll"));
    }

    /** 487 btnRefresh_Click: CommonServices.ItemGetForComboServiceBind = Sp_Item_GetAllMethod 'ReadAllForComboTwoColumns'. */
    public List<Map<String, Object>> allItems(UserAccount u) {
        return first(new Sql("Sp_Item_GetAllMethod").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()).p("Activity", "ReadAllForComboTwoColumns"));
    }

    /** 487 btnRefresh_Click: CommonServices.SupplierCustomerGetforComboServiceBind = Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationIdCompanyIdForBinding'. */
    public List<Map<String, Object>> allSuppliers(UserAccount u) {
        return first(new Sql("Sp_SupplierCustomer_GetAllMethod").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("Activity", "ReadByOrganizationIdCompanyIdForBinding"));
    }

    /** CommonServices.VoucherHeadIdGet: VoucherHead.GetVoucherHeadIdByDocumentTypeIdandRefDocNoId = Sp_Vouchers_GetMethods (0 when there is none). */
    public int voucherHeadId(UserAccount u, int id, int documentTypeId) {
        List<Map<String, Object>> rows = first(new Sql("Sp_Vouchers_GetMethods").p("Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId")
                .p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()).p("DocumentTypeId", documentTypeId).p("DocumentTypeSrNo", id));
        return rows.isEmpty() ? 0 : i(ci(rows.get(0), "Id"));
    }

    /** CommonServices.GetGlAccountIdBySupplierCustomerId: Sp_SupplierCustomer_GetAllMethod 'GetGlAccountIdBySupplierCustomerId' (first column). */
    public int glAccountIdOfParty(UserAccount u, int supplierCustomerId) {
        List<Map<String, Object>> rows = first(new Sql("Sp_SupplierCustomer_GetAllMethod").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("SupplierCustomerId", supplierCustomerId).p("Activity", "GetGlAccountIdBySupplierCustomerId"));
        if (rows.isEmpty() || rows.get(0).isEmpty()) return 0;
        return i(rows.get(0).values().iterator().next());
    }

    // ------------------------------------------------------------------ 491 SalePriceListWithDiscount
    /** CommonServices.GetItemCategoryFromPricingShedule(6): Sp_ItemPricingSchedule_GetAllMethod @PriceTypeId=6 'GetItemCategoryFromPricingShedule'; RoundingForItemPricing config. */
    public Map<String, Object> priceListLookups(UserAccount u, int yearId) {
        Map<String, Object> d = basics(u, yearId);
        d.put("categories", first(new Sql("Sp_ItemPricingSchedule_GetAllMethod").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("PriceTypeId", 6).p("Activity", "GetItemCategoryFromPricingShedule")));
        d.put("roundingFactor", dbl(config(u, "RoundingForItemPricing")));
        return d;
    }

    /** GetItemPriceList: USP_GetItemSalePriceListWithDiscounts @EffectedDate [, @ItemCategoryId when != 0]. */
    public List<Map<String, Object>> priceList(UserAccount u, LocalDate effected, int itemCategoryId) {
        return first(new Sql("USP_GetItemSalePriceListWithDiscounts").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .date("EffectedDate", effected).nz("ItemCategoryId", itemCategoryId));
    }

    // ------------------------------------------------------------------ 486 frmSaleInvoiceReturnRegister
    /** ComboFill: [dbo].[Usp_AllComboAgainstPurchaseInvoice] @DocumentTypeIds='98' [, @BranchesIds]; rows split by Activity (Supplier / ItemName / StockAccount). */
    public Map<String, Object> returnRegisterCombos(UserAccount u, String branchesIds) {
        Sql s = new Sql("Usp_AllComboAgainstPurchaseInvoice").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("DocumentTypeIds", "98").ne("BranchesIds", branchesIds);
        return new LinkedHashMap<>(byActivity(first(s), "Activity"));
    }

    /** SaleInvoiceRegisterBind: Sp_InvPurchaseInvoice_SaleReturnRegister @DocumentTypeId=98 plus the filters set. */
    public List<Map<String, Object>> saleReturnRegister(UserAccount u, Map<String, String> q) {
        Sql s = new Sql("Sp_InvPurchaseInvoice_SaleReturnRegister").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()).p("DocumentTypeId", 98);
        s.nz("SupplierCustomerId", i(q.get("supplierCustomerId"))).nz("ItemId", i(q.get("itemId"))).nz("ItemStockAccountId", i(q.get("stockAccountId")));
        s.date("DateFrom", day(q.get("fromDate"))).date("DateTo", day(q.get("toDate")));
        s.ne("BranchesIds", q.get("branchesIds"));
        return first(s);
    }

    // ------------------------------------------------------------------ 488 frmPurchaseAndSaleDetailByJobLot
    /** ComboFill: USP_Inventory_StockEvalautionDetail_DropDownAndLists @DocumentTypeIds='56,57,95,99' [, @BranchesIds]; rows split by ActivityType. */
    public Map<String, Object> jobLotCombos(UserAccount u, String branchesIds) {
        Sql s = new Sql("USP_Inventory_StockEvalautionDetail_DropDownAndLists").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .p("DocumentTypeIds", "56,57,95,99").ne("BranchesIds", branchesIds);
        return new LinkedHashMap<>(byActivity(first(s), "ActivityType"));
    }

    /** Show: usp_PurchaseAndSaleDetailByJobLot @FromDate, @ToDate [, @JobLotId when != 0] [, @BranchesIds]. */
    public List<Map<String, Object>> purchaseAndSaleByJobLot(UserAccount u, Map<String, String> q) {
        Sql s = new Sql("usp_PurchaseAndSaleDetailByJobLot").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId())
                .date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate"))).nz("JobLotId", i(q.get("jobLotId"))).ne("BranchesIds", q.get("branchesIds"));
        return first(s);
    }

    // ------------------------------------------------------------------ 489 DeliveryOrderHistory
    /** ComboFill: InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder = USP_GetDataForDropDownFromDeliveryOrder (org, company); rows split by Activity. */
    public Map<String, Object> deliveryOrderCombos(UserAccount u, int yearId) {
        Map<String, Object> d = basics(u, yearId);
        List<Map<String, Object>> rows = first(new Sql("USP_GetDataForDropDownFromDeliveryOrder").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()));
        d.put("combos", byActivity(rows, "Activity"));
        return d;
    }

    /**
     * GridFill: DeliverySchedule.GetDeliveryOrderForApproval = Sp_InvDeliveryOrderForApproval_Rpt. DocumentTypeId is the form's static field
     * (0 unless another form set it, so nothing is sent), BranchesId / UserId are never set by this form. statusId: 1 Not Approved,
     * 2 Approved, 3 All, 0 = empty status text (IsApproved false is sent). referred 1 Reffered / 2 Not Reffered (@DoReferedStatus),
     * weight 1 "1st Wt Only" / 2 "With 2nd Wt" (@WbWeightStatus), deliveryType = the delivery type TEXT (@DeliveryOrderType).
     */
    public Map<String, Object> deliveryOrders(UserAccount u, Map<String, String> q) {
        Sql s = new Sql("Sp_InvDeliveryOrderForApproval_Rpt").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId());
        s.nz("DocumentTypeId", i(q.get("documentTypeId")));
        s.nz("SupplierCustomerId", i(q.get("supplierCustomerId"))).nz("ItemId", i(q.get("itemId"))).nz("ExImInvoiceId", i(q.get("invoiceId")));
        s.nz("WbWeightStatus", i(q.get("weight"))).nz("DoReferedStatus", i(q.get("referred")));
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        int status = i(q.get("statusId"));
        if (status != 3) s.p("IsApproved", status == 2);
        s.ne("DeliveryOrderType", q.get("deliveryType"));
        List<List<Map<String, Object>>> ds = sets(s);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", ds.size() > 0 ? ds.get(0) : new ArrayList<>());
        out.put("company", ds.size() > 1 ? ds.get(1) : new ArrayList<>());
        return out;
    }
}
