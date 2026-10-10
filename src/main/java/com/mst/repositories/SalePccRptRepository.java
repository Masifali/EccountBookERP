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
 * Group P (Sale Pcc, module 85 / Sales Pcc Reports, module 88): the three report screens.
 * Every guarded parameter (the desktop's "if (obj.X != 0)") is OMITTED when unset, never sent as NULL. A procedure's result is its FIRST
 * result set (SqlDataAdapter.Fill(DataTable)).
 *   451 StockTranfserRegister            [pcc].[USP_GetDataForDropDownFromStockTransfer]   InvStockTransferHeader.AllComboAgainstStockTransfer1
 *                                        [pcc].[USP_InvStockTransferSlipRegister]           InvStockTransferHeader.StockTransferRegister
 *   561 Sales_EvaulationDetailReports    USP_GetDataFromInventoryStocksEvaluationsForSales  InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales
 *                                        [pcc].[USP_Sales_EvaulationDetailReports]          InvSaleInvoice.EvaulationDetailSalesReports
 *   562 SalesWages_Register              [pcc].[USP_DropDownFillFromInvSaleInvoice]         InvSaleInvoice.DropDownFillFromInvSaleInvoice
 *                                        [pcc].[USP_GetDataForDropDownFromWagesSchedule]    ContractorWagesActivity.GetDataForDropDownFromWagesSchedule
 *                                        [pcc].[usp_SalesWages_Register] / [pcc].[usp_SalesWages_Register_Summary]   SaleOrder.salewagesregister / SaleWagesRegisterSummary
 * Tenancy (Organization / Company / User / Branch / Financial Year) always comes from the caller (CurrentUserContext).
 */
@Repository
public class SalePccRptRepository {
    private final JdbcTemplate jdbc;

    public SalePccRptRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

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
                else if (v instanceof byte[]) v = null;
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

    public static String str(Object v) { return v == null ? "" : v.toString(); }

    public static LocalDate day(Object v) {
        if (v == null) return null;
        String s = v.toString().trim();
        if (s.length() < 10) return null;
        try { return LocalDate.parse(s.substring(0, 10)); } catch (Exception e) { return null; }
    }

    /** Rows of a lookup procedure grouped by their Activity column, order kept. */
    private static Map<String, List<Map<String, Object>>> byActivity(List<Map<String, Object>> rows) {
        Map<String, List<Map<String, Object>>> out = new LinkedHashMap<>();
        for (Map<String, Object> r : rows) out.computeIfAbsent(str(ci(r, "Activity")), k -> new ArrayList<>()).add(r);
        return out;
    }

    // ------------------------------------------------------------------ shared
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

    public Map<String, Object> basics(UserAccount u, int yearId) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("amountDecimals", amountDecimals(u));
        d.put("rateDecimals", rateDecimals(u));
        d.put("yearStart", yearStart(u, yearId));
        return d;
    }

    // ------------------------------------------------------------------ 451 StockTranfserRegister
    /**
     * ComboBind: InvStockTransferHeader.AllComboAgainstStockTransfer1 = [pcc].[USP_GetDataForDropDownFromStockTransfer] (OrganizationId, CompanyId;
     * @Activity is not set so it is not sent), split by the Activity column: Item, ItemType, ItemCategory, ParentCategory, VehicleNo (Id / ReferenceName).
     */
    public Map<String, Object> stockTransferLookups(UserAccount u, int yearId) {
        Map<String, Object> d = basics(u, yearId);
        d.put("combos", byActivity(first(new Sql("pcc.USP_GetDataForDropDownFromStockTransfer")
                .p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()))));
        return d;
    }

    /**
     * GridFill: InvStockTransferHeader.StockTransferRegister = [pcc].[USP_InvStockTransferSlipRegister]: OrganizationId, CompanyId, DocumentTypeId 1860,
     * then only when set: FromDocNo, ToDocNo, ItemId, FromDate, ToDate, VehicleNo, InventoryParentCategoriesId (never set by the form), ItemCategoryId,
     * ItemTypeId; ApprovedFilter is "All" so @IsApproved is never sent. The desktop's VehicleNo = Conversion.ToString(cmbvehicleno.Value), i.e. the combo's Id.
     */
    public List<Map<String, Object>> stockTransferRegister(UserAccount u, Map<String, String> q) {
        Sql s = new Sql("pcc.USP_InvStockTransferSlipRegister").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId()).p("DocumentTypeId", 1860);
        s.nz("FromDocNo", i(q.get("fromDocNo"))).nz("ToDocNo", i(q.get("toDocNo"))).nz("ItemId", i(q.get("itemId")));
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        s.ne("VehicleNo", q.get("vehicleNo"));
        s.nz("ItemCategoryId", i(q.get("itemCategoryId"))).nz("ItemTypeId", i(q.get("itemTypeId")));
        return first(s);
    }

    // ------------------------------------------------------------------ 561 Sales_EvaulationDetailReports
    /**
     * AllComboBind: InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales = USP_GetDataFromInventoryStocksEvaluationsForSales
     * (OrganizationId, CompanyId, UserId, AppId, DocType "Sale"; Activity is null so it is not sent), split by Activity: GetParentCategory, GetItemType,
     * GetItemClass, GetItemCategory, GetSupplierCustomer, GetItems, GetCity, GetJobLot, GetWarehouse, GetVehicleNos, GetDistrict (text RefName).
     */
    public Map<String, Object> evaluationLookups(UserAccount u, int yearId, int appId) {
        Map<String, Object> d = basics(u, yearId);
        d.put("combos", byActivity(first(new Sql("USP_GetDataFromInventoryStocksEvaluationsForSales").p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId()).p("UserId", u.getId()).p("AppId", appId).p("DocType", "Sale"))));
        return d;
    }

    /**
     * GridFill: InvSaleInvoice.EvaulationDetailSalesReports = [pcc].[USP_Sales_EvaulationDetailReports]: OrganizationId, CompanyId, then only when set
     * FromDate, ToDate, DocNoFrom, DocNoTo, InventoryParentCategories (the form fills obj.ParentCategoryId, which the BLL never reads: not sent), ItemTypeId,
     * ItemClassGroupId, WarehouseId, ItemId, ItemCategoryId, SupplierCustomerId, JobLotId, CityId, DistrictId, ActionId (1 Sales, 2 Returns), VehicleNo
     * (the combo text, only when its value &gt; 0) and @ActivityName (the activity text, when not empty).
     */
    public List<Map<String, Object>> evaluation(UserAccount u, Map<String, String> q) {
        Sql s = new Sql("pcc.USP_Sales_EvaulationDetailReports").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId());
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        s.nz("DocNoFrom", i(q.get("fromDocNo"))).nz("DocNoTo", i(q.get("toDocNo")));
        s.nz("ItemTypeId", i(q.get("itemTypeId"))).nz("ItemClassGroupId", i(q.get("itemClassId"))).nz("WarehouseId", i(q.get("warehouseId")));
        s.nz("ItemId", i(q.get("itemId"))).nz("ItemCategoryId", i(q.get("itemCategoryId"))).nz("SupplierCustomerId", i(q.get("supplierCustomerId")));
        s.nz("JobLotId", i(q.get("jobLotId"))).nz("CityId", i(q.get("cityId"))).nz("DistrictId", i(q.get("districtId")));
        s.nz("ActionId", i(q.get("actionId")));
        if (i(q.get("vehicleId")) > 0) s.ne("VehicleNo", q.get("vehicleNo"));
        s.ne("ActivityName", q.get("activity"));
        return first(s);
    }

    // ------------------------------------------------------------------ 562 SalesWages_Register
    /**
     * ComboFill: InvSaleInvoice.DropDownFillFromInvSaleInvoice = [pcc].[USP_DropDownFillFromInvSaleInvoice] (OrganizationId, CompanyId, @IsFromWages true
     * because RefDocumentTypeId = 1; Activity and DocumentTypeIds are not sent), split by Activity: Customer, Contractor (Id / ReferenceName).
     * ServiceActivityAndItem: ContractorWagesActivity.GetDataForDropDownFromWagesSchedule = [pcc].[USP_GetDataForDropDownFromWagesSchedule]
     * (OrganizationId, CompanyId, @IsFromWages true), split by Activity: GroupName (Item Name), Service Activity.
     */
    public Map<String, Object> wagesLookups(UserAccount u, int yearId) {
        Map<String, Object> d = basics(u, yearId);
        d.put("invoiceCombos", byActivity(first(new Sql("pcc.USP_DropDownFillFromInvSaleInvoice").p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId()).p("IsFromWages", true))));
        d.put("wagesCombos", byActivity(first(new Sql("pcc.USP_GetDataForDropDownFromWagesSchedule").p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId()).p("IsFromWages", true))));
        return d;
    }

    /**
     * btnshow_Click: Activity 1 (Detail) SaleOrder.salewagesregister = [pcc].[usp_SalesWages_Register], 2 (Summary) SaleOrder.SaleWagesRegisterSummary =
     * [pcc].[usp_SalesWages_Register_Summary]: OrganizationId, CompanyId, then only when set FromDate, ToDate, FromDocNo, ToDocNo, ItemId,
     * ContractorWagesActivityId, ContractorId, SupplierCustomerId.
     */
    public List<Map<String, Object>> wagesRegister(UserAccount u, boolean summary, Map<String, String> q) {
        Sql s = new Sql(summary ? "pcc.usp_SalesWages_Register_Summary" : "pcc.usp_SalesWages_Register").p("OrganizationId", u.getOrganizationId()).p("CompanyId", u.getCompanyId());
        s.date("FromDate", day(q.get("fromDate"))).date("ToDate", day(q.get("toDate")));
        s.nz("FromDocNo", i(q.get("fromDocNo"))).nz("ToDocNo", i(q.get("toDocNo"))).nz("ItemId", i(q.get("itemId")));
        s.nz("ContractorWagesActivityId", i(q.get("serviceActivityId"))).nz("ContractorId", i(q.get("contractorId"))).nz("SupplierCustomerId", i(q.get("supplierCustomerId")));
        return first(s);
    }
}
