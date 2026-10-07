package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.Inact1AccountsReconciliationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Screen 32 "Accounts Reconcilation" (Architecture.WinApp.Reconciliation.AccountsReconciliation).
 *   GET  /accounts/reconciliation/accounts-reconciliation               the form
 *   GET  /api/accounts/accounts-reconciliation/load                     DayBookVoucher_Load
 *   GET  /api/accounts/accounts-reconciliation/generate-code            GenerateCode
 *   GET  /api/accounts/accounts-reconciliation/balances                 DetailGridFill
 *   GET  /api/accounts/accounts-reconciliation/by-doc-no                txtDocNo_Leave
 *   GET  /api/accounts/accounts-reconciliation/by-id                    ReadById_Update
 *   POST /api/accounts/accounts-reconciliation/save                     Insert (Save / Update)
 * The form checks no Save / Update right (opening the screen is the right): the View right of screen 32 is enforced.
 */
@Controller
public class Inact1AccountsReconciliationController {
    private static final int SCREEN_ID = 32;
    private static final String API = "/api/accounts/accounts-reconciliation";

    @Autowired private Inact1AccountsReconciliationService service;
    @Autowired private CurrentUserContext context;
    @Autowired private DesktopReportRights rights;

    private void gate() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
    }

    @GetMapping("/accounts/reconciliation/accounts-reconciliation")
    public String page(Model model) {
        gate();
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", "Accounts Reconciliation");
        return "accounts/inact1_accounts_reconciliation";
    }

    @GetMapping(API + "/load") @ResponseBody
    public ResponseEntity<?> load() { return run(() -> { gate(); return service.load(); }); }

    @GetMapping(API + "/generate-code") @ResponseBody
    public ResponseEntity<?> generateCode() { return run(() -> { gate(); return service.generateCode(); }); }

    @GetMapping(API + "/balances") @ResponseBody
    public ResponseEntity<?> balances(@RequestParam(required = false) String docDate, @RequestParam(required = false) String groupAccountId,
                                      @RequestParam(required = false) String id) {
        return run(() -> { gate(); return service.balances(docDate, groupAccountId, id); });
    }

    @GetMapping(API + "/by-doc-no") @ResponseBody
    public ResponseEntity<?> byDocNo(@RequestParam(required = false) String docNo) { return run(() -> { gate(); return service.idByDocNo(docNo); }); }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam int id) { return run(() -> { gate(); return service.readById(id); }); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> { gate(); return service.save(body); }); }

    private ResponseEntity<?> run(Supplier<Object> work) {
        Map<String, Object> m = new LinkedHashMap<>();
        try {
            m.put("success", true);
            m.put("data", work.get());
            return ResponseEntity.ok(m);
        } catch (AccessDeniedException e) {
            m.clear(); m.put("success", false); m.put("message", e.getMessage());
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(m);
        } catch (Exception e) {
            Throwable c = e;
            while (c.getCause() != null && c.getCause() != c) c = c.getCause();
            m.clear(); m.put("success", false); m.put("message", c.getMessage() == null ? e.getClass().getSimpleName() : c.getMessage());
            boolean bad = e instanceof IllegalArgumentException || e instanceof IllegalStateException;
            return ResponseEntity.status(bad ? HttpStatus.OK : HttpStatus.INTERNAL_SERVER_ERROR).body(m);
        }
    }
}
