package com.mst.reports;

import com.fasterxml.jackson.databind.ObjectMapper;
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
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.io.ByteArrayInputStream;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/**
 * One API per desktop print the seeder knows (migration/report-contracts/02_seed_report_contracts.sql,
 * 239 prints), each rendered to PDF with JasperReports - no Crystal, no Windows.
 *
 *   GET  /api/print                         every print: its URL, its arguments, how it is rendered
 *   GET  /api/print/{key}                   one print's contract
 *   POST /api/print/{key}/pdf   {args}      the PDF        (GET with query args also works)
 *   GET  /api/print/{key}/jrxml?args        the Jasper template used for it, as text
 *   POST /api/print/{key}/save-jrxml {args} writes that template to src/main/resources/jasper/converted/<rpt name>.jrxml
 *
 * Which template prints, first match wins:
 *   1. a hand-built conversion on the classpath (/jasper/converted, e.g. 273, 105/106/107)
 *   2. a saved/edited copy in jasper/converted/<rpt name>.jrxml
 *   3. generated from the rows (GeneratedPrintTemplate): orientation and logo from the .rpt,
 *      columns from the procedure.
 *
 * Data is exactly the desktop's: the seeded contract's procedure and parameters through
 * ReportDataService. OrganizationId / CompanyId come from the session, never from the request,
 * even where the seed names them as arguments.
 */
@RestController
@RequestMapping("/api/print")
public class PrintController {

    private static final Logger LOG = LoggerFactory.getLogger(PrintController.class);

    private final ReportRegistry registry;
    private final ReportDataService data;
    private final CrystalJasperPrinter converted;
    private final CurrentUserContext context;
    private final Map<String, Map<String, Object>> facts = new LinkedHashMap<>();
    private final Map<String, JasperReport> compiledSaved = new ConcurrentHashMap<>();
    private final Path jasperDir;

    public PrintController(ReportRegistry registry, ReportDataService data, CrystalJasperPrinter converted,
                           CurrentUserContext context,
                           @Value("${reports.jasper.folder:D:/CShapEccorErp/EccountingERP/src/main/resources/jasper/converted}") String jasperDir) {
        this.registry = registry;
        this.data = data;
        this.converted = converted;
        this.context = context;
        this.jasperDir = Paths.get(jasperDir);
        loadFacts();
    }

    /** Page facts per print (orientation, embedded pictures) read from the .rpt files at build time. */
    @SuppressWarnings("unchecked")
    private void loadFacts() {
        for (String res : new String[]{"/reports/print-contracts.json", "/reports/print-contracts-bll.json"}) {
            try (InputStream in = getClass().getResourceAsStream(res)) {
                if (in == null) continue;
                Map<String, Object> root = new ObjectMapper().readValue(in, Map.class);
                for (Object o : (List<Object>) root.getOrDefault("prints", Collections.emptyList())) {
                    Map<String, Object> p = (Map<String, Object>) o;
                    String key = String.valueOf(p.get("key"));
                    facts.putIfAbsent(key, p);
                    byTemplate.putIfAbsent(String.valueOf(p.get("template")).toLowerCase(Locale.ROOT), key);
                }
            } catch (Exception e) {
                LOG.warn("{} could not be read", res, e);
            }
        }
    }

