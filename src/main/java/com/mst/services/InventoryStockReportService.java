package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;

import java.sql.Types;
import java.util.*;

/**
 * frmStockReport (Architecture.WinApp.Inventory_Stocks_Report/frmStockReport.cs) - "Stock Report", the dialog the
 * Sale Order / GDN / GRN screens open from their "Stock Report" button (btnStockReport_Click: rights row
 * ScreenName = "frmStockReport", else "You don't have right").
 * Data: BLL StocksReport (0125) - Sp_Inventory_InventoryTransactions_DropDownAndLists, usp_getInventoryStockAsOnDate,
 * usp_getPackUomByItemIdFromInventoryTransactions, Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt;
 * BranchesAllocationToUser.GetBranchsAllocatedToUser -> USP_GetBranchsAllocatedToUser.
 */
@Service
public class InventoryStockReportService {

    public static final String SCREEN = "frmStockReport";
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public InventoryStockReportService(JdbcTemplate jdbc, CurrentUserContext context) { this.jdbc = jdbc; this.context = context; }

    private static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null || String.valueOf(o).isBlank()) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static boolean truthy(Object o) { String s = str(o).toLowerCase(Locale.ROOT); return s.equals("true") || s.equals("1"); }
    private String config(String description) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity='GetConfigurationByOrgCompandConfigDescription'",
                context.currentOrganizationId(), context.currentCompanyId(), description);
        return rows.isEmpty() ? "" : str(rows.get(0).get("ConfigKey"));
    }

    /** clsGlobalVariables.ScreenViewReights row for frmStockReport (the caller's right check). */
    public boolean canView() {
        String role = Objects.toString(context.currentRoleName(), "");
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        return jdbc.queryForList("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?,@ScreenName=?,@RightName=?,@CompanyId=?,@Activity='GetByUserId'",
                        context.currentUserId(), SCREEN, role, context.currentCompanyId()).stream()
                .anyMatch(r -> "View".equalsIgnoreCase(str(r.get("RightName"))) && (Boolean.TRUE.equals(r.get("Value")) || "1".equals(str(r.get("Value")))));
    }

    /** frmStockReport_Load :203-440 */
    public Map<String, Object> init() {
        int org = context.currentOrganizationId(), comp = context.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        boolean fifo = jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?", org, comp).stream().anyMatch(r -> num(r.get("Id")) == 5);
        out.put("fifoFeature", fifo);
        Map<String, Object> cfg = new LinkedHashMap<>();
        for (String k : List.of("CheckStockItemWareHouse", "CheckStockItemWareHouseCropYear", "CheckStockItemWareHouseCropYearJobLot", "CheckStockItemWareHouseCropYearJobLotPackUom", "CheckStockItemWareHouseCropYearJobLotPackingTypeUom"))
            cfg.put(k, truthy(config(k)));
        out.put("config", cfg);
        var fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", context.currentFinancialYearId());
        out.put("fyStart", fy.isEmpty() || fy.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(fy.get(0).get("Start_Period")).substring(0, 10));
        // StocksReport.GetInventoryStockAsOnDate: AsOnDate + 1 day, or 1900-01-01 (null)
        String asOn = "";
        var d = jdbc.queryForList("EXEC dbo.usp_getInventoryStockAsOnDate @OrganizationId=?,@CompanyId=?", org, comp);
        if (!d.isEmpty() && d.get(0).get("AsOnDate") != null) {
            try { asOn = java.time.LocalDate.parse(String.valueOf(d.get(0).get("AsOnDate")).substring(0, 10)).plusDays(1).toString(); } catch (RuntimeException e) { asOn = ""; }
        }
        out.put("asOnDate", asOn);
        out.put("branches", branches());
        out.put("userBranchId", context.currentBranchId());
        out.put("combos", combos());
        out.put("reportTypes", List.of(
                rt("ItemStockSummary", "Item Stock Summary"), rt("ItemandWarehouseStockSummary", "Warehouse and Item Stock Summary"),
                rt("ItemandWarehouseStockSummary", "Item and WareHouse Stock Summary"), rt("ItemandCropYearStockSummary", "Crop Year and Item Stock Summary"),
                rt("ItemandCropYearandWarehouseStockSummary", "Crop Year, Warehouse and Item Stock Summary"), rt("JobLotandItemStockSummary", "JobLot and Item"),
                rt("WarehouseandJoblotandItemStockSummary", "Warehouse, Joblot and Item Stock Summary"), rt("ItemandPackSizeStockSummary", "Item and PackSize Stock Summary"),
                rt("ItemandPackingTypeStockSummary", "Item and PackingType Stock Summary"), rt("ItemandPackSizeandPackingTypeStockSummary", "Item and PackSize and PackingType Stock Summary"),
                rt("WIPStockPlantWiseSummary", "WIP Stock Plant Wise Summary")));
        return out;
    }
    private static Map<String, Object> rt(String id, String name) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", id); m.put("name", name); return m; }

    /** BranchesFill :420 - USP_GetBranchsAllocatedToUser (BranchId, BranchName). */
    public List<Map<String, Object>> branches() {
        return jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUser @OrganizationId=?,@CompanyId=?,@UserId=?", context.currentOrganizationId(), context.currentCompanyId(), context.currentUserId());
    }
    /** ParentCategoryFill :447 - Sp_Inventory_InventoryTransactions_DropDownAndLists @InventoryParentCategory='1,2,3,4,6' (ActivityType, Id, name, ParentCategoryId). */
    public List<Map<String, Object>> combos() {
        return jdbc.queryForList("EXEC dbo.Sp_Inventory_InventoryTransactions_DropDownAndLists @OrganizationId=?,@CompanyId=?,@InventoryParentCategory=?", context.currentOrganizationId(), context.currentCompanyId(), "1,2,3,4,6");
    }
    /** cmbItem_Leave :693 - usp_getPackUomByItemIdFromInventoryTransactions (Id, PackUom). */
    public List<Map<String, Object>> uoms(int itemId) {
        return jdbc.queryForList("EXEC dbo.usp_getPackUomByItemIdFromInventoryTransactions @OrganizationId=?,@CompanyId=?,@ItemId=?", context.currentOrganizationId(), context.currentCompanyId(), itemId);
    }

    /** GridBind :720 -> StocksReport.stockGeneralSummaryByWeight (BLL 0125:340-401), every parameter guarded as the BLL guards it. */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        if (str(p.get("branchesIds")).isEmpty()) throw new IllegalArgumentException("Select branch first");
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt @OrganizationId=?,@CompanyId=?");
        List<Object> a = new ArrayList<>(List.of(context.currentOrganizationId(), context.currentCompanyId()));
        if (!str(p.get("fromDate")).isEmpty()) { sql.append(",@DateFrom=?"); a.add(new SqlParameterValue(Types.TIMESTAMP, java.sql.Timestamp.valueOf(str(p.get("fromDate")).substring(0, 10) + " 00:00:00"))); }
        if (!str(p.get("toDate")).isEmpty()) { sql.append(",@DateTo=?"); a.add(new SqlParameterValue(Types.TIMESTAMP, java.sql.Timestamp.valueOf(str(p.get("toDate")).substring(0, 10) + " 00:00:00"))); }
        if (!str(p.get("ids")).isEmpty()) { sql.append(",@ParentCategoryIds=?"); a.add(str(p.get("ids"))); }
        if (num(p.get("itemTypeId")) != 0) { sql.append(",@ItemTypeId=?"); a.add(num(p.get("itemTypeId"))); }
        if (num(p.get("itemClassGroupId")) != 0) { sql.append(",@ClassGroupId=?"); a.add(num(p.get("itemClassGroupId"))); }
        if (num(p.get("itemCategoryId")) != 0) { sql.append(",@ItemCategoryId=?"); a.add(num(p.get("itemCategoryId"))); }
        if (!str(p.get("cropYear")).isEmpty()) { sql.append(",@CropYear=?"); a.add(str(p.get("cropYear"))); }
        if (num(p.get("jobLotId")) != 0) { sql.append(",@JobLotId=?"); a.add(num(p.get("jobLotId"))); }
        if (num(p.get("warehouseId")) != 0) { sql.append(",@WarehouseId=?"); a.add(num(p.get("warehouseId"))); }
        if (num(p.get("itemId")) != 0) { sql.append(",@ItemId=?"); a.add(num(p.get("itemId"))); }
        if (num(p.get("itemUomId")) != 0) { sql.append(",@ItemUomId=?"); a.add(num(p.get("itemUomId"))); }
        if (num(p.get("stockUOM")) != 0) { sql.append(",@IsPackSizeOn=?"); a.add(num(p.get("stockUOM"))); }
        if (num(p.get("rateUOM")) != 0) { sql.append(",@IsPackTypeOn=?"); a.add(num(p.get("rateUOM"))); }
        if (num(p.get("packingTypeId")) != 0) { sql.append(",@PackingTypeId=?"); a.add(num(p.get("packingTypeId"))); }
        if (num(p.get("zeroBalanceType")) != 0) { sql.append(",@SkipZero=?"); a.add(num(p.get("zeroBalanceType"))); }
        if (num(p.get("allowWipItem")) != 0) { sql.append(",@AllowWipItem=?"); a.add(num(p.get("allowWipItem"))); }
        sql.append(",@Activity=?"); a.add(str(p.get("activity")));
        sql.append(",@BranchesIds=?"); a.add(str(p.get("branchesIds")));
        return jdbc.queryForList(sql.toString(), a.toArray());
    }
}
