package com.mst.repositories;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * The report catalogue: every .rpt the desktop references, and plainly what is NOT known
 * about it.
 *
 * WHY A CATALOGUE EXISTS SEPARATELY FROM THE CONTRACTS
 * ----------------------------------------------------
 * RptReportContract holds the reports that can be CALLED - procedure, every parameter, its
 * guard and its source. There are 251 of those.
 *
 * The desktop references 1,138 templates. For 802 of them the stored procedure is known but
 * the PARAMETER SOURCES are not: the launch site prints the screen's own grid, or builds its
 * parameter object in a shape the sweep could not read. A procedure without its parameter
 * sources is not a contract - calling it with invented sources is how wrong-tenant and
 * wrong-document-type rows get served, which this port has already been bitten by twice.
 *
 * So those 802 are LISTED and refused, not silently missing and not quietly guessed. The
 * catalogue is what makes the gap visible; it is never used to call anything.
 */
@Repository
public class ReportCatalogRepository {

    private static final Logger LOG = LoggerFactory.getLogger(ReportCatalogRepository.class);

    private final JdbcTemplate jdbc;
    public ReportCatalogRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public boolean present() {
        try {
            Integer n = jdbc.queryForObject(
                    "SELECT COUNT(*) FROM sys.objects WHERE type = 'U' AND name = 'RptReportCatalog'",
                    Integer.class);
            return n != null && n == 1;
        } catch (Exception e) {
            LOG.debug("Report catalogue table could not be checked", e);
            return false;
        }
    }

    /** Every row, newest status first is meaningless here - ordered by template name. */
    public List<Map<String, Object>> all() {
        if (!present()) return Collections.emptyList();
        try {
            return jdbc.queryForList(
                    "SELECT TemplateFileName, Status, ReportKey, Procedures, DocumentTypeIds, "
                            + "DesktopMethod, SourceFile, SourceLine, LaunchSiteCount, TemplateOnDisk "
                            + "FROM dbo.RptReportCatalog ORDER BY TemplateFileName");
        } catch (Exception e) {
            LOG.warn("Report catalogue could not be read", e);
            return Collections.emptyList();
        }
    }

    /** Counts per status, for the page header and for anyone asking how far the port has got. */
    public Map<String, Object> summary() {
        Map<String, Object> m = new LinkedHashMap<>();
        if (!present()) { m.put("catalogueSeeded", false); return m; }
        m.put("catalogueSeeded", true);
        try {
            List<Map<String, Object>> rows = jdbc.queryForList(
                    "SELECT Status, COUNT(*) AS n FROM dbo.RptReportCatalog GROUP BY Status");
            int total = 0;
            Map<String, Object> by = new LinkedHashMap<>();
            for (Map<String, Object> r : rows) {
                int n = r.get("n") instanceof Number ? ((Number) r.get("n")).intValue() : 0;
                by.put(String.valueOf(r.get("Status")), n);
                total += n;
            }
            m.put("byStatus", by);
            m.put("total", total);
        } catch (Exception e) {
            LOG.warn("Report catalogue summary could not be read", e);
        }
        return m;
    }

    /** One row, by template name. */
    public Map<String, Object> byTemplate(String template) {
        if (!present()) return null;
        List<Map<String, Object>> rows = jdbc.queryForList(
                "SELECT TemplateFileName, Status, ReportKey, Procedures, DocumentTypeIds, "
                        + "DesktopMethod, SourceFile, SourceLine, LaunchSiteCount, TemplateOnDisk "
                        + "FROM dbo.RptReportCatalog WHERE TemplateFileName = ?", template);
        return rows.isEmpty() ? null : rows.get(0);
    }

    /** Why a given template cannot be run, in words, for the API and the page. */
    public static String refusal(Map<String, Object> row) {
        String status = row == null ? "" : String.valueOf(row.get("Status"));
        String procs = row == null ? "" : String.valueOf(row.get("Procedures"));
        if ("PROCEDURE_ONLY".equals(status)) {
            return "This report's procedure is known (" + procs + ") but its parameter sources were "
                 + "not traced from the desktop - it prints the screen's own grid, or its launch "
                 + "site builds the parameter object in a shape the sweep could not read. Running "
                 + "it would mean inventing the parameters, so it is refused. Desktop caller: "
                 + row.get("DesktopMethod") + " (" + row.get("SourceFile") + ":" + row.get("SourceLine") + ").";
        }
        if ("UNRESOLVED".equals(status)) {
            return "No stored procedure could be resolved for this template from the desktop source. "
                 + "Desktop caller: " + (row == null ? "?" : row.get("DesktopMethod")) + ".";
        }
        return "This report is not registered.";
    }

    public static List<String> statuses() {
        List<String> s = new ArrayList<>();
        s.add("RUNNABLE_TRACED"); s.add("RUNNABLE_SEEDED");
        s.add("PROCEDURE_ONLY");  s.add("UNRESOLVED");
        return s;
    }
}
