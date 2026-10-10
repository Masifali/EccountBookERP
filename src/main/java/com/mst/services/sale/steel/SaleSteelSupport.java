package com.mst.services.sale.steel;

import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.*;

/**
 * Plumbing shared by the Sale Steel screens (module 84 / 87, Architecture.WinApp.Steel.Sale / Steel.Reports.SalesReports).
 *
 * The tenancy / rights / configuration helpers are the ones of the Sale Engr screens (SaleEngrSupport); this class adds the one
 * thing those do not have: a procedure call that keeps the ORDER of the result columns, because the Janus grids of the Steel
 * screens are filled with grid.RetrieveStructure() and show the columns in the order the procedure selects them.
 * Null parameters are left out of the EXEC (ADO.NET AddWithValue(null)), exactly like DesktopProc.
 */
@Component
public class SaleSteelSupport {

    /** Result of a DataTable fill: column names in select order and the rows (insertion-ordered maps, dates as ISO text). */
    public static final class Table {
        public final List<String> cols = new ArrayList<>();
        public final List<Map<String, Object>> rows = new ArrayList<>();
    }

    private final SaleEngrSupport sup;
    private final JdbcTemplate jdbc;

    public SaleSteelSupport(SaleEngrSupport sup, JdbcTemplate jdbc) { this.sup = sup; this.jdbc = jdbc; }

    public SaleEngrSupport sup() { return sup; }

    public Table table(String proc, Object... kv) {
        Map<String, Object> params = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) params.put(String.valueOf(kv[i]), kv[i + 1]);
        return table(proc, params);
    }

    public Table table(String proc, Map<String, Object> params) {
        List<Object> args = new ArrayList<>();
        StringBuilder sb = new StringBuilder("EXEC ").append(proc.startsWith("[") ? proc : "dbo." + proc);
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;
            sb.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue());
            first = false;
        }
        return jdbc.execute(sb.toString(), (PreparedStatementCallback<Table>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            Table t = new Table();
            boolean isRs = ps.execute();
            boolean taken = false;
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        if (!taken) {
                            ResultSetMetaData md = rs.getMetaData();
                            int n = md.getColumnCount();
                            List<String> labels = new ArrayList<>();
                            for (int c = 1; c <= n; c++) {
                                String label = md.getColumnLabel(c);
                                if (label == null || label.isEmpty()) label = "Column" + c;
                                String base = label;
                                int k = 1;
                                while (labels.contains(label)) label = base + (++k);
                                labels.add(label);
                            }
                            t.cols.addAll(labels);
                            while (rs.next()) {
                                Map<String, Object> row = new LinkedHashMap<>();
                                for (int c = 1; c <= n; c++) row.put(labels.get(c - 1), json(rs.getObject(c)));
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

    /** {cols:[..], rows:[..]} for the grids that are filled with RetrieveStructure. */
    public Map<String, Object> grid(String proc, Object... kv) {
        Table t = table(proc, kv);
        return SaleEngrSupport.row("cols", t.cols, "rows", t.rows);
    }

    public static Map<String, Object> grid(Table t) { return SaleEngrSupport.row("cols", t.cols, "rows", t.rows); }

    /** Dates leave as text without a time zone ("yyyy-MM-ddTHH:mm:ss"): what the database holds is what the grid shows. */
    public static Object json(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp ts) return ts.toLocalDateTime().withNano(0).toString();
        if (v instanceof java.sql.Date d) return d.toLocalDate().toString() + "T00:00:00";
        if (v instanceof java.sql.Time t) return t.toString();
        if (v instanceof LocalDateTime l) return l.withNano(0).toString();
        if (v instanceof byte[]) return null;
        if (v instanceof BigDecimal b) return b;
        return v;
    }

    /** Case-insensitive column read of an ordered row. */
    public static Object get(Map<String, Object> r, String key) { return SaleEngrSupport.ci(r, key); }

    public static String s(Object o) { return o == null ? "" : String.valueOf(o); }

    private final Map<String, List<Object[]>> paramCache = new java.util.concurrent.ConcurrentHashMap<>();

    /**
     * GenericProvider.SetProc(model, proc) sends EVERY parameter of the procedure: a bit / int / decimal property that was never set goes as false / 0 / 0.0
     * while an unset string or date is left out. This completes a parameter map the same way (the given keys win, matched case-insensitively).
     */
    public Map<String, Object> full(String proc, Map<String, Object> given) {
        List<Object[]> params = paramCache.computeIfAbsent(proc, p -> {
            List<Object[]> l = new ArrayList<>();
            String obj = p.startsWith("[") || p.contains(".") ? p : "dbo." + p;
            for (Map<String, Object> r : jdbc.queryForList("SELECT p.name, t.name AS tname FROM sys.parameters p JOIN sys.types t ON p.user_type_id = t.user_type_id "
                    + "WHERE p.object_id = OBJECT_ID(?) ORDER BY p.parameter_id", obj))
                l.add(new Object[]{String.valueOf(r.get("name")).replace("@", ""), String.valueOf(r.get("tname")).toLowerCase()});
            return l;
        });
        Map<String, Object> out = new LinkedHashMap<>(given);
        for (Object[] p : params) {
            String n = (String) p[0], t = (String) p[1];
            boolean has = false;
            for (String k : out.keySet()) if (k.equalsIgnoreCase(n)) { has = true; break; }
            if (has) continue;
            switch (t) {
                case "bit" -> out.put(n, false);
                case "tinyint", "smallint", "int", "bigint" -> out.put(n, 0);
                case "float", "real", "decimal", "numeric", "money", "smallmoney" -> out.put(n, 0.0);
                default -> { }
            }
        }
        return out;
    }
}
