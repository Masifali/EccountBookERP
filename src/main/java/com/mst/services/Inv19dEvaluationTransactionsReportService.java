package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;

import java.sql.Types;
import java.util.*;

/**
 * EvaluationStockTransactionsReport (Architecture.WinApp.Pcc.Reports/EvaluationStockTransactionsReport.cs) - ScreenDefinition 815
 * "Evaluation Transactions Report". Line refs ":NNN" are that file.
 * Data: StocksReport.StockReportComboByOrganizationAndCompanyId -> Sp_Inventory_InventoryTransactions_DropDownAndLists (ActivityType, Id, name);
 * VoucherHead.DocumentTypeGetFromInventoryTransactions -> Sp_Vouchers_GetMethods @Activity='DocumentTypeGetFromInventoryTransactions'
 * (RefDocumentTypeId, DocumentTypeDescription); CommonServices.DateType -> fixed in-code list (CommonServices.cs :15850);
 * StocksReport.GetInventoryStockAsOnDate -> usp_getInventoryStockAsOnDate;
 * InventoryStockEvalautionDetail.EvaluationStockTransactionsReport -> [pcc].[USP-EvaluationStockTransactionsReport].
 */
@Service
public class Inv19dEvaluationTransactionsReportService {

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public Inv19dEvaluationTransactionsReportService(JdbcTemplate jdbc, CurrentUserContext context) { this.jdbc = jdbc; this.context = context; }

    /** CommonServices.DateType() :15850-15863 - Id 1..5. ParameterFill :168 binds Id / Parameters. */
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

    /** PurchaseOrderHistory_Load :141-166 */
    public Map<String, Object> init() {
        Map<String, Object> out = lookups();
        out.put("dateTypes", DATE_TYPES);
        var fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", context.currentFinancialYearId());
        out.put("fyStart", fy.isEmpty() || fy.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(fy.get(0).get("Start_Period")).substring(0, 10));
        // StocksReport.GetInventoryStockAsOnDate :155 - fromdate = AsOnDate when it is not the null date
        var d = jdbc.queryForList("EXEC dbo.usp_getInventoryStockAsOnDate @OrganizationId=?,@CompanyId=?", context.currentOrganizationId(), context.currentCompanyId());
        String asOn = "";
        if (!d.isEmpty() && d.get(0).get("AsOnDate") != null) {
            try { asOn = java.time.LocalDate.parse(String.valueOf(d.get(0).get("AsOnDate")).substring(0, 10)).toString(); } catch (RuntimeException e) { asOn = ""; }
        }
        out.put("asOnDate", asOn);
        return out;
    }

    /** StockComboFill :182 + DocumentTypeFillFromInventoryTransactions :342 (also btnRefresh_Click :735). */
    public Map<String, Object> lookups() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("combos", jdbc.queryForList("EXEC dbo.Sp_Inventory_InventoryTransactions_DropDownAndLists @OrganizationId=?,@CompanyId=?",
                context.currentOrganizationId(), context.currentCompanyId()));
        out.put("documentTypes", jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?,@CompanyId=?,@Activity=?",
                context.currentOrganizationId(), context.currentCompanyId(), "DocumentTypeGetFromInventoryTransactions"));
        return out;
    }

    /** btnshow_Click :451-538 -> [pcc].[USP-EvaluationStockTransactionsReport]; 0 / empty values are left at the procedure defaults (null). */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        String from = str(p.get("fromDate")), to = str(p.get("toDate"));
        if (from.isEmpty() || to.isEmpty()) throw new IllegalArgumentException("From Date and To Date are required.");
        StringBuilder sql = new StringBuilder("EXEC [pcc].[USP-EvaluationStockTransactionsReport] @OrganizationId=?,@CompanyId=?,@DateFrom=?,@DateTo=?");
        List<Object> a = new ArrayList<>(List.of(context.currentOrganizationId(), context.currentCompanyId(), day(from), day(to)));
        if (num(p.get("supplierCustomerId")) != 0) { sql.append(",@SupplierCustomerId=?"); a.add(num(p.get("supplierCustomerId"))); }
        if (num(p.get("itemId")) != 0) { sql.append(",@ItemId=?"); a.add(num(p.get("itemId"))); }
        if (num(p.get("jobLotId")) != 0) { sql.append(",@JobLotId=?"); a.add(num(p.get("jobLotId"))); }
        if (num(p.get("documentTypeId")) != 0) { sql.append(",@DocumentTypeId=?"); a.add(num(p.get("documentTypeId"))); }
        if (num(p.get("itemClassGroupId")) != 0) { sql.append(",@ItemClassGroupId=?"); a.add(num(p.get("itemClassGroupId"))); }
        if (num(p.get("parentCategoryId")) != 0) { sql.append(",@InventoryParentCategoriesId=?"); a.add(num(p.get("parentCategoryId"))); }
        if (num(p.get("itemCategoryId")) != 0) { sql.append(",@ItemCategoryId=?"); a.add(num(p.get("itemCategoryId"))); }
        if (!str(p.get("itemTypeIds")).isEmpty()) { sql.append(",@ItemTypeIds=?"); a.add(str(p.get("itemTypeIds")) + ","); }     // Ids + "," :493
        if (!str(p.get("warehouseIds")).isEmpty()) { sql.append(",@WarehouseIds=?"); a.add(str(p.get("warehouseIds")) + ","); }  // WareIds + "," :479
        return jdbc.queryForList(sql.toString(), a.toArray());
    }
}
