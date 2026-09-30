package com.mst.reports.jasper;

import com.fasterxml.jackson.databind.ObjectMapper;
import net.sf.jasperreports.engine.JRField;
import net.sf.jasperreports.engine.JRParameter;
import net.sf.jasperreports.engine.JasperCompileManager;
import net.sf.jasperreports.engine.JasperExportManager;
import net.sf.jasperreports.engine.JasperFillManager;
import net.sf.jasperreports.engine.JasperPrint;
import net.sf.jasperreports.engine.JasperReport;
import net.sf.jasperreports.engine.data.JRMapCollectionDataSource;
import net.sf.jasperreports.engine.design.JasperDesign;
import net.sf.jasperreports.engine.xml.JRXmlLoader;
import org.springframework.stereotype.Component;

import java.io.InputStream;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/**
 * Prints a desktop Crystal report through its converted JasperReports template - pure Java, no
 * Windows, no Crystal runtime.
 *
 * The templates are generated from the real .rpt by migration/rpt-to-jasper (RptLayoutDump.exe
 * reads the layout with the Crystal SDK, rpt2jrxml.py writes the .jrxml) and live on the
 * classpath under /jasper/converted/:
 *
 *     273_InvRptSaleOrderSlip.jrxml                                   main
 *     273_InvRptSaleOrderSlip__SaleOrderCustomerExpense_SubReport.jrxml   one per sub-report
 *     273_InvRptSaleOrderSlip.manifest.json                          which sub-report is which
 *
 * The flow is the desktop's own, line for line:
 *     DataTable dt = SaleOrderReports.SaleOrderReports273(obj);            -> rows
 *     new SubReportObject(SaleOrderCustomerExpense_SubReport(id), "...")   -> subRows
 *     val.RptPerameter("@CompanyName", ...)                                -> reportParams
 *     val.ShowReportWithDataTableSubReprt(dt, "273-...rpt", subs)          -> printPdf(...)
 */
@Component
/* Deliberately NOT a ReportRenderer bean: services inject ReportRenderer by type and expect the
   single Crystal bridge. Callers that want Jasper ask for CrystalJasperPrinter by name. */
public class CrystalJasperPrinter {

    public static final String ROOT = "/jasper/converted/";

    private final ObjectMapper json = new ObjectMapper();

    static {
        /* The .rpt files use Windows fonts (Arial). Where the JVM has no such font (a Linux host)
           Jasper would abort the PDF export; fall back to the default font instead. */
        net.sf.jasperreports.engine.DefaultJasperReportsContext.getInstance()
                .setProperty("net.sf.jasperreports.awt.ignore.missing.font", "true");
    }
    private final Map<String, JasperReport> compiled = new ConcurrentHashMap<>();

    /** Is there a converted template for this .rpt name? */
    /**
     * Folder of converted templates saved under the SAME name as their .rpt
     * (273-InvRptSaleOrderSlip.rpt -> 273-InvRptSaleOrderSlip.jrxml + .manifest.json). Checked
     * before the classpath copies, so a template edited there is used on the next print.
     */
    @org.springframework.beans.factory.annotation.Value("${reports.jasper.folder:D:/CShapEccorErp/EccountingERP/src/main/resources/jasper/converted}")
    private String folder;

    private java.nio.file.Path folderManifest(String rptName) {
        if (folder == null || folder.isBlank()) return null;
        java.nio.file.Path p = java.nio.file.Paths.get(folder, stripRpt(new java.io.File(rptName).getName()) + ".manifest.json");
        return java.nio.file.Files.isRegularFile(p) ? p : null;
    }

    public boolean hasTemplate(String rptName) {
        if (folderManifest(rptName) != null) return true;
        return getClass().getResource(ROOT + baseName(rptName) + ".manifest.json") != null
            || getClass().getResource(ROOT + stem(rptName) + ".manifest.json") != null;
    }

    /**
     * @param rptName      the .rpt name exactly as the desktop passes it, e.g. "273-InvRptSaleOrderSlip.rpt"
     * @param rows         the main DataTable's rows
     * @param subRows      sub-report rows keyed by the name the desktop gives the SubReportObject
     *                     (e.g. "SaleOrderCustomerExpense_SubReport.rpt"); may be empty
     * @param reportParams RptPerameter values, e.g. "@CompanyName" -> "..."
     */
    public byte[] printPdf(String rptName, List<Map<String, Object>> rows,
                           Map<String, List<Map<String, Object>>> subRows,
                           Map<String, Object> reportParams) throws Exception {
        return JasperExportManager.exportReportToPdf(fill(rptName, rows, subRows, reportParams));
    }

