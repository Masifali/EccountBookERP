package com.mst.controllers;

import com.mst.services.ExportCustomInvoiceService;
import com.mst.services.ExportFcyGdUtilizationService;
import com.mst.services.ExportGdBankRequestService;
import com.mst.services.ExportGdBreakUpManualService;
import com.mst.services.ExportGdMappingService;
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
 * Module 2047 "Bank Export & GD Management" of the Export application (dbo.App 8) plus the two GD
 * forms of module 100:
 *
 *   /export/custom-invoice               951  frmCustomInvoice                    "Custom / Bank Invoice"
 *   /export/gd-bank-invoice-mapping      952  frmGdBreakUpByInvoiceNew            "Goods Declaration (GD) / Bank Invoice & GD Mapping"
 *   /export/fcy-receipt-gd-utilization   953  frmGdUtilizationAgainstFcyReceipt   "Fcy Receipts Utilization Against Bank Invoice / GD"
 *   /export/gd-bank-request              197  GdBankRequest                       "Gd Bank Request"
 *   /export/gd-break-up-manual           198  GdBreakUpManual                     "GD Break Up Manual"
 *
 * No parameter carries tenancy or a user id; the services derive them from the session and check
 * the screen's rights through DesktopReportRights.
 */
@Controller
public class ExportBankGdController {


    @Autowired private ExportCustomInvoiceService ci;
    @Autowired private ExportGdMappingService gm;
    @Autowired private ExportFcyGdUtilizationService fc;
    @Autowired private ExportGdBankRequestService br;
    @Autowired private ExportGdBreakUpManualService bm;

    // ------------------------------------------------------------------ pages

    @GetMapping("/export/custom-invoice")
    public String customInvoicePage(Model model) { model.addAttribute("activeMenu", "export"); return "export/custom_invoice"; }

    @GetMapping("/export/gd-bank-invoice-mapping")
    public String gdMappingPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/gd_bank_invoice_mapping"; }

    @GetMapping("/export/fcy-receipt-gd-utilization")
    public String fcyPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/fcy_receipt_gd_utilization"; }

    @GetMapping("/export/gd-bank-request")
    public String bankRequestPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/gd_bank_request"; }

    @GetMapping("/export/gd-break-up-manual")
    public String breakUpManualPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/gd_break_up_manual"; }

    // ------------------------------------------------------------------ 951 Custom / Bank Invoice

    @GetMapping("/api/export/custom-invoice/setup") @ResponseBody
    public ResponseEntity<?> ciSetup() { return call(() -> ci.setup(), "Error occurred during database call."); }

