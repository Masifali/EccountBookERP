package com.mst.repositories;

import com.mst.models.SaleOutwardReportColumns;
import com.mst.models.SaleOutwardReportFilter;
import java.nio.file.Files;
import java.nio.file.Path;
import java.sql.Date;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import static org.junit.jupiter.api.Assertions.*;

/** Read-only comparison of the web report against its seeded desktop procedure rows. */
@EnabledIfSystemProperty(named = "sale.live", matches = "true")
class SaleOutwardReportLiveTest {
    @Test void seededReportRowsMatchTheDesktopProcedureAndMapToEveryGridColumn() throws Exception {
        var jdbc = SaleOutwardGatePassLiveTest.jdbc();
        jdbc.setQueryTimeout(45);
        var user = SaleOutwardGatePassLiveTest.user(jdbc);
        var from = LocalDate.of(2025, 1, 1);
        var to = LocalDate.of(2027, 1, 1);
        var branchText = "," + user.getBranchesId() + ",";
        var filter = new SaleOutwardReportFilter(from, to, 0, 0, 0, "", "", List.of(user.getBranchesId()), false);
        var actual = new SaleOutwardReportRepository(jdbc).rows(user, filter, branchText);
        var expected = jdbc.queryForList("EXEC dbo.Sp_GatePassOutward_SlipAndRegister_Rpt "
                        + "@OrganizationId=?,@CompanyId=?,@FromDate=?,@ToDate=?,@BranchesIds=?",
                user.getOrganizationId(), user.getCompanyId(), Date.valueOf(from), Date.valueOf(to), branchText);

        assertFalse(actual.isEmpty(), "The seeded branch should return outward gate pass rows");
        assertEquals(expected.size(), actual.size(), "The desktop and Java report row counts must match");
        assertEquals(canonical(expected), canonical(withoutDocumentType(actual)),
                "The Java repository must preserve the original report procedure rows");
        var gridColumns = new ArrayList<>(SaleOutwardReportColumns.names());
        gridColumns.add("DocumentTypeId");
        for (var row : actual) {
            assertEquals(gridColumns, new ArrayList<>(SaleOutwardReportColumns.project(row).keySet()));
        }

        Files.createDirectories(Path.of("migration/sale/evidence"));
        Files.writeString(Path.of("migration/sale/evidence/outward-report-read-parity.txt"),
                "Read-only seeded GoldenAcedb comparison passed for the original outward gate pass report procedure. "
                        + "Rows=" + actual.size() + "; all displayed grid columns projected; no business data written.\n");
    }

    private static List<String> canonical(List<Map<String, Object>> rows) {
        return rows.stream().map(row -> row.entrySet().stream()
                .sorted(Map.Entry.comparingByKey())
                .map(entry -> entry.getKey() + "=" + fingerprint(entry.getValue()))
                .reduce((left, right) -> left + "\u001f" + right).orElse("")).sorted().toList();
    }

    private static String fingerprint(Object value) {
        if (value instanceof byte[] bytes) return "bytes:" + bytes.length + ":" + Arrays.hashCode(bytes);
        if (value instanceof Object[] values) return Arrays.deepToString(values);
        return Objects.toString(value, "<null>");
    }

    private static List<Map<String, Object>> withoutDocumentType(List<Map<String, Object>> rows) {
        return rows.stream().map(row -> {
            var copy = new LinkedHashMap<>(row);
            copy.remove("DocumentTypeId");
            return (Map<String, Object>) copy;
        }).toList();
    }
}
