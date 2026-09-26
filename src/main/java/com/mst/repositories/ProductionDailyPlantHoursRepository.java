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
 * Screen 877 "Daily Plant Consumed Hours" - Architecture.WinApp.Production/frmDailyPlantConsumedHours.cs
 * (1,757 lines). Opened by frmProductionOutput.btnDailyPlantConsumedHours_Click:3221.
 *
 * Every read and write the form performs, traced form line -> BLL -> procedure. BLL parameter
 * guards are reproduced literally: a parameter the BLL adds only when non-zero / non-null is
 * OMITTED here in the same case, never sent as NULL or 0. Every name sent is declared by the
 * procedure (checked against procdure.utf8.sql).
 */
@Repository
public class ProductionDailyPlantHoursRepository {

    /** frmDailyPlantConsumedHours.base.Name (ScreenName). */
    public static final String DESKTOP_SCREEN_NAME = "frmDailyPlantConsumedHours";

    /** DownTimeReasonDbCall:260 - CommonServices.GetLookupsByTypeIdDt(21). */
    public static final int DOWNTIME_REASON_LOOKUP_TYPE = 21;

    @Autowired private JdbcTemplate jdbc;

    /* =============================================================================== pickers */

    /**
     * JobOrderNoDbCall:197 -> InvProductionJobOrder.JobOrdersForProduction -> usp_getJobOrdersForProduction
     * @OrganizationId, @CompanyId, @BranchesId (all three unguarded; @FinancialYearId never set).
     */
    public List<Map<String, Object>> jobOrdersForProduction(int organizationId, int companyId, int branchesId) {
        return rows("dbo.usp_getJobOrdersForProduction",
                p("@OrganizationId", organizationId, "@CompanyId", companyId, "@BranchesId", branchesId));
    }

    /**
     * PlantBindByJobOrderId:240 -> InvProductionJobOrder.PlantsForProductionByJobOrderId
     * -> usp_getPlantsForProductionByJobOrderId: five unguarded parameters. The desktop calls it
     * even when JobOrderId is 0 (after clearing the combo), so this does too.
     */
    public List<Map<String, Object>> plantsForProductionByJobOrderId(int organizationId, int companyId,
                                                                     int branchesId, int financialYearId,
                                                                     int jobOrderId) {
        return rows("dbo.usp_getPlantsForProductionByJobOrderId",
                p("@OrganizationId", organizationId, "@CompanyId", companyId,
                  "@FinancialYearId", financialYearId, "@BranchesId", branchesId,
                  "@JobOrderId", jobOrderId));
    }

    /**
     * DownTimeReasonDbCall:260 -> CommonServices.GetLookupsByTypeIdDt(21) -> InvLookUp.GetLookupsByTypeIdDt
     * -> Sp_InvLookup_GetAllMethod @OrganizationId, @CompanyId, @InvLookupTypeId,
     * @Activity='ReadByInvlookTypeId' (all unguarded).
     */
    public List<Map<String, Object>> lookupsByTypeId(int organizationId, int companyId, int lookupTypeId) {
        return rows("dbo.Sp_InvLookup_GetAllMethod",
                p("@OrganizationId", organizationId, "@CompanyId", companyId,
                  "@InvLookupTypeId", lookupTypeId, "@Activity", "ReadByInvlookTypeId"));
    }

    /* =============================================================================== history */

    /**
     * BindGrid:479 -> DailyPlantConsumedHours.FormHistoryAndReport
     * -> [dbo].[USP_DailyPlantConsumedHours_FormHistoryReport].
     * BLL guards: @OrganizationId, @CompanyId always; each date only when not CheckDateTimeNull;
     * @JobOrderId, @Id, @ReasonId only when != 0; @MonthDate only when set (never here).
     * The caller passes null for an unset date and 0 for an unset id.
     */
    public List<Map<String, Object>> formHistory(int organizationId, int companyId,
                                                 Timestamp fromDate, Timestamp toDate,
                                                 Timestamp entryFrom, Timestamp entryTo,
                                                 Timestamp modifyFrom, Timestamp modifyTo,
                                                 int jobOrderId, int reasonId) {
        return formHistory(organizationId, companyId, fromDate, toDate, entryFrom, entryTo,
                           modifyFrom, modifyTo, jobOrderId, reasonId, 0);
    }

