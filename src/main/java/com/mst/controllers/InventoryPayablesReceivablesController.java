package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.repositories.InventoryPayablesReceivablesRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.time.LocalDate;
import java.util.*;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

/** Screen 61 Inventory Payables and Receivables (InventoryPayablesandReceivables.cs). Read-only report. */
@Controller
public class InventoryPayablesReceivablesController {
    private static final int SCREEN_ID = 61;
    private final InventoryPayablesReceivablesRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public InventoryPayablesReceivablesController(InventoryPayablesReceivablesRepository repository, CurrentUserContext context, DesktopReportRights rights) {
        this.repository = repository; this.context = context; this.rights = rights;
    }

    @GetMapping({"/accounts/reports/inventory-payables-receivables", "/accounts/reports/inventory-payables-and-receivables"})
    public String page(Model model) {
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", "Inventory Payables and Receivables");
        return "accounts/reports/inventory_payables_receivables";
    }

    @GetMapping("/api/accounts/inventory-payables-receivables/lookups")
    @ResponseBody
    public Map<String, Object> lookups() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        return repository.lookups(u);
    }

    @GetMapping("/api/accounts/inventory-payables-receivables")
    @ResponseBody
    public List<Map<String, Object>> report(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate,
            @RequestParam(defaultValue = "0") int tranTypes,
            @RequestParam(defaultValue = "0") int reportType,
            @RequestParam(defaultValue = "false") boolean approved,
            @RequestParam(defaultValue = "0") double clDebit,
            @RequestParam(defaultValue = "0") double clCredit,
            @RequestParam(defaultValue = "") String customerGroups,
            @RequestParam(defaultValue = "") String tradeTypes,
            @RequestParam(defaultValue = "0") int customGroupId,
            @RequestParam(defaultValue = "") String branches) {
        if (!Set.of(0, 1, 2, 3).contains(tranTypes) || !Set.of(0, 1, 2, 3).contains(reportType)) throw new IllegalArgumentException("Invalid report type");
        for (String ids : List.of(customerGroups, tradeTypes, branches))
            if (!ids.isBlank() && !ids.matches("[0-9]+(,[0-9]+)*")) throw new IllegalArgumentException("Invalid selected groups, trade types or branches");
        if (customGroupId < 0) throw new IllegalArgumentException("Invalid custom group");
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        return repository.load(u, context.currentFinancialYearId(), fromDate, toDate, tranTypes, reportType, approved, clDebit, clCredit,
                customerGroups, tradeTypes, customGroupId, branches);
    }
}
