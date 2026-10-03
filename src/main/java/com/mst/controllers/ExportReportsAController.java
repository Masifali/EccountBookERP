package com.mst.controllers;

import com.mst.services.ExportReportsAService;
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
 * Module 17 "Export Reports" - group A. One page and one API prefix per desktop report form:
 *
 *   /export/receivable-by-due-date        235  ExportReceivableByDueDateRegisterA   api /api/export/receivable-by-due-date
 *   /export/contract-schedule-periodic    236  frmExportContractSchedulePeriodicB   api /api/export/contract-schedule-periodic
 *   /export/shipment-costing              238  ExImShipmentCosting                  api /api/export/shipment-costing
 *   /export/shipments-document-status     239  frmExportPendingWorksRegister        api /api/export/shipments-document-status
 *   /export/detail-history                249  ExportDetailHistoryReport            api /api/export/detail-history
 *   /export/delivery-order-report         250  DeliveryOrderHistory                 api /api/export/delivery-order-report
 *   /export/contract-register             251  ExImSaleContractRegister             api /api/export/contract-register
 *   /api/export/reports-a/contract-detail      frmContractDetailByContractId popup (ContractNo links of 235 / 251)
 *
 * No parameter carries tenancy or a user id; the service derives them from the session.
 */
@Controller
public class ExportReportsAController {

    private static final String RCV = "/api/export/receivable-by-due-date";
    private static final String SCH = "/api/export/contract-schedule-periodic";
    private static final String CST = "/api/export/shipment-costing";
    private static final String PND = "/api/export/shipments-document-status";
    private static final String DTH = "/api/export/detail-history";
    private static final String DOR = "/api/export/delivery-order-report";
    private static final String REG = "/api/export/contract-register";

    @Autowired private ExportReportsAService svc;

    // ------------------------------------------------------------------ pages

    @GetMapping("/export/receivable-by-due-date")
    public String receivablePage(Model model) { model.addAttribute("activeMenu", "export"); return "export/receivable_by_due_date"; }

    @GetMapping("/export/contract-schedule-periodic")
    public String schedulePage(Model model) { model.addAttribute("activeMenu", "export"); return "export/contract_schedule_periodic"; }

    @GetMapping("/export/shipment-costing")
    public String costingPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/shipment_costing"; }

    @GetMapping("/export/shipments-document-status")
    public String pendingPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/shipments_document_status"; }

    @GetMapping("/export/detail-history")
    public String detailHistoryPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/detail_history"; }

    @GetMapping("/export/delivery-order-report")
    public String deliveryOrderPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/delivery_order_report"; }

    @GetMapping("/export/contract-register")
    public String contractRegisterPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/contract_register"; }

    // ------------------------------------------------------------------ shared popup

    @GetMapping("/api/export/reports-a/contract-detail") @ResponseBody
    public ResponseEntity<?> contractDetail(@RequestParam("contractId") int contractId,
                                            @RequestParam(value = "screenId", defaultValue = "251") int screenId) {
        return call(() -> svc.contractDetail(screenId == 235 ? 235 : 251, contractId), "Load failed.");
    }

    // ------------------------------------------------------------------ 235

    @GetMapping(RCV + "/setup") @ResponseBody
    public ResponseEntity<?> rcvSetup() { return call(() -> svc.receivableSetup(), "Error occurred during database call."); }

    @GetMapping(RCV + "/combos") @ResponseBody
    public ResponseEntity<?> rcvCombos() { return call(() -> svc.receivableCombos(), "Load failed."); }

