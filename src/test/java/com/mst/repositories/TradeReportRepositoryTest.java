package com.mst.repositories;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.models.UserAccount;
import com.mst.models.dto.TradeReportRequest;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class TradeReportRepositoryTest {
    @Test void financialYearUsesCalendarDatesWithoutUtcDayShift() throws Exception {
        TimeZone previous = TimeZone.getDefault();
        try {
            TimeZone.setDefault(TimeZone.getTimeZone("Asia/Karachi"));
            Map<String,Object> year = new LinkedHashMap<>(Map.of("Id", 58,
                    "Start_Period", Timestamp.valueOf("2026-08-01 00:00:00"),
                    "End_Period", java.sql.Date.valueOf("2027-07-31")));
            JdbcTemplate jdbc = mock(JdbcTemplate.class, invocation -> {
                if (invocation.getMethod().getName().equals("queryForList"))
                    return invocation.<String>getArgument(0).contains("Proc_FinancialYear_") ? List.of(year) : List.of();
                return RETURNS_DEFAULTS.answer(invocation);
            });
            Map<?,?> actual = (Map<?,?>) new TradeReportRepository(jdbc).lookups(new UserAccount()).get("year");
            assertEquals("2026-08-01", actual.get("Start_Period"));
            assertEquals("2027-07-31", actual.get("End_Period"));
            assertTrue(new ObjectMapper().writeValueAsString(actual).contains("\"Start_Period\":\"2026-08-01\""));
            assertInstanceOf(Timestamp.class, year.get("Start_Period"), "Do not mutate the JDBC source row");
        } finally { TimeZone.setDefault(previous); }
    }

    @Test void emptyBranchesAreOmittedButExplicitBranchesRetainTheirSqlMeaning() {
        List<String> sql = new ArrayList<>();
        List<List<Object>> arguments = new ArrayList<>();
        JdbcTemplate jdbc = mock(JdbcTemplate.class, invocation -> {
            if (invocation.getMethod().getName().equals("queryForList")) {
                sql.add(invocation.getArgument(0));
                arguments.add(Arrays.asList(invocation.getArguments()).subList(1, invocation.getArguments().length));
                return new ArrayList<>();
            }
            return RETURNS_DEFAULTS.answer(invocation);
        });
        TradeReportRequest request = new TradeReportRequest();
        request.setFromDate(LocalDate.of(2026,8,1)); request.setToDate(LocalDate.of(2026,10,4));
        TradeReportRepository repository = new TradeReportRepository(jdbc);
        repository.load(new UserAccount(), request);
        assertFalse(sql.get(0).contains("@BranchesIds"));
        request.setBranches("58,59");
        repository.load(new UserAccount(), request);
        assertTrue(sql.get(1).contains("@BranchesIds=?"));
        assertTrue(arguments.get(1).contains("58,59"));
    }
}
