package com.mst.reports.prints;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.reports.ReportDataService;
import com.mst.reports.ReportDefinition;
import com.mst.reports.ReportRegistry;
import com.mst.reports.jasper.CR;
import com.mst.reports.jasper.CrystalJasperPrinter;
import com.mst.reports.jasper.GeneratedPrintTemplate;
import com.mst.security.CurrentUserContext;
import net.sf.jasperreports.engine.JRParameter;
import net.sf.jasperreports.engine.JasperCompileManager;
import net.sf.jasperreports.engine.JasperExportManager;
import net.sf.jasperreports.engine.JasperFillManager;
import net.sf.jasperreports.engine.JasperPrint;
import net.sf.jasperreports.engine.JasperReport;
import net.sf.jasperreports.engine.data.JRMapCollectionDataSource;
import net.sf.jasperreports.engine.xml.JRXmlLoader;
import org.springframework.stereotype.Service;

import javax.servlet.http.HttpServletResponse;
import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

/**
 * The shared print path behind every endpoint in com.mst.reports.prints:
 *
 *   request body  ->  the report's contract (seeder / BLL trace: procedure + parameters)
 *                 ->  ReportDataService.run   (EXEC the desktop's procedure, tenancy from the session)
 *                 ->  Jasper template jasper/converted/<same name as the .rpt>.jrxml
 *                     (or a layout generated from the procedure's columns when the .rpt had none)
 *                 ->  PDF written to the response, inline.
 */
@Service
public class ReportPdfService {

    private static final Set<String> TENANCY = Set.of("organizationid", "companyid");

    private final ReportRegistry registry;
    private final ReportDataService data;
    private final CrystalJasperPrinter converted;
    private final CurrentUserContext context;
    private final Map<String, Map<String, Object>> facts = new HashMap<>();

    public ReportPdfService(ReportRegistry registry, ReportDataService data, CrystalJasperPrinter converted,
                            CurrentUserContext context) {
        this.registry = registry;
        this.data = data;
        this.converted = converted;
        this.context = context;
        loadFacts();
    }

    @SuppressWarnings("unchecked")
    private void loadFacts() {
        for (String res : new String[]{"/reports/print-contracts.json", "/reports/print-contracts-bll.json"}) {
            try (InputStream in = getClass().getResourceAsStream(res)) {
                if (in == null) continue;
                Map<String, Object> root = new ObjectMapper().readValue(in, Map.class);
                for (Object o : (List<Object>) root.getOrDefault("prints", Collections.emptyList())) {
                    Map<String, Object> p = (Map<String, Object>) o;
                    facts.putIfAbsent(String.valueOf(p.get("template")).toLowerCase(Locale.ROOT), p);
                }
            } catch (Exception ignored) { }
        }
    }

    /** The registry key of a .rpt (seeded key first, then the BLL-traced one). */
    public String keyOf(String rpt) {
        Map<String, Object> f = facts.get(rpt.toLowerCase(Locale.ROOT));
        if (f != null && registry.get(String.valueOf(f.get("key"))) != null) return String.valueOf(f.get("key"));
        for (ReportDefinition d : registry.all()) if (d.template.equalsIgnoreCase(rpt)) return d.key;
        return null;
    }

    /**
     * Runs the report and writes the PDF to the response.
     * @param rpt  the .rpt name exactly as the desktop names it, e.g. "105-AcRptGeneralLedger.rpt"
     * @param args the request body as procedure arguments (null values are left out)
     */
    public void writePdf(HttpServletResponse response, String rpt, Map<String, Object> args) throws IOException {
        try {
            if (!signedIn()) { text(response, 403, "Sign in to print reports."); return; }
            String key = keyOf(rpt);
            if (key == null) { text(response, 404, "No contract (procedure + parameters) is registered for " + rpt + "."); return; }
            byte[] pdf = pdf(key, rpt, args);
            if (pdf == null) { text(response, 404, "No Record Found For Display"); return; }   // the desktop's message
            String fileName = rpt.replaceAll("(?i)\\.rpt$", "") + ".pdf";
            response.setContentType("application/pdf");
            response.setHeader("Content-Disposition", "inline; filename=\"" + fileName + "\"");
            response.setContentLength(pdf.length);
            response.getOutputStream().write(pdf);
            response.getOutputStream().flush();
        } catch (IllegalArgumentException e) {
            text(response, 400, msg(e));
        } catch (Exception e) {
            text(response, 500, msg(e));
        }
    }