    @PostMapping(RCV + "/show") @ResponseBody
    public ResponseEntity<?> rcvShow(@RequestBody Map<String, Object> body) { return call(() -> svc.receivableShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 236

    @GetMapping(SCH + "/setup") @ResponseBody
    public ResponseEntity<?> schSetup() { return call(() -> svc.scheduleSetup(), "Error occurred during database call."); }

    @PostMapping(SCH + "/show") @ResponseBody
    public ResponseEntity<?> schShow(@RequestBody Map<String, Object> body) { return call(() -> svc.scheduleShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 238

    @GetMapping(CST + "/setup") @ResponseBody
    public ResponseEntity<?> cstSetup() { return call(() -> svc.costingSetup(), "Error occurred during database call."); }

    @GetMapping(CST + "/show") @ResponseBody
    public ResponseEntity<?> cstShow(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId) {
        return call(() -> svc.costingShow(invoiceId), "Load failed.");
    }

    // ------------------------------------------------------------------ 239

    @GetMapping(PND + "/setup") @ResponseBody
    public ResponseEntity<?> pndSetup() { return call(() -> svc.pendingSetup(), "Error occurred during database call."); }

    @GetMapping(PND + "/combos") @ResponseBody
    public ResponseEntity<?> pndCombos() { return call(() -> svc.pendingCombos(), "Load failed."); }

    @PostMapping(PND + "/show") @ResponseBody
    public ResponseEntity<?> pndShow(@RequestBody Map<String, Object> body) { return call(() -> svc.pendingShow(body), "Load failed."); }

    @PostMapping(PND + "/status") @ResponseBody
    public ResponseEntity<?> pndStatus(@RequestBody Map<String, Object> body) {
        Object v = body.get("invoiceId");
        int id;
        try { id = v == null ? 0 : (int) Double.parseDouble(String.valueOf(v)); } catch (NumberFormatException e) { id = 0; }
        final int invoiceId = id;
        final String status = String.valueOf(body.get("status"));
        return call(() -> svc.pendingStatusUpdate(invoiceId, status), "Update failed.");
    }

    // ------------------------------------------------------------------ 249

    @GetMapping(DTH + "/setup") @ResponseBody
    public ResponseEntity<?> dthSetup() { return call(() -> svc.detailHistorySetup(), "Error occurred during database call."); }

    @PostMapping(DTH + "/show") @ResponseBody
    public ResponseEntity<?> dthShow(@RequestBody Map<String, Object> body) { return call(() -> svc.detailHistoryShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 250

    @GetMapping(DOR + "/setup") @ResponseBody
    public ResponseEntity<?> dorSetup() { return call(() -> svc.doSetup(), "Error occurred during database call."); }

    @GetMapping(DOR + "/combos") @ResponseBody
    public ResponseEntity<?> dorCombos() { return call(() -> svc.doCombosRefresh(), "Load failed."); }

    @PostMapping(DOR + "/show") @ResponseBody
    public ResponseEntity<?> dorShow(@RequestBody Map<String, Object> body) { return call(() -> svc.doShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 251

    @GetMapping(REG + "/setup") @ResponseBody
    public ResponseEntity<?> regSetup() { return call(() -> svc.registerSetup(), "Error occurred during database call."); }

    @GetMapping(REG + "/combos") @ResponseBody
    public ResponseEntity<?> regCombos() { return call(() -> svc.registerCombos(), "Load failed."); }

    @PostMapping(REG + "/card") @ResponseBody
    public ResponseEntity<?> regCard(@RequestBody Map<String, Object> body) { return call(() -> svc.registerCard(body), "Load failed."); }

    @PostMapping(REG + "/summary") @ResponseBody
    public ResponseEntity<?> regSummary(@RequestBody Map<String, Object> body) { return call(() -> svc.registerSummary(body), "Load failed."); }

    @PostMapping(REG + "/product-wise") @ResponseBody
    public ResponseEntity<?> regProductWise(@RequestBody Map<String, Object> body) { return call(() -> svc.registerProductWise(body), "Load failed."); }

    @PostMapping(REG + "/status") @ResponseBody
    public ResponseEntity<?> regStatus(@RequestBody Map<String, Object> body) { return call(() -> svc.registerStatusChange(body), "Update failed."); }

    // ------------------------------------------------------------------ plumbing

    private interface Call { Object run() throws Exception; }

    /** 400 for the form's own validation text, 403 for a missing right, 500 with the innermost message otherwise. */
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
