package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Types;
import java.util.*;

/**
 * frmStockReportWithValues (Architecture.WinApp.Inventory_Stocks_Report/frmStockReportWithValues.cs) - ScreenDefinition 299
 * "Stock Report With Values". Line refs ":NNN" are that file.
 * Data: BLL StocksReport (0125) - Evalaution_DropDown_ByParentCategories -> USP_Evalaution_DropDown_ByParentCategories,
 * GetInventoryStockAsOnDate -> usp_getInventoryStockAsOnDate, stockReportWithValues -> Sp_ItemStockReportWithValues_Rpt;
 * ChartofAccount.ReadAllAccountgroup -> Sp_ChartofAccount_GetAllMethodFromCOA (@CoaType='ReadAllAccountGroup');
 * BranchesAllocationToUser.GetBranchsAllocatedToUser -> USP_GetBranchsAllocatedToUser;
 * StockSummaryByItem.Save -> USP_StockSummaryByItem_InsertAndUpdate (one call per row, one transaction).
 */
@Service
public class InventoryStockReportWithValuesService {

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public InventoryStockReportWithValuesService(JdbcTemplate jdbc, CurrentUserContext context) { this.jdbc = jdbc; this.context = context; }

    private static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null || String.valueOf(o).isBlank()) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
    private static double dbl(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null || String.valueOf(o).isBlank()) return 0d;
        try { return Double.parseDouble(String.valueOf(o).trim().replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }
    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static boolean truthy(Object o) { String s = str(o).toLowerCase(Locale.ROOT); return s.equals("true") || s.equals("1"); }
    private String config(String description) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity='GetConfigurationByOrgCompandConfigDescription'",
                context.currentOrganizationId(), context.currentCompanyId(), description);
        return rows.isEmpty() ? "" : str(rows.get(0).get("ConfigKey"));
    }
    private static SqlParameterValue day(String iso) {
        return new SqlParameterValue(Types.TIMESTAMP, java.sql.Timestamp.valueOf(iso.substring(0, 10) + " 00:00:00"));
    }

    /** frmStockReport_Load :197-349 */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        // ManualRateAndAmountForItemStockSummary :212
        out.put("manualRateConfig", truthy(config("ManualRateAndAmountForItemStockSummary")));
        // clsGlobalVariables.ActiveYr.Start_Period :248
        var fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", context.currentFinancialYearId());
        out.put("fyStart", fy.isEmpty() || fy.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(fy.get(0).get("Start_Period")).substring(0, 10));
        out.put("asOnDate", asOnDate());
        out.put("branches", branches());
        out.put("userBranchId", context.currentBranchId());
        out.put("combos", combos());
        out.put("groupAccounts", groupAccounts());
        return out;
    }

    /** StocksReport.GetInventoryStockAsOnDate: AsOnDate + 1 day, or none (1900-01-01). Also BtnReset_Click :1356. */
    public String asOnDate() {
        var d = jdbc.queryForList("EXEC dbo.usp_getInventoryStockAsOnDate @OrganizationId=?,@CompanyId=?", context.currentOrganizationId(), context.currentCompanyId());
        if (!d.isEmpty() && d.get(0).get("AsOnDate") != null) {
            try { return java.time.LocalDate.parse(String.valueOf(d.get(0).get("AsOnDate")).substring(0, 10)).plusDays(1).toString(); } catch (RuntimeException e) { return ""; }
        }
        return "";
    }

    /** BranchesFill :351 - USP_GetBranchsAllocatedToUser (BranchId, BranchName). */
    public List<Map<String, Object>> branches() {
        return jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUser @OrganizationId=?,@CompanyId=?,@UserId=?", context.currentOrganizationId(), context.currentCompanyId(), context.currentUserId());
    }

    /** ParentCategoryComboFill :610 - StocksReport.Evalaution_DropDown_ByParentCategories, Ids = "1,2,3,4,6"
     *  (ActivityType, Id, name, InventoryParentCategoriesId). */
    public List<Map<String, Object>> combos() {
        return jdbc.queryForList("EXEC dbo.USP_Evalaution_DropDown_ByParentCategories @OrganizationId=?,@CompanyId=?,@InventoryParentCategory=?",
                context.currentOrganizationId(), context.currentCompanyId(), "1,2,3,4,6");
    }

    /** AccountFill3rdLevel :668 - ChartofAccount.ReadAllAccountgroup (FinancialYearId, Account_Level 3, AccountTypeId 4, AccountClassId 2). */
    public List<Map<String, Object>> groupAccounts() {
        return jdbc.queryForList("EXEC dbo.Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?,@CompanyId=?,@FinancialYearId=?,@Account_Level=?,@AccountTypeId=?,@AccountClassId=?,@CoaType=?",
                context.currentOrganizationId(), context.currentCompanyId(), context.currentFinancialYearId(), 3, 4, 2, "ReadAllAccountGroup");
    }

    /** GridBind :800 -> StocksReport.stockReportWithValues (BLL 0125), every parameter guarded as the BLL guards it. */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        if (str(p.get("branchesIds")).isEmpty()) throw new IllegalArgumentException("Select Branch First");
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_ItemStockReportWithValues_Rpt @OrganizationId=?,@CompanyId=?");
        List<Object> a = new ArrayList<>(List.of(context.currentOrganizationId(), context.currentCompanyId()));
        if (!str(p.get("fromDate")).isEmpty()) { sql.append(",@DateFrom=?"); a.add(day(str(p.get("fromDate")))); }
        if (!str(p.get("toDate")).isEmpty()) { sql.append(",@DateTo=?"); a.add(day(str(p.get("toDate")))); }
        if (num(p.get("itemTypeId")) != 0) { sql.append(",@ItemTypeId=?"); a.add(num(p.get("itemTypeId"))); }
        if (num(p.get("itemCategoryId")) != 0) { sql.append(",@ItemCategoryId=?"); a.add(num(p.get("itemCategoryId"))); }
        if (!str(p.get("cropYear")).isEmpty()) { sql.append(",@CropYear=?"); a.add(str(p.get("cropYear"))); }
        if (num(p.get("jobLotId")) != 0) { sql.append(",@JobLotId=?"); a.add(num(p.get("jobLotId"))); }
        if (num(p.get("warehouseId")) != 0) { sql.append(",@WarehouseId=?"); a.add(num(p.get("warehouseId"))); }
        if (num(p.get("itemId")) != 0) { sql.append(",@ItemId=?"); a.add(num(p.get("itemId"))); }
        if (num(p.get("stockUOM")) != 0) { sql.append(",@IsPackSizeOn=?"); a.add(num(p.get("stockUOM"))); }
        if (num(p.get("itemStockAc")) != 0) { sql.append(",@ItemStockAc=?"); a.add(num(p.get("itemStockAc"))); }
        if (num(p.get("groupAccountId")) != 0) { sql.append(",@AccountGroupId=?"); a.add(num(p.get("groupAccountId"))); }
        if (num(p.get("packingTypeId")) != 0) { sql.append(",@PackingTypeId=?"); a.add(num(p.get("packingTypeId"))); }
        if (num(p.get("rateUOM")) != 0) { sql.append(",@IsPackTypeOn=?"); a.add(num(p.get("rateUOM"))); }
        if (num(p.get("itemClassGroupId")) != 0) { sql.append(",@ItemClassGroupId=?"); a.add(num(p.get("itemClassGroupId"))); }
        if (!str(p.get("ids")).isEmpty()) { sql.append(",@InventoryParentCategoriesIds=?"); a.add(str(p.get("ids"))); }
        if (num(p.get("saleValue")) != 0) { sql.append(",@SaleValue=?"); a.add(num(p.get("saleValue"))); }
        if (num(p.get("zeroBalanceType")) != 0) { sql.append(",@SkipZero=?"); a.add(num(p.get("zeroBalanceType"))); }
        if (num(p.get("allowWipItem")) != 0) { sql.append(",@AllowWipItem=?"); a.add(num(p.get("allowWipItem"))); }
        sql.append(",@BranchesIds=?"); a.add(str(p.get("branchesIds")));
        sql.append(",@Activity=?"); a.add(str(p.get("activity")));
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    /** InsertInCaseOfItemSummary :720 - rows with M_Amount > 0, (int)M_Rate > 0 and (int)M_Rate != (int)Saved_M_Rate,
     *  each through USP_StockSummaryByItem_InsertAndUpdate inside one transaction (DAL StockSummaryByItem.SetData). */
    @Transactional
    @SuppressWarnings("unchecked")
    public void update(Map<String, Object> body) {
        if (!truthy(config("ManualRateAndAmountForItemStockSummary"))) throw new IllegalArgumentException("No Record Found For Update...");
        String docDate = str(body.get("toDate"));
        if (docDate.isEmpty()) throw new IllegalArgumentException("Date To is required.");
        Object rowsObj = body.get("rows");
        List<Map<String, Object>> rows = rowsObj instanceof List ? (List<Map<String, Object>>) rowsObj : new ArrayList<>();
        List<Map<String, Object>> list = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (dbl(r.get("M_Amount")) > 0.0 && num(r.get("M_Rate")) > 0 && num(r.get("M_Rate")) != num(r.get("Saved_M_Rate"))) list.add(r);
        }
        if (list.isEmpty()) throw new IllegalArgumentException("No Record Found For Update...");
        final int org = context.currentOrganizationId(), comp = context.currentCompanyId(), fy = context.currentFinancialYearId(), user = context.currentUserId();
        final String sql = "EXEC dbo.USP_StockSummaryByItem_InsertAndUpdate @ItemId=?,@ItemUOMId=?,@PackingTypeId=?,@ItemQty=?,@ItemRate=?,@NetWeight=?,@RateUOM=?,@Amount=?,"
                + "@OrganizationId=?,@CompanyId=?,@FinancialYearId=?,@EntryDate=?,@ModifyDate=?,@DocDate=?,@EntryUserId=?,@ModifyUserId=?";
        for (Map<String, Object> r : list) {
            final Object[] args = new Object[] {
                    num(r.get("ItemId")), num(r.get("PackUomId")), num(r.get("PackingTypeId")),
                    dbl(r.get("BalQty")), dbl(r.get("M_Rate")), dbl(r.get("BalWeight")), 40.0d, dbl(r.get("M_Amount")),
                    org, comp, fy,
                    new java.sql.Timestamp(System.currentTimeMillis()), new java.sql.Timestamp(System.currentTimeMillis()),
                    java.sql.Timestamp.valueOf(docDate.substring(0, 10) + " 00:00:00"), user, user };
            jdbc.execute(sql, (PreparedStatementCallback<Object>) ps -> {
                for (int i = 0; i < args.length; i++) ps.setObject(i + 1, args[i]);
                ps.execute();
                return null;
            });
        }
    }
}
