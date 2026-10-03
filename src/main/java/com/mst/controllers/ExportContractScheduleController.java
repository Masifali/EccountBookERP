package com.mst.controllers;

import com.mst.services.ExportContractScheduleService;
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
 * Screen 216 "Export Contract Schedule" - frmSaleContractSchedule.cs (Architecture.WinApp.Export),
 * DocumentTypeId 244 (ScreenName FrmExportSalesContractSchedule). Page /export/contract-schedule,
 * API /api/export/contract-schedule/... . Tenancy and the user come from the session; the service
 * checks the rights of ScreenDefinition 216.
 */
@Controller
public class ExportContractScheduleController {

    private static final String API = "/api/export/contract-schedule";

    @Autowired private ExportContractScheduleService svc;

    @GetMapping("/export/contract-schedule")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/contract_schedule";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping(API + "/refresh-main") @ResponseBody
    public ResponseEntity<?> refreshMain() { return call(() -> svc.refreshMain(), "Load failed."); }

    @GetMapping(API + "/pending") @ResponseBody
    public ResponseEntity<?> pending(@RequestParam(value = "actionId", defaultValue = "1") int actionId,
                                     @RequestParam(value = "noOfRecords", defaultValue = "0") int noOfRecords) {
        return call(() -> svc.pendingApi(actionId, noOfRecords), "Load failed.");
    }

    @GetMapping(API + "/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos() { return call(() -> svc.historyCombosApi(), "Load failed."); }

    @GetMapping(API + "/contract") @ResponseBody
    public ResponseEntity<?> contract(@RequestParam("contractId") int contractId) { return call(() -> svc.contract(contractId), "Load failed."); }

    @GetMapping(API + "/load-dates") @ResponseBody
    public ResponseEntity<?> loadDates(@RequestParam("contractId") int contractId,
                                       @RequestParam(value = "detailRecIds", required = false) String detailRecIds) {
        return call(() -> svc.loadDatesApi(contractId, detailRecIds), "Load failed.");
    }

    @GetMapping(API + "/items") @ResponseBody
    public ResponseEntity<?> items(@RequestParam("contractId") int contractId) { return call(() -> svc.itemsApi(contractId), "Load failed."); }

    @GetMapping(API + "/uom") @ResponseBody
    public ResponseEntity<?> uom(@RequestParam("itemId") int itemId) { return call(() -> svc.uom(itemId), "Load failed."); }

    @GetMapping(API + "/schedule-mton") @ResponseBody
    public ResponseEntity<?> scheduleMton(@RequestParam("contractId") int contractId, @RequestParam("scheduleId") int scheduleId) {
        return call(() -> svc.scheduleMton(contractId, scheduleId), "Load failed.");
    }

    @GetMapping(API + "/custom-groups") @ResponseBody
    public ResponseEntity<?> customGroups(@RequestParam(value = "supplierCustomerId", defaultValue = "0") int supplierCustomerId) {
        return call(() -> svc.customGroupsApi(supplierCustomerId), "Load failed.");
    }

    @GetMapping(API + "/plants") @ResponseBody
    public ResponseEntity<?> plants() { return call(() -> svc.plants(), "Load failed."); }

    @PostMapping(API + "/save-main") @ResponseBody
    public ResponseEntity<?> saveMain(@RequestBody Map<String, Object> body) { return call(() -> svc.saveMain(body), "Save failed."); }

    @PostMapping(API + "/save-detail") @ResponseBody
    public ResponseEntity<?> saveDetail(@RequestBody Map<String, Object> body) { return call(() -> svc.saveDetail(body), "Save failed."); }

    @PostMapping(API + "/custom-group-save") @ResponseBody
    public ResponseEntity<?> customGroupSave(@RequestBody Map<String, Object> body) { return call(() -> svc.customGroupSave(body), "Save failed."); }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return call(() -> svc.history(body), "Load failed."); }

    @PostMapping(API + "/department") @ResponseBody
    public ResponseEntity<?> department(@RequestBody Map<String, Object> body) { return call(() -> svc.department(body), "Load failed."); }

    @PostMapping(API + "/save-department") @ResponseBody
    public ResponseEntity<?> saveDepartment(@RequestBody Map<String, Object> body) { return call(() -> svc.saveDepartment(body), "Save failed."); }

    // ------------------------------------------------------------------ plumbing

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
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
