package com.mst.repositories;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Types;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * "Report Allocate to Company" (Architecture.WinApp.Configurations.frmCompanyReport).
 *
 * Opened from Define Reports' toolStrip1 button ReportAllocatToCompany, whose desktop handler is
 * new frmCompanyReport(UserAccount).Show().
 *
 * ---------------------------------------------------------------------------------------------
 * THE DESKTOP CHAIN
 * ---------------------------------------------------------------------------------------------
 *   frmCompanyReport_Load  -> grdFill(), CompanyFill(), ReportHeaderFill()
 *   grdFill()              BLL 0079 ReportConfig.GetAll(new ReportConfig())
 *                            -> Sp_ReportConfig_GetAllMethod @Activity='ReadAll'
 *                          only six of its columns are copied into the grid's own DataTable
 *   CompanyFill()          BLL 0062 Company.GetAllCompanies()
 *                            -> Sp_Company_GetAllMethod @Activity='GetAllCompaniesforCombo'
 *   ReportHeaderFill()     BLL 0063 CompanyReport.GetAllReportHeader()
 *                            -> Sp_ReportHeader_GetAllMethod @Activity='ReadAll'
 *   History / RetrivedData BLL 0063 CompanyReport.GetAll / GetByID
 *                            -> Sp_CompanyReport_GetAllMethod @Activity='ReadAll' | @Id,'ReadById'
 *   Insert()               BLL 0063 CompanyReport.Save(obj)
 *                            -> CompanyReportId == 0 ? DAL SetData(obj,"Sp_CompanyReport_Insert")
 *                                                    : DAL SetData(obj,"Sp_CompanyReport_Update")
 *
 * ---------------------------------------------------------------------------------------------
 * PARAMETERS - Architecture.Model.CompanyReport, declaration order
 * ---------------------------------------------------------------------------------------------
 * The model declares exactly FOUR properties and none is virtual, so SetProc sends all four:
 *
 *     CompanyId, CompanyReportId, ReportConfigId, ReportHeaderId
 *
 * There are no audit columns on this model at all - no CreatedById, no CreatedOn, no
 * ActionTypeId. Nothing is invented to fill that gap; the desktop sends four parameters and so
 * does this.
 *
 * NOT VERIFIED THIS SESSION: the four procedure signatures could not be checked against
 * GoldenAceDb(0509)t.sql. The device's Linux workspace would not start and the dump is 443 MB,
 * over the staging limit. Every call below therefore surfaces its SQL error rather than
 * swallowing it, so a missing procedure or an unaccepted parameter reports itself on first use.
 */
@Repository
public class CompanyReportRepository {

    private static final String P_GET        = "Sp_CompanyReport_GetAllMethod";
    private static final String P_INSERT     = "Sp_CompanyReport_Insert";
    private static final String P_UPDATE     = "Sp_CompanyReport_Update";
    private static final String P_HEADER     = "Sp_ReportHeader_GetAllMethod";
    private static final String P_COMPANY    = "Sp_Company_GetAllMethod";
    private static final String P_REPORTCFG  = "Sp_ReportConfig_GetAllMethod";

    /** Architecture.Model.CompanyReport, declaration order. All four are non-virtual. */
    private static final String[] PARAMS = {
            "CompanyId", "CompanyReportId", "ReportConfigId", "ReportHeaderId"
    };

    private final JdbcTemplate jdbc;
    public CompanyReportRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ================================================================================== read

    /**
     * grdFill(). The desktop calls the SAME procedure Define Reports uses, then copies only six
     * columns into the grid's DataTable - ReportConfigId, ReportTitle, ReportShortName,
     * ReportFolder, ReportFileName, ReportProcedureName. The other eleven are read and dropped.
     * That projection is done here so the page receives what the desktop grid shows.
     */
    public List<Map<String, Object>> reportsForSelection() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbc.queryForList("EXEC dbo." + P_REPORTCFG + " @Activity=?", "ReadAll")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ReportConfigId",      ci(r, "ReportConfigId"));
            m.put("ReportTitle",         ci(r, "ReportTitle"));
            m.put("ReportShortName",     ci(r, "ReportShortName"));
            m.put("ReportFolder",        ci(r, "ReportFolder"));
            m.put("ReportFileName",      ci(r, "ReportFileName"));
            m.put("ReportProcedureName", ci(r, "ReportProcedureName"));
            out.add(m);
        }
        return out;
    }

    /** CompanyFill(). BindDDLNew(dtCompany, cmbCompany, "Id", "CompName", ...). */
    public List<Map<String, Object>> companies() {
        return jdbc.queryForList("EXEC dbo." + P_COMPANY + " @Activity=?", "GetAllCompaniesforCombo");
    }

    /** ReportHeaderFill(). BindDDLNew(dtHeader, cmbReportHeader, "ReportHeaderId", "HeaderPrefix", ...). */
    public List<Map<String, Object>> reportHeaders() {
        return jdbc.queryForList("EXEC dbo." + P_HEADER + " @Activity=?", "ReadAll");
    }

    /** CompanyReport.GetAll - the History grid. */
    public List<Map<String, Object>> all() {
        return jdbc.queryForList("EXEC dbo." + P_GET + " @Activity=?", "ReadAll");
    }

    /** CompanyReport.GetByID(Id). The desktop indexes [0] and would throw on an empty result. */
    public Map<String, Object> byId(int id) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_GET + " @Id=?, @Activity=?", id, "ReadById");
        return rows.isEmpty() ? null : rows.get(0);
    }

    // ================================================================================== write

    /** One call per checked grid row, exactly as Insert()'s foreach does. */
    @Transactional(propagation = Propagation.REQUIRED)
    public int save(Map<String, Object> model) {
        int id = intOf(model.get("CompanyReportId"));
        Integer returned = exec(id == 0 ? P_INSERT : P_UPDATE, model);
        return (returned != null && returned > 0) ? returned : id;
    }

    private Integer exec(String proc, Map<String, Object> model) {
        StringBuilder sql = new StringBuilder("EXEC dbo.").append(proc).append(' ');
        List<Object> args = new ArrayList<>();
        for (int i = 0; i < PARAMS.length; i++) {
            if (i > 0) sql.append(", ");
            sql.append('@').append(PARAMS[i]).append("=?");
            args.add(typed(model.get(PARAMS[i])));
        }
        List<Map<String, Object>> rows = jdbc.queryForList(sql.toString(), args.toArray());
        if (rows.isEmpty()) return null;
        for (Object v : rows.get(0).values()) {
            if (v instanceof Number) return ((Number) v).intValue();
        }
        return null;
    }

    /** All four columns are int, so a null goes down typed rather than as an untyped NULL. */
    private static Object typed(Object value) {
        return value != null ? value : new SqlParameterValue(Types.INTEGER, null);
    }

    private static Object ci(Map<String, Object> row, String key) {
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        }
        return null;
    }

    public static int intOf(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    public static Map<String, Object> blankModel() {
        Map<String, Object> m = new LinkedHashMap<>();
        for (String p : PARAMS) m.put(p, null);
        return m;
    }
}
