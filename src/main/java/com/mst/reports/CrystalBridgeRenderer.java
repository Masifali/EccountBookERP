package com.mst.reports;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.repositories.ReportTemplateRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;

import java.io.File;
import java.math.BigDecimal;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Base64;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.TimeUnit;

/**
 * Prints a report by handing its rows to the REAL Crystal engine running the REAL .rpt.
 *
 * WHY THIS EXISTS RATHER THAN A REDRAWN TEMPLATE
 * ----------------------------------------------
 * A .rpt holds far more than a grid: band heights, fonts, group headers, running totals, formula
 * fields and suppression rules. None of it is readable outside Crystal - the definition lives in
 * the file's compressed OLE streams. So the only way to get output that IS the desktop's output,
 * rather than something that resembles it, is to let Crystal render it.
 *
 * This is exactly what the desktop does. It never lets the template open a connection:
 *     DataTable dt = <BLL>.<Method>(new ReportsParameters { ... });   // the procedure
 *     Reporting.ShowReportWithDataTableSubReprt(dt, "203-...rpt", subReports);
 * The rows are fetched first and PUSHED IN. ReportDataService already reproduces the first line;
 * this class reproduces the second, through CrystalReportBridge.exe.
 *
 * OPERATIONAL SHAPE
 * -----------------
 * The bridge is a .NET console program, so this renderer only works where the Crystal runtime is
 * installed - a Windows host. It is DISABLED BY DEFAULT and reports why, so a Linux deployment
 * serves the data endpoints and simply says printing is unavailable rather than failing oddly.
 *
 * NO DATABASE IS OPENED BY THE BRIDGE. It receives rows and parameters as JSON and nothing else,
 * which is why no connection string is passed to it here.
 */
@Component
public class CrystalBridgeRenderer implements ReportRenderer {

    private static final Logger LOG = LoggerFactory.getLogger(CrystalBridgeRenderer.class);

    private final CrystalPrintProperties props;
    private final ReportTemplateRepository templates;
    private final ObjectMapper json = new ObjectMapper();

    public CrystalBridgeRenderer(CrystalPrintProperties props, ReportTemplateRepository templates) {
        this.props = props;
        this.templates = templates;
    }

    @Override
    public boolean available() { return unavailableReason().isEmpty(); }

    @Override
    public String unavailableReason() {
        if (!props.isEnabled()) {
            return "Crystal printing is switched off (reports.crystal.enabled=false). It needs a "
                 + "Windows host with the SAP Crystal runtime installed, because the bridge is a "
                 + ".NET program.";
        }
        if (props.getBridgeExe() == null || props.getBridgeExe().trim().isEmpty()) {
            return "reports.crystal.bridge-exe is not set.";
        }
        if (!new File(props.getBridgeExe()).isFile()) {
            return "The Crystal bridge was not found at " + props.getBridgeExe();
        }
        if (props.getTemplateRoot() == null || props.getTemplateRoot().trim().isEmpty()) {
            return "reports.crystal.template-root is not set.";
        }
        if (!new File(props.getTemplateRoot()).isDirectory()) {
            return "The template folder was not found at " + props.getTemplateRoot();
        }
        if (!templates.seeded()) {
            return "The configured report template folder is unavailable.";
        }
        return "";
    }

