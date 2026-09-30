package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.repositories.PayablesSubsidiaryBillsRepository;
import com.mst.security.CurrentUserContext;
import java.time.LocalDate;
import java.util.*;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

/**
 * R3 2026-09-30. Pages and data for the "Subsidiary Payables Report" and "Bills Payables Report" hub entries, which were
 * static mock-ups with invented rows. The explicit GET mappings take precedence over AccountsModuleViewController's
 * /accounts/reports/{reportType} pattern, so that shared controller is not edited.
 */
@Controller
public class PayablesSubsidiaryBillsController {
    private final PayablesSubsidiaryBillsRepository repository;
    private final CurrentUserContext context;

    public PayablesSubsidiaryBillsController(PayablesSubsidiaryBillsRepository repository, CurrentUserContext context) {
        this.repository = repository;
        this.context = context;
    }

    /** Screen 89 SubsidiaryPayableReceiveables ("Payable Control Account Wise"). */
    @GetMapping("/accounts/reports/subsidiary-payables-report")
    public String subsidiaryPage(Model model) {
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", "Subsidiary Payable Receiveables");
        return "accounts/reports/subsidiary_payables_report";
    }

    /** Account_Reports.PayablesWithBillAmount ("Payables Report", 141-Print). */
    @GetMapping("/accounts/reports/bills-payables-report")
    public String billsPage(Model model) {
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", "Payables Report");
        return "accounts/reports/bills_payables_report";
    }

    @GetMapping("/api/accounts/payables-sub-bills/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() {
        try {
            return ResponseEntity.ok(repository.lookups(context.requireAccountingUser(), context.currentFinancialYearId()));
        } catch (RuntimeException e) {
            return fail(e);
        }
    }

    /** SubsidiaryPayableReceiveables.GetData - the form reads only when ControlAccountId > 0. */
    @GetMapping("/api/accounts/subsidiary-payables")
    @ResponseBody
    public ResponseEntity<?> subsidiary(@RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
                                        @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate,
                                        @RequestParam(defaultValue = "0") int controlAccountId,
                                        @RequestParam(defaultValue = "1") int partyTypeId) {
        try {
            if (controlAccountId <= 0) return ResponseEntity.ok(Collections.emptyList());
            if (partyTypeId < 0 || partyTypeId > 2) throw new IllegalArgumentException("Invalid party type");
            return ResponseEntity.ok(repository.subsidiary(context.requireAccountingUser(), fromDate, toDate, controlAccountId, partyTypeId));
        } catch (RuntimeException e) {
            return fail(e);
        }
    }

    /** PayablesWithBillAmount.GetData. */
    @GetMapping("/api/accounts/bills-payables")
    @ResponseBody
    public ResponseEntity<?> bills(@RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
                                   @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate,
                                   @RequestParam(defaultValue = "") String balanceFrom,
                                   @RequestParam(defaultValue = "") String balanceTo,
                                   @RequestParam(defaultValue = "") String parentAccountCodes,
                                   @RequestParam(defaultValue = "") String branches) {
        try {
            UserAccount u = context.requireAccountingUser();
            String codes = parentAccountCodes.trim();
            if (!codes.isEmpty() && !codes.matches("[0-9A-Za-z.\\-]+(,[0-9A-Za-z.\\-]+)*")) throw new IllegalArgumentException("Invalid group accounts");
            String branchIds = branchIdsByFeature(repository.features(u), branches);
            return ResponseEntity.ok(repository.bills(u, context.currentFinancialYearId(), fromDate, toDate,
                    toInt(balanceFrom), toInt(balanceTo), codes, branchIds));
        } catch (RuntimeException e) {
            return fail(e);
        }
    }

    /** Conversion.ToInt on txtBalanceFrom / txtBalanceTo: blank or non-numeric text is 0. */
    private static int toInt(String s) {
        try { return s == null || s.isBlank() ? 0 : (int) Math.round(Double.parseDouble(s.trim())); } catch (NumberFormatException e) { return 0; }
    }

    /** InfragisticsHelper.GetBranchesIdsByFeature: consolidated (17 and 18) -> ",id,id" of the checked branches (or "");
     *  branch feature only -> the one selected id, "Please Select Branch first!" when none; no branch feature -> "". */
    private static String branchIdsByFeature(List<Integer> features, String requested) {
        boolean bf = features.contains(17), cons = features.contains(18);
        List<Integer> ids = new ArrayList<>();
        for (String s : (requested == null ? "" : requested).split(",")) {
            s = s.trim();
            if (!s.isEmpty()) ids.add(Integer.parseInt(s));
        }
        if (bf && cons) {
            StringBuilder sb = new StringBuilder();
            for (Integer id : ids) sb.append(',').append(id);
            return sb.toString();
        }
        if (bf) {
            if (ids.isEmpty() || ids.get(0) == 0) throw new IllegalArgumentException("Please Select Branch first!");
            return String.valueOf(ids.get(0));
        }
        return "";
    }

    private static ResponseEntity<Map<String, Object>> fail(RuntimeException e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("message", e.getMessage() == null ? e.getClass().getSimpleName() : e.getMessage());
        return ResponseEntity.status(e instanceof IllegalArgumentException ? 400 : 500).body(m);
    }
}
