package com.mst.controllers;

import com.mst.services.UserDefineService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
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
import java.util.function.Supplier;

/**
 * Admin Panel -> "User Rights" (DashboardNew btnadminpaneluserright_Click -> frmUserRights) and the
 * "Branches Allocation To User" form its toolbar opens (Lookups.frmBranchesAllocationToUser).
 *
 *   /user-management/user-define                    frmUserRights (Define User / ApplicationsAllocateToUser, User Define / User History)
 *   /user-management/branches-allocation-to-user    frmBranchesAllocationToUser
 *
 * API under /api/user-define. Every call is gated on RoleName == "Admin" in UserDefineService.admin().
 */
@Controller
public class UserDefineController {

    private static final String API = "/api/user-define";

    @Autowired private UserDefineService service;

    @GetMapping("/user-management/user-define")
    public String page(Model model) {
        service.admin();
        model.addAttribute("activeMenu", "apps");
        return "userAccounts/user_define";
    }

    @GetMapping("/user-management/branches-allocation-to-user")
    public String branchesPage(Model model) {
        service.admin();
        model.addAttribute("activeMenu", "apps");
        return "userAccounts/branches_allocation_to_user";
    }

    // ------------------------------------------------------------------ Define User / User History

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return run(service::setup, "Load failed."); }

    @GetMapping(API + "/users") @ResponseBody
    public ResponseEntity<?> users() { return run(service::users, "Load failed."); }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return run(() -> service.user(id), "Load failed."); }

    @GetMapping(API + "/profile-image")
    public ResponseEntity<?> profileImage(@RequestParam("id") int id) {
        try {
            byte[] bytes = service.profileImage(id);
            MediaType type = MediaType.APPLICATION_OCTET_STREAM;
            if (bytes.length > 3 && (bytes[0] & 0xFF) == 0x89 && bytes[1] == 'P') type = MediaType.IMAGE_PNG;
            else if (bytes.length > 2 && (bytes[0] & 0xFF) == 0xFF && (bytes[1] & 0xFF) == 0xD8) type = MediaType.IMAGE_JPEG;
            return ResponseEntity.ok().contentType(type).header("Cache-Control", "no-store").body(bytes);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.notFound().build();
        } catch (Exception e) {
            return error(e, "Picture could not be read.");
        }
    }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> service.save(body), "Save failed."); }

    // ------------------------------------------------------------------ ApplicationsAllocateToUser tab

    @GetMapping(API + "/apps/users") @ResponseBody
    public ResponseEntity<?> appUsers() { return run(service::appUsers, "Load failed."); }

    @GetMapping(API + "/apps/companies") @ResponseBody
    public ResponseEntity<?> appCompanies(@RequestParam("userId") int userId) { return run(() -> service.appCompanies(userId), "Load failed."); }

    @GetMapping(API + "/apps/grids") @ResponseBody
    public ResponseEntity<?> appGrids(@RequestParam("userId") int userId, @RequestParam("companyId") int companyId) {
        return run(() -> service.appGrids(userId, companyId), "Load failed.");
    }

    @PostMapping(API + "/apps/save") @ResponseBody
    public ResponseEntity<?> appSave(@RequestBody Map<String, Object> body) { return run(() -> service.saveApps(body), "Save failed."); }

    // ------------------------------------------------------------------ frmBranchesAllocationToUser

    @GetMapping(API + "/branches/users") @ResponseBody
    public ResponseEntity<?> branchUsers() { return run(service::branchUsers, "Load failed."); }

    @GetMapping(API + "/branches/grids") @ResponseBody
    public ResponseEntity<?> branchGrids(@RequestParam("userId") int userId) { return run(() -> service.branchGrids(userId), "Load failed."); }

    @PostMapping(API + "/branches/allocate") @ResponseBody
    public ResponseEntity<?> branchAllocate(@RequestBody Map<String, Object> body) { return run(() -> service.allocateBranches(body), "Save failed."); }

    @PostMapping(API + "/branches/deallocate") @ResponseBody
    public ResponseEntity<?> branchDeallocate(@RequestBody Map<String, Object> body) { return run(() -> service.deallocateBranches(body), "Save failed."); }

    // ------------------------------------------------------------------ plumbing

    private static ResponseEntity<?> run(Supplier<Object> call, String fallback) {
        try {
            return ResponseEntity.ok(call.get());
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(root(e, fallback)));
        } catch (Exception e) {
            return error(e, fallback);
        }
    }

    /** The desktop shows ex.Message - for a RAISERROR that is the procedure's own text (user limit, BranchesId, ...). */
    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static ResponseEntity<Map<String, Object>> error(Exception e, String fallback) {
        if (e instanceof AccessDeniedException) return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied.")));
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback)));
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