    /** The same call with the BLL's @Id guard (ReportsParameters.Id, sent only when != 0). */
    public List<Map<String, Object>> formHistory(int organizationId, int companyId,
                                                 Timestamp fromDate, Timestamp toDate,
                                                 Timestamp entryFrom, Timestamp entryTo,
                                                 Timestamp modifyFrom, Timestamp modifyTo,
                                                 int jobOrderId, int reasonId, int id) {
        LinkedHashMap<String, Object> m = p("@OrganizationId", organizationId, "@CompanyId", companyId);
        if (fromDate != null) m.put("@FromDate", fromDate);
        if (toDate != null) m.put("@ToDate", toDate);
        if (entryFrom != null) m.put("@EntryFromDate", entryFrom);
        if (entryTo != null) m.put("@EntryToDate", entryTo);
        if (modifyFrom != null) m.put("@ModifyFromDate", modifyFrom);
        if (modifyTo != null) m.put("@ModifyToDate", modifyTo);
        if (jobOrderId != 0) m.put("@JobOrderId", jobOrderId);
        if (id != 0) m.put("@Id", id);
        if (reasonId != 0) m.put("@ReasonId", reasonId);
        return rows("[dbo].[USP_DailyPlantConsumedHours_FormHistoryReport]", m);
    }

    /* ================================================================================= write */

    /**
     * Insert():337 -> DailyPlantConsumedHours.Save -> DAL SetData("USP_DailyPlantConsumedHours_InsertAndUpdate"):
     * one SqlTransaction, GenericProvider sends every model property (all fourteen are declared by
     * the procedure), ExecuteScalar, Conversion.ToInt, commit.
     *
     * @return the procedure's scalar (the new or updated Id).
     */
    public int save(LinkedHashMap<String, Object> model) {
        return jdbc.execute((ConnectionCallback<Integer>) con -> {
            boolean auto = con.getAutoCommit();
            con.setAutoCommit(false);
            try {
                Object r = walk(con, "dbo.USP_DailyPlantConsumedHours_InsertAndUpdate", model);
                con.commit();
                return toInt(r);
            } catch (SQLException | RuntimeException e) {
                try { con.rollback(); } catch (SQLException ignored) { /* the original error matters */ }
                throw e;
            } finally {
                try { con.setAutoCommit(auto); } catch (SQLException ignored) { }
            }
        });
    }

    /* ============================================================================== plumbing */

    private List<Map<String, Object>> rows(String proc, LinkedHashMap<String, Object> params) {
        return jdbc.execute((ConnectionCallback<List<Map<String, Object>>>) con -> {
            List<Map<String, Object>> out = new ArrayList<>();
            run(con, proc, params, out);
            return out;
        });
    }

    private static Object walk(Connection con, String proc, LinkedHashMap<String, Object> params)
            throws SQLException {
        List<Map<String, Object>> out = new ArrayList<>();
        run(con, proc, params, out);
        if (out.isEmpty()) return null;
        Map<String, Object> first = out.get(0);
        return first.isEmpty() ? null : first.values().iterator().next();
    }

    /** EXEC with named parameters; a null value is "not supplied". First result set captured, the rest drained. */
    private static void run(Connection con, String proc, LinkedHashMap<String, Object> params,
                            List<Map<String, Object>> firstResult) throws SQLException {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;
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
        else if (v instanceof BigDecimal) ps.setBigDecimal(i, (BigDecimal) v);
        else if (v instanceof Boolean) ps.setBoolean(i, (Boolean) v);
        else if (v instanceof Timestamp) ps.setTimestamp(i, (Timestamp) v);
        else if (v instanceof String) ps.setNString(i, (String) v);
        else if (v instanceof Long) ps.setLong(i, (Long) v);
        else if (v instanceof Number) ps.setDouble(i, ((Number) v).doubleValue());
        else if (v == null) ps.setNull(i, Types.NULL);
        else ps.setObject(i, v);
    }

    /** Dates as local "yyyy-MM-ddTHH:mm:ss" text (no UTC shift), decimals as doubles. */
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
