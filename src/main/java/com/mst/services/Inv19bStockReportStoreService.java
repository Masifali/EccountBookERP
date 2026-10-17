package com.mst.services;

import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;

import java.sql.Types;
import java.util.*;

/**
 * RptGenrateStocksStore (Architecture.WinApp.Inventory_Stocks_Report/RptGenrateStocksStore.cs) - ScreenDefinition 300
 * "Stock Stocks Report" (hub title "Stock Report Store"). Line refs ":NNN" are that file.
 * Data: StocksReport.StockReportComboByOrganizationAndCompanyId (AllComboBind :173, Ids "7,8")
 *       -> Sp_Inventory_InventoryTransactions_DropDownAndLists (ActivityType, Id, name, ParentCategoryId);
 *       InventoryStockEvalautionDetail.InventoryStockStoreGenrate (GridBind :312) -> Sp_InventoryStockEvalautionDetail_GenerateStocksStore;
 *       InfragisticsHelper.BindComboDateType (Datetypefill :161) -> the five literal date types, default 5 (Financial Year);
 *       cmbDateType_ValueChanged :647 -> clsGlobalVariables.ActiveYr.Start_Period for type 5.
 * Print: PrintRegister :612 -> "401-StockEvalautionDetail_GenerateStocks_Register -Store.rpt" over the raw procedure rows.
 */
@Service
public class Inv19bStockReportStoreService {

    public static final int SCREEN_ID = 300;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public Inv19bStockReportStoreService(JdbcTemplate jdbc, CurrentUserContext context, DesktopReportRights rights) {
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

    /** clsGlobalVariables.ActiveYr.Start_Period (cmbDateType_ValueChanged :675). */
    public String fyStart() {
        var fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", context.currentFinancialYearId());
        return fy.isEmpty() || fy.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(fy.get(0).get("Start_Period")).substring(0, 10);
    }

    /** InventoryStockEvaluation_GenrateStocks_Load :141 - ParentCategoryFill (literal 7 Packing Material / 8 Store :198), AllComboBind, Datetypefill. */
    public Map<String, Object> init() {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("fyStart", fyStart());
        out.put("today", java.time.LocalDate.now().toString());
        List<Map<String, Object>> parents = new ArrayList<>();
        parents.add(Map.of("Id", 7, "ParentCategory", "Packing Material"));
        parents.add(Map.of("Id", 8, "ParentCategory", "Store"));
        out.put("parentCategories", parents);
        out.put("combos", combos());
        return out;
    }

    /** AllComboBind :173 - StocksReport.StockReportComboByOrganizationAndCompanyId, Ids = "7,8"
     *  -> Sp_Inventory_InventoryTransactions_DropDownAndLists (ActivityType, Id, name, ParentCategoryId). */
    public List<Map<String, Object>> combos() {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "View");
        return jdbc.queryForList("EXEC dbo.Sp_Inventory_InventoryTransactions_DropDownAndLists @OrganizationId=?,@CompanyId=?,@InventoryParentCategory=?",
                context.currentOrganizationId(), context.currentCompanyId(), "7,8");
    }

    /** GridBind :312 -> InventoryStockEvalautionDetail.InventoryStockStoreGenrate -> Sp_InventoryStockEvalautionDetail_GenerateStocksStore.
     *  Zero ids are not sent (the procedure's NULL defaults then apply, as the BLL guards them); ZeroBalanceType = 1 when Skip Zero is ticked :331. */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "View");
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InventoryStockEvalautionDetail_GenerateStocksStore @OrganiztionId=?,@CompanyId=?");
        List<Object> a = new ArrayList<>(List.of(context.currentOrganizationId(), context.currentCompanyId()));
        if (!str(p.get("fromDate")).isEmpty()) { sql.append(",@DateFrom=?"); a.add(day(str(p.get("fromDate")))); }
        if (!str(p.get("toDate")).isEmpty()) { sql.append(",@DateTo=?"); a.add(day(str(p.get("toDate")))); }
        if (num(p.get("itemId")) != 0) { sql.append(",@ItemId=?"); a.add(num(p.get("itemId"))); }
        if (num(p.get("warehouseId")) != 0) { sql.append(",@WarehouseId=?"); a.add(num(p.get("warehouseId"))); }
        if (num(p.get("itemTypeId")) != 0) { sql.append(",@ItemTypeId=?"); a.add(num(p.get("itemTypeId"))); }
        if (num(p.get("itemCategoryId")) != 0) { sql.append(",@ItemCategoryId=?"); a.add(num(p.get("itemCategoryId"))); }
        if (num(p.get("parentCategoryId")) != 0) { sql.append(",@ParentCategoryId=?"); a.add(num(p.get("parentCategoryId"))); }
        if (num(p.get("zeroBalanceType")) != 0) { sql.append(",@SkipZero=?"); a.add(num(p.get("zeroBalanceType"))); }
        if (num(p.get("itemConditionId")) != 0) { sql.append(",@ItemConditionId=?"); a.add(num(p.get("itemConditionId"))); }
        if (num(p.get("storeRackId")) != 0) { sql.append(",@RackId=?"); a.add(num(p.get("storeRackId"))); }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }
}
