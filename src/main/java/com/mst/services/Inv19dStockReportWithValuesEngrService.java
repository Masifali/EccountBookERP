package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;

import java.sql.Types;
import java.util.*;

/**
 * frmStockReportWithValues_Engr (Architecture.WinApp.Mfg.Reports/frmStockReportWithValues_Engr.cs) - ScreenDefinition 805
 * "Stock Report With Values (Engr)". Line refs ":NNN" are that file.
 * Data: BranchesAllocationToUser.GetBranchsAllocatedToUser -> USP_GetBranchsAllocatedToUser;
 * StocksReport.Evalaution_DropDown_ByParentCategories -> USP_Evalaution_DropDown_ByParentCategories (@BranchesIds, @InventoryParentCategory='1,2,3,4');
 * StocksReport.StockReportWithValues_Engr -> [Mfg].[usp_ItemStockReportWithValues];
 * CommonServices.GetConfigurationByOrgCompandConfigDescription -> Sp_ConfigrationsAllocation_GetAllMethod.
 * The desktop never shows BtnUpdate on this form (:190, :668 hide it and nothing re-shows it), so there is no update endpoint.
 */
@Service
public class Inv19dStockReportWithValuesEngrService {

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public Inv19dStockReportWithValuesEngrService(JdbcTemplate jdbc, CurrentUserContext context) { this.jdbc = jdbc; this.context = context; }

