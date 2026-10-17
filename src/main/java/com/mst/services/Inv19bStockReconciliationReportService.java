package com.mst.services;

import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;

import java.sql.Types;
import java.util.*;

/**
 * StockReportReconcilation (Architecture.WinApp.Inventory_Stocks_Report/StockReportReconcilation.cs) - ScreenDefinition 295
 * "Stock Report Reconcilation". Line refs ":NNN" are that file.
 * Data: StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists (StockComboFill :141) -> USP_Inventory_StockEvalautionDetail_DropDownAndLists
 *       @OrganizationId @CompanyId (ActivityType, Id, name);
 *       StocksReport.StockReconcilationReport (GridBind :285) -> USP_StockReconcilation;
 *       CommonServices.DateType() (DateTypefill :126) -> the five literal date types, Rows[3] active (= 3 This Month, zero row at index 0);
 *       cmbDateType_ValueChanged :233 -> clsGlobalVariables.ActiveYr.Start_Period for type 5.
 * Print: btnPrint_Click :546 -> "481-StockReconcilationRegister.rpt" over the procedure rows.
 */
@Service
public class Inv19bStockReconciliationReportService {

    public static final int SCREEN_ID = 295;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public Inv19bStockReconciliationReportService(JdbcTemplate jdbc, CurrentUserContext context, DesktopReportRights rights) {
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

    /** StockReportReconcilation_Load :109 */
    public Map<String, Object> init() {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        var fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", context.currentFinancialYearId());
        out.put("fyStart", fy.isEmpty() || fy.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(fy.get(0).get("Start_Period")).substring(0, 10));
        out.put("today", java.time.LocalDate.now().toString());
        out.put("combos", combos());
        return out;
    }

    /** StockComboFill :141 - USP_Inventory_StockEvalautionDetail_DropDownAndLists @OrganizationId @CompanyId; the form keeps
     *  ParentCategories with Id 1..4, ItemTypes, ItemCategories, Items, Warehouse. */
    public List<Map<String, Object>> combos() {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "View");
        return jdbc.queryForList("EXEC dbo.USP_Inventory_StockEvalautionDetail_DropDownAndLists @OrganizationId=?,@CompanyId=?",
                context.currentOrganizationId(), context.currentCompanyId());
    }

    /** GridBind :285 -> StocksReport.StockReconcilationReport -> USP_StockReconcilation. FormValidation :399 first.
     *  The desktop never sends the parent category to the procedure (:291-303); chkIncludePackUom sets SkipZero = 1 (:300),
     *  which the BLL hands to the procedure's only matching flag @IncludePackUom. Zero ids are not sent. */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "View");
        if (num(p.get("parentCategoryId")) == 0) throw new IllegalArgumentException("Please Select Parent Category First");
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_StockReconcilation @OrganizationId=?,@CompanyId=?");
        List<Object> a = new ArrayList<>(List.of(context.currentOrganizationId(), context.currentCompanyId()));
        if (!str(p.get("fromDate")).isEmpty()) { sql.append(",@DateFrom=?"); a.add(day(str(p.get("fromDate")))); }
        if (!str(p.get("toDate")).isEmpty()) { sql.append(",@DateTo=?"); a.add(day(str(p.get("toDate")))); }
        if (num(p.get("itemId")) != 0) { sql.append(",@ItemId=?"); a.add(num(p.get("itemId"))); }
        if (num(p.get("warehouseId")) != 0) { sql.append(",@WarehouseId=?"); a.add(num(p.get("warehouseId"))); }
        if (num(p.get("itemCategoryId")) != 0) { sql.append(",@ItemCategoryId=?"); a.add(num(p.get("itemCategoryId"))); }
        if (num(p.get("itemTypeId")) != 0) { sql.append(",@ItemTypeId=?"); a.add(num(p.get("itemTypeId"))); }
        if (num(p.get("includePackUom")) != 0) { sql.append(",@IncludePackUom=?"); a.add(num(p.get("includePackUom"))); }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }
}
