package com.mst.reports;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.repositories.ReportContractRepository;
import com.mst.reports.jasper.CrystalJasperPrinter;
import com.mst.reports.prints.ReportPdfService;
import com.mst.security.CurrentUserContext;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.ObjectProvider;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.test.util.ReflectionTestUtils;

import java.nio.file.Files;
import java.nio.file.Path;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.regex.Pattern;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class SeededPrintCoverageTest {
    private final ObjectMapper json = new ObjectMapper();

    @Test
    @SuppressWarnings("unchecked")
    void everyExecutableSqlSeedHasABundledDefinitionAndNamedJavaRoute() throws Exception {
        Map<String, JsonNode> bundled = new LinkedHashMap<>();
        for (String name : List.of("print-contracts.json", "print-contracts-bll.json")) {
            try (var input = getClass().getResourceAsStream("/reports/" + name)) {
                for (JsonNode report : json.readTree(input).get("prints")) {
                    assertNull(bundled.put(report.get("key").asText(), report), "Duplicate report key");
                }
            }
        }
        Map<String, String> routes;
        try (var input = getClass().getResourceAsStream("/reports/print-endpoints.json")) {
            routes = json.readValue(input, Map.class);
        }
        Pattern inserts = Pattern.compile("INSERT\\s+INTO\\s+dbo\\.RptReportContract\\s*\\([^;]*?\\)\\s*VALUES\\s*"
                + "\\(N'([^']+)',\\s*N'([^']+)',\\s*N'([^']+)'", Pattern.CASE_INSENSITIVE);
        int count = 0;
        for (String file : List.of("02_seed_report_contracts.sql", "05_seed_report_contracts_bll.sql")) {
            String source = Files.readString(Path.of("migration", "report-contracts", file));
            var matcher = inserts.matcher(source);
            int found = 0;
            while (matcher.find()) {
                found++;
                String key = matcher.group(1), template = matcher.group(2), procedure = matcher.group(3);
                JsonNode report = bundled.get(key);
                assertNotNull(report, "Seeded report missing from Java bundle: " + key);
                assertEquals(template, report.get("template").asText(), key);
                assertEquals(procedure, report.get("procedure").asText(), key);
                assertFalse(report.get("params").isEmpty(), "Missing parameter contract: " + key);
                assertNotNull(routes.get(template.toLowerCase(Locale.ROOT)), "No named Java action for " + template);
            }
            assertTrue(found > 0, "Did not read seed " + file);
            assertEquals(Pattern.compile("INSERT\\s+INTO\\s+dbo\\.RptReportContract\\s*\\(", Pattern.CASE_INSENSITIVE)
                    .matcher(source).results().count(), found, "Unparsed contracts in " + file);
            count += found;
        }
        assertTrue(count >= 1063, "Executable seeder coverage must not decrease");
    }

    @Test
    @SuppressWarnings("unchecked")
    void allNamedTemplatesResolveThroughTheRealRegistryWithoutRunningTheSeeder() throws Exception {
        var repository = new ReportContractRepository();
        ReflectionTestUtils.setField(repository, "jdbcTemplate", mock(JdbcTemplate.class));
        var registry = new ReportRegistry();
        ObjectProvider<ReportContractRepository> provider = mock(ObjectProvider.class);
        when(provider.getIfAvailable()).thenReturn(repository);
        ReflectionTestUtils.setField(registry, "contractRepository", provider);
        ReflectionTestUtils.invokeMethod(registry, "loadSeededContracts");
        var pdfs = new ReportPdfService(registry, mock(ReportDataService.class),
                mock(CrystalJasperPrinter.class), mock(CurrentUserContext.class));
        Map<String, String> routes;
        try (var input = getClass().getResourceAsStream("/reports/print-endpoints.json")) {
            routes = json.readValue(input, Map.class);
        }
        for (String template : routes.keySet()) {
            String key = pdfs.keyOf(template);
            assertNotNull(key, "No runtime report key for " + template);
            ReportDefinition definition = registry.get(key);
            assertNotNull(definition, template);
            assertEquals(template, definition.template.toLowerCase(Locale.ROOT));
            assertFalse(definition.procedure.isBlank(), template);
            assertFalse(definition.params.isEmpty(), template);
        }
        assertTrue(registry.isHandTraced("203-invrptpurchaseorderriceslip"));
        assertEquals(2, registry.get("203-invrptpurchaseorderriceslip").subReports.size(),
                "Seeder coverage must preserve the verified subreport definitions");
    }
}