    /** @return the PDF, or null when the procedure returned no rows. */
    @SuppressWarnings("unchecked")
    public byte[] pdf(String key, String rpt, Map<String, Object> args) throws Exception {
        Map<String, Object> a = new LinkedHashMap<>();
        if (args != null) args.forEach((k, v) -> { if (v != null && !TENANCY.contains(k.toLowerCase(Locale.ROOT))) a.put(k, v); });
        a.put("organizationId", context.currentOrganizationId());   // never from the request
        a.put("companyId", context.currentCompanyId());
        /* the BLL passes clsGlobalVariables.UserId as @UserId - the signed-in user, never the request */
        try { a.put("clsGlobalVariables", context.currentUserId()); } catch (RuntimeException ignored) { }

        Map<String, Object> result = data.run(key, a);
        List<Map<String, Object>> rows = (List<Map<String, Object>>) result.getOrDefault("rows", new ArrayList<>());
        if (rows.isEmpty()) return null;

        ReportDefinition def = registry.get(key);
        String template = def != null ? def.template : rpt;
        if (converted.hasTemplate(template)) return converted.renderPdf(result);   // jasper/converted/<rpt name>.jrxml

        // no .jrxml for this .rpt: lay it out from the procedure's columns
        Map<String, Object> f = facts.getOrDefault(template.toLowerCase(Locale.ROOT), Collections.emptyMap());
        GeneratedPrintTemplate.Meta m = new GeneratedPrintTemplate.Meta();
        m.name = CrystalJasperPrinter.stem(template);
        m.title = GeneratedPrintTemplate.titleOf(template);
        m.landscape = "Landscape".equals(f.get("orientation"));
        m.source = template;
        String jrxml = GeneratedPrintTemplate.build(m, rows, new LinkedHashMap<>());
        JasperReport report = JasperCompileManager.compileReport(
                JRXmlLoader.load(new ByteArrayInputStream(jrxml.getBytes(StandardCharsets.UTF_8))));
        Map<String, Object> params = new HashMap<>();
        params.put(JRParameter.REPORT_LOCALE, Locale.US);
        params.put("CR_VARS", new CR.Vars());
        Map<String, Object> rp = result.get("reportParameters") instanceof Map
                ? (Map<String, Object>) result.get("reportParameters") : new HashMap<>();
        params.put("CompanyName", rp.get("@CompanyName"));
        params.put("CompanyAddress", rp.get("@CompanyAddress"));
        try { params.put("PrintedBy", context.requireAccountingUser().getUserName()); } catch (RuntimeException ignored) { }
        JasperPrint print = JasperFillManager.fillReport(report, params,
                new JRMapCollectionDataSource(new ArrayList<Map<String, ?>>(CrystalJasperPrinter.normalize(report, rows))));
        return JasperExportManager.exportReportToPdf(print);
    }

