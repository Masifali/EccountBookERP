package com.mst.controllers;

import com.mst.services.AccountReportsHSupport;
import com.mst.services.CommissionAgentReportService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/** Screen 886 Commission Agent Report (CommissionAgentLedger.cs). Tenancy comes from CurrentUserContext inside the service; nothing is read from the request but the filters. */
@RestController
@RequestMapping("/accounts/api/reports/commission-agent-886")
public class CommissionAgentReportRestController {

    @Autowired
    private CommissionAgentReportService service;

    @GetMapping("/init")
    public ResponseEntity<?> init() { return run(service::init); }

    /** btnRefresh_Click - CommissionAgentFill again. */
    @GetMapping("/combos")
    public ResponseEntity<?> combos() { return run(service::combos); }

    @PostMapping("/show")
    public ResponseEntity<?> show(@RequestBody Map<String, Object> body) { return run(() -> service.show(body)); }

    private ResponseEntity<?> run(Supplier<Object> body) {
        try {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("success", true);
            m.put("data", body.get());
            return ResponseEntity.ok(m);
        } catch (AccountReportsHSupport.Refusal e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (org.springframework.security.access.AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable t = e;
            while (t.getCause() != null && t.getCause() != t) t = t.getCause();
            String msg = t.getMessage() == null || t.getMessage().trim().isEmpty() ? String.valueOf(e.getMessage()) : t.getMessage();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
