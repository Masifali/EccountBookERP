package com.mst.reports;

import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Runs a report's traced procedure and returns its rows — the Java equivalent of the desktop's
 * "get the DataTable, then hand it to Crystal".
 *
 * The desktop pushes data INTO the .rpt (Reporting.ShowReportWithDataTable / ...SubReprt), it
 * does not let the template open its own connection. So the rows are the portable half of a
 * report and this service produces them; how they are rendered is a separate decision.
 *
 * TENANCY: @OrganizationId and @CompanyId come from the session, never from the request.
 * GUARDED parameters are omitted when unset, exactly as the BLL omits them.
 */
@Service
public class ReportDataService {

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private ReportRegistry registry;

    public Map<String, Object> run(String key, Map<String, Object> args) {
        ReportDefinition def = registry.get(key);
        if (def == null) {
            throw new IllegalArgumentException("Unknown report '" + key + "'. "
                    + "Registered: " + registry.all().size()
                    + "; unavailable and why: " + ReportRegistry.UNAVAILABLE.keySet());
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("key", def.key);
        out.put("template", def.template);
        out.put("procedure", def.procedure);
        out.put("desktopCaller", def.desktopCaller);
        Map<String, Object> reportParams = resolveReportParams();
        /* "rpt:@Name" entries are template parameters the desktop pushes with RptPerameter beyond
           the company pair (812's @PrintedBy, for one). They are resolved like any other source
           but never sent to the procedure, and only a definition that names one gets it - the
           bridge's SetParameterValue fails on a parameter the .rpt does not declare. */
        Map<String, Object> defaults = new LinkedHashMap<>(reportParams);
        for (ReportDefinition.Param p : def.params) {
            if (p.name.startsWith(RPT_PARAM)) {
                /* "same:@CompanyName" re-uses a default value under another name - 604 pushes
                   "CompanyName"/"CompanyAddress" WITHOUT the @ (FoodProductionComparisonRpt:1170). */
                Object v = p.source.startsWith("same:") ? defaults.get(p.source.substring(5))
                                                        : resolve(p.source, args);
                reportParams.put(p.name.substring(RPT_PARAM.length()), v == null ? "" : v);
            }
        }
        /* A definition that renames the company pair does not also get the @-named defaults: the
           bridge's SetParameterValue fails on a name the template does not declare. */
        if (reportParams.containsKey("CompanyName"))    reportParams.remove("@CompanyName");
        if (reportParams.containsKey("CompanyAddress")) reportParams.remove("@CompanyAddress");
        out.put("reportParameters", reportParams);
        List<Map<String, Object>> mainRows = exec(def.procedure, def.params, args);
        out.put("rows", mainRows);

        if (!def.subReports.isEmpty()) {
            /* "rows:Column" - a sub-report parameter the desktop builds from the main DataTable:
               string.Join(",", rows.Select(r => r[Column].ToString()).Distinct()). */
            Map<String, Object> subArgs = new LinkedHashMap<>();
            if (args != null) subArgs.putAll(args);
            subArgs.put(MAIN_ROWS, mainRows);
            List<Map<String, Object>> subs = new ArrayList<>();
            for (ReportDefinition.SubReport sr : def.subReports) {
                Map<String, Object> s = new LinkedHashMap<>();
                s.put("template", sr.template);
                s.put("procedure", sr.procedure);
                s.put("rows", exec(sr.procedure, sr.params, subArgs));
                subs.add(s);
            }
            out.put("subReports", subs);
        }
        return out;
    }

    /**
     * Parameters the desktop pushes into the template itself, not into the procedure:
     *   val.RptPerameter("@CompanyName",    UserAccount.CompName)
     *   val.RptPerameter("@CompanyAddress", UserAccount.CompAddress)
     *
     * The desktop fills those onto UserAccount at login (LoginNew.cs:224). There is no such
     * field on the Java session object, so they are read from the company row itself —
     * Sp_Company_GetAllMethod @Activity='ReadById', columns CompName and CompAddress, which are
     * the real columns of Architecture.Model.Company. Not invented, not hard-coded.
     */
    private Map<String, Object> resolveReportParams() {
        Map<String, Object> m = new LinkedHashMap<>();
        String name = "", address = "";
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC Sp_Company_GetAllMethod @Id=?, @Activity=?",
                    currentUserContext.currentCompanyId(), "ReadById");
            if (!rows.isEmpty()) {
                name    = str(ci(rows.get(0), "CompName"));
                address = str(ci(rows.get(0), "CompAddress"));
            }
        } catch (Exception e) {
            /* A missing header is a cosmetic gap; it must not fail the report's data. */
            LOG.warn("Company header for the report could not be read", e);
        }
        m.put("@CompanyName", name);
        m.put("@CompanyAddress", address);
        return m;
    }

    /** Prefix of a definition parameter that is a template parameter, not a procedure one. */
    static final String RPT_PARAM = "rpt:";

    /** Key under which the main result travels to sub-report parameters; not a request argument. */
    private static final String MAIN_ROWS = "\u0000mainRows";

    private static final org.slf4j.Logger LOG =
            org.slf4j.LoggerFactory.getLogger(ReportDataService.class);

    private static Object ci(Map<String, Object> row, String key) {
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }
    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private List<Map<String, Object>> exec(String procedure,
                                           List<ReportDefinition.Param> params,
                                           Map<String, Object> args) {
        List<String> names = new ArrayList<>();
        List<Object> values = new ArrayList<>();

        for (ReportDefinition.Param p : params) {
            if (p.name.startsWith(RPT_PARAM)) continue;
            Object v = resolve(p.source, args);
            if (p.mode == ReportDefinition.Mode.GUARDED && isUnset(v)) continue;  // omit, never NULL
            names.add(p.name);
            values.add(v);
        }

        StringBuilder sql = new StringBuilder("EXEC ").append(procedure).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) sql.append(", ");
            sql.append(names.get(i)).append("=?");
        }
        return jdbcTemplate.queryForList(sql.toString(), values.toArray());
    }

    @SuppressWarnings("unchecked")
    private Object resolve(String source, Map<String, Object> args) {
        int i = source.indexOf(':');
        String kind = source.substring(0, i), name = source.substring(i + 1);
        switch (kind) {
            case "session":
                switch (name) {
                    case "organizationId": return currentUserContext.currentOrganizationId();
                    case "companyId":      return currentUserContext.currentCompanyId();
                    case "branchId":       return currentUserContext.currentBranchId();
                    case "financialYearId":return currentUserContext.currentFinancialYearId();
                    case "userId":         return currentUserContext.currentUserId();
                    case "userName":       return currentUserContext.requireAccountingUser().getUserName();
                    default: throw new IllegalStateException("unknown session value " + name);
                }
            case "const": return name;
            case "rows": {
                Object rows = args == null ? null : args.get(MAIN_ROWS);
                if (!(rows instanceof List)) return "";
                java.util.LinkedHashSet<String> distinct = new java.util.LinkedHashSet<>();
                for (Object r : (List<?>) rows) {
                    Object v = r instanceof Map ? ci((Map<String, Object>) r, name) : null;
                    distinct.add(v == null ? "" : String.valueOf(v));
                }
                return String.join(",", distinct);
            }
            case "arg":   return args == null ? null : args.get(name);
            default: throw new IllegalStateException("unknown source " + source);
        }
    }

    /** The BLL's own guards: 0 for ids/numbers, empty for strings, null for dates. */
    private boolean isUnset(Object v) {
        if (v == null) return true;
        if (v instanceof Number) return ((Number) v).doubleValue() == 0d;
        String s = String.valueOf(v).trim();
        return s.isEmpty() || "0".equals(s);
    }
}
