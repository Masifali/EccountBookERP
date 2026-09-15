package com.mst.controllers;

import com.mst.services.ReceivablesAgingService;
import java.time.LocalDate;
import java.util.*;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class ReceivablesAgingController {
    private final ReceivablesAgingService service;

    public ReceivablesAgingController(ReceivablesAgingService service) {
        this.service = service;
    }

    @GetMapping({"/accounts/reports/receivables-aging", "/accounts/reports/receivables-new"})
    public String page() {
        return "accounts/reports/receivables_new";
    }

    @GetMapping({"/api/accounts/receivables-aging/lookups", "/api/accounts/receivables-new/lookups"})
    @ResponseBody
    public Map<String, Object> lookups() {
        return service.lookups();
    }

    @GetMapping({"/api/accounts/receivables-aging", "/api/accounts/receivables-new"})
    @ResponseBody
    public Map<String, Object> report(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate asOnDate,
            @RequestParam(defaultValue = "30") int agingDays,
            @RequestParam(defaultValue = "1") int reportId,
            @RequestParam(defaultValue = "0") int parentId,
            @RequestParam(defaultValue = "0") int accountId,
            @RequestParam(defaultValue = "0") int customGroupId,
            @RequestParam(defaultValue = "0") int costCenterId,
            @RequestParam(defaultValue = "") String branches) {
        return service.load(asOnDate, agingDays, reportId, parentId, accountId, customGroupId, costCenterId, branches);
    }
}
