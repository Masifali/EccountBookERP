package com.mst.controllers;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.mst.services.BankReconUploadDesktopService;

/**
 * JSON endpoints of screen 884 (frmBankReconciliationUploadExcelSheet) used by
 * accounts/bank_reconciliation_upload_excel.html. The page itself is still served by
 * AccountsModuleViewController (/accounts/bank-reconciliation-upload-excel).
 */
@RestController
@RequestMapping("/accounts/api/bank-reconciliation-upload")
public class BankReconUploadDesktopController {

    private final BankReconUploadDesktopService service;

    public BankReconUploadDesktopController(BankReconUploadDesktopService service) {
        this.service = service;
    }

    @GetMapping("/lookups")
    public Map<String, Object> lookups() {
        return service.lookups();
    }

    @GetMapping("/bank-accounts")
    public List<Map<String, Object>> bankAccounts() {
        return service.bankAccounts();
    }

    @GetMapping("/history")
    public Map<String, Object> history(@RequestParam(value = "bankAccountId", required = false, defaultValue = "0") int bankAccountId,
                                       @RequestParam(value = "dateType", required = false, defaultValue = "entry") String dateType,
                                       @RequestParam(value = "from", required = false) String from,
                                       @RequestParam(value = "to", required = false) String to) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", service.formHistory(bankAccountId, dateType, from, to));
            res.put("success", true);
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", e.getMessage());
        }
        return res;
    }

    /** Body {bankAccountId, bankAccountText, updateMode, rows:[...], removed:[...]}. */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();
        try {
            Object b = body.get("bankAccountId");
            int bankAccountId = b instanceof Number ? ((Number) b).intValue() : 0;
            res.put("message", service.saveList(bankAccountId,
                    body.get("bankAccountText") == null ? "" : String.valueOf(body.get("bankAccountText")),
                    Boolean.TRUE.equals(body.get("updateMode")), maps(body.get("rows")), maps(body.get("removed"))));
            res.put("success", true);
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", e.getMessage());
        }
        return res;
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> maps(Object o) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) for (Object e : (List<Object>) o) if (e instanceof Map) out.add((Map<String, Object>) e);
        return out;
    }
}
