package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;

import java.sql.Types;
import java.util.*;

/**
 * StockReportWithDocumentWiseIncludeOrders (Architecture.WinApp.Pcc.Reports/StockReportWithDocumentWiseIncludeOrders.cs) - ScreenDefinition 843
 * "Stock Report With Document Wise Include Orders". Line refs ":NNN" are that file.
 * Data: StocksReport.StockDocumentWise_DropDownListsConcrete -> [pcc].[USP_StockDocumentWise_DropDownLists] @OrganizationId,@CompanyId
 * (ActivityType, Id, name); CommonServices.DateType -> fixed in-code list (CommonServices.cs :15850);
 * StocksReport.StockReportWithDocumentWiseIncludeOrders -> [pcc].[USP_StockReportWithDocumentWiseIncludeOrders].
 */
@Service
public class Inv19dStockReportDocumentWiseIncludeOrdersService {

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public Inv19dStockReportDocumentWiseIncludeOrdersService(JdbcTemplate jdbc, CurrentUserContext context) { this.jdbc = jdbc; this.context = context; }

    /** CommonServices.DateType() :15850-15863 - Id 1..5. Datetypefill :119 binds Id / Parameters with ZeroIndex:true and activates Rows[2] = "This Week". */
    private static final List<Map<String, Object>> DATE_TYPES;
    static {
        String[] names = { "This Day", "This Week", "This Month", "This Year", "Financial Year" };
        List<Map<String, Object>> l = new ArrayList<>();
        for (int i = 0; i < names.length; i++) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", i + 1); m.put("Parameters", names[i]); l.add(m); }
        DATE_TYPES = Collections.unmodifiableList(l);
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

    /** InventoryStockEvaluation_GenrateStocks_Load :101-117 */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("combos", combos());
        out.put("dateTypes", DATE_TYPES);
        var fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", context.currentFinancialYearId());   // clsGlobalVariables.ActiveYr.Start_Period :237
        out.put("fyStart", fy.isEmpty() || fy.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(fy.get(0).get("Start_Period")).substring(0, 10));
        return out;
    }

    /** StockComboFill :134 - [pcc].[USP_StockDocumentWise_DropDownLists] with Organization/Company only (ParentCategories, ItemTypes, ItemCategories, Items). */
    public List<Map<String, Object>> combos() {
        return jdbc.queryForList("EXEC [pcc].[USP_StockDocumentWise_DropDownLists] @OrganizationId=?,@CompanyId=?", context.currentOrganizationId(), context.currentCompanyId());
    }

    /** GridBind :246-303 -> [pcc].[USP_StockReportWithDocumentWiseIncludeOrders]; 0 values are left at the procedure defaults (null). */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        String from = str(p.get("fromDate")), to = str(p.get("toDate"));
        if (from.isEmpty() || to.isEmpty()) throw new IllegalArgumentException("Date From and Date To are required.");
        StringBuilder sql = new StringBuilder("EXEC [pcc].[USP_StockReportWithDocumentWiseIncludeOrders] @OrganizationId=?,@CompanyId=?,@DateFrom=?,@DateTo=?");
        List<Object> a = new ArrayList<>(List.of(context.currentOrganizationId(), context.currentCompanyId(), day(from), day(to)));
        if (num(p.get("itemId")) != 0) { sql.append(",@ItemId=?"); a.add(num(p.get("itemId"))); }
        if (num(p.get("itemTypeId")) != 0) { sql.append(",@ItemTypeId=?"); a.add(num(p.get("itemTypeId"))); }
        if (num(p.get("itemCategoryId")) != 0) { sql.append(",@ItemCategoryId=?"); a.add(num(p.get("itemCategoryId"))); }
        if (num(p.get("inventoryParentCategories")) != 0) { sql.append(",@InventoryParentCategoriesId=?"); a.add(num(p.get("inventoryParentCategories"))); }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }
}
