package com.mst.controllers;

import com.mst.services.LicenseKeyService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.ResponseBody;

import javax.servlet.http.HttpSession;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.UUID;

/** System Utilities -> "License Key" (Architecture.WinApp.LicenseKey). */
@Controller
public class LicenseKeyController {

    private static final String TOKEN = "licenseKeyCsrf";

    private final LicenseKeyService service;
    public LicenseKeyController(LicenseKeyService service) { this.service = service; }

    @GetMapping("/utilities/license-key")
    public String page() { return "utilities/license_key"; }

    /** LicenseKey_Load -> FillDate(). */
    @GetMapping("/utilities/license-key/api/load")
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
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    /** btnSubmit_Click. */
    @PostMapping("/utilities/license-key/api/submit")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> submit(@RequestBody Map<String, Object> body,
                                                      HttpSession session) {
        try {
            Object expected = session.getAttribute(TOKEN);
            if (expected == null || !expected.equals(body.get("token"))) {
                return ResponseEntity.status(HttpStatus.FORBIDDEN)
                        .body(fail("This page has expired. Refresh it and try again."));
            }
            Object v = body.get("license");
            return ResponseEntity.ok(service.submit(v == null ? "" : String.valueOf(v)));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            /* Insert_CLV's RAISERROR 'Invalid Key!' arrives here and is passed through. */
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
