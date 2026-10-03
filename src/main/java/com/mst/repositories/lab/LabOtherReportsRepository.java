package com.mst.repositories.lab;

import com.mst.repositories.support.DesktopProc;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

/**
 * Lab Report (module 1011) - the BLL calls of two desktop report forms, parameter for parameter.
 *
 *   627  Architecture.WinApp.Lab.InvLabPurchaseReport  "Lab Purchase Analysis Report (Not Use)"
 *        BLL architecture.bll/0403_Architecture.BLL.Lab.InvLabAnalysisPurchaseHeader.cs ("P:nnn")
 *   628  Architecture.WinApp.Lab.InvLabSaleRegister    "Lab Sale Analysis Report"
 *        BLL architecture.bll/0405_Architecture.BLL.Lab.InvLabAnalysisSaleHeader.cs ("S:nnn")
 *
 * A null parameter value is OMITTED from the EXEC (the BLL adds those parameters only when they are
 * non-zero / a real date); it is never bound as NULL. Every procedure and parameter name below was checked
 * against procdure.utf8.sql / procdure_index.csv.
 *
 * {@link #table} keeps the result set's column order: InvLabSaleRegister addresses the procedure result BY
 * COLUMN POSITION (columnNames[i + 35], Rows[i][23 + j]).
 */
@Repository
public class LabOtherReportsRepository {

    /** A DataTable: column names in result-set order and the rows as positional values. */
    public static final class Table {
        public final List<String> columns = new ArrayList<>();
        public final List<Object[]> rows = new ArrayList<>();

        /** Position of a column (case-insensitive, as DataColumnCollection lookups are), -1 when absent. */
        public int find(String name) {
            for (int i = 0; i < columns.size(); i++) if (columns.get(i).equalsIgnoreCase(name)) return i;
            return -1;
        }

        /** DataRow["name"] - throws the DataTable's own message when the column is absent. */
        public int index(String name) {
            int i = find(name);
            if (i < 0) throw new IllegalArgumentException("Column '" + name + "' does not belong to table .");
            return i;
        }
    }

    private final JdbcTemplate jdbc;

    public LabOtherReportsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ===================================================================== 627 InvLabPurchaseReport

    private static final String PURCHASE_GET_ALL = "Sp_InvLabAnalysisPurchaseHeader_GetAllMethod";

    /**
     * cmbsupplierfill: CommonServices.SupplierCustomerGetforComboServiceBind() (CommonServices.cs:1204) ->
     * SupplierCustomer.GetforComboBinding -> Sp_SupplierCustomer_GetAllMethod,
     * @Activity = 'ReadByOrganizationIdCompanyIdForBinding'. Rows: Id, CompanyName, GlAccountId, PartyCode ...
     */
    public List<Map<String, Object>> suppliers(int organizationId, int companyId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", organizationId);
        p.put("CompanyId", companyId);
        p.put("Activity", "ReadByOrganizationIdCompanyIdForBinding");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    /**
     * InvLabAnalysisPurchaseHeader.PurchaseReport (P:652) -> Sp_InvLabAnalysisPurchase_RiceHistory_Rpt.
     * BLL order: @OrganizationId, @CompanyId, @FromDate (real date), @ToDate (sent when FROMDATE is a real date -
     * the BLL tests obj.FromDate twice), @Id (!= 0, never set by the form), @OrderId (!= 0, never set),
     * @SupplierId (!= 0), @FromGpNo (!= 0), @ToGpNo (!= 0), @BranchesId (!= 0, never set).
     */
    public Table purchaseReport(int organizationId, int companyId, Timestamp fromDate, Timestamp toDate,
                                Integer supplierId, Integer fromGpNo, Integer toGpNo) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", organizationId);
        p.put("CompanyId", companyId);
        p.put("FromDate", fromDate);
        p.put("ToDate", fromDate == null ? null : toDate);
        p.put("SupplierId", supplierId);
        p.put("FromGpNo", fromGpNo);
        p.put("ToGpNo", toGpNo);
        return table("dbo.Sp_InvLabAnalysisPurchase_RiceHistory_Rpt", p);
    }