    @Override
    public byte[] renderPdf(Map<String, Object> result) throws Exception {
        String reason = unavailableReason();
        if (!reason.isEmpty()) throw new IllegalStateException(reason);

        String template = str(result.get("template"));
        if (template.isEmpty()) throw new IllegalArgumentException("The report did not name a template.");

        // Resolve the registered template against the original file inventory.
        String relative = templates.relativePathOf(template);
        if (relative == null) {
            throw new IllegalArgumentException("The template '" + template + "' is not present in "
                    + "the configured template folder, so it cannot be printed here.");
        }
        Path rpt = Paths.get(props.getTemplateRoot()).resolve(relative).normalize();
        if (!rpt.startsWith(Paths.get(props.getTemplateRoot()).normalize()) || !Files.isRegularFile(rpt)) {
            throw new IllegalArgumentException("Resolved template is not a file under the template root: " + relative);
        }

        Map<String, Object> input = new LinkedHashMap<>();
        List<Map<String, Object>> rows = rows(result.get("rows"));
        input.put("columns", columns(rows));
        input.put("rows", encode(rows));
        input.put("parameters", result.get("reportParameters") == null
                ? new LinkedHashMap<String, Object>() : result.get("reportParameters"));

        List<Map<String, Object>> subs = new ArrayList<>();
        Object sr = result.get("subReports");
        if (sr instanceof List) {
            for (Object o : (List<?>) sr) {
                if (!(o instanceof Map)) continue;
                Map<?, ?> s = (Map<?, ?>) o;
                List<Map<String, Object>> srows = rows(s.get("rows"));
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("name", str(s.get("template")));
                m.put("columns", columns(srows));
                m.put("rows", encode(srows));
                subs.add(m);
            }
        }
        input.put("subreports", subs);

        Path dir = workDir();
        Path in  = Files.createTempFile(dir, "rpt-in-", ".json");
        Path out = Files.createTempFile(dir, "rpt-out-", ".pdf");
        try {
            Files.write(in, json.writeValueAsBytes(input));
            /* The bridge writes the file itself; an existing empty one would be overwritten, but
               deleting first keeps a stale PDF from ever being returned if the run fails. */
            Files.deleteIfExists(out);

            ProcessBuilder pb = new ProcessBuilder(
                    props.getBridgeExe(), rpt.toString(), in.toString(), out.toString());
            Path diagnostics=Files.createTempFile(dir,"rpt-diagnostic-",".txt");
            String stderr;
            try {
                pb.redirectErrorStream(true);
                pb.redirectOutput(diagnostics.toFile());
                Process p = pb.start();
                boolean done;
                try { done=p.waitFor(Math.max(5, props.getTimeoutSeconds()), TimeUnit.SECONDS); }
                catch(InterruptedException interrupted) { p.destroyForcibly(); Thread.currentThread().interrupt(); throw interrupted; }
                if (!done) {
                    p.destroyForcibly();
                    throw new IllegalStateException("Crystal did not finish within " + props.getTimeoutSeconds() + "s for " + template + ".");
                }
                stderr=Files.readString(diagnostics,StandardCharsets.UTF_8);
            if (p.exitValue() != 0) {
                throw new IllegalStateException("Crystal failed on " + template + ": "
                        + (stderr.trim().isEmpty() ? "no detail" : stderr.trim()));
            }
            } finally { Files.deleteIfExists(diagnostics); }
            if (!Files.isRegularFile(out) || Files.size(out) == 0) {
                throw new IllegalStateException("Crystal reported success but produced no PDF for " + template + ".");
            }
            if (!stderr.trim().isEmpty()) {
                /* The bridge warns rather than fails when a named sub-report is not in the
                   template. That must not be lost - an empty band is otherwise silent. */
                LOG.warn("Crystal warnings for {}: {}", template, stderr.trim());
            }
            return Files.readAllBytes(out);
        } finally {
            try { Files.deleteIfExists(in); } catch (Exception ignored) { }
            try { Files.deleteIfExists(out); } catch (Exception ignored) { }
        }
    }

    private static byte[] readAll(java.io.InputStream is) throws java.io.IOException {
        java.io.ByteArrayOutputStream b = new java.io.ByteArrayOutputStream();
        byte[] buf = new byte[8192];
        int n;
        while ((n = is.read(buf)) > 0) b.write(buf, 0, n);
        return b.toByteArray();
    }

    private Path workDir() throws Exception {
        String w = props.getWorkDir();
        Path dir = (w == null || w.trim().isEmpty())
                ? Paths.get(System.getProperty("java.io.tmpdir"))
                : Paths.get(w);
        Files.createDirectories(dir);
        return dir;
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rows(Object o) {
        return o instanceof List ? (List<Map<String, Object>>) o : new ArrayList<Map<String, Object>>();
    }

    /**
     * The bridge builds its DataTable with Type.GetType(name), so every column needs a .NET type.
     *
     * The type is taken from the first NON-NULL value in the column, not from the first row - a
     * column whose first row happens to be null would otherwise be typed String and then reject
     * the numbers below it.
     */
    private static List<Map<String, Object>> columns(List<Map<String, Object>> rows) {
        List<Map<String, Object>> cols = new ArrayList<>();
        if (rows.isEmpty()) return cols;
        for (String name : rows.get(0).keySet()) {
            String type = "System.String";
            for (Map<String, Object> r : rows) {
                Object v = r.get(name);
                if (v != null) { type = netType(v); break; }
            }
            Map<String, Object> c = new LinkedHashMap<>();
            c.put("name", name);
            c.put("type", type);
            cols.add(c);
        }
        return cols;
    }

    private static String netType(Object v) {
        if (v instanceof Integer || v instanceof Short)  return "System.Int32";
        if (v instanceof Long)                            return "System.Int64";
        if (v instanceof BigDecimal)                      return "System.Decimal";
        if (v instanceof Double || v instanceof Float)    return "System.Double";
        if (v instanceof Boolean)                         return "System.Boolean";
        if (v instanceof byte[])                          return "System.Byte[]";
        if (v instanceof java.sql.Timestamp
         || v instanceof java.sql.Date
         || v instanceof java.util.Date)                  return "System.DateTime";
        return "System.String";
    }

    /** byte[] goes across as base64 - the bridge decodes it; dates go as ISO, which .NET parses. */
    private static List<Map<String, Object>> encode(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof byte[]) m.put(e.getKey(), Base64.getEncoder().encodeToString((byte[]) v));
                else if (v instanceof java.sql.Timestamp || v instanceof java.sql.Date || v instanceof java.util.Date)
                    m.put(e.getKey(), String.valueOf(v));
                else m.put(e.getKey(), v);
            }
            out.add(m);
        }
        return out;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
}
