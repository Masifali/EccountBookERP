package com.mst.controllers;

import com.mst.services.PayablesReportService;
import java.time.LocalDate;
import java.util.*;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class PayablesReportController {
    private final PayablesReportService service;

    public PayablesReportController(PayablesReportService service) {
        this.service = service;
    }

    @GetMapping({"/api/accounts/payables-report/lookups", "/api/accounts/payables-report-new/lookups"})
    @ResponseBody
    public Map<String, Object> lookups() {
        return service.lookups();
    }

    @GetMapping({"/api/accounts/payables-report", "/api/accounts/payables-report-new"})
    @ResponseBody
    public Map<String, Object> report(
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate,
            @RequestParam(defaultValue = "0") int controlAccountId,
            @RequestParam(defaultValue = "0") int accountId,
            @RequestParam(defaultValue = "0") int customGroupId,
            @RequestParam(defaultValue = "0") int inventoryGroupId,
            @RequestParam(defaultValue = "0") int cityId,
            @RequestParam(defaultValue = "0.0") double closingFrom,
            @RequestParam(defaultValue = "0.0") double closingTo,
            @RequestParam(defaultValue = "false") boolean onlyCredit,
            @RequestParam(defaultValue = "false") boolean onlyDebit,
            @RequestParam(defaultValue = "false") boolean tradeParties,
            @RequestParam(defaultValue = "false") boolean approvedTransactions,
            @RequestParam(defaultValue = "AccountClassification") String classification,
            @RequestParam(defaultValue = "Payables") String typeNature,
            @RequestParam(defaultValue = "title") String sortField,
            @RequestParam(defaultValue = "asc") String sortOrder) {
        return service.load(fromDate, toDate, controlAccountId, accountId, customGroupId, inventoryGroupId, cityId, closingFrom, closingTo, onlyCredit, onlyDebit, tradeParties, approvedTransactions, classification, typeNature, sortField, sortOrder);
    }
}
