package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.TradeReportRequest;
import java.math.BigDecimal;
import java.nio.file.Files;
import java.nio.file.Path;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import static org.junit.jupiter.api.Assertions.*;

/** Opt-in, read-only comparison of the reported opening/transaction split. */
@EnabledIfSystemProperty(named = "trade.live", matches = "true")
class TradeReportLiveTest {
    @Test void unfilteredBranchesReturnOneRowWithTheDesktopBalances() throws Exception {
        Properties config = new Properties();
        try (var in = Files.newInputStream(Path.of("src/main/resources/application-local.properties"))) { config.load(in); }
        JdbcTemplate jdbc = new JdbcTemplate(new DriverManagerDataSource(config.getProperty("spring.datasource.url"),
                config.getProperty("spring.datasource.username"), config.getProperty("spring.datasource.password")));
        jdbc.setQueryTimeout(60);
        Map<String,Object> account = jdbc.queryForMap("SELECT ID, OrganizationId, CompanyId, BranchesId, AppId FROM UserAccount WHERE UserName='numan'");
        UserAccount user = new UserAccount();
        user.setId(n(account.get("ID"))); user.setOrganizationId(n(account.get("OrganizationId")));
        user.setCompanyId(n(account.get("CompanyId"))); user.setBranchesId(n(account.get("BranchesId"))); user.setAppId(n(account.get("AppId")));
        TradeReportRepository repository = new TradeReportRepository(jdbc);
        Map<String,Object> lookups = repository.lookups(user);
        assertTrue(((List<Map<String,Object>>)lookups.get("features")).stream().noneMatch(f -> n(f.get("Id")) == 17),
                "This regression fixture requires the screenshot company's disabled Branches feature");
        TradeReportRequest request = new TradeReportRequest();
        request.setFromDate(LocalDate.of(2026,8,1)); request.setToDate(LocalDate.of(2026,10,4));
        request.setAccountClass(2); request.setCredit(true); request.setDebit(true);
        request.setBranches(String.valueOf(user.getBranchesId()));
        var split = party(repository.load(user, request));
        request.setBranches("");
        var combined = party(repository.load(user, request));
        assertEquals(2, split.size(), "The old hidden branch selection reproduces the split");
        assertEquals(1, combined.size(), "No branch filter must produce the desktop's single account row");
        var row = combined.get(0);
        for (String field : List.of("Opening", "CurrDebit", "CurrCredit", "Closing")) {
            var total = split.stream().map(r -> money(r.get(field))).reduce(BigDecimal.ZERO, BigDecimal::add);
            assertEquals(0, total.compareTo(money(row.get(field))), field + " must preserve both old rows' amounts");
        }
        assertEquals(0, money(row.get("Opening")).add(money(row.get("CurrDebit")))
                .subtract(money(row.get("CurrCredit"))).compareTo(money(row.get("Closing"))));
        for (var oldRow : split) {
            assertEquals(0, money(oldRow.get("LastBillsAmount")).compareTo(money(row.get("LastBillsAmount"))),
                    "Last bill amount is metadata and must not be doubled when opening and movements share one row");
        }
        assertEquals("2026-08-01", ((Map<?,?>)lookups.get("year")).get("Start_Period"));
        Map<String,Object> evidence = new LinkedHashMap<>();
        for (String field : List.of("Opening", "CurrDebit", "CurrCredit", "Closing", "LastBillsAmount")) evidence.put(field, row.get(field));
        System.out.println("Local database ZAM ZAM RICE TRADERS FAISLABAD: 2 branch-filtered rows -> 1 combined row. " + evidence);
    }
    private static List<Map<String,Object>> party(List<Map<String,Object>> rows) {
        return rows.stream().filter(r -> "150404487".equals(String.valueOf(r.get("AccountCode")))).toList();
    }
    private static int n(Object value) { return ((Number)value).intValue(); }
    private static BigDecimal money(Object value) { return new BigDecimal(value.toString()); }
}