    @GetMapping("/api/export/custom-invoice/refresh") @ResponseBody
    public ResponseEntity<?> ciRefresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> ci.refresh(recId), "Load failed."); }

    @GetMapping("/api/export/custom-invoice/generate-code") @ResponseBody
    public ResponseEntity<?> ciGenerateCode() { return call(() -> ci.generateCode(), "Load failed."); }

    @GetMapping("/api/export/custom-invoice/commercial-invoices") @ResponseBody
    public ResponseEntity<?> ciCommercialInvoices(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> ci.commercialInvoices(recId), "Load failed."); }

    @GetMapping("/api/export/custom-invoice/consignees") @ResponseBody
    public ResponseEntity<?> ciConsignees(@RequestParam(value = "customerId", defaultValue = "0") int customerId) { return call(() -> ci.consignees(customerId), "Load failed."); }

    @GetMapping("/api/export/custom-invoice/uoms") @ResponseBody
    public ResponseEntity<?> ciUoms(@RequestParam(value = "itemId", defaultValue = "0") int itemId) { return call(() -> ci.uoms(itemId), "Load failed."); }

    @GetMapping("/api/export/custom-invoice/hs-codes") @ResponseBody
    public ResponseEntity<?> ciHsCodes(@RequestParam(value = "itemId", defaultValue = "0") int itemId,
                                       @RequestParam(value = "customerId", defaultValue = "0") int customerId) {
        return call(() -> ci.hsCodes(itemId, customerId), "Load failed.");
    }

    @GetMapping("/api/export/custom-invoice/fis") @ResponseBody
    public ResponseEntity<?> ciFis(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> ci.fis(recId), "Load failed."); }

    @GetMapping("/api/export/custom-invoice/history-combos") @ResponseBody
    public ResponseEntity<?> ciHistoryCombos() { return call(() -> ci.historyCombos(), "Load failed."); }

    @PostMapping("/api/export/custom-invoice/history") @ResponseBody
    public ResponseEntity<?> ciHistory(@RequestBody Map<String, Object> body) { return call(() -> ci.history(body), "Load failed."); }

    @GetMapping("/api/export/custom-invoice/history-detail") @ResponseBody
    public ResponseEntity<?> ciHistoryDetail(@RequestParam("id") int id) { return call(() -> ci.historyDetail(id), "Load failed."); }

    @GetMapping("/api/export/custom-invoice/by-id") @ResponseBody
    public ResponseEntity<?> ciById(@RequestParam("id") int id) { return call(() -> ci.readById(id), "Load failed."); }

    @PostMapping("/api/export/custom-invoice/save") @ResponseBody
    public ResponseEntity<?> ciSave(@RequestBody Map<String, Object> body) { return call(() -> ci.save(body), "Save failed."); }

    // ------------------------------------------------------------------ 952 GD / Bank Invoice & GD Mapping

    @GetMapping("/api/export/gd-bank-invoice-mapping/setup") @ResponseBody
    public ResponseEntity<?> gmSetup() { return call(() -> gm.setup(), "Error occurred during database call."); }

    @GetMapping("/api/export/gd-bank-invoice-mapping/refresh") @ResponseBody
    public ResponseEntity<?> gmRefresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> gm.refreshGdBreakUp(recId), "Load failed."); }

    @GetMapping("/api/export/gd-bank-invoice-mapping/invoices") @ResponseBody
    public ResponseEntity<?> gmInvoices(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> gm.invoices(recId), "Load failed."); }

    @GetMapping("/api/export/gd-bank-invoice-mapping/by-invoice") @ResponseBody
    public ResponseEntity<?> gmByInvoice(@RequestParam("invoiceId") int invoiceId) { return call(() -> gm.byInvoice(invoiceId), "Load failed."); }

    @GetMapping("/api/export/gd-bank-invoice-mapping/history") @ResponseBody
    public ResponseEntity<?> gmHistory(@RequestParam(value = "fromDate", required = false) String fromDate,
                                       @RequestParam(value = "toDate", required = false) String toDate,
                                       @RequestParam(value = "bankId", defaultValue = "0") int bankId) {
        return call(() -> gm.gdHistory(fromDate, toDate, bankId), "Load failed.");
    }

    @GetMapping("/api/export/gd-bank-invoice-mapping/history-banks") @ResponseBody
    public ResponseEntity<?> gmHistoryBanks() { return call(() -> gm.historyBanks(), "Load failed."); }

    @PostMapping("/api/export/gd-bank-invoice-mapping/save") @ResponseBody
    public ResponseEntity<?> gmSave(@RequestBody Map<String, Object> body) { return call(() -> gm.saveGdBreakUp(body), "Save failed."); }

    @GetMapping("/api/export/gd-bank-invoice-mapping/custom-invoices") @ResponseBody
    public ResponseEntity<?> gmCustomInvoices(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId) { return call(() -> gm.customInvoices(invoiceId), "Load failed."); }

    @GetMapping("/api/export/gd-bank-invoice-mapping/payment-terms") @ResponseBody
    public ResponseEntity<?> gmPaymentTerms(@RequestParam("invoiceId") int invoiceId) { return call(() -> gm.paymentTerms(invoiceId), "Load failed."); }

    @GetMapping("/api/export/gd-bank-invoice-mapping/advance/history-invoices") @ResponseBody
    public ResponseEntity<?> gmAdvHistoryInvoices() { return call(() -> gm.advanceHistoryInvoices(), "Load failed."); }

    @GetMapping("/api/export/gd-bank-invoice-mapping/advance/history") @ResponseBody
    public ResponseEntity<?> gmAdvHistory(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId,
                                          @RequestParam(value = "fromDate", required = false) String fromDate,
                                          @RequestParam(value = "toDate", required = false) String toDate) {
        return call(() -> gm.advanceHistory(invoiceId, fromDate, toDate), "Load failed.");
    }

    @PostMapping("/api/export/gd-bank-invoice-mapping/advance/save") @ResponseBody
    public ResponseEntity<?> gmAdvSave(@RequestBody Map<String, Object> body) { return call(() -> gm.saveAdvanceUtilize(body), "Save failed."); }

    // ------------------------------------------------------------------ 953 Fcy Receipts Utilization

    @GetMapping("/api/export/fcy-receipt-gd-utilization/setup") @ResponseBody
    public ResponseEntity<?> fcSetup() { return call(() -> fc.setup(), "Error occurred during database call."); }

    @GetMapping("/api/export/fcy-receipt-gd-utilization/pending") @ResponseBody
    public ResponseEntity<?> fcPending() { return call(() -> fc.pending(), "Load failed."); }

    @GetMapping("/api/export/fcy-receipt-gd-utilization/gds") @ResponseBody
    public ResponseEntity<?> fcGds(@RequestParam(value = "fcyReceiptId", defaultValue = "0") int fcyReceiptId) { return call(() -> fc.gds(fcyReceiptId), "Load failed."); }

    @GetMapping("/api/export/fcy-receipt-gd-utilization/history") @ResponseBody
    public ResponseEntity<?> fcHistory(@RequestParam(value = "fromDate", required = false) String fromDate,
                                       @RequestParam(value = "toDate", required = false) String toDate) {
        return call(() -> fc.history(fromDate, toDate), "Load failed.");
    }

    @GetMapping("/api/export/fcy-receipt-gd-utilization/utilization") @ResponseBody
    public ResponseEntity<?> fcUtilization(@RequestParam("id") int id) { return call(() -> fc.utilizationByReceipt(id), "Load failed."); }

    @GetMapping("/api/export/fcy-receipt-gd-utilization/by-id") @ResponseBody
    public ResponseEntity<?> fcById(@RequestParam("id") int id) { return call(() -> fc.readById(id), "Load failed."); }

    @PostMapping("/api/export/fcy-receipt-gd-utilization/save") @ResponseBody
    public ResponseEntity<?> fcSave(@RequestBody Map<String, Object> body) { return call(() -> fc.save(body), "Save failed."); }

    @PostMapping("/api/export/fcy-receipt-gd-utilization/delete-by-receipt") @ResponseBody
    public ResponseEntity<?> fcDelete(@RequestBody Map<String, Object> body) {
        final int id = intOf(body.get("id"));
        return call(() -> fc.deleteByReceipt(id), "Delete failed.");
    }

    // ------------------------------------------------------------------ 197 Gd Bank Request

    @GetMapping("/api/export/gd-bank-request/setup") @ResponseBody
    public ResponseEntity<?> brSetup() { return call(() -> br.setup(), "Error occurred during database call."); }

    @GetMapping("/api/export/gd-bank-request/generate-code") @ResponseBody
    public ResponseEntity<?> brGenerateCode() { return call(() -> br.generateCode(), "Load failed."); }

    @GetMapping("/api/export/gd-bank-request/banks") @ResponseBody
    public ResponseEntity<?> brBanks() { return call(() -> br.banks(), "Load failed."); }

    @GetMapping("/api/export/gd-bank-request/history-banks") @ResponseBody
    public ResponseEntity<?> brHistoryBanks() { return call(() -> br.historyBanks(), "Load failed."); }

    @GetMapping("/api/export/gd-bank-request/gds") @ResponseBody
    public ResponseEntity<?> brGds(@RequestParam("bankId") int bankId, @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> br.gds(bankId, recId), "Load failed.");
    }

    @GetMapping("/api/export/gd-bank-request/by-id") @ResponseBody
    public ResponseEntity<?> brById(@RequestParam("id") int id) { return call(() -> br.readById(id), "Load failed."); }

    @GetMapping("/api/export/gd-bank-request/history-detail") @ResponseBody
    public ResponseEntity<?> brHistoryDetail(@RequestParam("id") int id) { return call(() -> br.historyDetail(id), "Load failed."); }

    @PostMapping("/api/export/gd-bank-request/history") @ResponseBody
    public ResponseEntity<?> brHistory(@RequestBody Map<String, Object> body) { return call(() -> br.history(body), "Load failed."); }

    @PostMapping("/api/export/gd-bank-request/save") @ResponseBody
    public ResponseEntity<?> brSave(@RequestBody Map<String, Object> body) { return call(() -> br.save(body), "Save failed."); }

    // ------------------------------------------------------------------ 198 GD Break Up Manual

    @GetMapping("/api/export/gd-break-up-manual/setup") @ResponseBody
    public ResponseEntity<?> bmSetup() { return call(() -> bm.setup(), "Error occurred during database call."); }

    @GetMapping("/api/export/gd-break-up-manual/history") @ResponseBody
    public ResponseEntity<?> bmHistory() { return call(() -> bm.history(), "Load failed."); }

    @GetMapping("/api/export/gd-break-up-manual/by-id") @ResponseBody
    public ResponseEntity<?> bmById(@RequestParam("id") int id) { return call(() -> bm.byId(id), "Load failed."); }

    @PostMapping("/api/export/gd-break-up-manual/save") @ResponseBody
    public ResponseEntity<?> bmSave(@RequestBody Map<String, Object> body) { return call(() -> bm.save(body), "Save failed."); }

    // ------------------------------------------------------------------ plumbing

    private interface Call { Object run() throws Exception; }

    private static int intOf(Object v) {
        try { return v == null ? 0 : (int) Double.parseDouble(String.valueOf(v)); } catch (NumberFormatException e) { return 0; }
    }

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