    /**
     * Grid prints: the desktop pushes the SCREEN'S GRID (a DataTable the form built) into the .rpt -
     * 128, 128-01, 840, 135, 143, 06, 1003_01, 421 ... There is no procedure to call; the page sends
     * the rows it shows and they are laid out as a Jasper report named like the .rpt.
     */
    @SuppressWarnings("unchecked")
    public void writeGridPdf(HttpServletResponse response, String rpt, String title, List<Map<String, Object>> rows)
            throws IOException {
        try {
            if (!signedIn()) { text(response, 403, "Sign in to print reports."); return; }
            if (rows == null || rows.isEmpty()) { text(response, 404, "No Record Found For Display"); return; }
            String name = rpt == null || rpt.isBlank() ? "Print" : rpt;
            /* "450 - Department Request Register" -> 450-RptDepartmentRequestRegister.rpt (the .rpt the desktop
               pushes these rows into); when its converted template reads these columns, print through it. */
            String resolved = resolveRpt(name, title);
            if (resolved != null) {
                name = resolved;
                if (converted.hasTemplate(resolved) && fits(resolved, rows)) {
                    Map<String, Object> rp = new LinkedHashMap<>();
                    rp.put("@CompanyName", companyValue("CompName"));
                    rp.put("@CompanyAddress", companyValue("CompAddress"));
                    try { rp.put("@PrintedBy", context.requireAccountingUser().getUserName()); } catch (RuntimeException ignored) { }
                    byte[] pdf = converted.printPdf(resolved, typed(rows), new LinkedHashMap<>(), rp);
                    response.setContentType("application/pdf");
                    response.setHeader("Content-Disposition", "inline; filename=\"" + CrystalJasperPrinter.stem(resolved) + ".pdf\"");
                    response.setContentLength(pdf.length);
                    response.getOutputStream().write(pdf);
                    response.getOutputStream().flush();
                    return;
                }
            }
            GeneratedPrintTemplate.Meta m = new GeneratedPrintTemplate.Meta();
            m.name = CrystalJasperPrinter.stem(name);
            m.title = title != null && !title.isBlank() ? title : GeneratedPrintTemplate.titleOf(name);
            m.source = name + " (screen grid)";
            Map<String, Object> f = rptFacts().getOrDefault(new java.io.File(name).getName().toLowerCase(Locale.ROOT), Collections.emptyMap());
            m.landscape = "Landscape".equals(f.get("orientation")) || (rows.get(0).size() > 9);
            Object imgs = f.get("images");
            if (imgs instanceof List && !((List<?>) imgs).isEmpty())
                m.logoResource = "/jasper/print/images/" + ((Map<String, Object>) ((List<?>) imgs).get(0)).get("file");
            List<Map<String, Object>> typedRows = typed(rows);     // "1,250.50" -> number, dates -> dates, then the layout
            String jrxml = GeneratedPrintTemplate.build(m, typedRows, new LinkedHashMap<>());
            JasperReport report = JasperCompileManager.compileReport(
                    JRXmlLoader.load(new ByteArrayInputStream(jrxml.getBytes(StandardCharsets.UTF_8))));
            Map<String, Object> params = new HashMap<>();
            params.put(JRParameter.REPORT_LOCALE, Locale.US);
            params.put("CR_VARS", new CR.Vars());
            params.put("CompanyName", companyValue("CompName"));
            params.put("CompanyAddress", companyValue("CompAddress"));
            try { params.put("PrintedBy", context.requireAccountingUser().getUserName()); } catch (RuntimeException ignored) { }
            JasperPrint print = JasperFillManager.fillReport(report, params,
                    new JRMapCollectionDataSource(new ArrayList<Map<String, ?>>(CrystalJasperPrinter.normalize(report, typedRows))));
            byte[] pdf = JasperExportManager.exportReportToPdf(print);
            response.setContentType("application/pdf");
            response.setHeader("Content-Disposition", "inline; filename=\"" + m.name + ".pdf\"");
            response.setContentLength(pdf.length);
            response.getOutputStream().write(pdf);
            response.getOutputStream().flush();
        } catch (Exception e) {
            text(response, 500, msg(e));
        }
    }

