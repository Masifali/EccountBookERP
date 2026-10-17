package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;

import java.sql.Types;
import java.util.*;

/**
 * StockReportByFifo (Architecture.WinApp.Pcc.Reports/StockReportByFifo.cs) - ScreenDefinition 842 "Stock By FiFo" (Inventory hub, module 19 Stock Reports).
 * Line refs ":NNN" are that file.
 * Data: BLL StocksReport.StockReportComboByOrganizationAndCompanyId -> dbo.Sp_Inventory_InventoryTransactions_DropDownAndLists (@OrganizationId,@CompanyId)
 *       (the form keeps ActivityType 'Items', 'JobLot', 'Warehouse' :146-157);
 *       ItemAttributeVarient.GetAllForCombo(org, comp, itemId) -> dbo.USP_ItemAttributeVarient_GetAllMethod @Activity='GetAllForCombo' (ItemAttributeVarientId, ItemAttribute);
 *       InvStockTransferHeader.StockByFiFo (BLL.pcc) -> pcc.USP_GetStockByFifo_Report (@CompanyId,@OrganizationId,@ItemId,@VarientId,@DocDate,@WarehouseId,@JobLotId,...)
 *       - confirmed by rpt_registry for 1887-GetReportByFiFo.rpt.
 */
@Service
public class Inv19aStockByFifoPccService {

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public Inv19aStockByFifoPccService(JdbcTemplate jdbc, CurrentUserContext context) { this.jdbc = jdbc; this.context = context; }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null || String.valueOf(o).isBlank()) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
    private static SqlParameterValue day(String iso) { return new SqlParameterValue(Types.TIMESTAMP, java.sql.Timestamp.valueOf(iso.substring(0, 10) + " 00:00:00")); }

    /** frmStockReport_Load :105 - focus txtDateFrom (designer default = today), StockComboFill :121. */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docDate", java.time.LocalDate.now().toString());
        out.put("combos", combos());
        return out;
    }

    /** StockComboFill :121 - (ActivityType, Id, name). */
    public List<Map<String, Object>> combos() {
        return jdbc.queryForList("EXEC dbo.Sp_Inventory_InventoryTransactions_DropDownAndLists @OrganizationId=?,@CompanyId=?", context.currentOrganizationId(), context.currentCompanyId());
    }

    /** bindvarientunit :200 - ItemAttributeVarient.GetAllForCombo(OrganizationId, CompanyId, ItemId) -> (ItemAttributeVarientId, ItemAttribute). */
    public List<Map<String, Object>> varients(int itemId) {
        return jdbc.queryForList("EXEC dbo.USP_ItemAttributeVarient_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@Activity=?",
                context.currentOrganizationId(), context.currentCompanyId(), itemId, "GetAllForCombo");
    }

    /** GridBind :242 -> InvStockTransferHeader.StockByFiFo(OrganizationId, CompanyId, DocDate, ItemId, VarientId, WarehouseId, JobLotId).
     *  @ItemId has no default in pcc.USP_GetStockByFifo_Report, so it is always sent (0 when no item is chosen -> no rows); the optional ids only when > 0. */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        if (str(p.get("docDate")).isEmpty()) throw new IllegalArgumentException("Doc Date is required.");
        StringBuilder sql = new StringBuilder("EXEC pcc.USP_GetStockByFifo_Report @CompanyId=?,@OrganizationId=?,@ItemId=?,@DocDate=?");
        List<Object> a = new ArrayList<>(List.of(context.currentCompanyId(), context.currentOrganizationId(), num(p.get("itemId")), day(str(p.get("docDate")))));
        if (num(p.get("varientId")) != 0) { sql.append(",@VarientId=?"); a.add(num(p.get("varientId"))); }
        if (num(p.get("warehouseId")) != 0) { sql.append(",@WarehouseId=?"); a.add(num(p.get("warehouseId"))); }
        if (num(p.get("jobLotId")) != 0) { sql.append(",@JobLotId=?"); a.add(num(p.get("jobLotId"))); }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }
}
