package com.mst.controllers;

import com.mst.reports.CrystalBridgeRenderer;
import com.mst.repositories.ReportTemplateRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** Report availability and template-file inspection. PDF output is in the ERPPrint module controllers. */
@Controller
public class ReportPrintController {

    private static final Logger LOG = LoggerFactory.getLogger(ReportPrintController.class);

    private final CrystalBridgeRenderer crystal;
    private final ReportTemplateRepository templates;
    private final CurrentUserContext context;

    public ReportPrintController(CrystalBridgeRenderer crystal, ReportTemplateRepository templates, CurrentUserContext context) {
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
            m.put("available", true);
            m.put("crystalAvailable", crystal.available());
            m.put("jasper", "every report prints through Jasper: the converted template, or a layout generated from the procedure (no Crystal runtime)");
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
