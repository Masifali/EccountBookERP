package com.mst.services;

import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;

import java.sql.Types;
import java.util.*;

/**
 * frmInventoryProfitLoss (Architecture.WinApp.Inventory_Stocks_Report/frmInventoryProfitLoss.cs) - ScreenDefinition 289
 * "Inventory Stock Evaluation Profit & Loss (Not Use)" (Inventory hub, module 19 Stock Reports). Line refs ":NNN" are that file.
 * Data: StocksReport.StockReportComboByOrganizationAndCompanyId -> USP_Inventory_StockEvalautionDetail_DropDownAndLists
 * (ActivityType, Id, name: ParentCategories, ItemTypes, ItemCategories, ItemClassGroup, Items, JobLot, CropYear, Warehouse - StockComboFill :137);
 * ItemCategory.GetItemClassGroup -> Sp_ItemCategory_GetAllMethod @Activity='GetItemClassGroup' (Id, ClassGroupName - ClassGroupNameFill :233);
 * CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4") -> USP_Accounts_GetAccountTitleByAccountTypeIds (Id, AccountTitle - ItemStockAcFill :250);
 * StocksReport.InventoryProfitLoss -> Sp_InventoryProfitLossRpt (GridBind :289).
 * The BLL/DAL sources are not in the recovered tree: parameter guards follow the BLL convention of this code base (StocksReport.stockReportWithValues
 * in InventoryStockReportWithValuesService) - identity always, dates and Activity always, every other value only when it is not 0 / not empty.
 */
@Service
public class Inv19cInventoryProfitLossService {

    public static final int SCREEN_ID = 289;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public Inv19cInventoryProfitLossService(JdbcTemplate jdbc, CurrentUserContext context, DesktopReportRights rights) {
        this.jdbc = jdbc; this.context = context; this.rights = rights;
    }

    private static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null || String.valueOf(o).isBlank()) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static SqlParameterValue day(String iso) {
        return new SqlParameterValue(Types.TIMESTAMP, java.sql.Timestamp.valueOf(iso.substring(0, 10) + " 00:00:00"));
    }

    /** frmStockReport_Load :115 - Start_Period (:120), StockComboFill, ItemStockAcFill, ClassGroupNameFill. */
    public Map<String, Object> init() {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        var fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", context.currentFinancialYearId());
        out.put("fyStart", fy.isEmpty() || fy.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(fy.get(0).get("Start_Period")).substring(0, 10));
        out.put("combos", combos());
        out.put("stockAccounts", stockAccounts());
        out.put("classGroups", classGroups());
        return out;
    }

    /** StockComboFill :137 - StocksReport.StockReportComboByOrganizationAndCompanyId (OrganizationId, CompanyId only). */
    public List<Map<String, Object>> combos() {
        return jdbc.queryForList("EXEC dbo.USP_Inventory_StockEvalautionDetail_DropDownAndLists @OrganizationId=?,@CompanyId=?",
                context.currentOrganizationId(), context.currentCompanyId());
    }

    /** ItemStockAcFill :250 - CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4"): stock accounts (AccountTypeId 4). */
    public List<Map<String, Object>> stockAccounts() {
        return jdbc.queryForList("EXEC dbo.USP_Accounts_GetAccountTitleByAccountTypeIds @OrganizationId=?,@CompanyId=?,@UserId=?,@AccountTypeIds=?,@AppId=?",
                context.currentOrganizationId(), context.currentCompanyId(), context.currentUserId(), "4", context.currentAppId());
    }

    /** ClassGroupNameFill :233 - ItemCategory.GetItemClassGroup (Id, ClassGroupName); re-binds CmbClassGroup after StockComboFill. */
    public List<Map<String, Object>> classGroups() {
        return jdbc.queryForList("EXEC dbo.Sp_ItemCategory_GetAllMethod @Activity=?", "GetItemClassGroup");
    }

    /** GridBind :289 -> StocksReport.InventoryProfitLoss (Sp_InventoryProfitLossRpt). ChkItemWiseStock :309: ticked -> stockUOM 0 / PackingTypeId 0,
     *  unticked -> 1 / 1; the procedure's pack switches are @IsPackSizeOn / @IsPackTypeOn (it has no @PackingTypeId). PurchaseGLAC -> @ItemStockAc. */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "View");
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InventoryProfitLossRpt @OrganizationId=?,@CompanyId=?");
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
        if (num(p.get("packingTypeId")) != 0) { sql.append(",@IsPackTypeOn=?"); a.add(num(p.get("packingTypeId"))); }
        if (num(p.get("purchaseGLAC")) != 0) { sql.append(",@ItemStockAc=?"); a.add(num(p.get("purchaseGLAC"))); }
        if (num(p.get("itemClassGroupId")) != 0) { sql.append(",@ItemClassGroupId=?"); a.add(num(p.get("itemClassGroupId"))); }
        if (num(p.get("inventoryParentCategories")) != 0) { sql.append(",@InventoryParentCategoriesId=?"); a.add(num(p.get("inventoryParentCategories"))); }
        sql.append(",@Activity=?"); a.add(str(p.get("activity")));
        return jdbc.queryForList(sql.toString(), a.toArray());
    }
}
