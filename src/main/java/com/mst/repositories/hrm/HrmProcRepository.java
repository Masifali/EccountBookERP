package com.mst.repositories.hrm;

import com.mst.models.hrm.DesktopModel;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Supplier;

/**
 * The HRM DAL plumbing: the desktop's two GenericProvider calls over the desktop's own
 * procedures, nothing else. Every HRM repository (package com.mst.repositories.hrm) delegates here.
 *
 *  rows(proc, "Activity", "ReadAll", "OrganizationId", 1, ...)  GetDataTableProc / GetProc
 *  set(proc, model)                                             SetProc(model) -> Convert.ToInt32(ExecuteScalar)
 *  set(proc, model, "Activity")                                 SetProc(model, proc, Activity)
 *  tx(() -> { ... several set() calls ... })                    one SqlTransaction, rolled back on any exception
 *
 * Keys are bare (no '@'); DesktopProc prefixes it. A null value is omitted (ADO.NET AddWithValue(null)).
 * No table, column or procedure is created or changed.
 */
@Repository
public class HrmProcRepository {

    private final JdbcTemplate jdbc;

    public HrmProcRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** GetDataTableProc: first result set, case-insensitive keys. */
    public List<Map<String, Object>> rows(String proc, Object... kv) {
        return DesktopProc.rows(jdbc, proc, bare(DesktopProc.params(kv)));
    }

    public List<Map<String, Object>> rows(String proc, Map<String, Object> params) {
        return DesktopProc.rows(jdbc, proc, bare(params));
    }

    /** GenericProvider.SetProc(sqlTrn, obj, ProcName): every property of the model, Convert.ToInt32(ExecuteScalar). */
    @Transactional(rollbackFor = Exception.class)
    public int set(String proc, DesktopModel model) {
        return DesktopProc.setProc(jdbc, proc, model.toParams());
    }

    /** SetProc with an Activity (GenericProvider.SetProc(sqlTrn, obj, ProcName, Activity)). */
    @Transactional(rollbackFor = Exception.class)
    public int set(String proc, DesktopModel model, String activity) {
        Map<String, Object> p = model.toParams();
        if (activity != null && !activity.isEmpty()) p.put("Activity", activity);
        return DesktopProc.setProc(jdbc, proc, p);
    }

    /** SetProc from an explicit map (for DALs that build SqlParameter lists by hand). */
    @Transactional(rollbackFor = Exception.class)
    public int set(String proc, Map<String, Object> params) {
        return DesktopProc.setProc(jdbc, proc, bare(params));
    }

    /** ExecuteScalar read leniently (null when no row). */
    @Transactional(rollbackFor = Exception.class)
    public Integer scalar(String proc, Map<String, Object> params) {
        return DesktopProc.scalar(jdbc, proc, bare(params));
    }

    /** One transaction around several calls - the DAL's BeginTransaction ... Commit / Rollback. */
    @Transactional(rollbackFor = Exception.class)
    public <T> T tx(Supplier<T> work) {
        return work.get();
    }

    public JdbcTemplate jdbc() { return jdbc; }

    private static Map<String, Object> bare(Map<String, Object> in) {
        Map<String, Object> out = new LinkedHashMap<>();
        if (in == null) return out;
        for (Map.Entry<String, Object> e : in.entrySet()) {
            String k = e.getKey();
            out.put(k.startsWith("@") ? k.substring(1) : k, e.getValue());
        }
        return out;
    }
}
