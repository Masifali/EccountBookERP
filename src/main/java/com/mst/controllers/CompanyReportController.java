package com.mst.controllers;

import com.mst.services.CompanyReportService;
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

import javax.servlet.http.HttpSession;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.UUID;

/** "Report Allocate to Company" (frmCompanyReport), opened from Define Reports. */
@Controller
public class CompanyReportController {

    private static final String TOKEN = "companyReportCsrf";

    private final CompanyReportService service;
    public CompanyReportController(CompanyReportService service) { this.service = service; }

    @GetMapping("/configurations/report-allocate-to-company")
    public String page(Model model) {
        model.addAttribute("canOpen", service.canOpen());
        return "configurations/company_report";
    }

    /** frmCompanyReport_Load - grid, companies, headers - plus the page's CSRF token. */
    @GetMapping("/configurations/report-allocate-to-company/api/load")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> load(HttpSession session) {
        try {
            Map<String, Object> body = new LinkedHashMap<>(service.load());
            synchronized (session) {
                if (session.getAttribute(TOKEN) == null) {
                    session.setAttribute(TOKEN, UUID.randomUUID().toString());
                }
                body.put("token", session.getAttribute(TOKEN));
            }
            return ResponseEntity.ok(body);
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    @GetMapping("/configurations/report-allocate-to-company/api/history")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> history() {
        try {
            Map<String, Object> body = new LinkedHashMap<>();
            body.put("rows", service.history());
            return ResponseEntity.ok(body);
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    /** RetrivedData(Id). */
    @GetMapping("/configurations/report-allocate-to-company/api/{id}")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> byId(@PathVariable int id) {
        try {
            return ResponseEntity.ok(service.byId(id));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    /** Insert(). */
    @PostMapping("/configurations/report-allocate-to-company/api/save")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> save(@RequestBody Map<String, Object> form,
                                                    HttpSession session) {
        try {
            Object expected = session.getAttribute(TOKEN);
            if (expected == null || !expected.equals(form.get("token"))) {
                return ResponseEntity.status(HttpStatus.FORBIDDEN)
                        .body(fail("This page has expired. Refresh it and try again."));
            }
            return ResponseEntity.ok(service.save(form));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
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