    /** InvLabAnalysisPurchaseHeader.GetById (P:479) -> Sp_InvLabAnalysisPurchaseHeader_GetAllMethod @Id, @Activity = 'ReadById'. */
    public List<Map<String, Object>> purchaseById(int id) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Id", id);
        p.put("Activity", "ReadById");
        return DesktopProc.rows(jdbc, PURCHASE_GET_ALL, p);
    }

    /**
     * Sp_InvLabAnalysisPurchaseHeader_GetAllMethod @Id, @Activity = 'ReadDetailByHeaderId' (DAL 0359 GetData) -
     * the analysis rows of a header: d.* (InAnalysisResult, RemarksDetail ...), MinValue, MaxValue,
     * AnalysisParameterDescription. Used only when 'ReadById' no longer returns those columns (see the service).
     */
    public List<Map<String, Object>> purchaseDetailByHeaderId(int id) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Id", id);
        p.put("Activity", "ReadDetailByHeaderId");
        return DesktopProc.rows(jdbc, PURCHASE_GET_ALL, p);
    }

    // ===================================================================== 628 InvLabSaleRegister

    private static final String SALE_GET_ALL = "Sp_InvLabAnalysisSaleHeader_GetAllMethod";

    /** InvLabAnalysisSaleHeader.GetItemFromSaleAnalysis (S:205) - Id, ItemName. */
    public List<Map<String, Object>> saleItems(int organizationId, int companyId) {
        return saleLookup(organizationId, companyId, "GetItemFromSaleAnalysis");
    }

    /** InvLabAnalysisSaleHeader.GetParentCategoryFromSaleAnalysis (S:233) - Id, InvParentCateDescription. */
    public List<Map<String, Object>> saleParentCategories(int organizationId, int companyId) {
        return saleLookup(organizationId, companyId, "GetParentCategoryFromSaleAnalysis");
    }

    /** InvLabAnalysisSaleHeader.GetSaleOrderFromSaleAnalysis (S:261) - Id, DocNo. */
    public List<Map<String, Object>> saleOrders(int organizationId, int companyId) {
        return saleLookup(organizationId, companyId, "GetSaleOrderFromSaleAnalysis");
    }

    /** InvLabAnalysisSaleHeader.GetCustomerFromSaleAnalysis (S:289) - Id, CompanyName. */
    public List<Map<String, Object>> saleCustomers(int organizationId, int companyId) {
        return saleLookup(organizationId, companyId, "GetCustomerFromSaleAnalysis");
    }

    /**
     * @OrganizationId, @CompanyId, @Activity. The procedure text carries the 'GetItemFromSaleAnalysis' and
     * 'GetParentCategoryFromSaleAnalysis' blocks twice, so those two activities return two identical result
     * sets; GenericProvider.GetDataTableProc (and this call) use the first.
     */
    private List<Map<String, Object>> saleLookup(int organizationId, int companyId, String activity) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", organizationId);
        p.put("CompanyId", companyId);
        p.put("Activity", activity);
        Table t = table("dbo." + SALE_GET_ALL, p);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Object[] r : t.rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (int c = 0; c < t.columns.size(); c++) m.put(t.columns.get(c), r[c]);
            out.add(m);
        }
        return out;
    }

    /**
     * InvLabAnalysisSaleHeader.LabSaleAnalysisRegister (S:317) -> [USP_LabSaleAnalysis_Register].
     * BLL order: @OrganizationId, @CompanyId, @Datefrom (real date), @DateTo (real date), @GpSrNoFrom (!= 0),
     * @GpSrNoTo (!= 0), @ItemId (!= 0), @SupplierCustomerId (!= 0), <order> (!= 0), @IsAccepted
     * (ApprovedFilter != "All"), @InventoryParentCategorId (!= 0).
     *
     * ORDER PARAMETER: the BLL names it "@PurchaseOrderId", but the procedure declares
     * @SaleOrderId and has no @PurchaseOrderId (procdure_index.csv: @organizationid @companyid @datefrom @dateto
     * @saleorderid @itemid @suppliercustomerid @inventoryparentcategorid @gpsrnoto @gpsrnofrom @isaccepted), so on
     * the desktop picking an Order No makes SQL Server reject the call. The procedure's own name is used here.
     */
    public Table saleRegister(int organizationId, int companyId, Timestamp dateFrom, Timestamp dateTo, Integer gpSrNoFrom,
                              Integer gpSrNoTo, Integer itemId, Integer supplierCustomerId, Integer saleOrderId,
                              Boolean isAccepted, Integer parentCategoryId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", organizationId);
        p.put("CompanyId", companyId);
        p.put("Datefrom", dateFrom);
        p.put("DateTo", dateTo);
        p.put("GpSrNoFrom", gpSrNoFrom);
        p.put("GpSrNoTo", gpSrNoTo);
        p.put("ItemId", itemId);
        p.put("SupplierCustomerId", supplierCustomerId);
        p.put("SaleOrderId", saleOrderId);
        p.put("IsAccepted", isAccepted);
        p.put("InventoryParentCategorId", parentCategoryId);      // sic - the procedure's spelling
        return table("[dbo].[USP_LabSaleAnalysis_Register]", p);
    }

    // ===================================================================== plumbing

    /**
     * GenericProvider.GetDataTableProc: the first result set with its columns in order. The register procedure
     * runs SELECT INTO / dynamic UPDATE statements before its final SELECT, so update counts are skipped until a
     * result set arrives; later results are drained so a late error surfaces.
     */
    private Table table(String proc, Map<String, Object> params) {
        List<Object> args = new ArrayList<>();
        StringBuilder sql = new StringBuilder("EXEC ").append(proc);
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;                    // the BLL did not add it
            sql.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue());
            first = false;
        }
        return jdbc.execute(sql.toString(), (PreparedStatementCallback<Table>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            Table t = new Table();
            boolean taken = false;
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        if (!taken) {
                            ResultSetMetaData md = rs.getMetaData();
                            int n = md.getColumnCount();
                            for (int c = 1; c <= n; c++) {
                                String label = md.getColumnLabel(c);
                                t.columns.add(label == null || label.isEmpty() ? "Column" + c : label);
                            }
                            while (rs.next()) {
                                Object[] row = new Object[n];
                                for (int c = 1; c <= n; c++) row[c - 1] = rs.getObject(c);
                                t.rows.add(row);
                            }
                            taken = true;
                        } else {
                            while (rs.next()) { /* drain */ }
                        }
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isRs = ps.getMoreResults();
            }
            return t;
        });
    }
}
