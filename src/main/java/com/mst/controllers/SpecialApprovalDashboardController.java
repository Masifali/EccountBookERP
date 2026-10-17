package com.mst.controllers;

import com.mst.services.SpecialApprovalDashboardService;
import java.util.Map;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

@Controller
public class SpecialApprovalDashboardController {
    private static final Logger LOG = LoggerFactory.getLogger(SpecialApprovalDashboardController.class);
    private final SpecialApprovalDashboardService service;

    public SpecialApprovalDashboardController(SpecialApprovalDashboardService service) {
        this.service = service;
    }

    @GetMapping("/dashboard/special-approval-dashboard")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Special Approval Dashboard");
        return "dashboard/special_approval_dashboard";
    }

    @GetMapping("/api/dashboard/special-approvals/setup")
    @ResponseBody
    public Map<String, Object> setup() { return service.setup(); }

    @GetMapping("/api/dashboard/special-approvals/cards")
    @ResponseBody
    public Map<String, Object> cards(@RequestParam(defaultValue = "0") int branchId) {
        return service.cards(branchId);
    }

    @ExceptionHandler(AccessDeniedException.class)
    @ResponseBody
    public ResponseEntity<?> denied(AccessDeniedException error) {
        return ResponseEntity.status(HttpStatus.FORBIDDEN).body(Map.of("message", error.getMessage()));
    }

    @ExceptionHandler(IllegalArgumentException.class)
    @ResponseBody
    public ResponseEntity<?> invalid(IllegalArgumentException error) {
        return ResponseEntity.badRequest().body(Map.of("message", error.getMessage()));
    }

    @ExceptionHandler(org.springframework.dao.DataAccessException.class)
    @ResponseBody
    public ResponseEntity<?> failed(org.springframework.dao.DataAccessException error) {
        LOG.error("Special approval dashboard could not be read", error);
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                .body(Map.of("message", "Special approvals could not be loaded. Please retry."));
    }
}
