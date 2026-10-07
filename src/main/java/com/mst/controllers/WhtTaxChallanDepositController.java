package com.mst.controllers;

import com.mst.services.WhtTaxChallanDepositService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Screen 37 "With Holding Tax Deposit Challan" - Architecture.WinApp.Account_Definition.frmWhtTaxChallanDeposit.
 * Page /accounts/wht-tax-challan-deposit, API /api/accounts/wht-tax-challan-deposit/**. Rules live in
 * WhtTaxChallanDepositService (rights of screen 37; tenancy from the session, never from a parameter).
 */
@Controller
public class WhtTaxChallanDepositController {

    private static final String API = "/api/accounts/wht-tax-challan-deposit";

    @Autowired private WhtTaxChallanDepositService service;

    @GetMapping("/accounts/wht-tax-challan-deposit")
    public String page(Model model) {
        service.user("View");
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("screenId", WhtTaxChallanDepositService.SCREEN_ID);
        return "accounts/wht_tax_challan_deposit";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> service.whtChallanSetup(), "Load failed."); }

    @GetMapping(API + "/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return call(() -> service.whtChallanDocNo(), "Load failed."); }

    @GetMapping(API + "/pending") @ResponseBody
    public ResponseEntity<?> pending(@RequestParam("partyAccountId") int partyAccountId,
                                     @RequestParam(value = "branchId", defaultValue = "0") int branchId,
                                     @RequestParam(value = "projectId", defaultValue = "0") int projectId) {
        return call(() -> service.whtChallanPending(partyAccountId, branchId, projectId), "Load failed.");
    }

    @GetMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history() { return call(() -> service.whtChallanHistory(), "Load failed."); }

    @GetMapping(API + "/voucher") @ResponseBody
    public ResponseEntity<?> voucher(@RequestParam("id") int id) { return call(() -> service.whtChallanVoucher(id), "Load failed."); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> service.saveWhtChallan(body), "Save failed."); }

    private interface Call { Object run() throws Exception; }

    private static ResponseEntity<?> call(Call c, String fallback) {
        try { return ResponseEntity.ok(c.run()); }
        catch (IllegalArgumentException | IllegalStateException e) { return ResponseEntity.badRequest().body(fail(root(e, fallback))); }
        catch (AccessDeniedException e) { return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied."))); }
        catch (Exception e) { return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback))); }
    }

    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
