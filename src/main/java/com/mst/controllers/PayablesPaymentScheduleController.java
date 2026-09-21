package com.mst.controllers;

import com.mst.services.PayablesPaymentScheduleService;
import java.time.LocalDate;
import java.util.*;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class PayablesPaymentScheduleController {
    private final PayablesPaymentScheduleService service;

    public PayablesPaymentScheduleController(PayablesPaymentScheduleService service) {
        this.service = service;
    }

    @GetMapping("/accounts/reports/payables-payment-schedule-page")
    public String page() {
        return "accounts/reports/payables-payment-schedule";
    }

    @GetMapping({"/api/accounts/payables-payment-schedule/lookups", "/api/accounts/payables-payment-schedule-new/lookups"})
    @ResponseBody
    public Map<String, Object> lookups() {
        return service.lookups();
    }

    @GetMapping({"/api/accounts/payables-payment-schedule", "/api/accounts/payables-payment-schedule-new"})
    @ResponseBody
    public Map<String, Object> report(
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dueFrom,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dueUpTo,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate purchaseFrom,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate purchaseTo,
            @RequestParam(defaultValue = "30") int intervalDays,
            @RequestParam(defaultValue = "30") int agingDays,
            @RequestParam(defaultValue = "1") int reportId,
            @RequestParam(defaultValue = "0") int parentId,
            @RequestParam(defaultValue = "0") int accountId,
            @RequestParam(defaultValue = "0") int customGroupId,
            @RequestParam(defaultValue = "0") int costCenterId,
            @RequestParam(defaultValue = "0") int customerGroupId,
            @RequestParam(defaultValue = "0") String controlAccountId,
            @RequestParam(defaultValue = "") String branches) {
        return service.load(fromDate, toDate, dueFrom, dueUpTo, purchaseFrom, purchaseTo, intervalDays, agingDays, reportId, parentId, accountId, customGroupId, costCenterId, customerGroupId, controlAccountId, branches);
    }
}
