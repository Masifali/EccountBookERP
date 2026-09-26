package com.mst.controllers;

import com.mst.services.PayablesAgingService;
import java.time.LocalDate;
import java.util.*;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class PayablesAgingController {
    private final PayablesAgingService service;

    public PayablesAgingController(PayablesAgingService service) {
        this.service = service;
    }

    @GetMapping("/api/accounts/payables-aging/lookups")
    @ResponseBody
    public Map<String, Object> lookups() {
        return service.lookups();
    }

    @GetMapping("/api/accounts/payables-aging")
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
