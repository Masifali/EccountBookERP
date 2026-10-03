package com.mst.controllers;

import java.math.BigDecimal;
import java.time.LocalDate;
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

import com.mst.services.banking.BankBalanceManualEntryService;

/**
 * Screen 705 "Bank Balance Entry" (desktop Account_Definition.BankBalanceManualEntry) - API of the page
 * /accounts/bank-balance-manual-entry. Every call goes through BankBalanceManualEntryService, i.e. the
 * desktop's procedures (SpStaticColumnNames 'SourceBy', Sp_Accounts_CashBankBalancesSummery_Rpt,
 * USP_AcBankBalanceManualEntry_Insert). Tenancy comes from CurrentUserContext only.
 *
 * The old /api/banking/bank-manual-balance/* endpoints (BankingManagementRestController) wrote to a table
 * "BankManualBalance" they created themselves; the desktop never touches it. The page no longer calls them.
 */
@RestController
@RequestMapping("/accounts/api/banking/bank-balance-manual-entry")
public class BankBalanceManualEntryController {

    private final BankBalanceManualEntryService service;

    public BankBalanceManualEntryController(BankBalanceManualEntryService service) {
        this.service = service;
    }

    /** FrmBankBalancesRpt_Load: rights, Source value list, amount decimals, then SummeryGrd for today. */
    @GetMapping("/init")
    public Map<String, Object> init() {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rights", service.rights());
            res.put("sourceBy", service.sourceBy());
            res.put("amountDecimals", service.amountDecimals());
            res.put("today", LocalDate.now().toString());
            res.put("success", true);
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", rootMessage(e));
        }
        return res;
    }

    /** SummeryGrd (Load / Show / New / after Save). */
    @GetMapping("/summary")
    public Map<String, Object> summary(@RequestParam(value = "balanceDate", required = false) String balanceDate) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("rows", service.summary(date(balanceDate)));
            res.put("success", true);
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", rootMessage(e));
        }
        return res;
    }

    /** btnSave_Click. Body: {balanceDate, rows:[{accountId, time, bankBalance, source, sourceText}]}. */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();
        try {
            List<BankBalanceManualEntryService.Row> rows = new ArrayList<>();
            Object list = body.get("rows");
            if (list instanceof List) {
                for (Object o : (List<?>) list) {
                    if (!(o instanceof Map)) continue;
                    Map<?, ?> m = (Map<?, ?>) o;
                    BankBalanceManualEntryService.Row r = new BankBalanceManualEntryService.Row();
                    r.accountId = toInt(m.get("accountId"));
                    r.time = m.get("time") == null ? null : String.valueOf(m.get("time"));
                    r.bankBalance = toDec(m.get("bankBalance"));
                    r.source = toInt(m.get("source"));
                    r.sourceText = m.get("sourceText") == null ? "" : String.valueOf(m.get("sourceText"));
                    rows.add(r);
                }
            }
            res.put("id", service.save(date(body.get("balanceDate") == null ? null : String.valueOf(body.get("balanceDate"))), rows));
            res.put("success", true);
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", rootMessage(e));
        }
        return res;
    }

    private static LocalDate date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        String v = s.trim();
        return LocalDate.parse(v.length() > 10 ? v.substring(0, 10) : v);
    }

    /** Conversion.ToInt: anything not a number is 0. */
    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    /** Conversion.ToDecimal: anything not a number is 0. */
    private static BigDecimal toDec(Object o) {
        if (o == null) return BigDecimal.ZERO;
        try { return new BigDecimal(String.valueOf(o).trim().replace(",", "")); } catch (Exception e) { return BigDecimal.ZERO; }
    }

    private static String rootMessage(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() != null ? t.getMessage() : e.getMessage();
    }
}