    /** Arguments of a print and the desktop controls that feed them (print-controls.json), for print-rpt.js. */
    @GetMapping("/controls")
    @SuppressWarnings("unchecked")
    public ResponseEntity<Map<String, Object>> controls(@RequestParam("rpt") String rpt) {
        if (!signedIn()) return ResponseEntity.status(HttpStatus.FORBIDDEN).body(Map.of("message", "Sign in to print reports."));
        if (controlMap == null) {
            Map<String, Object> m = new HashMap<>();
            try (InputStream in = getClass().getResourceAsStream("/reports/print-controls.json")) {
                if (in != null) m = new ObjectMapper().readValue(in, Map.class);
            } catch (Exception e) { LOG.warn("print-controls.json could not be read", e); }
            controlMap = m;
        }
        String k = rpt.toLowerCase(Locale.ROOT);
        if (!k.endsWith(".rpt")) k += ".rpt";
        Object o = controlMap.get(k);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rpt", rpt);
        out.put("key", keyFor(rpt));
        if (o instanceof Map) out.putAll((Map<String, Object>) o);
        else {
            /* no traced controls: still list the arguments so the page can resolve them by name */
            ReportDefinition def = keyFor(rpt) == null ? null : registry.get(keyFor(rpt));
            List<String> args = new ArrayList<>();
            if (def != null) for (ReportDefinition.Param p : def.params)
                if (p.source.startsWith("arg:") && !TENANCY.contains(p.source.substring(4).toLowerCase(Locale.ROOT))) args.add(p.source.substring(4));
            out.put("args", args);
            out.put("controls", Map.of());
        }
        /* arguments the desktop ALWAYS sends (a slip's document id): the page must supply them */
        ReportDefinition rd = keyFor(rpt) == null ? null : registry.get(keyFor(rpt));
        List<String> required = new ArrayList<>();
        if (rd != null) for (ReportDefinition.Param p : rd.params)
            if (p.source.startsWith("arg:") && p.mode == ReportDefinition.Mode.ALWAYS) required.add(p.source.substring(4));
        out.put("required", required);
        return ResponseEntity.ok(out);
    }

    private volatile Map<String, Object> controlMap;

    /** .rpt name (lower case) -> print key, so a screen can print by the template name the desktop uses. */
    private final Map<String, String> byTemplate = new LinkedHashMap<>();

    private String keyFor(String template) {
        String t = template.toLowerCase(Locale.ROOT);
        if (!t.endsWith(".rpt")) t += ".rpt";
        String k = byTemplate.get(t);
        if (k != null) return k;
        for (ReportDefinition d : registry.all())
            if (d.template.equalsIgnoreCase(t)) return d.key;
        return null;
    }

    /**
     * Print by .rpt name: POST /api/print/by-template/105-AcRptGeneralLedger.rpt/pdf {"accountId":..}
     * (GET with query args too). This is what the screens use - the same name the desktop passes to
     * ShowReportWithDataTable.
     */
    @PostMapping("/by-template/{template}/pdf")
    public ResponseEntity<byte[]> byTemplatePost(@PathVariable("template") String template,
                                                 @RequestBody(required = false) Map<String, Object> body) {
        String key = keyFor(template);
        if (key == null) return textResponse(HttpStatus.NOT_FOUND, "No print is registered for " + template
                + " - the desktop never prints it, or its data source is not traced.", "text/plain");
        return pdf(key, body == null ? new LinkedHashMap<>() : new LinkedHashMap<>(body));
    }

    @GetMapping("/by-template/{template}/pdf")
    public ResponseEntity<byte[]> byTemplateGet(@PathVariable("template") String template,
                                                @RequestParam Map<String, String> query) {
        String key = keyFor(template);
        if (key == null) return textResponse(HttpStatus.NOT_FOUND, "No print is registered for " + template
                + " - the desktop never prints it, or its data source is not traced.", "text/plain");
        return pdf(key, new LinkedHashMap<>(query));
    }

    // ------------------------------------------------------------------ listing
    @GetMapping
    public ResponseEntity<Map<String, Object>> list() {
        if (!signedIn()) return ResponseEntity.status(HttpStatus.FORBIDDEN).body(Map.of("message", "Sign in to print reports."));
        List<Map<String, Object>> out = new ArrayList<>();
        for (String key : facts.keySet()) out.add(describe(key));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("count", out.size());
        m.put("registry", registry.composition());
        m.put("prints", out);
        return ResponseEntity.ok(m);
    }

