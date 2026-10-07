package com.mst.repositories;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** Data access for the two CMTr register forms. Calls the same DDL procedures as the desktop BLL. */
@Repository
public class CmtrRegisterRepository {
    private final JdbcTemplate jdbc;

    public CmtrRegisterRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    public List<Map<String, Object>> call(String procedure, LinkedHashMap<String, Object> parameters) {
        StringBuilder sql = new StringBuilder("EXEC ").append(procedure);
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> entry : parameters.entrySet()) {
            sql.append(first ? " " : ", ").append('@').append(entry.getKey()).append("=?");
            args.add(entry.getValue());
            first = false;
        }
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    /** Looks up the live ScreenDefinition id by the desktop TargetUrl, for its existing rights. */
    public Integer screenId(String desktopForm) {
        List<Integer> ids = jdbc.query("SELECT TOP (1) Id FROM dbo.ScreenDefinition "
                        + "WHERE IsActive=1 AND TargetUrl LIKE ? ORDER BY Id",
                (rs, row) -> rs.getInt(1), "%" + desktopForm);
        return ids.isEmpty() ? null : ids.get(0);
    }
}
