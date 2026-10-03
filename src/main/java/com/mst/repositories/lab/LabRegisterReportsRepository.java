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
 *   629 / 632  Architecture.WinApp.Lab.InvLabSampleRegister
 *              BLL architecture.bll/0407_Architecture.BLL.Lab.InvLabSampleAnalysisHeader.cs
 *   631        Architecture.WinApp.Lab.InProcessLabAnalysisRegister
 *              BLL architecture.bll/0397_...LabInProcessAnalysisRawHeader.cs and 0401_...InvLabAnalysisInProcessHeader.cs
 *
 * A null parameter value is OMITTED from the EXEC (the BLL adds those parameters only when they are
 * non-zero / non-empty / a real date); it is never bound as NULL.
 *
 * The two register forms address the procedure result BY COLUMN POSITION (dt.Rows[i][26 + j]), so
 * {@link #table} keeps the result set's column order, which DesktopProc.rows (a TreeMap per row) does not.
 */
@Repository
public class LabRegisterReportsRepository {

    /** A DataTable: column names in result-set order and the rows as positional values. */
    public static final class Table {
        public final List<String> columns = new ArrayList<>();
        public final List<Object[]> rows = new ArrayList<>();

        /** DataRow["name"] - case-insensitive, as DataColumnCollection lookups are. */
        public int index(String name) {
            for (int i = 0; i < columns.size(); i++) if (columns.get(i).equalsIgnoreCase(name)) return i;
            throw new IllegalStateException("Column '" + name + "' does not belong to table .");
        }
    }

    private final JdbcTemplate jdbc;

    public LabRegisterReportsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ===================================================================== 629 / 632 InvLabSampleRegister

    private static final String SAMPLE_GET_ALL = "Sp_InvLabSampleAnalysisHeader_GetAllMethod";

    /** InvLabSampleAnalysisHeader.GetItemFromSampleAnalysis (0407:573) - Id, ItemName. */
    public List<Map<String, Object>> sampleItems(int organizationId, int companyId) {
        return sampleLookup(organizationId, companyId, "GetItemFromSampleAnalysis");
    }

    /** InvLabSampleAnalysisHeader.GetParentCategoryFromSampleAnalysis (0407:601) - Id, InvParentCateDescription. */
    public List<Map<String, Object>> sampleParentCategories(int organizationId, int companyId) {
        return sampleLookup(organizationId, companyId, "GetParentCategoryFromSampleAnalysis");
    }

    /** InvLabSampleAnalysisHeader.GetSuppliersFromSampleAnalysis (0407:657) - Id, CompanyName. */
    public List<Map<String, Object>> sampleSuppliers(int organizationId, int companyId) {
        return sampleLookup(organizationId, companyId, "GetSuppliersFromSampleAnalysis");
    }

    private List<Map<String, Object>> sampleLookup(int organizationId, int companyId, String activity) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", organizationId);
        p.put("CompanyId", companyId);
        p.put("Activity", activity);
        return DesktopProc.rows(jdbc, SAMPLE_GET_ALL, p);
    }

    /**
     * InvLabSampleAnalysisHeader.LabSampleAnalysisRegister (0407:685) -> [USP_LabSampleAnalysis_Register].
     * BLL order: @OrganizationId, @CompanyId, @Datefrom (real date), @DateTo (real date), @ItemId (!= 0),
     * @SupplierCustomerId (!= 0), @IsAccepted (ApprovedFilter != "All"), @InventoryParentCategorId (!= 0).
     * The caller passes null for whatever the BLL leaves out.
     */
    public Table sampleRegister(int organizationId, int companyId, Timestamp dateFrom, Timestamp dateTo,
                                Integer itemId, Integer supplierCustomerId, Boolean isAccepted, Integer parentCategoryId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", organizationId);
        p.put("CompanyId", companyId);
        p.put("Datefrom", dateFrom);
        p.put("DateTo", dateTo);
        p.put("ItemId", itemId);
        p.put("SupplierCustomerId", supplierCustomerId);
        p.put("IsAccepted", isAccepted);
        p.put("InventoryParentCategorId", parentCategoryId);      // sic - the procedure's spelling
        return table("[dbo].[USP_LabSampleAnalysis_Register]", p);
    }

    // ===================================================================== 631 InProcessLabAnalysisRegister

    /**
     * LabInProcessAnalysisRawHeader.GetDataForDropDownFromLabInProcessAnalysisRawHeader (0397:338) ->
     * [dbo].[USP_GetDataForDropDownFromLabInProcessAnalysisRawHeader]: @OrganizationId, @CompanyId,
     * @FinancialYearId always; @Activity and @DocumentTypeIds only when not empty. The form never sets
     * ParentCategoryId / PageSize / PageNumber / Keyword, and the BLL has no @PlantId at all.
     * Rows: Id, ReferenceName, Activity.
     */
    public List<Map<String, Object>> inProcessDropDown(int organizationId, int companyId, int financialYearId,
                                                        String activity, String documentTypeIds) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", organizationId);
        p.put("CompanyId", companyId);
        p.put("FinancialYearId", financialYearId);
        p.put("Activity", activity == null || activity.isEmpty() ? null : activity);
        p.put("DocumentTypeIds", documentTypeIds == null || documentTypeIds.isEmpty() ? null : documentTypeIds);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromLabInProcessAnalysisRawHeader]", p);
    }

    /**
     * InvLabAnalysisInProcessHeader.DateFill (0401:268) -> USp_InvLabAnalysisInProcessHeader_GetAllMethod,
     * @Activity = 'GetDocDateFromInProcessAnalysis'. BLL order: @OrganizationId, @CompanyId, @ItemId (!= 0),
     * @PlantId (!= 0), @JobOrderId (!= 0), @Activity. Rows: DocDate.
     */
    public List<Map<String, Object>> inProcessDates(int organizationId, int companyId, Integer itemId, Integer plantId, Integer jobOrderId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", organizationId);
        p.put("CompanyId", companyId);
        p.put("ItemId", itemId);
        p.put("PlantId", plantId);
        p.put("JobOrderId", jobOrderId);
        p.put("Activity", "GetDocDateFromInProcessAnalysis");
        return DesktopProc.rows(jdbc, "USp_InvLabAnalysisInProcessHeader_GetAllMethod", p);
    }

    /** InvLabAnalysisInProcessHeader.InProcessStepAnalysisRegister (0401:237) -> USP_InProcessAnalysisRegister. */
    public Table inProcessStepRegister(int organizationId, int companyId, Integer plantId, Integer jobOrderId, Integer itemId, Timestamp docDate) {
        return table("USP_InProcessAnalysisRegister", inProcessParams(organizationId, companyId, plantId, jobOrderId, itemId, docDate));
    }

    /** InvLabAnalysisInProcessHeader.InProcessAnalysisRecovery (0401:206) -> USP_InProcessAnalysisRecovery_Register. */
    public Table inProcessRecovery(int organizationId, int companyId, Integer plantId, Integer jobOrderId, Integer itemId, Timestamp docDate) {
        return table("USP_InProcessAnalysisRecovery_Register", inProcessParams(organizationId, companyId, plantId, jobOrderId, itemId, docDate));
    }

    /** Both BLL methods: @OrganizationId, @CompanyId, @PlantId (!= 0), @JobOrderId (!= 0), @ItemId (!= 0), @DocDate (real date). */
    private static Map<String, Object> inProcessParams(int organizationId, int companyId, Integer plantId, Integer jobOrderId, Integer itemId, Timestamp docDate) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", organizationId);
        p.put("CompanyId", companyId);
        p.put("PlantId", plantId);
        p.put("JobOrderId", jobOrderId);
        p.put("ItemId", itemId);
        p.put("DocDate", docDate);
        return p;
    }

    // ===================================================================== plumbing

    /**
     * GenericProvider.GetDataTableProc: the first result set with its columns in order. The register
     * procedures run SELECT INTO / dynamic UPDATE statements before their final SELECT, so update
     * counts are skipped until a result set arrives; later results are drained so a late error surfaces.
     */
    private Table table(String proc, Map<String, Object> params) {
        List<Object> args = new ArrayList<>();
        StringBuilder sql = new StringBuilder("EXEC ").append(proc.startsWith("[") ? proc : "dbo." + proc);
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