    @GetMapping("/{key}")
    public ResponseEntity<Map<String, Object>> one(@PathVariable("key") String key) {
        if (!signedIn()) return ResponseEntity.status(HttpStatus.FORBIDDEN).body(Map.of("message", "Sign in to print reports."));
        if (!facts.containsKey(key) && registry.get(key) == null)
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(Map.of("message", "Unknown print '" + key + "'."));
        return ResponseEntity.ok(describe(key));
    }

    private Map<String, Object> describe(String key) {
        Map<String, Object> f = facts.getOrDefault(key, Collections.emptyMap());
        ReportDefinition def = registry.get(key);
        String template = def != null ? def.template : String.valueOf(f.get("template"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("key", key);
        m.put("template", template);
        m.put("title", GeneratedPrintTemplate.titleOf(template));
        m.put("procedure", def != null ? def.procedure : f.get("procedure"));
        m.put("desktopCaller", def != null ? def.desktopCaller : f.get("desktopCaller"));
        m.put("pdf", "/api/print/" + key + "/pdf");
        m.put("pdfByTemplate", "/api/print/by-template/" + template + "/pdf");
        m.put("traced", f.getOrDefault("traced", "seeder"));
        if (f.get("notes") instanceof List && !((List<?>) f.get("notes")).isEmpty()) m.put("notes", f.get("notes"));
        List<Map<String, Object>> args = new ArrayList<>();
        if (def != null) {
            for (ReportDefinition.Param p : def.params) {
                if (!p.source.startsWith("arg:")) continue;
                String a = p.source.substring(4);
                if (TENANCY.contains(a.toLowerCase(Locale.ROOT))) continue;
                Map<String, Object> am = new LinkedHashMap<>();
                am.put("arg", a);
                am.put("procedureParameter", p.name);
                am.put("required", p.mode == ReportDefinition.Mode.ALWAYS);
                args.add(am);
            }
        }
        m.put("args", args);
        m.put("available", def != null);
        m.put("renderer", converted.hasTemplate(template) ? "converted template"
                : Files.isRegularFile(savedPath(template)) ? "saved template " + savedPath(template).getFileName()
                : "generated from the procedure's columns");
        m.put("orientation", f.getOrDefault("orientation", "Portrait"));
        m.put("templateOnDisk", f.getOrDefault("templateOnDisk", false));
        return m;
    }

    // ------------------------------------------------------------------ printing
    @PostMapping("/{key}/pdf")
    public ResponseEntity<byte[]> pdfPost(@PathVariable("key") String key,
                                          @RequestBody(required = false) Map<String, Object> body) {
        return pdf(key, body == null ? new LinkedHashMap<>() : new LinkedHashMap<>(body));
    }

    @GetMapping("/{key}/pdf")
    public ResponseEntity<byte[]> pdfGet(@PathVariable("key") String key, @RequestParam Map<String, String> query) {
        return pdf(key, new LinkedHashMap<>(query));
    }

    @GetMapping("/{key}/jrxml")
    public ResponseEntity<byte[]> jrxml(@PathVariable("key") String key, @RequestParam Map<String, String> query) {
        try {
            Prepared p = prepare(key, new LinkedHashMap<>(query));
            if (p.error != null) return p.error;
            return textResponse(HttpStatus.OK, p.jrxml == null ? "This print uses a converted template: /jasper/converted/"
                    + CrystalJasperPrinter.stem(p.def.template) + ".jrxml" : p.jrxml, "application/xml");
        } catch (Exception e) {
            LOG.warn("jrxml for '{}' failed", key, e);
            return textResponse(HttpStatus.INTERNAL_SERVER_ERROR, msg(e), "text/plain");
        }
    }

    /** Writes the template used for this print to GoldenAceRice/Jasper so it can be edited; the saved file wins afterwards. */
    @PostMapping("/{key}/save-jrxml")
    public ResponseEntity<byte[]> saveJrxml(@PathVariable("key") String key,
                                            @RequestBody(required = false) Map<String, Object> body) {
        try {
            Prepared p = prepare(key, body == null ? new LinkedHashMap<>() : new LinkedHashMap<>(body));
            if (p.error != null) return p.error;
            if (p.jrxml == null) return textResponse(HttpStatus.CONFLICT, "This print already has a converted template.", "text/plain");
            Path target = savedPath(p.def.template);
            Files.createDirectories(target.getParent());
            Files.write(target, p.jrxml.getBytes(StandardCharsets.UTF_8));
            /* the manifest makes CrystalJasperPrinter pick the saved file up on the next print */
            Map<String, Object> manifest = new LinkedHashMap<>();
            manifest.put("source", p.def.template);
            manifest.put("main", target.getFileName().toString());
            manifest.put("subreports", new ArrayList<>());
            manifest.put("issues", List.of("generated from the procedure's columns; layout not read from the .rpt"));
            Files.write(jasperDir.resolve(CrystalJasperPrinter.baseName(p.def.template) + ".manifest.json"),
                    new ObjectMapper().writerWithDefaultPrettyPrinter().writeValueAsBytes(manifest));
            compiledSaved.remove(target.toString());
            return textResponse(HttpStatus.OK, "Saved " + target, "text/plain");
        } catch (Exception e) {
            LOG.warn("save-jrxml for '{}' failed", key, e);
            return textResponse(HttpStatus.INTERNAL_SERVER_ERROR, msg(e), "text/plain");
        }
    }

    private ResponseEntity<byte[]> pdf(String key, Map<String, Object> args) {
        try {
            Prepared p = prepare(key, args);
            if (p.error != null) return p.error;
            byte[] pdf;
            if (p.jrxml == null && p.saved == null) {
                pdf = converted.renderPdf(p.result);                       // hand-built conversion
            } else {
                JasperReport report = p.saved != null ? p.saved
                        : JasperCompileManager.compileReport(JRXmlLoader.load(new ByteArrayInputStream(p.jrxml.getBytes(StandardCharsets.UTF_8))));
                Map<String, Object> params = new HashMap<>();
                params.put(JRParameter.REPORT_LOCALE, Locale.US);
                params.put("CR_VARS", new com.mst.reports.jasper.CR.Vars());
                Map<String, Object> rp = map(p.result.get("reportParameters"));
                params.put("CompanyName", rp.get("@CompanyName"));
                params.put("CompanyAddress", rp.get("@CompanyAddress"));
                params.put("PrintedBy", printedBy());
                List<Map<String, Object>> rows = CrystalJasperPrinter.normalize(report, p.rows);
                JasperPrint jp = JasperFillManager.fillReport(report, params,
                        new JRMapCollectionDataSource(new ArrayList<Map<String, ?>>(rows)));
                pdf = JasperExportManager.exportReportToPdf(jp);
            }
            HttpHeaders h = new HttpHeaders();
            h.setContentType(MediaType.APPLICATION_PDF);
            h.setContentDisposition(ContentDisposition.inline()
                    .filename(CrystalJasperPrinter.stem(p.def.template) + ".pdf").build());
            return new ResponseEntity<>(pdf, h, HttpStatus.OK);
        } catch (IllegalArgumentException e) {
            return textResponse(HttpStatus.BAD_REQUEST, msg(e), "text/plain");
        } catch (Exception e) {
            LOG.warn("Print '{}' failed", key, e);
            return textResponse(HttpStatus.INTERNAL_SERVER_ERROR, msg(e), "text/plain");
        }
    }

    // ------------------------------------------------------------------ shared path
    private static final class Prepared {
        ReportDefinition def;
        Map<String, Object> result;
        List<Map<String, Object>> rows;
        String jrxml;          // generated template (null = converted or saved)
        JasperReport saved;    // saved/edited template
        ResponseEntity<byte[]> error;
    }

    private static final java.util.Set<String> TENANCY = java.util.Set.of("organizationid", "companyid");

    @SuppressWarnings("unchecked")
    private Prepared prepare(String key, Map<String, Object> args) throws Exception {
        Prepared p = new Prepared();
        if (!signedIn()) { p.error = textResponse(HttpStatus.FORBIDDEN, "Sign in to print reports.", "text/plain"); return p; }
        p.def = registry.get(key);
        if (p.def == null) {
            String why = ReportRegistry.UNAVAILABLE.get(String.valueOf(facts.getOrDefault(key, Map.of()).get("template")));
            p.error = textResponse(HttpStatus.NOT_FOUND, "Unknown print '" + key + "'." + (why == null ? "" : " " + why), "text/plain");
            return p;
        }
        /* Tenancy from the session, whatever the request says - some seeded contracts name
           organizationId/companyId as arguments because the desktop copied them from UserAccount. */
        args.keySet().removeIf(k -> TENANCY.contains(k.toLowerCase(Locale.ROOT)));
        args.put("organizationId", context.currentOrganizationId());
        args.put("companyId", context.currentCompanyId());
        for (Map.Entry<String, Object> e : new ArrayList<>(args.entrySet())) {
            if (e.getValue() instanceof String) {                       // query strings arrive as text
                String s = ((String) e.getValue()).trim();
                if (s.matches("-?\\d{1,9}")) e.setValue(Integer.valueOf(s));
            }
        }

        p.result = data.run(key, args);
        Object rows = p.result.get("rows");
        p.rows = rows instanceof List ? (List<Map<String, Object>>) rows : new ArrayList<>();
        if (p.rows.isEmpty()) {
            p.error = textResponse(HttpStatus.NOT_FOUND, "No Record Found For Display", "text/plain");   // the desktop's message
            return p;
        }
        if (converted.hasTemplate(p.def.template)) return p;

        Path saved = savedPath(p.def.template);
        if (Files.isRegularFile(saved)) {
            String k = saved.toString() + "@" + Files.getLastModifiedTime(saved).toMillis();
            JasperReport r = compiledSaved.get(k);
            if (r == null) {
                try (InputStream in = Files.newInputStream(saved)) {
                    r = JasperCompileManager.compileReport(JRXmlLoader.load(in));
                }
                compiledSaved.put(k, r);
            }
            p.saved = r;
            return p;
        }

        Map<String, Object> f = facts.getOrDefault(key, Collections.emptyMap());
        GeneratedPrintTemplate.Meta m = new GeneratedPrintTemplate.Meta();
        m.name = CrystalJasperPrinter.stem(p.def.template);
        m.title = GeneratedPrintTemplate.titleOf(p.def.template);
        m.landscape = "Landscape".equals(f.get("orientation"));
        m.source = p.def.template;
        Object imgs = f.get("images");
        if (imgs instanceof List && !((List<?>) imgs).isEmpty()) {
            m.logoResource = "/jasper/print/images/" + ((Map<String, Object>) ((List<?>) imgs).get(0)).get("file");
        }
        p.jrxml = GeneratedPrintTemplate.build(m, p.rows, new LinkedHashMap<>());
        return p;
    }

    private Path savedPath(String template) {
        return jasperDir.resolve(CrystalJasperPrinter.baseName(template) + ".jrxml");
    }

    private boolean signedIn() {
        try { return context.currentUserId() > 0; } catch (RuntimeException e) { return false; }
    }

    private String printedBy() {
        try { return context.requireAccountingUser().getUserName(); } catch (RuntimeException e) { return ""; }
    }

    @SuppressWarnings("unchecked")
    private static Map<String, Object> map(Object o) {
        return o instanceof Map ? (Map<String, Object>) o : new HashMap<>();
    }

    private static ResponseEntity<byte[]> textResponse(HttpStatus status, String body, String type) {
        HttpHeaders h = new HttpHeaders();
        h.setContentType(MediaType.parseMediaType(type + ";charset=UTF-8"));
        return new ResponseEntity<>(body.getBytes(StandardCharsets.UTF_8), h, status);
    }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage() == null ? e.getMessage() : t.getMessage();
        return m == null ? "Printing failed." : m;
    }
}
