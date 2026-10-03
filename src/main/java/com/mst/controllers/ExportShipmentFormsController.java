package com.mst.controllers;

import com.mst.services.ExportDhlTrackingService;
import com.mst.services.ExportFiUtilizedAgainstShipmentService;
import com.mst.services.ExportGoodsReceiptsAtPortService;
import com.mst.services.ExportInvoiceAgainstForwardingService;
import com.mst.services.ExportPmIssuanceForShipmentService;
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
 * Five Export shipment forms (Architecture.WinApp.Export) that have no hub row of their own yet (no / inactive
 * ScreenDefinition row); they open from the Export hub when the coordinator maps a screen id, or from parent pages:
 *
 *   /export/goods-receipts-at-port        ExImGoodsReceiptsAtPort (+ GetForwardingDataForGoodsReceiptsAsPort)
 *   /export/pm-issuance-for-shipment      PackingMaterialIssuanceForShipment          (DocumentTypeId 213)
 *   /export/fi-utilized-against-shipment  FinancialInstrumentsUtilizedAgainstShipment (rights of screen 202)
 *   /export/invoice-against-forwarding    ExportInvoiceAgainstForwarding (+ LoadExportForwarding, DocumentTypeId 208)
 *   /export/dhl-tracking                  frmDhlTracking                              (DocumentTypeId 248)
 *
 * API under /api/export/<route>/...; no parameter carries tenancy or a user id - the services take them from the session.
 * 400 = the form's own message, 403 = a missing right, 500 = the innermost message (an SQL RAISERROR text).
 */
@Controller
public class ExportShipmentFormsController {

    private static final String GR = "/api/export/goods-receipts-at-port";
    private static final String PM = "/api/export/pm-issuance-for-shipment";
    private static final String FI = "/api/export/fi-utilized-against-shipment";
    private static final String IA = "/api/export/invoice-against-forwarding";
    private static final String DH = "/api/export/dhl-tracking";

    @Autowired private ExportGoodsReceiptsAtPortService gr;
    @Autowired private ExportPmIssuanceForShipmentService pm;
    @Autowired private ExportFiUtilizedAgainstShipmentService fi;
    @Autowired private ExportInvoiceAgainstForwardingService ia;
    @Autowired private ExportDhlTrackingService dh;

    // ------------------------------------------------------------------ pages

    @GetMapping("/export/goods-receipts-at-port")
    public String goodsReceiptsAtPortPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/goods_receipts_at_port"; }

    @GetMapping("/export/pm-issuance-for-shipment")
    public String pmIssuancePage(Model model) { model.addAttribute("activeMenu", "export"); return "export/pm_issuance_for_shipment"; }

    @GetMapping("/export/fi-utilized-against-shipment")
    public String fiUtilizedPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/fi_utilized_against_shipment"; }

    @GetMapping("/export/invoice-against-forwarding")
    public String invoiceAgainstForwardingPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/invoice_against_forwarding"; }

    @GetMapping("/export/dhl-tracking")
    public String dhlTrackingPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/dhl_tracking"; }

    // ------------------------------------------------------------------ Goods Receipts At Port

    @GetMapping(GR + "/setup") @ResponseBody
    public ResponseEntity<?> grSetup() { return call(() -> gr.setup(), "Load failed."); }

    @GetMapping(GR + "/history") @ResponseBody
    public ResponseEntity<?> grHistory() { return call(() -> gr.history(), "Load failed."); }

    @GetMapping(GR + "/loader/combos") @ResponseBody
    public ResponseEntity<?> grLoaderCombos() { return call(() -> gr.loaderCombos(), "Load failed."); }

    @GetMapping(GR + "/loader") @ResponseBody
    public ResponseEntity<?> grLoader(@RequestParam(value = "partyId", defaultValue = "0") int partyId,
                                      @RequestParam(value = "itemId", defaultValue = "0") int itemId,
                                      @RequestParam(value = "currencyId", defaultValue = "0") int currencyId) {
        return call(() -> gr.loaderRows(partyId, itemId, currencyId), "Load failed.");
    }

    @PostMapping(GR + "/save") @ResponseBody
    public ResponseEntity<?> grSave(@RequestBody Map<String, Object> body) { return call(() -> gr.save(body), "Save failed."); }

    // ------------------------------------------------------------------ Packing Material Issuance For Shipment

    @GetMapping(PM + "/setup") @ResponseBody
    public ResponseEntity<?> pmSetup() { return call(() -> pm.setup(), "Load failed."); }

