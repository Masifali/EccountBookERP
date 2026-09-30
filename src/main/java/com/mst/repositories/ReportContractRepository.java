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
        List<ReportDefinition> base = new ArrayList<>(loadSeedOrTables());
        java.util.Set<String> keys = new java.util.HashSet<>();
        java.util.Set<String> templates = new java.util.HashSet<>();
        for (ReportDefinition d : base) { keys.add(d.key); templates.add(d.template.toLowerCase(java.util.Locale.ROOT)); }
        int added = 0;
        /* Every other converted .rpt, traced from the desktop BLL (bll_contracts.py): same contract
           shape as the seeder, added after it so a seeded contract always wins. */
        for (ReportDefinition d : loadBundled("/reports/print-contracts-bll.json")) {
            if (keys.contains(d.key) || templates.contains(d.template.toLowerCase(java.util.Locale.ROOT))) continue;
            base.add(d); keys.add(d.key); added++;
        }
        LOG.info("Report contracts: {} from the seeder, {} traced from the BLL", base.size() - added, added);
        return base;
    }

    private List<ReportDefinition> loadSeedOrTables() {
        if (!seedPresent()) {
            /* The same 239 contracts, bundled with the application by
               migration/rpt-to-jasper/extract_seeded_prints.py from 02_seed_report_contracts.sql, so
               the prints work before the seeder has been run against the database. */
            List<ReportDefinition> bundled = loadBundled("/reports/print-contracts.json");
            LOG.info("Report contract seed tables absent - {} contracts loaded from the bundled seed "
                     + "(reports/print-contracts.json)", bundled.size());
            return bundled;
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

    /**
     * The seeder's contracts as shipped on the classpath. Same rules as the tables: a contract
     * without parameters is skipped, sub-reports are not loaded.
     */
    @SuppressWarnings("unchecked")
    public List<ReportDefinition> loadBundled(String resource) {
        List<ReportDefinition> out = new ArrayList<>();
        try (java.io.InputStream in = getClass().getResourceAsStream(resource)) {
            if (in == null) return out;
            Map<String, Object> root = new com.fasterxml.jackson.databind.ObjectMapper().readValue(in, Map.class);
            for (Object o : (List<Object>) root.getOrDefault("prints", Collections.emptyList())) {
                Map<String, Object> r = (Map<String, Object>) o;
                List<ReportDefinition.Param> ps = new ArrayList<>();
                for (Object po : (List<Object>) r.getOrDefault("params", Collections.emptyList())) {
                    Map<String, Object> p = (Map<String, Object>) po;
                    ps.add(new ReportDefinition.Param(str(p.get("name")), str(p.get("source")),
                            "GUARDED".equalsIgnoreCase(str(p.get("mode")))
                                    ? ReportDefinition.Mode.GUARDED : ReportDefinition.Mode.ALWAYS));
                }
                if (ps.isEmpty()) {
                    LOG.warn("Bundled report '{}' has no parameters - skipped", r.get("key"));
                    continue;
                }
                out.add(new ReportDefinition(str(r.get("key")), str(r.get("template")), str(r.get("procedure")),
                        str(r.get("desktopCaller")) + " (" + str(r.get("sourceFile")) + ":" + str(r.get("sourceLine")) + ")",
                        ps, Collections.emptyList()));
            }
        } catch (Exception e) {
            LOG.warn("Bundled report contracts could not be read", e);
        }
        return out;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
}
