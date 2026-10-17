package com.mst.services;

import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Types;
import java.util.*;

/**
 * StockReconcilation (Architecture.WinApp.Inventory_Reports/StockReconcilation.cs) - ScreenDefinition 303 "Stock Reconcilation (Not Use)".
 * Line refs ":NNN" are that file.
 * Data: CommonServices.GetItemsFromInventoryStocksEvaluations / GetItemTypeFromInventoryStocksEvaluations / GetItemCategoryFromInventoryStocksEvaluations
 *       -> Sp_Item_GetAllMethod @Activity of the same name (@OrganizationId @CompanyId); CommonServices.JobLotGetAllService -> SP_JobLot_ReadMethod 'GetAll';
 *       GeneralReprots.StockReconcilation_VIEWCGS (GridFill :181) -> USP_StockReconcilation_VIEWCGS;
 *       GeneralReprots.StockReconcilation_CGS (toolStripButton1_Click :287 "Reconcilation") -> USP_StockReconcilation_CGS.
 * No print button on this form.
 */
@Service
public class Inv19bStockReconcilationService {

    public static final int SCREEN_ID = 303;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public Inv19bStockReconcilationService(JdbcTemplate jdbc, CurrentUserContext context, DesktopReportRights rights) {
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

    /** frmGatePassReport_Load :114 - ToDate = now, ItemNameFill, ItemCategoryFill, ItemTypeFill, JobLotBind. */
    public Map<String, Object> init() {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("today", java.time.LocalDate.now().toString());
        out.put("items", itemsActivity("GetItemsFromInventoryStocksEvaluations"));            // ItemId, ItemName :87
        out.put("itemCategories", itemsActivity("GetItemCategoryFromInventoryStocksEvaluations"));   // ItemCategoryId, CategoryDescription :151
        out.put("itemTypes", itemsActivity("GetItemTypeFromInventoryStocksEvaluations"));     // ItemTypeId, TypeDescription :134
        out.put("jobLots", jdbc.queryForList("EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'",
                context.currentOrganizationId(), context.currentCompanyId()));                  // Id, JobLotDescription :171
        return out;
    }

    private List<Map<String, Object>> itemsActivity(String activity) {
        return jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity=?",
                context.currentOrganizationId(), context.currentCompanyId(), activity);
    }

    /** GridFill :181 -> USP_StockReconcilation_VIEWCGS @OrganizationId @CompanyId @ItemId @TillDate(ToDate) @EntryUser(UserAccount.ID) [@JobLotId].
     *  @ItemId and @TillDate have no procedure default; the desktop sends ToDate.Value and the combo value as it is (0 when none). */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "View");
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_StockReconcilation_VIEWCGS @OrganizationId=?,@CompanyId=?,@ItemId=?,@TillDate=?,@EntryUser=?");
        List<Object> a = new ArrayList<>(List.of(context.currentOrganizationId(), context.currentCompanyId(), num(p.get("itemId")),
                day(str(p.get("toDate")).isEmpty() ? java.time.LocalDate.now().toString() : str(p.get("toDate"))), context.currentUserId()));
        if (num(p.get("jobLotId")) != 0) { sql.append(",@JobLotId=?"); a.add(num(p.get("jobLotId"))); }
        List<Map<String, Object>> rows = jdbc.queryForList(sql.toString(), a.toArray());
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not found For Display");   // :204
        return rows;
    }

    /** toolStripButton1_Click :287 "Reconcilation" -> USP_StockReconcilation_CGS @OrganizationId @CompanyId @ItemId @TillDate(DocDate = ToDate) @EntryUser [@JobLotId];
     *  only when an item row is active (:299). Message "Record Update" :310. */
    @Transactional
    public void reconcile(Map<String, Object> p) {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "Update");
        if (num(p.get("itemId")) == 0) throw new IllegalArgumentException("Select Item Name first.");
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_StockReconcilation_CGS @OrganizationId=?,@CompanyId=?,@ItemId=?,@TillDate=?,@EntryUser=?");
        List<Object> a = new ArrayList<>(List.of(context.currentOrganizationId(), context.currentCompanyId(), num(p.get("itemId")),
                java.sql.Timestamp.valueOf((str(p.get("toDate")).isEmpty() ? java.time.LocalDate.now().toString() : str(p.get("toDate")).substring(0, 10)) + " 00:00:00"), context.currentUserId()));
        if (num(p.get("jobLotId")) != 0) { sql.append(",@JobLotId=?"); a.add(num(p.get("jobLotId"))); }
        final Object[] args = a.toArray();
        jdbc.execute(sql.toString(), (PreparedStatementCallback<Object>) ps -> {
            for (int i = 0; i < args.length; i++) ps.setObject(i + 1, args[i]);
            ps.execute();
            return null;
        });
    }
}
