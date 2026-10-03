package com.mst.controllers;

import com.mst.services.ExportReportsCService;
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
 * Export report screens, agent C (App 8 "Export"; module 133 "Export Customise Report" rows 241-246,
 * 256-258, plus 203 of module 11 and 859 of module 136). One page + one API prefix per screen:
 *
 *   /export/fcy-receipts-summary-register              241  FcyReceiptsSummaryRegister           "5017 Fcy Receipts Report"
 *   /export/shipment-data-for-brokery-tax              242  ShipmentdataForBrokeryTax            "Shipment data For Brokery Tax"
 *   /export/packing-list-register                      243  PackingListRegister_Export           "Export Packing List Register"
 *   /export/pending-forwarding-for-commercial-invoice  244  PendingForwardingForCommercialInvoice
 *   /export/gd-break-up-and-realized-register          245  GDBreakUpandRealized_Register
 *   /export/fi-balance-summary                         246  FIBalanceSummary
 *   /export/commission-agent-fcy-ledger                256  CommissionAgentFcyLedger
 *   /export/ee-report-export-gd                        257  EEReport_ExportGD                    "EEReport"
 *   /export/commercial-invoice-shipments               258  Commercial_Invoice_Shipments
 *   /export/consignment-follow-up                      203  frmshippedConsignment_followup       "5009 Consignment Follow Up Report"
 *   /export/third-party-inspection-lot-tracking-report 859  frmThirdPartyInspectionLotTrackingReport
 *
 * Filters travel as a JSON body (POST .../show); nothing carries tenancy or a user id - the service
 * takes them from the session. Prints go through /api/print/by-template/{rpt}/pdf (print-rpt.js).
 */
@Controller
public class ExportReportsCController {

    private static final String FCY = "/api/export/fcy-receipts-summary-register";
    private static final String BRK = "/api/export/shipment-data-for-brokery-tax";
    private static final String PLR = "/api/export/packing-list-register";
    private static final String PFW = "/api/export/pending-forwarding-for-commercial-invoice";
    private static final String GDR = "/api/export/gd-break-up-and-realized-register";
    private static final String FIB = "/api/export/fi-balance-summary";
    private static final String CAL = "/api/export/commission-agent-fcy-ledger";
    private static final String EER = "/api/export/ee-report-export-gd";
    private static final String CIS = "/api/export/commercial-invoice-shipments";
    private static final String CFU = "/api/export/consignment-follow-up";
    private static final String LOT = "/api/export/third-party-inspection-lot-tracking-report";

    @Autowired private ExportReportsCService svc;

    // ------------------------------------------------------------------ pages

    @GetMapping("/export/fcy-receipts-summary-register")
    public String fcyPage(Model m) { m.addAttribute("activeMenu", "export"); return "export/fcy_receipts_summary_register"; }

    @GetMapping("/export/shipment-data-for-brokery-tax")
    public String brkPage(Model m) { m.addAttribute("activeMenu", "export"); return "export/shipment_data_for_brokery_tax"; }

    @GetMapping("/export/packing-list-register")
    public String plrPage(Model m) { m.addAttribute("activeMenu", "export"); return "export/packing_list_register"; }

    @GetMapping("/export/pending-forwarding-for-commercial-invoice")
    public String pfwPage(Model m) { m.addAttribute("activeMenu", "export"); return "export/pending_forwarding_for_commercial_invoice"; }

    @GetMapping("/export/gd-break-up-and-realized-register")
    public String gdrPage(Model m) { m.addAttribute("activeMenu", "export"); return "export/gd_break_up_and_realized_register"; }

    @GetMapping("/export/fi-balance-summary")
    public String fibPage(Model m) { m.addAttribute("activeMenu", "export"); return "export/fi_balance_summary"; }

    @GetMapping("/export/commission-agent-fcy-ledger")
    public String calPage(Model m) { m.addAttribute("activeMenu", "export"); return "export/commission_agent_fcy_ledger"; }

    @GetMapping("/export/ee-report-export-gd")
    public String eerPage(Model m) { m.addAttribute("activeMenu", "export"); return "export/ee_report_export_gd"; }

