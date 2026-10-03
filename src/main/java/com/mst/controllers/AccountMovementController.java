package com.mst.controllers;

import com.mst.services.AccountMovementService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Admin Panel -> "Account Movement" (DashboardNew.cs:3151 btnAccountMovement_Click -> AccountToAccountTransfer).
 *
 *   /admin/account-movement                       the form
 *   GET  /api/admin/account-movement/load         AcfrmAcAllocation_Load (3rd level accounts + global account list)
 *   GET  /api/admin/account-movement/accounts     btnRefresh_Click: GlobalServicesDbCall("AccountsWithCustomGroupId")
 *   GET  /api/admin/account-movement/third-level  btnRefresh_Click: ThirdLevelAccountsFill
 *   POST /api/admin/account-movement/update       btnUpdate_Click -> usp_AccountMoveToAnotherParentGroup
 *
 * Every call is gated on RoleName == "Admin" in AccountMovementService.admin(), the desktop's only gate.
 */
@Controller
public class AccountMovementController {

    private static final String API = "/api/admin/account-movement";

    @Autowired private AccountMovementService service;

    @GetMapping("/admin/account-movement")
    public String page(Model model) {
        service.admin();
        model.addAttribute("activeMenu", "apps");
        return "admin/account_movement";
    }

    @GetMapping(API + "/load") @ResponseBody
    public ResponseEntity<?> load() { return run(service::load, "Load failed."); }

    @GetMapping(API + "/accounts") @ResponseBody
    public ResponseEntity<?> accounts() { return run(service::accounts, "Load failed."); }

    @GetMapping(API + "/third-level") @ResponseBody
    public ResponseEntity<?> thirdLevel() { return run(service::thirdLevelAccounts, "Load failed."); }

    @PostMapping(API + "/update") @ResponseBody
    public ResponseEntity<?> update(@RequestBody Map<String, Object> body) { return run(() -> service.update(body), "Update failed."); }

    // ------------------------------------------------------------------ plumbing

    private static ResponseEntity<?> run(Supplier<Object> call, String fallback) {
        try {
            return ResponseEntity.ok(call.get());
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(root(e, fallback)));
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied.")));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback)));
        }
    }

    /** The desktop shows ex.Message - for a RAISERROR that is the procedure's own text. */
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
