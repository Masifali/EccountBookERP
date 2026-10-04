package com.mst.repositories.support;

import org.springframework.core.io.ClassPathResource;
import org.springframework.jdbc.core.JdbcTemplate;
import java.io.*;
import java.nio.charset.StandardCharsets;
import java.util.*;
import java.util.regex.Pattern;

/** The native summary's shared scratch table is unsafe across concurrent desktop/web viewers. */
public final class SaleOrderSummaryQuery {
    private SaleOrderSummaryQuery() { }
    private static final String SQL = load();
    private static final List<String> PARAMETERS = Pattern.compile("(?m)^DECLARE @(\\w+) .+? = \\?;$")
            .matcher(SQL).results().map(match -> match.group(1)).toList();

    public static List<Map<String,Object>> rows(JdbcTemplate jdbc, Map<String,Object> values) {
        // Unset native parameters default to NULL. SQL declares each parameter's original type.
        Object[] args = PARAMETERS.stream().map(values::get).toArray();
        return jdbc.queryForList(SQL, args);
    }
    private static String load() {
        try (var input = new ClassPathResource("sql/sale-order-summary.sql").getInputStream()) {
            return new String(input.readAllBytes(), StandardCharsets.UTF_8);
        } catch (IOException e) { throw new UncheckedIOException("Sale Order Summary SQL is missing", e); }
    }
}