    /** Field names (lower case) of the converted template's main report - to tell whether a set of
        rows (a screen's DataTable) is what this template reads. */
    @SuppressWarnings("unchecked")
    public java.util.Set<String> fieldNames(String rptName) throws Exception {
        String base = getClass().getResource(ROOT + baseName(rptName) + ".manifest.json") != null
                ? baseName(rptName) : stem(rptName);
        java.nio.file.Path fm = folderManifest(rptName);
        java.nio.file.Path dir = fm == null ? null : fm.getParent();
        Map<String, Object> manifest;
        try (InputStream in = fm != null ? java.nio.file.Files.newInputStream(fm) : open(base + ".manifest.json")) {
            manifest = json.readValue(in, Map.class);
        }
        JasperReport main = compile(dir, String.valueOf(manifest.get("main")));
        java.util.Set<String> out = new java.util.HashSet<>();
        if (main.getFields() != null) for (JRField f : main.getFields()) out.add(f.getName().toLowerCase(Locale.ROOT));
        return out;
    }

    @SuppressWarnings("unchecked")
    public JasperPrint fill(String rptName, List<Map<String, Object>> rows,
                            Map<String, List<Map<String, Object>>> subRows,
                            Map<String, Object> reportParams) throws Exception {
        /* Templates carry the .rpt's own name (273-InvRptSaleOrderSlip.jrxml); the older
           underscore spelling (273_InvRptSaleOrderSlip) is still accepted. */
        String base = getClass().getResource(ROOT + baseName(rptName) + ".manifest.json") != null
                ? baseName(rptName) : stem(rptName);
        java.nio.file.Path fm = folderManifest(rptName);
        java.nio.file.Path dir = fm == null ? null : fm.getParent();
        Map<String, Object> manifest;
        try (InputStream in = fm != null ? java.nio.file.Files.newInputStream(fm) : open(base + ".manifest.json")) {
            manifest = json.readValue(in, Map.class);
        }
        JasperReport main = compile(dir, String.valueOf(manifest.get("main")));

        Map<String, Object> params = new HashMap<>();
        params.put(JRParameter.REPORT_LOCALE, Locale.US);
        params.put("CR_VARS", new CR.Vars());
        if (reportParams != null) {
            for (Map.Entry<String, Object> e : reportParams.entrySet()) {
                params.put(sanitize(e.getKey()), coerceParam(main, sanitize(e.getKey()), e.getValue()));
            }
        }

        /* Sub-reports: matched the way the Crystal bridge matches them - by the sub-report's own
           name or by the .rpt file name the desktop uses; both spellings are accepted. */
        Object subs = manifest.get("subreports");
        if (subs instanceof List) {
            for (Object o : (List<?>) subs) {
                Map<String, Object> s = (Map<String, Object>) o;
                String name = String.valueOf(s.get("name"));
                String key = String.valueOf(s.get("key"));
                JasperReport sub = compile(dir, String.valueOf(s.get("file")));
                params.put("SUBREPORT_" + key, sub);
                List<Map<String, Object>> data = findSubRows(subRows, name);
                params.put("SUBDATA_" + key, normalize(sub, data));
                // the sub-report's own copy of the company parameters
                if (reportParams != null) {
                    for (Map.Entry<String, Object> e : reportParams.entrySet())
                        params.putIfAbsent(sanitize(e.getKey()), e.getValue());
                }
            }
        }

        Collection<Map<String, ?>> data = new ArrayList<>(normalize(main, rows));
        return JasperFillManager.fillReport(main, params, new JRMapCollectionDataSource(data));
    }

    /** ReportRenderer: exactly what ReportDataService.run returned. */
    @SuppressWarnings("unchecked")
    public byte[] renderPdf(Map<String, Object> result) throws Exception {
        String template = String.valueOf(result.get("template"));
        List<Map<String, Object>> rows = (List<Map<String, Object>>) result.getOrDefault("rows", new ArrayList<>());
        Map<String, List<Map<String, Object>>> subs = new LinkedHashMap<>();
        Object sr = result.get("subReports");
        if (sr instanceof List) {
            for (Object o : (List<?>) sr) {
                Map<String, Object> s = (Map<String, Object>) o;
                subs.put(String.valueOf(s.get("template")), (List<Map<String, Object>>) s.get("rows"));
            }
        }
        return printPdf(template, rows, subs, (Map<String, Object>) result.get("reportParameters"));
    }

