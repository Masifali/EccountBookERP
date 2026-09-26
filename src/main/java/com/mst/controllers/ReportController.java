package com.mst.controllers;

import com.mst.repositories.ReportCatalogRepository;
import com.mst.reports.ReportDefinition;
import com.mst.reports.ReportDataService;
import com.mst.reports.ReportRegistry;
import com.mst.security.CurrentUserContext;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * The single generic entry point for every ported Crystal report.
 *
 * There is ONE controller, not one per report. A report is DATA - a row in RptReportContract
 * naming its procedure and parameters - so adding reports adds rows, not Java files, and nothing
 * has to be recompiled to gain one.
 *
 * WHAT IS PORTABLE AND WHAT IS NOT
 * --------------------------------
 * The desktop pushes rows INTO the .rpt (Reporting.ShowReportWithDataTable), it does not let the
 * template open its own connection. So a report's ROWS are fully recoverable and this controller
 * produces them. The PIXEL LAYOUT lives inside the .rpt's compressed OLE streams and cannot be
 * read here at all - the grid this returns is faithful in data, not in appearance.
 *
 * Every endpoint that cannot serve a report says WHY, naming the desktop method and the C# line,
 * rather than 404-ing or returning an empty result that reads like "no data".
 */
@Controller
public class ReportController {

    private final ReportRegistry registry;
    private final ReportDataService data;
    private final ReportCatalogRepository catalog;
    private final CurrentUserContext context;

    public ReportController(ReportRegistry registry, ReportDataService data,
                            ReportCatalogRepository catalog, CurrentUserContext context) {
        this.registry = registry;
        this.data = data;
        this.catalog = catalog;
        this.context = context;
    }

    @GetMapping("/reports")
    public String page(Model model) {
        model.addAttribute("canOpen", canOpen());
        return "reports/reports";
    }

    private boolean canOpen() {
        try { return context.currentUserId() > 0; }
        catch (RuntimeException e) { return false; }
    }

    private void require() {
        if (!canOpen()) throw new AccessDeniedException("Sign in to run reports.");
    }

    /**
     * The catalogue: all 1,138 templates with their status, plus the registry's own composition
     * so the runnable count and the seeded/hand-traced split are visible rather than assumed.
     */
    @GetMapping("/api/reports")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> list() {
        try {
            require();
            Map<String, Object> body = new LinkedHashMap<>();
            body.put("summary", catalog.summary());
            body.put("registry", registry.composition());
            body.put("statuses", ReportCatalogRepository.statuses());

            List<Map<String, Object>> rows = catalog.all();
            if (rows.isEmpty()) {
                /* The catalogue has not been seeded. Fall back to what the registry itself holds,
                   so the page still works, and say so rather than showing an empty list. */
                body.put("catalogueSeeded", false);
                List<Map<String, Object>> fallback = new ArrayList<>();
                for (ReportDefinition d : registry.all()) {
                    Map<String, Object> m = new LinkedHashMap<>();
                    m.put("TemplateFileName", d.template);
                    m.put("Status", registry.isHandTraced(d.key) ? "RUNNABLE_TRACED" : "RUNNABLE_SEEDED");
                    m.put("ReportKey", d.key);
                    m.put("Procedures", d.procedure);
                    m.put("DesktopMethod", d.desktopCaller);
                    fallback.add(m);
                }
                body.put("reports", fallback);
            } else {
                body.put("catalogueSeeded", true);
                body.put("reports", rows);
            }
            return ResponseEntity.ok(body);
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    /**
     * One report's contract: its procedure and every parameter, with the guard and the source.
     * The page builds its input form from this - the arg: parameters are the ones a user fills.
     */
    @GetMapping("/api/reports/{key}/contract")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> contract(@PathVariable String key) {
        try {
            require();
            ReportDefinition d = registry.get(key);
            if (d == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail(notRunnable(key)));

            Map<String, Object> body = new LinkedHashMap<>();
            body.put("key", d.key);
            body.put("template", d.template);
            body.put("procedure", d.procedure);
            body.put("desktopCaller", d.desktopCaller);
            body.put("handTraced", registry.isHandTraced(d.key));

            List<Map<String, Object>> ps = new ArrayList<>();
            for (ReportDefinition.Param p : d.params) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("name", p.name);
                m.put("source", p.source);
                m.put("mode", p.mode.name());
                /* Only arg: parameters are the user's to supply. session: comes from the signed-in
                   user and const: is fixed by the desktop caller - neither is ever accepted from
                   the request, which is what keeps tenancy out of the query string. */
                m.put("userSupplied", p.source != null && p.source.startsWith("arg:"));
                ps.add(m);
            }
            body.put("parameters", ps);

            List<Map<String, Object>> subs = new ArrayList<>();
            for (ReportDefinition.SubReport sr : d.subReports) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("template", sr.template);
                m.put("procedure", sr.procedure);
                subs.add(m);
            }
            body.put("subReports", subs);
            return ResponseEntity.ok(body);
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    /**
     * Run it. Only arg: parameters are taken from the body; @OrganizationId and @CompanyId come
     * from the session inside ReportDataService and are never read from the request.
     */
    @PostMapping("/api/reports/{key}/run")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> run(@PathVariable String key,
                                                   @RequestBody(required = false) Map<String, Object> args) {
        try {
            require();
            if (registry.get(key) == null) {
                return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail(notRunnable(key)));
            }
            Map<String, Object> out = new LinkedHashMap<>(data.run(key, args == null ? new LinkedHashMap<>() : args));
            out.put("success", true);
            return ResponseEntity.ok(out);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    /** A template that is catalogued but not runnable explains itself, naming the C# it came from. */
    private String notRunnable(String key) {
        Map<String, Object> row = null;
        try {
            for (Map<String, Object> r : catalog.all()) {
                Object k = r.get("ReportKey");
                Object t = r.get("TemplateFileName");
                if (key.equals(String.valueOf(k)) || key.equalsIgnoreCase(String.valueOf(t))) { row = r; break; }
            }
        } catch (Exception ignored) { /* the catalogue is optional; fall through to the plain message */ }
        if (row != null) return ReportCatalogRepository.refusal(row);
        return "Unknown report '" + key + "'. " + registry.all().size() + " are registered; "
             + "reports that are catalogued but not runnable say why on /api/reports.";
    }

    private static String msg(Exception e) {
        String m = e.getMessage();
        return (m == null || m.trim().isEmpty()) ? "Request failed." : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
