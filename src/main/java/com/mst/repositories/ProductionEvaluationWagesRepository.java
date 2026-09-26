package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.sql.Types;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * "Wages Register" - Architecture.WinApp.Inventory_Reports/frmEvaulationDetailWagesReports.cs (2,351 lines).
 * Opened from frmProductionSettlement.grdOverHeadSettlement_LinkClicked:972 (document types 80/112).
 *
 * Every read the form performs, traced form line -> BLL -> procedure, BLL guards reproduced
 * literally (a guarded parameter is omitted, never sent as NULL or 0). Every name sent is declared
 * by its procedure (checked against procdure.utf8.sql).
 */
@Repository
public class ProductionEvaluationWagesRepository {

    public static final String DESKTOP_SCREEN_NAME = "frmEvaulationDetailWagesReports";

    @Autowired private JdbcTemplate jdbc;

    /* ============================================================================== switches */

    /** CommonServices.GetERPFeatureById(id) - clsGlobalVariables.ErpFeaturesList is USP_GetERPFeaturesByCompanyId. */
    public boolean erpFeature(int organizationId, int companyId, int featureId) {
        for (Map<String, Object> r : rows("dbo.USP_GetERPFeaturesByCompanyId",
                p("@OrganizationId", organizationId, "@CompanyId", companyId))) {
            if (toInt(col(r, "Id")) == featureId) return true;
        }
        return false;
    }

