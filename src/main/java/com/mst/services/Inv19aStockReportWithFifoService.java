package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;

import java.sql.Types;
import java.util.*;

/**
 * StockReportyWithFIFO (Architecture.WinApp.RetailStockReports/StockReportyWithFIFO.cs) - ScreenDefinition 296 "Stock Report With FIFO"
 * (Inventory hub, module 19 Stock Reports). Line refs ":NNN" are that file.
 * Data: BLL StocksReport.StockReportComboByOrganizationAndCompanyId -> dbo.Sp_Inventory_InventoryTransactions_DropDownAndLists (@OrganizationId,@CompanyId)
 *       (ActivityType, Id, name - the form keeps 'Items' and 'Warehouse' :123-130);
 *       StocksReport.StockByFiFo -> dbo.USP_GetStockByFifo_Report (@CompanyId,@OrganizationId,@DocDate,@ItemId,@PackUomId,@WarehouseId,@JobLotId,@PackingTypeId,@CropYear).
 * (No BLL/DAL source was recovered; USP_GetStockByFifo_Report is confirmed by rpt_registry for 390_01_GetStockByFifo.rpt, the combo procedure is the
 *  only dbo list procedure whose ActivityType values are exactly 'Items'/'Warehouse'/'JobLot'.)
 */
@Service
public class Inv19aStockReportWithFifoService {

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public Inv19aStockReportWithFifoService(JdbcTemplate jdbc, CurrentUserContext context) { this.jdbc = jdbc; this.context = context; }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null || String.valueOf(o).isBlank()) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
    private static SqlParameterValue day(String iso) { return new SqlParameterValue(Types.TIMESTAMP, java.sql.Timestamp.valueOf(iso.substring(0, 10) + " 00:00:00")); }

    /** frmStockReport_Load :83 - txtDocDate = clsGlobalVariables.ActiveYr.Start_Period; StockComboFill :101. */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        var fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", context.currentFinancialYearId());
        out.put("fyStart", fy.isEmpty() || fy.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(fy.get(0).get("Start_Period")).substring(0, 10));
        out.put("combos", combos());
        return out;
    }

    /** StockComboFill :101 - StocksReport.StockReportComboByOrganizationAndCompanyId (ActivityType, Id, name). */
    public List<Map<String, Object>> combos() {
        return jdbc.queryForList("EXEC dbo.Sp_Inventory_InventoryTransactions_DropDownAndLists @OrganizationId=?,@CompanyId=?", context.currentOrganizationId(), context.currentCompanyId());
    }

    /** GridBind :149 -> StocksReport.StockByFiFo(OrganizationId, CompanyId, DocDate, ItemId, WarehouseId); zero ids are not sent (the procedure's
     *  "@X is null or" guards), @ItemId is mandatory inside the procedure (it.ItemId = @ItemId) so no item means no rows. */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        if (str(p.get("docDate")).isEmpty()) throw new IllegalArgumentException("Doc Date is required.");
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_GetStockByFifo_Report @CompanyId=?,@OrganizationId=?,@DocDate=?");
        List<Object> a = new ArrayList<>(List.of(context.currentCompanyId(), context.currentOrganizationId(), day(str(p.get("docDate")))));
        if (num(p.get("itemId")) != 0) { sql.append(",@ItemId=?"); a.add(num(p.get("itemId"))); }
        if (num(p.get("warehouseId")) != 0) { sql.append(",@WarehouseId=?"); a.add(num(p.get("warehouseId"))); }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }
}