    /**
     * Rows a service already fetched (the desktop's DataTable) printed through the .rpt's Jasper template:
     * the converted one when it exists, otherwise a layout generated from the rows. Used where a screen
     * called the Crystal bridge directly (ReportRenderer) - there is no Crystal runtime.
     */
    @SuppressWarnings("unchecked")
    public byte[] rowsPdf(String rpt, List<Map<String, Object>> rows, Map<String, Object> reportParams) throws Exception {
        if (converted.hasTemplate(rpt)) return converted.printPdf(rpt, rows, new LinkedHashMap<>(), reportParams);
        Map<String, Object> rp = reportParams == null ? new HashMap<>() : reportParams;
        GeneratedPrintTemplate.Meta m = new GeneratedPrintTemplate.Meta();
        m.name = CrystalJasperPrinter.stem(rpt);
        m.title = GeneratedPrintTemplate.titleOf(rpt);
        m.source = rpt;
        Map<String, Object> f = rptFacts().getOrDefault(new java.io.File(rpt).getName().toLowerCase(Locale.ROOT), Collections.emptyMap());
        m.landscape = "Landscape".equals(f.get("orientation")) || (!rows.isEmpty() && rows.get(0).size() > 9);
        Object imgs = f.get("images");
        if (imgs instanceof List && !((List<?>) imgs).isEmpty())
            m.logoResource = "/jasper/print/images/" + ((Map<String, Object>) ((List<?>) imgs).get(0)).get("file");
        String jrxml = GeneratedPrintTemplate.build(m, rows, new LinkedHashMap<>());
        JasperReport report = JasperCompileManager.compileReport(
                JRXmlLoader.load(new ByteArrayInputStream(jrxml.getBytes(StandardCharsets.UTF_8))));
        Map<String, Object> params = new HashMap<>();
        params.put(JRParameter.REPORT_LOCALE, Locale.US);
        params.put("CR_VARS", new CR.Vars());
        Object cn = rp.containsKey("@CompanyName") ? rp.get("@CompanyName") : rp.get("CompanyName");
        Object ca = rp.containsKey("@CompanyAddress") ? rp.get("@CompanyAddress") : rp.get("CompanyAddress");
        params.put("CompanyName", cn != null ? cn : companyValue("CompName"));
        params.put("CompanyAddress", ca != null ? ca : companyValue("CompAddress"));
        try { params.put("PrintedBy", context.requireAccountingUser().getUserName()); } catch (RuntimeException ignored) { }
        JasperPrint print = JasperFillManager.fillReport(report, params,
                new JRMapCollectionDataSource(new ArrayList<Map<String, ?>>(CrystalJasperPrinter.normalize(report, rows))));
        return JasperExportManager.exportReportToPdf(print);
    }

    /** A print title or a loose name -> the .rpt it names by its number ("408-Stock Transfer Register"
        -> "408-StockTransferRegister.rpt"); null when nothing matches. */
    String resolveRpt(String name, String title) {
        String n = name == null ? "" : name.trim();
        if (n.toLowerCase(Locale.ROOT).endsWith(".rpt")) return rptSpelling().getOrDefault(new java.io.File(n).getName().toLowerCase(Locale.ROOT), n);
        java.util.regex.Matcher m = java.util.regex.Pattern.compile("^\\s*(\\d{1,4}[A-Za-z]?(?:[_-]\\d{1,2})?)\\s*[-_ :]").matcher(n.isEmpty() ? String.valueOf(title) : n);
        if (!m.find()) return null;
        String code = m.group(1).toLowerCase(Locale.ROOT).replace('-', '_');
        String words = (n + " " + (title == null ? "" : title)).toLowerCase(Locale.ROOT).replaceAll("[^a-z0-9]", "");
        String best = null; int bestScore = Integer.MIN_VALUE;
        for (Map.Entry<String, String> e : rptSpelling().entrySet()) {
            String k = e.getKey().replace('-', '_');
            if (!k.startsWith(code) || k.length() == code.length()) continue;
            char c = k.charAt(code.length());
            if ("-_ (.".indexOf(c) < 0) continue;                       // 45 must not match 450, 102 not 102A
            int score = common(words, k.replaceAll("\\.rpt$", "").replaceAll("[^a-z0-9]", "")) * 100 - k.length();
            if (k.matches(".*(old|copy|\\(\\d\\)|shortcut|\\.rpt\\.rpt).*")) score -= 1000;   // spare copies in the folder
            if (score > bestScore) { best = e.getValue(); bestScore = score; }
        }
        return best;
    }

    private static int common(String a, String b) {          // longest common substring length
        int best = 0; int[] prev = new int[b.length() + 1];
        for (int i = 1; i <= a.length(); i++) {
            int[] cur = new int[b.length() + 1];
            for (int j = 1; j <= b.length(); j++)
                if (a.charAt(i - 1) == b.charAt(j - 1)) { cur[j] = prev[j - 1] + 1; if (cur[j] > best) best = cur[j]; }
            prev = cur;
        }
        return best;
    }

    /** Do these rows carry most of the template's fields? */
    private boolean fits(String rpt, List<Map<String, Object>> rows) {
        try {
            Set<String> f = converted.fieldNames(rpt);
            if (f.isEmpty() || rows.isEmpty()) return false;
            int hit = 0;
            for (String k : rows.get(0).keySet()) if (f.contains(k.toLowerCase(Locale.ROOT))) hit++;
            return hit >= Math.max(2, (int) Math.ceil(f.size() * 0.6));
        } catch (Exception e) {
            return false;
        }
    }

