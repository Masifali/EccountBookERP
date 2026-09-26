package com.mst.repositories;

import com.mst.reports.ReportDefinition;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Loads the report contracts traced from the desktop C# and seeded into
 * dbo.RptReportContract / dbo.RptReportContractParam.
 *
 * WHY A TABLE AND NOT MORE HAND-WRITTEN JAVA
 * ------------------------------------------
 * The desktop has 1,138 referenced .rpt templates behind 630 procedures. Hand-transcribing
 * each contract into ReportRegistry does not scale, and every retype is a chance to introduce
 * the kind of defect this port has already hit twice (CommissionAgentId, StockWeight). The
 * seed is generated mechanically from the C# and every row carries the file and line it came
 * from, so any row can be re-checked against the desktop rather than trusted.
 *
 * PRECEDENCE: hand-traced definitions in ReportRegistry always win. A seeded row for a key
 * that is already registered is skipped, never merged - a mechanically-read contract must not
 * overwrite one a human verified.
 *
 * ABSENT TABLES ARE NOT AN ERROR. Until the seeder is run this returns an empty list and the
 * registry keeps exactly the hand-traced entries it has today.
 */
@Repository
public class ReportContractRepository {

    private static final Logger LOG = LoggerFactory.getLogger(ReportContractRepository.class);

    @Autowired
    private JdbcTemplate jdbcTemplate;

    /** True only when both seed tables exist. Checked, never assumed. */
    public boolean seedPresent() {
        try {
            Integer n = jdbcTemplate.queryForObject(
                    "SELECT COUNT(*) FROM sys.objects WHERE type = 'U' AND name IN "
                            + "('RptReportContract','RptReportContractParam')", Integer.class);
            return n != null && n == 2;
        } catch (Exception e) {
            LOG.debug("Report contract seed tables could not be checked", e);
            return false;
        }
    }

    /**
     * Every active seeded contract, as ReportDefinitions.
     *
     * Sub-reports are deliberately NOT loaded here: the sweep resolves a sub-report's procedure
     * but not always the argument it is called with, and a sub-report run with the wrong key
     * returns another document's rows. Sub-reports stay hand-traced in ReportRegistry.
     */
    public List<ReportDefinition> loadAll() {
        if (!seedPresent()) {
            LOG.info("Report contract seed tables absent - registry keeps its hand-traced entries only");
            return Collections.emptyList();
        }

        Map<String, List<ReportDefinition.Param>> params = new LinkedHashMap<>();
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "SELECT ReportKey, ParameterName, ParameterSource, ParameterMode "
                            + "FROM dbo.RptReportContractParam ORDER BY ReportKey, Ordinal")) {
                String key = str(r.get("ReportKey"));
                ReportDefinition.Mode mode = "GUARDED".equalsIgnoreCase(str(r.get("ParameterMode")))
                        ? ReportDefinition.Mode.GUARDED
                        : ReportDefinition.Mode.ALWAYS;
                params.computeIfAbsent(key, k -> new ArrayList<>())
                      .add(new ReportDefinition.Param(str(r.get("ParameterName")),
                                                      str(r.get("ParameterSource")), mode));
            }
        } catch (Exception e) {
            LOG.warn("Report contract parameters could not be read; seeded contracts skipped", e);
            return Collections.emptyList();
        }

        List<ReportDefinition> out = new ArrayList<>();
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "SELECT ReportKey, TemplateFileName, ProcedureName, DesktopCaller, "
                            + "SourceFile, SourceLine FROM dbo.RptReportContract "
                            + "WHERE IsActive = 1 ORDER BY ReportKey")) {
                String key = str(r.get("ReportKey"));
                List<ReportDefinition.Param> ps = params.get(key);
                if (ps == null || ps.isEmpty()) {
                    /* A contract with no parameters is not a contract. Skip it loudly rather
                       than call a procedure with nothing and return another tenant's rows. */
                    LOG.warn("Seeded report '{}' has no parameters - skipped", key);
                    continue;
                }
                out.add(new ReportDefinition(key,
                        str(r.get("TemplateFileName")),
                        str(r.get("ProcedureName")),
                        str(r.get("DesktopCaller")) + " (" + str(r.get("SourceFile"))
                                + ":" + str(r.get("SourceLine")) + ")",
                        ps, Collections.emptyList()));
            }
        } catch (Exception e) {
            LOG.warn("Report contracts could not be read; seeded contracts skipped", e);
            return Collections.emptyList();
        }
        return out;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
}