    /** GetConfigurationByOrgCompandConfigDescription (GetDecimalConfiguration's source). */
    public String configValue(int organizationId, int companyId, String configDescription) {
        List<Map<String, Object>> r = rows("dbo.Sp_ConfigrationsAllocation_GetAllMethod",
                p("@OrganizationId", organizationId, "@CompanyId", companyId,
                  "@ConfigDescription", configDescription,
                  "@Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = col(r.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** clsGlobalVariables.ActiveYr - the active-year rows (Start_Period is read by the service). */
    public List<Map<String, Object>> activeFinancialYears(int organizationId, int companyId) {
        return rows("dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                p("@OrganizationId", organizationId, "@CompanyId", companyId));
    }

    /**
     * UserAccount.BranchName (filled at login on the desktop; the Java session carries only the id):
     * Sp_Branches_GetAllMethod @Id, @Activity='GetById'.
     */
    public String branchName(int branchId) {
        List<Map<String, Object>> r = rows("dbo.Sp_Branches_GetAllMethod", p("@Id", branchId, "@Activity", "GetById"));
        if (r.isEmpty()) return "";
        Object v = col(r.get(0), "BranchName");
        return v == null ? "" : String.valueOf(v);
    }

    /* =============================================================================== pickers */

    /**
     * BranchesFill:256 -> BranchesAllocationToUser.GetBranchsAllocatedToUserFromWages(Org, Company, UserId, 0)
     * -> [dbo].[USP_GetBranchsAllocatedToUserFromWages] @OrganizationId, @CompanyId, @UserId;
     * @DocumentTypeId only when != 0 (0 here - omitted).
     */
    public List<Map<String, Object>> branchesAllocatedFromWages(int organizationId, int companyId, int userId) {
        return rows("[dbo].[USP_GetBranchsAllocatedToUserFromWages]",
                p("@OrganizationId", organizationId, "@CompanyId", companyId, "@UserId", userId));
    }

    /**
     * ComboFill:315 -> InvContractorWagesBillHeader.ComboAgainstContractorWages -> Usp_AllComboAgainstContractorWages:
     * @OrganizationId, @CompanyId always; @Activity, @DocumentTypeIds (never set here) and
     * @BranchesIds only when not null and not "".
     */
    public List<Map<String, Object>> comboAgainstContractorWages(int organizationId, int companyId, String branchesIds) {
        LinkedHashMap<String, Object> m = p("@OrganizationId", organizationId, "@CompanyId", companyId);
        if (branchesIds != null && !branchesIds.isEmpty()) m.put("@BranchesIds", branchesIds);
        return rows("dbo.Usp_AllComboAgainstContractorWages", m);
    }

    /**
     * JobOrderFill:383 -> InvContractorWagesBillHeader.JobOrderNoFromContractorWages
     * -> Usp_JobOrderNoFromContractorWages: @OrganizationId, @CompanyId; @BranchesIds when not null/"".
     */
    public List<Map<String, Object>> jobOrderNoFromContractorWages(int organizationId, int companyId, String branchesIds) {
        LinkedHashMap<String, Object> m = p("@OrganizationId", organizationId, "@CompanyId", companyId);
        if (branchesIds != null && !branchesIds.isEmpty()) m.put("@BranchesIds", branchesIds);
        return rows("dbo.Usp_JobOrderNoFromContractorWages", m);
    }

    /**
     * GridFill:548 -> InventoryStockEvalautionDetail.WagesRegister -> USp_WagesRegister.
     * The service builds the parameter list with the BLL's guards; this only executes it.
     */
    public List<Map<String, Object>> wagesRegister(LinkedHashMap<String, Object> params) {
        return rows("dbo.USp_WagesRegister", params);
    }

    /* ============================================================================== plumbing */

    private List<Map<String, Object>> rows(String proc, LinkedHashMap<String, Object> params) {
        return jdbc.execute((ConnectionCallback<List<Map<String, Object>>>) con -> {
            List<Map<String, Object>> out = new ArrayList<>();
            run(con, proc, params, out);
            return out;
        });
    }

    private static void run(Connection con, String proc, LinkedHashMap<String, Object> params,
                            List<Map<String, Object>> firstResult) throws SQLException {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;            /* .NET null: not supplied */
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        try (PreparedStatement ps = con.prepareStatement(sql.toString())) {
            for (int i = 0; i < values.size(); i++) bind(ps, i + 1, values.get(i));
            boolean isResult = ps.execute();
            boolean captured = false;
            while (true) {
                if (isResult) {
                    try (ResultSet rs = ps.getResultSet()) {
                        ResultSetMetaData md = rs.getMetaData();
                        int n = md.getColumnCount();
                        while (rs.next()) {
                            if (captured) continue;
                            Map<String, Object> row = new LinkedHashMap<>();
                            for (int c = 1; c <= n; c++) row.put(md.getColumnLabel(c), plain(rs.getObject(c)));
                            firstResult.add(row);
                        }
                    }
                    captured = true;
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isResult = ps.getMoreResults();
            }
        }
    }

    private static void bind(PreparedStatement ps, int i, Object v) throws SQLException {
        if (v instanceof Integer) ps.setInt(i, (Integer) v);
        else if (v instanceof Double) ps.setDouble(i, (Double) v);
        else if (v instanceof Boolean) ps.setBoolean(i, (Boolean) v);
        else if (v instanceof Timestamp) ps.setTimestamp(i, (Timestamp) v);
        else if (v instanceof String) ps.setNString(i, (String) v);
        else if (v instanceof Long) ps.setLong(i, (Long) v);
        else if (v instanceof Number) ps.setDouble(i, ((Number) v).doubleValue());
        else if (v == null) ps.setNull(i, Types.NULL);
        else ps.setObject(i, v);
    }

    private static Object plain(Object o) {
        if (o instanceof Timestamp) return ((Timestamp) o).toLocalDateTime().toString();
        if (o instanceof java.sql.Date) return ((java.sql.Date) o).toLocalDate().toString();
        if (o instanceof BigDecimal) return ((BigDecimal) o).doubleValue();
        return o;
    }

    public static LinkedHashMap<String, Object> p(Object... kv) {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put((String) kv[i], kv[i + 1]);
        return m;
    }

    public static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        Object v = row.get(name);
        if (v != null || row.containsKey(name)) return v;
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    public static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Integer) return (Integer) o;
        if (o instanceof Number) return (int) Math.rint(((Number) o).doubleValue());
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        String s = String.valueOf(o).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) { return 0; }
    }
}
