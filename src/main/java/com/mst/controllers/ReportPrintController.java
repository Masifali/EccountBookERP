package com.mst.controllers;

import com.mst.reports.CrystalBridgeRenderer;
import com.mst.reports.ReportDataService;
import com.mst.reports.ReportDefinition;
import com.mst.reports.ReportRegistry;
import com.mst.repositories.ReportTemplateRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Printing: the same rows the data endpoint returns, rendered by the real Crystal engine against
 * the real .rpt, so the PDF is the desktop's output rather than an imitation of it.
 *
 * /api/reports/{key}/print.pdf is deliberately separate from /run. Running a report and printing
 * it are the same query but different operational requirements - printing needs a Windows host
 * with the Crystal runtime, running needs nothing - and a deployment without that host must still
 * serve data rather than fail.
 */
@Controller
public class ReportPrintController {

    private static final Logger LOG = LoggerFactory.getLogger(ReportPrintController.class);

    private final ReportRegistry registry;
    private final ReportDataService data;
    private final CrystalBridgeRenderer crystal;
    private final ReportTemplateRepository templates;
    private final CurrentUserContext context;

    public ReportPrintController(ReportRegistry registry, ReportDataService data,
                                 CrystalBridgeRenderer crystal, ReportTemplateRepository templates,
                                 CurrentUserContext context) {
        this.registry = registry;
        this.data = data;
        this.crystal = crystal;
        this.templates = templates;
        this.context = context;
    }

    private void require() {
        boolean in;
        try { in = context.currentUserId() > 0; } catch (RuntimeException e) { in = false; }
        if (!in) throw new AccessDeniedException("Sign in to print reports.");
    }

    /** Whether printing is possible, and if not, exactly why - so the page can say so up front. */
    @GetMapping("/api/reports/print/status")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> status() {
        Map<String, Object> m = new LinkedHashMap<>();
        try {
            require();
            m.put("available", crystal.available());
            m.put("reason", crystal.unavailableReason());
            m.put("templatesSeeded", templates.seeded());
            m.put("templateFiles", templates.count());
            return ResponseEntity.ok(m);
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    /** Which files on disk carry this template's name - 60 names exist in more than one folder. */
    @GetMapping("/api/reports/print/templates/{name}")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> copies(@PathVariable String name) {
        try {
            require();
            List<Map<String, Object>> copies = templates.copiesOf(name);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("template", name);
            m.put("copies", copies);
            m.put("onDisk", !copies.isEmpty());
            return ResponseEntity.ok(m);
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    /**
     * Run the report and return its PDF.
     *
     * The body carries only the arg: parameters, exactly as /run does; OrganizationId and
     * CompanyId are resolved from the session inside ReportDataService and are never read from
     * the request.
     */
    @PostMapping(value = "/api/reports/{key}/print.pdf", produces = MediaType.APPLICATION_PDF_VALUE)
    @ResponseBody
    public ResponseEntity<byte[]> print(@PathVariable String key,
                                        @RequestBody(required = false) Map<String, Object> args) {
        try {
            require();
            ReportDefinition def = registry.get(key);
            if (def == null) {
                return text(HttpStatus.NOT_FOUND, "Unknown report '" + key + "'.");
            }
            if (!crystal.available()) {
                return text(HttpStatus.SERVICE_UNAVAILABLE, crystal.unavailableReason());
            }

            Map<String, Object> result = data.run(key, args == null ? new LinkedHashMap<>() : args);
            byte[] pdf = crystal.renderPdf(result);

            HttpHeaders h = new HttpHeaders();
            h.setContentType(MediaType.APPLICATION_PDF);
            /* inline: the desktop shows the report in a viewer, it does not download a file. */
            h.set(HttpHeaders.CONTENT_DISPOSITION,
                  "inline; filename=\"" + def.template.replaceAll("\\.rpt$", "") + ".pdf\"");
            h.setContentLength(pdf.length);
            return new ResponseEntity<>(pdf, h, HttpStatus.OK);

        } catch (AccessDeniedException e) {
            return text(HttpStatus.FORBIDDEN, e.getMessage());
        } catch (IllegalArgumentException e) {
            return text(HttpStatus.BAD_REQUEST, msg(e));
        } catch (IllegalStateException e) {
            return text(HttpStatus.SERVICE_UNAVAILABLE, msg(e));
        } catch (Exception e) {
            LOG.warn("Printing '{}' failed", key, e);
            return text(HttpStatus.INTERNAL_SERVER_ERROR, msg(e));
        }
    }

    /** A failure on a PDF endpoint comes back as readable text, not a corrupt PDF. */
    private static ResponseEntity<byte[]> text(HttpStatus status, String message) {
        HttpHeaders h = new HttpHeaders();
        h.setContentType(MediaType.TEXT_PLAIN);
        byte[] b = (message == null ? "Request failed." : message)
                .getBytes(java.nio.charset.StandardCharsets.UTF_8);
        h.setContentLength(b.length);
        return new ResponseEntity<>(b, h, status);
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