    @GetMapping(PM + "/refresh") @ResponseBody
    public ResponseEntity<?> pmRefresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> pm.refresh(recId), "Load failed."); }

    @GetMapping(PM + "/doc-no") @ResponseBody
    public ResponseEntity<?> pmDocNo() { return call(() -> pm.docNo(), "Load failed."); }

    @GetMapping(PM + "/invoices") @ResponseBody
    public ResponseEntity<?> pmInvoices(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> pm.invoices(recId), "Load failed."); }

    @GetMapping(PM + "/invoice-info") @ResponseBody
    public ResponseEntity<?> pmInvoiceInfo(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId) {
        return call(() -> pm.invoiceInfo(invoiceId), "Load failed.");
    }

    @GetMapping(PM + "/stock") @ResponseBody
    public ResponseEntity<?> pmStock(@RequestParam(value = "recId", defaultValue = "0") int recId,
                                     @RequestParam(value = "itemId", defaultValue = "0") int itemId,
                                     @RequestParam(value = "docDate", required = false) String docDate,
                                     @RequestParam(value = "itemConditionId", defaultValue = "0") int itemConditionId,
                                     @RequestParam(value = "warehouseId", defaultValue = "0") int warehouseId,
                                     @RequestParam(value = "rackId", defaultValue = "0") int rackId) {
        return call(() -> pm.stock(recId, itemId, docDate, itemConditionId, warehouseId, rackId), "Load failed.");
    }

    @PostMapping(PM + "/rates-on-date") @ResponseBody
    public ResponseEntity<?> pmRatesOnDate(@RequestBody Map<String, Object> body) { return call(() -> pm.ratesOnDate(body), "Load failed."); }

    @GetMapping(PM + "/history") @ResponseBody
    public ResponseEntity<?> pmHistory(@RequestParam(value = "fromDate", required = false) String fromDate,
                                       @RequestParam(value = "toDate", required = false) String toDate,
                                       @RequestParam(value = "fromDocNo", defaultValue = "0") int fromDocNo,
                                       @RequestParam(value = "toDocNo", defaultValue = "0") int toDocNo) {
        return call(() -> pm.history(fromDate, toDate, fromDocNo, toDocNo), "Load failed.");
    }

    @GetMapping(PM + "/by-id") @ResponseBody
    public ResponseEntity<?> pmById(@RequestParam("id") int id) { return call(() -> pm.readById(id), "Load failed."); }

    @GetMapping(PM + "/print-check") @ResponseBody
    public ResponseEntity<?> pmPrintCheck(@RequestParam(value = "id", defaultValue = "0") int id) { return call(() -> pm.printCheck(id), "Print failed."); }

    @PostMapping(PM + "/save") @ResponseBody
    public ResponseEntity<?> pmSave(@RequestBody Map<String, Object> body) { return call(() -> pm.save(body), "Save failed."); }

    // ------------------------------------------------------------------ Financial Instruments Utilized Against Shipment

    @GetMapping(FI + "/setup") @ResponseBody
    public ResponseEntity<?> fiSetup() { return call(() -> fi.setup(), "Load failed."); }

    @GetMapping(FI + "/refresh") @ResponseBody
    public ResponseEntity<?> fiRefresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> fi.refresh(recId), "Load failed."); }

    @GetMapping(FI + "/invoices") @ResponseBody
    public ResponseEntity<?> fiInvoices(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> fi.invoices(recId), "Load failed."); }

    @GetMapping(FI + "/fi-balance") @ResponseBody
    public ResponseEntity<?> fiBalance(@RequestParam(value = "documentTypeId", defaultValue = "0") int documentTypeId,
                                       @RequestParam(value = "id", defaultValue = "0") int id) {
        return call(() -> fi.fiBalance(documentTypeId, id), "Load failed.");
    }

    @GetMapping(FI + "/history-combos") @ResponseBody
    public ResponseEntity<?> fiHistoryCombos() { return call(() -> fi.historyCombos(), "Load failed."); }

    @GetMapping(FI + "/history") @ResponseBody
    public ResponseEntity<?> fiHistory(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId,
                                       @RequestParam(value = "eFormRegistrationId", defaultValue = "0") int eFormRegistrationId,
                                       @RequestParam(value = "documentTypeId", defaultValue = "0") int documentTypeId) {
        return call(() -> fi.history(invoiceId, eFormRegistrationId, documentTypeId), "Load failed.");
    }

    @GetMapping(FI + "/by-id") @ResponseBody
    public ResponseEntity<?> fiById(@RequestParam("id") int id) { return call(() -> fi.readById(id), "Load failed."); }

    @PostMapping(FI + "/save") @ResponseBody
    public ResponseEntity<?> fiSave(@RequestBody Map<String, Object> body) { return call(() -> fi.save(body), "Save failed."); }

    // ------------------------------------------------------------------ Export Invoice Against Forwarding

    @GetMapping(IA + "/setup") @ResponseBody
    public ResponseEntity<?> iaSetup() { return call(() -> ia.setup(), "Load failed."); }

    @GetMapping(IA + "/customers") @ResponseBody
    public ResponseEntity<?> iaCustomers() { return call(() -> ia.customers(), "Load failed."); }

    @GetMapping(IA + "/loader/combos") @ResponseBody
    public ResponseEntity<?> iaLoaderCombos() { return call(() -> ia.loaderCombos(), "Load failed."); }

    @GetMapping(IA + "/loader") @ResponseBody
    public ResponseEntity<?> iaLoader(@RequestParam(value = "partyId", defaultValue = "0") int partyId,
                                      @RequestParam(value = "fromDate", required = false) String fromDate,
                                      @RequestParam(value = "toDate", required = false) String toDate) {
        return call(() -> ia.loaderRows(partyId, fromDate, toDate), "Load failed.");
    }

    @GetMapping(IA + "/forwarding-rows") @ResponseBody
    public ResponseEntity<?> iaForwardingRows(@RequestParam(value = "ids", defaultValue = "") String ids) {
        return call(() -> ia.forwardingRows(ids), "Load failed.");
    }

    @GetMapping(IA + "/history") @ResponseBody
    public ResponseEntity<?> iaHistory(@RequestParam(value = "noOfRecords", defaultValue = "0") int noOfRecords) {
        return call(() -> ia.history(noOfRecords), "Load failed.");
    }

    @GetMapping(IA + "/by-id") @ResponseBody
    public ResponseEntity<?> iaById(@RequestParam("id") int id) { return call(() -> ia.readById(id), "Load failed."); }

    @GetMapping(IA + "/print-check") @ResponseBody
    public ResponseEntity<?> iaPrintCheck() { return call(() -> ia.printCheck(), "Print failed."); }

    @PostMapping(IA + "/save") @ResponseBody
    public ResponseEntity<?> iaSave(@RequestBody Map<String, Object> body) { return call(() -> ia.save(body), "Save failed."); }

    // ------------------------------------------------------------------ DHL Tracking

    @GetMapping(DH + "/setup") @ResponseBody
    public ResponseEntity<?> dhSetup() { return call(() -> dh.setup(), "Load failed."); }

    @GetMapping(DH + "/refresh") @ResponseBody
    public ResponseEntity<?> dhRefresh() { return call(() -> dh.refresh(), "Load failed."); }

    @PostMapping(DH + "/history") @ResponseBody
    public ResponseEntity<?> dhHistory(@RequestBody Map<String, Object> body) { return call(() -> dh.history(body), "Load failed."); }

    @GetMapping(DH + "/by-id") @ResponseBody
    public ResponseEntity<?> dhById(@RequestParam("id") long id) { return call(() -> dh.readById(id), "Load failed."); }

    @PostMapping(DH + "/save") @ResponseBody
    public ResponseEntity<?> dhSave(@RequestBody Map<String, Object> body) { return call(() -> dh.save(body), "Save failed."); }

    @PostMapping(DH + "/delete") @ResponseBody
    public ResponseEntity<?> dhDelete(@RequestBody Map<String, Object> body) {
        long id = toLong(body.get("id")), open = toLong(body.get("openRecId"));
        return call(() -> dh.delete(id, open), "Delete failed.");
    }

    // ------------------------------------------------------------------ plumbing

    private interface Call { Object run() throws Exception; }

    private static ResponseEntity<?> call(Call c, String fallback) {
        try { return ResponseEntity.ok(c.run()); }
        catch (IllegalArgumentException | IllegalStateException | UnsupportedOperationException e) { return ResponseEntity.badRequest().body(fail(root(e, fallback))); }
        catch (AccessDeniedException e) { return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied."))); }
        catch (Exception e) { return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback))); }
    }

    private static long toLong(Object v) {
        if (v == null) return 0L;
        if (v instanceof Number) return ((Number) v).longValue();
        try { return (long) Double.parseDouble(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0L; }
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
