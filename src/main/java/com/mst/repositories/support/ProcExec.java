package com.mst.repositories.support;

import org.springframework.jdbc.core.ArgumentPreparedStatementSetter;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;

import java.sql.PreparedStatement;
import java.sql.ResultSet;

/**
 * Execute a stored procedure the way the desktop executes one.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THIS EXISTS
 * ---------------------------------------------------------------------------------------------
 * Architecture.DAL.Common.GenericProvider.SetProc ends with:
 *
 *     return sqlCommand.ExecuteScalar();
 *
 * ExecuteScalar runs the command and reads the first column of the first row of the FIRST result
 * set, ignoring everything else - and it does not care whether a result set comes back at all.
 * The insert procedures in this database are written for that: they finish with a SELECT of the
 * new identity, so the DAL can assign `obj.Id = num`.
 *
 * `JdbcTemplate.update()` is the wrong instrument for such a procedure. It calls
 * PreparedStatement.executeUpdate(), and the Microsoft driver refuses outright when the statement
 * produced rows:
 *
 *     com.microsoft.sqlserver.jdbc.SQLServerException: A result set was generated for update.
 *
 * That is the exact failure seen when saving a Purchase Order:
 *     EXEC [dbo].[USP_PurchaseOrderPaymentTermsDetail_Insert] @Id=?, @PurchaseOrderId=?, ...
 *
 * `queryForList()` is not a safe blanket replacement either: it calls executeQuery(), which fails
 * the opposite way ("The statement did not return a result set") on a procedure that only writes.
 * Across this port both shapes exist, and which is which is a property of each procedure's body -
 * not something to guess per call site.
 *
 * So this runs the statement with plain `execute()`, which tolerates either shape, then walks the
 * results exactly as ExecuteScalar does: take the first scalar if one is offered, drain the rest,
 * return null when the procedure produced no rows. Callers that need the new id read it; callers
 * that do not can ignore it.
 *
 * Parameters are bound with Spring's own ArgumentPreparedStatementSetter, so null handling and
 * type inference are identical to what `update()` was doing - this changes how the statement is
 * executed and nothing about how its arguments are set.
 */
public final class ProcExec {

    private ProcExec() { }

    /**
     * @return the first column of the first row of the first result set, as an int, or null when
     *         the procedure returned no rows or a non-numeric value.
     */
    public static Integer call(JdbcTemplate jdbcTemplate, String sql, Object... args) {
        return jdbcTemplate.execute(sql, (PreparedStatementCallback<Integer>) ps -> {
            new ArgumentPreparedStatementSetter(args).setValues(ps);
            return walk(ps);
        });
    }

    /** The same call where the caller has no use for the returned id. */
    public static void run(JdbcTemplate jdbcTemplate, String sql, Object... args) {
        call(jdbcTemplate, sql, args);
    }

    /**
     * ExecuteScalar semantics, plus a full drain.
     *
     * Every result set and update count is consumed before returning. Leaving one pending on a
     * pooled connection is how a later, unrelated statement ends up failing on the same
     * connection - so the loop runs to completion even once the scalar is in hand.
     */
    private static Integer walk(PreparedStatement ps) throws java.sql.SQLException {
        Integer scalar = null;
        boolean isResultSet = ps.execute();
        while (true) {
            if (isResultSet) {
                try (ResultSet rs = ps.getResultSet()) {
                    while (rs.next()) {
                        if (scalar == null) scalar = asInt(rs.getObject(1));
                    }
                }
            } else if (ps.getUpdateCount() == -1) {
                break;                       // no more results and no more update counts
            }
            isResultSet = ps.getMoreResults();
        }
        return scalar;
    }

    private static Integer asInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return null;
        try {
            return Integer.valueOf(String.valueOf(o).trim());
        } catch (NumberFormatException e) {
            return null;                     // ExecuteScalar would hand back a non-numeric too
        }
    }
}
