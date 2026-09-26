package com.mst.repositories.support;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;

/**
 * The desktop's two procedure calls, reproduced with the desktop's null semantics.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT A NULL MEANS ON THE DESKTOP
 * ---------------------------------------------------------------------------------------------
 * GenericProvider.SetProc and GetDataTableProc both use {@code SqlCommand.Parameters.AddWithValue}.
 * When the value handed to AddWithValue is a CLR {@code null} (an unset string, an unset
 * {@code DateTime?}), ADO.NET does NOT send a SQL NULL - it leaves the parameter out, and the
 * procedure uses its declared default. Only {@code DBNull.Value} sends NULL, and the desktop never
 * passes that through these helpers.
 *
 * So a null here is OMITTED from the EXEC, never bound as NULL. Binding NULL instead would change
 * what a column defaulting to '' or GETDATE() receives.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT COMES BACK
 * ---------------------------------------------------------------------------------------------
 * {@link #scalar} is ExecuteScalar: the first column of the first row of the first result set,
 * with every later result drained so a RAISERROR raised after the SELECT still surfaces as an
 * exception (and rolls the caller's transaction back) instead of being left unread.
 *
 * {@link #rows} is a DataTable fill: the first result set, or an empty list when the procedure
 * returned none. Keys are case-insensitive, as DataRow column lookups are.
 */
public final class DesktopProc {

    private DesktopProc() { }

    /**
     * ExecuteScalar with its result discarded or read leniently — the first column of the first row
     * of the FIRST result set (null when there is none). Every later result is still drained, so a
     * RAISERROR after the first SELECT surfaces as an exception.
     */
    public static Integer scalar(JdbcTemplate jdbc, String proc, Map<String, Object> params) {
        Object[] first = firstCell(jdbc, proc, params);
        return first == null ? null : asInt(first[0]);
    }

    /**
     * GenericProvider.SetProc: {@code Convert.ToInt32(cmd.ExecuteScalar())}. No row at all is 0
     * (Convert.ToInt32(null)); a NULL first cell throws, exactly as Convert.ToInt32(DBNull.Value)
     * does on the desktop, so a save can never continue on a header id nobody returned.
     */
    public static int setProc(JdbcTemplate jdbc, String proc, Map<String, Object> params) {
        Object[] first = firstCell(jdbc, proc, params);
        if (first == null) return 0;
        if (first[0] == null) throw new IllegalStateException("Object cannot be cast from DBNull to other types. (" + proc + ")");
        Integer v = asInt(first[0]);
        if (v == null) throw new IllegalStateException("Input string was not in a correct format. (" + proc + ")");
        return v;
    }

    /** {cell} of the first row of the first result set; null when that result set has no row. */
    private static Object[] firstCell(JdbcTemplate jdbc, String proc, Map<String, Object> params) {
        List<Object> args = new ArrayList<>();
        String sql = sql(proc, params, args);
        return jdbc.execute(sql, (PreparedStatementCallback<Object[]>) ps -> {
            bind(ps, args);
            Object[] first = null;
            boolean seenResultSet = false;
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        boolean firstSet = !seenResultSet;
                        seenResultSet = true;
                        while (rs.next()) {
                            if (firstSet && first == null) first = new Object[] { rs.getObject(1) };
                        }
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isRs = ps.getMoreResults();
            }
            return first;
        });
    }

    /** First result set as case-insensitive rows; empty when the procedure returned none. */
    public static List<Map<String, Object>> rows(JdbcTemplate jdbc, String proc, Map<String, Object> params) {
        List<Object> args = new ArrayList<>();
        String sql = sql(proc, params, args);
        return jdbc.execute(sql, (PreparedStatementCallback<List<Map<String, Object>>>) ps -> {
            bind(ps, args);
            List<Map<String, Object>> out = new ArrayList<>();
            boolean isRs = ps.execute();
            boolean taken = false;
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        if (!taken) {
                            ResultSetMetaData md = rs.getMetaData();
                            int n = md.getColumnCount();
                            while (rs.next()) {
                                Map<String, Object> row = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
                                for (int c = 1; c <= n; c++) {
                                    String label = md.getColumnLabel(c);
                                    if (label == null || label.isEmpty()) label = "Column" + c;
                                    if (!row.containsKey(label)) row.put(label, rs.getObject(c));
                                }
                                out.add(row);
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
            return out;
        });
    }

    /** Convenience for building an ordered parameter map. */
    public static Map<String, Object> params(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    // ------------------------------------------------------------------------------ plumbing

    private static String sql(String proc, Map<String, Object> params, List<Object> args) {
        StringBuilder sb = new StringBuilder("EXEC ");
        sb.append(proc.startsWith("[") ? proc : "dbo." + proc);
        boolean first = true;
        if (params != null) {
            for (Map.Entry<String, Object> e : params.entrySet()) {
                if (e.getValue() == null) continue;           // ADO.NET: null => not sent
                sb.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
                args.add(e.getValue());
                first = false;
            }
        }
        return sb.toString();
    }

    private static void bind(PreparedStatement ps, List<Object> args) throws java.sql.SQLException {
        for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
    }

    private static Integer asInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return null;
        try { return Integer.valueOf(String.valueOf(o).trim()); }
        catch (NumberFormatException e) { return null; }
    }
}
