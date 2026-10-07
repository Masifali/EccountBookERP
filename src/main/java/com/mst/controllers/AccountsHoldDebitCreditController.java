package com.mst.controllers;

import java.util.HashMap;
import java.util.Map;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import com.mst.services.AccountsHoldDebitCreditDesktopService;

/** Screen 12 - Debit Credit Hold (desktop form AccountsHoldForDebitOrCredit). */
@Controller
@RequestMapping("/accounts/hold_debit_credit")
public class AccountsHoldDebitCreditController {

    private final AccountsHoldDebitCreditDesktopService service;

    public AccountsHoldDebitCreditController(AccountsHoldDebitCreditDesktopService service) {
        this.service = service;
    }

    @GetMapping
    public String view(Model model) {
        model.addAttribute("activeMenu", "accounts");
        return "accounts/hold_debit_credit";
    }

    /** AccountsHoldForDebitOrCredit_Load: rights, GridHistoryfill + AccountTitleCombo data. */
    @GetMapping("/init")
    @ResponseBody
    public Map<String, Object> init() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rights", service.rights());
            res.put("accounts", service.accountTitles());
            res.put("success", true);
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", rootMessage(e));
        }
        return res;
    }

    @GetMapping("/history")
    @ResponseBody
    public Map<String, Object> history() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", service.history());
            res.put("success", true);
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", rootMessage(e));
        }
        return res;
    }

    @GetMapping("/{id}")
    @ResponseBody
    public Map<String, Object> read(@PathVariable("id") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("row", service.readById(id));
            res.put("success", true);
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", rootMessage(e));
        }
        return res;
    }

    @PostMapping("/save")
    @ResponseBody
    public Map<String, Object> save(@RequestBody Map<String, Object> b) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("message", service.save(toInt(b.get("id")), toInt(b.get("coaAccountId")),
                    toBool(b.get("debitHold")), toBool(b.get("creditHold")),
                    str(b.get("debitReason")), str(b.get("creditReason"))));
            res.put("success", true);
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", rootMessage(e));
        }
        return res;
    }

    private static String rootMessage(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() != null ? t.getMessage() : e.getMessage();
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        try { return o == null ? 0 : Integer.parseInt(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static boolean toBool(Object o) {
        return o instanceof Boolean ? (Boolean) o : o != null && Boolean.parseBoolean(String.valueOf(o));
    }
}