    private static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null || String.valueOf(o).isBlank()) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static boolean truthy(Object o) { String s = str(o).toLowerCase(Locale.ROOT); return s.equals("true") || s.equals("1"); }
    private static SqlParameterValue day(String iso) {
        return new SqlParameterValue(Types.TIMESTAMP, java.sql.Timestamp.valueOf(iso.substring(0, 10) + " 00:00:00"));
    }
    private String config(String description) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity='GetConfigurationByOrgCompandConfigDescription'",
                context.currentOrganizationId(), context.currentCompanyId(), description);
        return rows.isEmpty() ? "" : str(rows.get(0).get("ConfigKey"));
    }

    /** frmStockReport_Load :174-242 */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        // ManualRateAndAmountForItemStockSummary :192 (read by the desktop, never used on this form)
        out.put("manualRateConfig", truthy(config("ManualRateAndAmountForItemStockSummary")));
        // clsGlobalVariables.ActiveYr.Start_Period :227
        var fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", context.currentFinancialYearId());
        out.put("fyStart", fy.isEmpty() || fy.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(fy.get(0).get("Start_Period")).substring(0, 10));
        out.put("branches", branches());
        int branchId = context.currentBranchId();
        out.put("userBranchId", branchId);
        // BranchesFill :282 sets Text = UserAccount.BranchName, so the first ParentCategoryComboFill :291 runs for that branch
        out.put("combos", combos(String.valueOf(branchId)));
        return out;
    }

    /** BranchesFill :263 - USP_GetBranchsAllocatedToUser (BranchId, BranchName). */
    public List<Map<String, Object>> branches() {
        return jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUser @OrganizationId=?,@CompanyId=?,@UserId=?",
                context.currentOrganizationId(), context.currentCompanyId(), context.currentUserId());
    }

    /** ParentCategoryComboFill :291-317 - StocksReport.Evalaution_DropDown_ByParentCategories with BranchesIds (",id,id" in the desktop)
     *  and Ids = "1,2,3,4" (ActivityType, Id, name, InventoryParentCategoriesId). An empty branch text clears the parent combo (:527). */
    public List<Map<String, Object>> combos(String branchesIds) {
        String b = str(branchesIds);
        if (b.isEmpty()) return new ArrayList<>();
        return jdbc.queryForList("EXEC dbo.USP_Evalaution_DropDown_ByParentCategories @OrganizationId=?,@CompanyId=?,@BranchesIds=?,@InventoryParentCategory=?",
                context.currentOrganizationId(), context.currentCompanyId(), "," + b, "1,2,3,4");
    }

    /** GridBind :662-771 -> StocksReport.StockReportWithValues_Engr -> [Mfg].[usp_ItemStockReportWithValues]; 0 / empty values are left at the procedure defaults. */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        if (str(p.get("branchesIds")).isEmpty()) throw new IllegalArgumentException("Select Branch First");      // :765
        if (str(p.get("ids")).isEmpty()) throw new IllegalArgumentException("Select Parent Category First");       // :762
        StringBuilder sql = new StringBuilder("EXEC [Mfg].[usp_ItemStockReportWithValues] @OrganizationId=?,@CompanyId=?,@DateFrom=?,@DateTo=?");
        List<Object> a = new ArrayList<>(List.of(context.currentOrganizationId(), context.currentCompanyId()));
        String from = str(p.get("fromDate")), to = str(p.get("toDate"));
        if (from.isEmpty() || to.isEmpty()) throw new IllegalArgumentException("Date From and Date To are required.");
        a.add(day(from)); a.add(day(to));
        if (num(p.get("itemTypeId")) != 0) { sql.append(",@ItemTypeId=?"); a.add(num(p.get("itemTypeId"))); }
        if (num(p.get("itemCategoryId")) != 0) { sql.append(",@ItemCategoryId=?"); a.add(num(p.get("itemCategoryId"))); }
        if (num(p.get("warehouseId")) != 0) { sql.append(",@WarehouseId=?"); a.add(num(p.get("warehouseId"))); }
        if (num(p.get("itemId")) != 0) { sql.append(",@ItemId=?"); a.add(num(p.get("itemId"))); }
        if (num(p.get("varientId")) != 0) { sql.append(",@VarientId=?"); a.add(num(p.get("varientId"))); }                 // CmbModalDescription :710
        if (num(p.get("productionStageId")) != 0) { sql.append(",@ProductionStageId=?"); a.add(num(p.get("productionStageId"))); }
        if (num(p.get("workOrderId")) != 0) { sql.append(",@WorkOrderId=?"); a.add(num(p.get("workOrderId"))); }
        if (num(p.get("productionStageWise")) != 0) { sql.append(",@ProductionStageWise=?"); a.add(1); }
        if (num(p.get("modalDescriptionWise")) != 0) { sql.append(",@ModalDescriptionWise=?"); a.add(1); }
        if (num(p.get("warehouseWise")) != 0) { sql.append(",@Warehousewise=?"); a.add(1); }
        if (num(p.get("batchWise")) != 0) { sql.append(",@BatchWise=?"); a.add(1); }
        if (num(p.get("workOrderWise")) != 0) { sql.append(",@WorkOrderWise=?"); a.add(1); }
        if (num(p.get("castingTypeWise")) != 0) { sql.append(",@CastingTypeWise=?"); a.add(1); }
        if (num(p.get("itemStockAc")) != 0) { sql.append(",@ItemStockAc=?"); a.add(num(p.get("itemStockAc"))); }         // PurchaseGLAC :705
        sql.append(",@InventoryParentCategoriesIds=?"); a.add(str(p.get("ids")));
        sql.append(",@SaleValue=?"); a.add(num(p.get("saleValue")));                                                     // ActionId :716
        if (num(p.get("zeroBalanceType")) != 0) { sql.append(",@SkipZero=?"); a.add(1); }
        if (num(p.get("castingTypeId")) != 0) { sql.append(",@CastingTypeId=?"); a.add(num(p.get("castingTypeId"))); }
        sql.append(",@Activity=?"); a.add(str(p.get("activity")).isEmpty() ? "ItemStockSummary" : str(p.get("activity")));
        sql.append(",@BranchesIds=?"); a.add(str(p.get("branchesIds")));
        if (!str(p.get("batchNo")).isEmpty()) { sql.append(",@BatchNo=?"); a.add(str(p.get("batchNo"))); }            // CmbBatchNo.Text :709
        return jdbc.queryForList(sql.toString(), a.toArray());
    }
}
