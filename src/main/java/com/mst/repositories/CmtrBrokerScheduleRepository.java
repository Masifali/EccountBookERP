package com.mst.repositories;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** Data access for the desktop CmTr CommissionBrokerySchedule form. */
@Repository
public class CmtrBrokerScheduleRepository {
    private static final String ALL = "[CmTr].[USP_CommTradeRevenueExpenseSchedule_GetAllMethod]";
    private final JdbcTemplate jdbc;

    public CmtrBrokerScheduleRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String, Object>> exec(String procedure, LinkedHashMap<String, Object> params) {
        StringBuilder sql = new StringBuilder("EXEC ").append(procedure);
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            sql.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue()); first = false;
        }
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    public List<Map<String, Object>> lookups() {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        // The desktop calls GetLookUpsBylookTypeId(0, 0): its BLL omits both optional
        // parameters at zero, so the stored procedure returns the complete lookup table.
        p.put("Activity", "ReadByInvlookTypeId");
        return exec(ALL, p);
    }

    public List<Map<String, Object>> history(LinkedHashMap<String, Object> p) { return exec(ALL, p); }

    public List<Map<String, Object>> readHeader(int id) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("Id", id); p.put("Activity", "ReadById");
        return exec(ALL, p);
    }

    public List<Map<String, Object>> readDetails(int id) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("Id", id); p.put("Activity", "ReadByCommTradeRevenueExpenseScheduleId");
        return exec(ALL, p);
    }

    public Object saveHeader(String procedure, LinkedHashMap<String, Object> p, boolean insert) {
        StringBuilder sql = new StringBuilder("EXEC ").append(procedure);
        List<Object> args = new ArrayList<>(); boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            sql.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue()); first = false;
        }
        if (insert) {
            List<Map<String,Object>> rows = jdbc.queryForList(sql.toString(), args.toArray());
            if (rows.isEmpty() || rows.get(0).isEmpty()) throw new IllegalStateException("The desktop procedure did not return the saved schedule id.");
            return rows.get(0).values().iterator().next();
        }
        jdbc.update(sql.toString(), args.toArray());
        return p.get("Id");
    }

    public void saveDetail(LinkedHashMap<String, Object> p) {
        StringBuilder sql = new StringBuilder("EXEC [CmTr].[USP_CommTradeRevenueExpenseScheduleDetail_Insert]");
        List<Object> args = new ArrayList<>(); boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            sql.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue()); first = false;
        }
        jdbc.update(sql.toString(), args.toArray());
    }
}