    private volatile Map<String, String> rptSpelling;
    /** lower-case .rpt file name -> its real spelling (from rpt-facts.json, the 1,181 .rpt files) */
    @SuppressWarnings("unchecked")
    private Map<String, String> rptSpelling() {
        if (rptSpelling == null) {
            Map<String, String> m = new HashMap<>();
            try (InputStream in = getClass().getResourceAsStream("/reports/rpt-facts.json")) {
                if (in != null) {
                    Map<String, Object> raw = new ObjectMapper().readValue(in, Map.class);
                    for (String k : raw.keySet()) {
                        String f = new java.io.File(k.replace('\\', '/')).getName();
                        if (f.toLowerCase(Locale.ROOT).endsWith(".rpt")) m.put(f.toLowerCase(Locale.ROOT), f);
                    }
                }
            } catch (Exception ignored) { }
            rptSpelling = m;
        }
        return rptSpelling;
    }

    /** Grid cells arrive as text: "1,250.50" -> number, "03/07/2025" -> date, so totals and formats work. */
    private static List<Map<String, Object>> typed(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                boolean key = e.getKey() != null && e.getKey().trim().matches("(?i).*(\\bno\\.?|\\bid|code|#|srno|sr\\.? ?no|cnic|phone|cell|mobile|ntn|strn)$");
                if (v instanceof String && !key) {
                    String s = ((String) v).trim();
                    if (s.matches("-?[0-9][0-9,]*(\\.[0-9]+)?")) v = CR.num(s);
                    else if (s.matches("\\d{1,2}[/-]\\d{1,2}[/-]\\d{4}.*|\\d{4}-\\d{2}-\\d{2}.*")) { Object d = CR.date(s); if (d != null) v = d; }
                    else if (s.isEmpty()) v = null;
                }
                m.put(e.getKey(), v);
            }
            out.add(m);
        }
        return out;
    }

    private volatile Map<String, Map<String, Object>> rptFacts;

    @SuppressWarnings("unchecked")
    private Map<String, Map<String, Object>> rptFacts() {
        if (rptFacts == null) {
            Map<String, Map<String, Object>> m = new HashMap<>();
            try (InputStream in = getClass().getResourceAsStream("/reports/rpt-facts.json")) {
                if (in != null) {
                    Map<String, Object> raw = new ObjectMapper().readValue(in, Map.class);
                    for (Map.Entry<String, Object> e : raw.entrySet())
                        m.put(new java.io.File(e.getKey().replace('\\', '/')).getName().toLowerCase(Locale.ROOT), (Map<String, Object>) e.getValue());
                }
            } catch (Exception ignored) { }
            rptFacts = m;
        }
        return rptFacts;
    }

    @org.springframework.beans.factory.annotation.Autowired
    private org.springframework.jdbc.core.JdbcTemplate jdbcTemplate;

    /** CompName / CompAddress of the signed-in company - what the desktop passes as @CompanyName / @CompanyAddress. */
    private Object companyValue(String col) {
        try {
            List<Map<String, Object>> r = jdbcTemplate.queryForList("EXEC Sp_Company_GetAllMethod @Id=?, @Activity=?",
                    context.currentCompanyId(), "ReadById");
            if (!r.isEmpty()) for (Map.Entry<String, Object> e : r.get(0).entrySet())
                if (e.getKey().equalsIgnoreCase(col)) return e.getValue();
        } catch (Exception ignored) { }
        return "";
    }

    /** Throws 400 when an argument the desktop always sends (a slip's document id) is missing. */
    public static void require(Object value, String name) {
        if (value == null || (value instanceof Number && ((Number) value).longValue() == 0)
                || String.valueOf(value).trim().isEmpty())
            throw new IllegalArgumentException(name + " is required");
    }

    private boolean signedIn() {
        try { return context.currentUserId() > 0; } catch (RuntimeException e) { return false; }
    }

    private static void text(HttpServletResponse response, int status, String message) throws IOException {
        if (response.isCommitted()) return;
        response.reset();
        response.setStatus(status);
        response.setContentType("text/plain;charset=UTF-8");
        response.getOutputStream().write(message.getBytes(StandardCharsets.UTF_8));
    }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage() == null ? e.getMessage() : t.getMessage();
        return m == null ? "Printing failed." : m;
    }
}