    @GetMapping("/export/commercial-invoice-shipments")
    public String cisPage(Model m) { m.addAttribute("activeMenu", "export"); return "export/commercial_invoice_shipments"; }

    @GetMapping("/export/consignment-follow-up")
    public String cfuPage(Model m) { m.addAttribute("activeMenu", "export"); return "export/consignment_follow_up"; }

    @GetMapping("/export/third-party-inspection-lot-tracking-report")
    public String lotPage(Model m) { m.addAttribute("activeMenu", "export"); return "export/third_party_inspection_lot_tracking_report"; }

    // ------------------------------------------------------------------ 241

    @GetMapping(FCY + "/setup") @ResponseBody
    public ResponseEntity<?> fcySetup() { return call(() -> svc.fcyReceiptsSetup(), "Error occurred during database call."); }

    @GetMapping(FCY + "/combos") @ResponseBody
    public ResponseEntity<?> fcyCombos() { return call(() -> svc.fcyReceiptsCombos(), "Load failed."); }

    @PostMapping(FCY + "/show") @ResponseBody
    public ResponseEntity<?> fcyShow(@RequestBody Map<String, Object> body) { return call(() -> svc.fcyReceiptsShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 242

    @GetMapping(BRK + "/setup") @ResponseBody
    public ResponseEntity<?> brkSetup() { return call(() -> svc.brokeryTaxSetup(), "Error occurred during database call."); }

    @PostMapping(BRK + "/show") @ResponseBody
    public ResponseEntity<?> brkShow(@RequestBody Map<String, Object> body) { return call(() -> svc.brokeryTaxShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 243

    @GetMapping(PLR + "/setup") @ResponseBody
    public ResponseEntity<?> plrSetup() { return call(() -> svc.packingListSetup(), "Error occurred during database call."); }

    @GetMapping(PLR + "/combos") @ResponseBody
    public ResponseEntity<?> plrCombos() { return call(() -> svc.packingListCombos(), "Load failed."); }

    @PostMapping(PLR + "/show") @ResponseBody
    public ResponseEntity<?> plrShow(@RequestBody Map<String, Object> body) { return call(() -> svc.packingListShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 244

    @GetMapping(PFW + "/setup") @ResponseBody
    public ResponseEntity<?> pfwSetup() { return call(() -> svc.pendingForwardingSetup(), "Error occurred during database call."); }

    @PostMapping(PFW + "/show") @ResponseBody
    public ResponseEntity<?> pfwShow(@RequestBody Map<String, Object> body) { return call(() -> svc.pendingForwardingShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 245

    @GetMapping(GDR + "/setup") @ResponseBody
    public ResponseEntity<?> gdrSetup() { return call(() -> svc.gdRealizedSetup(), "Error occurred during database call."); }

    @GetMapping(GDR + "/combos") @ResponseBody
    public ResponseEntity<?> gdrCombos() { return call(() -> svc.gdRealizedCombos(), "Load failed."); }

    @PostMapping(GDR + "/show") @ResponseBody
    public ResponseEntity<?> gdrShow(@RequestBody Map<String, Object> body) { return call(() -> svc.gdRealizedShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 246

    @GetMapping(FIB + "/setup") @ResponseBody
    public ResponseEntity<?> fibSetup() { return call(() -> svc.fiBalanceSetup(), "Error occurred during database call."); }

    @PostMapping(FIB + "/show") @ResponseBody
    public ResponseEntity<?> fibShow(@RequestBody Map<String, Object> body) { return call(() -> svc.fiBalanceShow(body), "Load failed."); }

    @GetMapping(FIB + "/detail") @ResponseBody
    public ResponseEntity<?> fibDetail(@RequestParam("id") int id) { return call(() -> svc.fiBalanceDetail(id), "Load failed."); }

    // ------------------------------------------------------------------ 256

    @GetMapping(CAL + "/setup") @ResponseBody
    public ResponseEntity<?> calSetup() { return call(() -> svc.commAgentSetup(), "Error occurred during database call."); }

    @GetMapping(CAL + "/combos") @ResponseBody
    public ResponseEntity<?> calCombos() { return call(() -> svc.commAgents(), "Load failed."); }

    @PostMapping(CAL + "/show") @ResponseBody
    public ResponseEntity<?> calShow(@RequestBody Map<String, Object> body) { return call(() -> svc.commAgentShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 257

    @GetMapping(EER + "/setup") @ResponseBody
    public ResponseEntity<?> eerSetup() { return call(() -> svc.eeSetup(), "Error occurred during database call."); }

    @GetMapping(EER + "/combos") @ResponseBody
    public ResponseEntity<?> eerCombos() { return call(() -> svc.eeCombos(), "Load failed."); }

    @PostMapping(EER + "/show") @ResponseBody
    public ResponseEntity<?> eerShow(@RequestBody Map<String, Object> body) { return call(() -> svc.eeShow(body), "Load failed."); }

    @PostMapping(EER + "/save-row") @ResponseBody
    public ResponseEntity<?> eerSaveRow(@RequestBody Map<String, Object> body) { return call(() -> svc.eeSaveRow(body), "Save failed."); }

    // ------------------------------------------------------------------ 258

    @GetMapping(CIS + "/setup") @ResponseBody
    public ResponseEntity<?> cisSetup() { return call(() -> svc.ciShipmentsSetup(), "Error occurred during database call."); }

    @GetMapping(CIS + "/combos") @ResponseBody
    public ResponseEntity<?> cisCombos() { return call(() -> svc.ciCustomers(), "Load failed."); }

    @PostMapping(CIS + "/show") @ResponseBody
    public ResponseEntity<?> cisShow(@RequestBody Map<String, Object> body) { return call(() -> svc.ciShipmentsShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 203

    @GetMapping(CFU + "/setup") @ResponseBody
    public ResponseEntity<?> cfuSetup() { return call(() -> svc.followUpSetup(), "Error occurred during database call."); }

    @GetMapping(CFU + "/combos") @ResponseBody
    public ResponseEntity<?> cfuCombos() { return call(() -> svc.followUpCombos(), "Load failed."); }

    @PostMapping(CFU + "/grid") @ResponseBody
    public ResponseEntity<?> cfuGrid(@RequestBody Map<String, Object> body) { return call(() -> svc.followUpGrid(body), "Load failed."); }

    @GetMapping(CFU + "/by-invoice") @ResponseBody
    public ResponseEntity<?> cfuByInvoice(@RequestParam("invoiceId") int invoiceId) { return call(() -> svc.followUpByInvoice(invoiceId), "Load failed."); }

    @PostMapping(CFU + "/save") @ResponseBody
    public ResponseEntity<?> cfuSave(@RequestBody Map<String, Object> body) { return call(() -> svc.followUpSave(body), "Save failed."); }

    @PostMapping(CFU + "/update-row") @ResponseBody
    public ResponseEntity<?> cfuUpdateRow(@RequestBody Map<String, Object> body) { return call(() -> svc.followUpUpdateRow(body), "Save failed."); }

    @PostMapping(CFU + "/update-statuses") @ResponseBody
    public ResponseEntity<?> cfuUpdateStatuses(@RequestBody Map<String, Object> body) { return call(() -> svc.followUpUpdateStatuses(body), "Save failed."); }

    @GetMapping(CFU + "/cro-check") @ResponseBody
    public ResponseEntity<?> cfuCroCheck(@RequestParam("invoiceId") int invoiceId) { return call(() -> svc.croSlipCheck(invoiceId), "Load failed."); }

    // ------------------------------------------------------------------ 859

    @GetMapping(LOT + "/setup") @ResponseBody
    public ResponseEntity<?> lotSetup() { return call(() -> svc.lotTrackingSetup(), "Error occurred during database call."); }

    @GetMapping(LOT + "/combos") @ResponseBody
    public ResponseEntity<?> lotCombos() { return call(() -> svc.lotTrackingCombos(), "Load failed."); }

    @PostMapping(LOT + "/show") @ResponseBody
    public ResponseEntity<?> lotShow(@RequestBody Map<String, Object> body) { return call(() -> svc.lotTrackingShow(body), "Load failed."); }

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
