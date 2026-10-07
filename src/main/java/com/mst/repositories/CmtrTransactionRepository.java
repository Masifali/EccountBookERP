package com.mst.repositories;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** Executes the existing CmTr DDL procedures used by the two transaction forms. */
@Repository
public class CmtrTransactionRepository {
    private final JdbcTemplate jdbc;

    public CmtrTransactionRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<List<Map<String, Object>>> execute(String procedure, LinkedHashMap<String, Object> parameters) {
        StringBuilder sql = new StringBuilder("EXEC ").append(procedure);
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> entry : parameters.entrySet()) {
            sql.append(first ? " " : ", ").append('@').append(entry.getKey()).append("=?");
            args.add(entry.getValue());
            first = false;
        }
        return jdbc.execute((org.springframework.jdbc.core.ConnectionCallback<List<List<Map<String, Object>>>>) connection -> {
            try (PreparedStatement statement = connection.prepareStatement(sql.toString())) {
                for (int i = 0; i < args.size(); i++) statement.setObject(i + 1, args.get(i));
                List<List<Map<String, Object>>> sets = new ArrayList<>();
                boolean hasResult = statement.execute();
                while (true) {
                    if (hasResult) {
                        try (ResultSet result = statement.getResultSet()) {
                            ResultSetMetaData meta = result.getMetaData();
                            List<Map<String, Object>> rows = new ArrayList<>();
                            while (result.next()) {
                                Map<String, Object> row = new LinkedHashMap<>();
                                for (int c = 1; c <= meta.getColumnCount(); c++) {
                                    row.put(meta.getColumnLabel(c), result.getObject(c));
                                }
                                rows.add(row);
                            }
                            sets.add(rows);
                        }
                    } else if (statement.getUpdateCount() == -1) {
                        break;
                    }
                    hasResult = statement.getMoreResults();
                }
                return sets;
            }
        });
    }

    public List<Map<String, Object>> first(String procedure, LinkedHashMap<String, Object> parameters) {
        List<List<Map<String, Object>>> sets = execute(procedure, parameters);
        return sets.isEmpty() ? List.of() : sets.get(0);
    }
}
