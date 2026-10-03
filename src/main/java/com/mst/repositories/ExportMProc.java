package com.mst.repositories;

import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.support.DesktopProc;
import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.sql.Types;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * GenericProvider.SetProc / GetDataTableProc plumbing shared by the batch-M Export repositories
 * (ExportReturnRepository, ExportOpeningRepository).
 *
 *  - {@link #setProc(JdbcTemplate, String, Object)} sends every public non-static field of a voucher model
 *    (SaleInvoiceModels.VoucherHead / VoucherDetail) as @Name, never binding a null (ADO.NET leaves a CLR null
 *    out); a non-nullable DateTime left unset fails by name, as "SqlDateTime overflow" does on the desktop.
 *  - {@link #setMap(JdbcTemplate, String, Map)} is SetProc over a model map (DesktopProc.setProc semantics:
 *    Convert.ToInt32(ExecuteScalar())).
 */
public final class ExportMProc {

    private ExportMProc() { }

    public static int setProc(JdbcTemplate jdbc, String proc, Object model) {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc.startsWith("[") || proc.contains(".") ? proc : "dbo." + proc).append(' ');
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Field f : model.getClass().getFields()) {
            if (Modifier.isStatic(f.getModifiers())) continue;
            if (f.isAnnotationPresent(SaleInvoiceModels.NotParam.class)) continue;
            Object val;
            try { val = f.get(model); } catch (IllegalAccessException e) { continue; }
            if (val == null) {
                if (f.getType() == LocalDateTime.class && !f.isAnnotationPresent(SaleInvoiceModels.Nullable.class))
                    throw new IllegalStateException("SqlDateTime overflow: " + model.getClass().getSimpleName() + "." + f.getName() + " was not set");
                continue;
            }
            Object bound;
            if (val instanceof LocalDateTime) bound = new SqlParameterValue(Types.TIMESTAMP, Timestamp.valueOf((LocalDateTime) val));
            else if (val instanceof BigDecimal) bound = new SqlParameterValue(Types.DECIMAL, val);
            else if (val instanceof Double) bound = new SqlParameterValue(Types.DOUBLE, val);
            else if (val instanceof Boolean) bound = new SqlParameterValue(Types.BIT, val);
            else if (val instanceof Integer) bound = new SqlParameterValue(Types.INTEGER, val);
            else bound = new SqlParameterValue(Types.NVARCHAR, String.valueOf(val));
            sql.append(first ? "" : ", ").append('@').append(f.getName()).append("=?");
            args.add(bound);
            first = false;
        }
        Integer r = ProcExec.call(jdbc, sql.toString(), args.toArray());
        return r == null ? 0 : r;
    }

    public static int setMap(JdbcTemplate jdbc, String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }
}
