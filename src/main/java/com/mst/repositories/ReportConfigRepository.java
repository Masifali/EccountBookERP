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
 * Admin Panel -> "Define Reports" (Architecture.WinApp.Configurations.frmReportConfig).
 *
 * ---------------------------------------------------------------------------------------------
 * THE DESKTOP CHAIN
 * ---------------------------------------------------------------------------------------------
 *   GridFill()      BLL 0079 ReportConfig.GetAll(new ReportConfig())
 *                     -> Sp_ReportConfig_GetAllMethod @Activity='ReadAll'
 *   RetrivedData(Id) BLL 0079 ReportConfig.GetByID(Id)
 *                     -> Sp_ReportConfig_GetAllMethod @Id, @Activity='ReadById'
 *   Insert()        BLL 0079 ReportConfig.Save(obj)
 *                     -> ReportConfigId == 0 ? DAL SetData(obj,"Sp_ReportConfig_Insert")
 *                                            : DAL SetData(obj,"Sp_ReportConfig_Update")
 *
 * DAL 0080 SetData: own connection, BeginTransaction, SetProc, rollback on any exception. The
 * Insert ends with SELECT @ReportConfigId; the Update selects nothing, so the scalar is null,
 * Convert.ToInt32(null) is 0 and the existing id is kept. Both branches reproduced.
 *
 * ---------------------------------------------------------------------------------------------
 * PARAMETERS - Architecture.Model.ReportConfig, declaration order
 * ---------------------------------------------------------------------------------------------
 * The model declares seventeen properties and NONE of them is virtual, so SetProc sends all
 * seventeen. Both procedures declare all seventeen, each with a default - checked against the
 * schema, nothing unaccepted.
 *
 * Two things the procedures do that the form does not show, reproduced as found:
 *   Sp_ReportConfig_Insert  overwrites @ReportConfigId AND @ReportSeqNo with MAX(..)+1, so what
 *                           the client sends for either is irrelevant on an insert.
 *   Sp_ReportConfig_Update  assigns every column including ReportSeqNo, ReportIconURL,
 *                           TargetSource, TargetFunctionName, CreatedById and CreatedOn - none of
 *                           which this form has an input for. See the service.
 */
@Repository
public class ReportConfigRepository {

    private static final String P_GET    = "Sp_ReportConfig_GetAllMethod";
    private static final String P_INSERT = "Sp_ReportConfig_Insert";
    private static final String P_UPDATE = "Sp_ReportConfig_Update";

    /** Architecture.Model.ReportConfig, declaration order. All seventeen are non-virtual. */
    private static final String[] PARAMS = {
            "ActionTypeId", "IsActive", "IsSubReport", "AlteredOn", "CreatedOn",
            "ReportConfigId", "ReportSeqNo", "AlteredById", "CreatedById", "ReportFileName",
            "ReportFolder", "ReportIconURL", "ReportProcedureName", "ReportShortName",
            "ReportTitle", "TargetFunctionName", "TargetSource"
    };

    private final JdbcTemplate jdbc;
    public ReportConfigRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ================================================================================== read

    /** GridFill(). The procedure takes no organization filter on this activity - ReportConfig is
     *  a global definition table on the desktop, and adding a filter here would hide rows the
     *  desktop shows. */
    public List<Map<String, Object>> all() {
        return jdbc.queryForList("EXEC dbo." + P_GET + " @Activity=?", "ReadAll");
    }

    /** RetrivedData(Id). */
    public Map<String, Object> byId(int id) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_GET + " @Id=?, @Activity=?", id, "ReadById");
        return rows.isEmpty() ? null : rows.get(0);
    }

    // ================================================================================== write

    @Transactional(propagation = Propagation.REQUIRED)
    public int save(Map<String, Object> model) {
        int id = intOf(model.get("ReportConfigId"));
        Integer returned = exec(id == 0 ? P_INSERT : P_UPDATE, model);
        return (returned != null && returned > 0) ? returned : id;
    }

    private Integer exec(String proc, Map<String, Object> model) {
        StringBuilder sql = new StringBuilder("EXEC dbo.").append(proc).append(' ');
        List<Object> args = new ArrayList<>();
        for (int i = 0; i < PARAMS.length; i++) {
            if (i > 0) sql.append(", ");
            String name = PARAMS[i];
            sql.append('@').append(name).append("=?");
            args.add(typed(name, model.get(name)));
        }
        List<Map<String, Object>> rows = jdbc.queryForList(sql.toString(), args.toArray());
        if (rows.isEmpty()) return null;
        for (Object v : rows.get(0).values()) {
            if (v instanceof Number) return ((Number) v).intValue();
        }
        return null;
    }

    /** Typed nulls, so a null never reaches the driver as an INTEGER and clashes. */
    private static Object typed(String name, Object value) {
        if (value != null) return value;
        switch (name) {
            case "AlteredOn":
            case "CreatedOn":
                return new SqlParameterValue(Types.TIMESTAMP, null);
            case "IsActive":
            case "IsSubReport":
                return new SqlParameterValue(Types.BIT, null);
            case "ActionTypeId":
            case "ReportConfigId":
            case "ReportSeqNo":
            case "AlteredById":
            case "CreatedById":
                return new SqlParameterValue(Types.INTEGER, null);
            default:
                return new SqlParameterValue(Types.VARCHAR, null);
        }
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