    public boolean available() { return true; }
    public String unavailableReason() { return ""; }

    // ------------------------------------------------------------------ internals
    /** dir = the same-name folder (edits there are picked up by modification time), or null for the classpath. */
    private JasperReport compile(java.nio.file.Path dir, String file) throws Exception {
        java.nio.file.Path onDisk = dir == null ? null : dir.resolve(file);
        String key = onDisk == null ? file
                : onDisk + "@" + java.nio.file.Files.getLastModifiedTime(onDisk).toMillis();
        JasperReport r = compiled.get(key);
        if (r != null) return r;
        synchronized (compiled) {
            r = compiled.get(key);
            if (r != null) return r;
            try (InputStream in = onDisk != null ? java.nio.file.Files.newInputStream(onDisk) : open(file)) {
                JasperDesign design = JRXmlLoader.load(in);
                r = JasperCompileManager.compileReport(design);
            }
            compiled.put(key, r);
            return r;
        }
    }

    /** Clears compiled templates, e.g. after re-running the converter in development. */
    public void clearCache() { compiled.clear(); }

    private InputStream open(String file) {
        InputStream in = getClass().getResourceAsStream(ROOT + file);
        if (in == null) throw new IllegalArgumentException("No converted Jasper template " + ROOT + file
                + " - run migration/rpt-to-jasper for this report.");
        return in;
    }

    private static List<Map<String, Object>> findSubRows(Map<String, List<Map<String, Object>>> subRows, String name) {
        if (subRows == null) return new ArrayList<>();
        String want = stripRpt(name);
        for (Map.Entry<String, List<Map<String, Object>>> e : subRows.entrySet()) {
            if (stripRpt(e.getKey()).equalsIgnoreCase(want)) return e.getValue() == null ? new ArrayList<>() : e.getValue();
        }
        return new ArrayList<>();
    }

    /** Each row keyed by the template's field names (case-insensitive match) and typed to the field class. */
    public static List<Map<String, Object>> normalize(JasperReport report, List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (rows == null) return out;
        JRField[] fields = report.getFields() == null ? new JRField[0] : report.getFields();
        for (Map<String, Object> row : rows) {
            Map<String, Object> ci = new HashMap<>();
            for (Map.Entry<String, Object> e : row.entrySet()) ci.put(e.getKey().toLowerCase(Locale.ROOT), e.getValue());
            Map<String, Object> m = new HashMap<>();
            for (JRField f : fields) {
                m.put(f.getName(), coerce(ci.get(f.getName().toLowerCase(Locale.ROOT)), f.getValueClass()));
            }
            out.add(m);
        }
        return out;
    }

    private static Object coerceParam(JasperReport r, String name, Object v) {
        for (JRParameter p : r.getParameters()) {
            if (p.getName().equals(name)) return coerce(v, p.getValueClass());
        }
        return v;
    }

    static Object coerce(Object v, Class<?> cls) {
        if (v == null || cls == null || cls == Object.class || cls.isInstance(v)) return v;
        if (cls == BigDecimal.class) return CR.num(v);
        if (cls == String.class) return String.valueOf(v);
        if (Date.class.isAssignableFrom(cls)) return CR.date(v);
        if (cls == Boolean.class) return CR.bool(v);
        if (cls == Integer.class) { BigDecimal b = CR.num(v); return b == null ? null : b.intValue(); }
        return v;
    }

    public static String sanitize(String name) {
        String s = (name == null ? "" : name).replaceFirst("^@+", "").replaceAll("[^A-Za-z0-9_]", "_");
        if (s.isEmpty() || Character.isDigit(s.charAt(0))) s = "P_" + s;
        return s;
    }

    /** "273-InvRptSaleOrderSlip.rpt" -> "273-InvRptSaleOrderSlip": the file name the templates are saved under. */
    public static String baseName(String rptName) { return stripRpt(new java.io.File(rptName == null ? "" : rptName).getName()); }

    static String stripRpt(String n) { return n == null ? "" : n.replaceAll("(?i)\\.rpt$", ""); }

    /** "273-InvRptSaleOrderSlip(A).rpt" -> "273_InvRptSaleOrderSlip_A_" (the converter's file names). */
    public static String stem(String rptName) { return stripRpt(rptName).replaceAll("[^A-Za-z0-9_]", "_"); }
}
