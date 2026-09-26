package com.mst.controllers;

import com.mst.models.dto.PurchaseInvoiceStoreMgmtDto;
import com.mst.services.PurchaseInvoiceStoreMgmtService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Store Purchase (ModuleId 67) — screen 323 "Purchase Invoice Store Management",
 * {@code PurchaseInvoiceStoreManagement.cs} (ScreenName "PurchaseInvoiceStoreManagement"),
 * DocumentTypeId 64, at /store/purchase-invoice-store-management; JSON under
 * /api/store/purchase-invoice-store-management. The grid is filled only by the Load-GRN dialog
 * {@code frmPendingGrnStoreLoader.cs} (GRN DocumentTypeId 48), served by the /loader endpoints.
 *
 * Business rules, desktop-behaviour notes and deviations: see {@link PurchaseInvoiceStoreMgmtService}.
 * Errors answer 400 with {message} (403 for a missing right), as the other Store controllers do.
 */
@Controller
public class PurchaseInvoiceStoreMgmtController {

    private static final String API = "/api/store/purchase-invoice-store-management";

    private final PurchaseInvoiceStoreMgmtService service;
    public PurchaseInvoiceStoreMgmtController(PurchaseInvoiceStoreMgmtService service) { this.service = service; }

    @GetMapping("/store/purchase-invoice-store-management")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/purchase_invoice_store_management";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups); }

    @GetMapping(API + "/numbers")
    @ResponseBody
    public ResponseEntity<?> numbers() { return run(service::numbers); }

    @GetMapping(API + "/refresh")
    @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::formRefresh); }

    @GetMapping(API + "/history-refresh")
    @ResponseBody
    public ResponseEntity<?> historyRefresh() { return run(service::historyRefresh); }

    @GetMapping(API + "/tax-schedule")
    @ResponseBody
    public ResponseEntity<?> taxSchedule(@RequestParam(required = false) String itemIds,
                                         @RequestParam(required = false) String docDate,
                                         @RequestParam(defaultValue = "0") int recId) {
        return run(() -> service.taxSchedule(itemIds, docDate, recId));
    }

    @GetMapping(API + "/loader/lookups")
    @ResponseBody
    public ResponseEntity<?> loaderLookups() { return run(service::loaderLookups); }

    @GetMapping(API + "/loader/combos")
    @ResponseBody
    public ResponseEntity<?> loaderCombos(@RequestParam(required = false) String branchIds) {
        return run(() -> service.loaderCombos(branchIds));
    }

    @GetMapping(API + "/loader/pending")
    @ResponseBody
    public ResponseEntity<?> loaderPending(@RequestParam(required = false) String branchIds,
                                           @RequestParam(required = false) String fromDate,
                                           @RequestParam(required = false) String toDate,
                                           @RequestParam(defaultValue = "0") int docNoFrom,
                                           @RequestParam(defaultValue = "0") int docNoTo,
                                           @RequestParam(defaultValue = "0") int itemId,
                                           @RequestParam(defaultValue = "0") int billToPartyId) {
        return run(() -> service.pendingGrns(branchIds, fromDate, toDate, docNoFrom, docNoTo, itemId, billToPartyId));
    }

    @GetMapping(API + "/loader/extras")
    @ResponseBody
    public ResponseEntity<?> loaderExtras(@RequestParam(required = false) String grnIds,
                                          @RequestParam(required = false) String challanIds) {
        return run(() -> service.grnExtras(grnIds, challanIds));
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "false") boolean branchChosen,
                                     @RequestParam(required = false) String branchIds,
                                     @RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(defaultValue = "true") boolean fromChecked,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(defaultValue = "true") boolean toChecked,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false) String fromDocNo,
                                     @RequestParam(required = false) String toDocNo,
                                     @RequestParam(defaultValue = "0") int supplierId) {
        return run(() -> service.history(branchChosen, branchIds, dateType, fromChecked, fromDate, toChecked, toDate,
                fromDocNo, toDocNo, supplierId));
    }

    @GetMapping(API + "/voucher-slip")
    @ResponseBody
    public ResponseEntity<?> voucherSlip(@RequestParam(defaultValue = "0") int id) {
        return run(() -> service.voucherSlip(id));
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(fail(msg(e)));
        }
    }

    @GetMapping(API + "/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id, @RequestParam(defaultValue = "64") int documentTypeId) {
        return run(() -> service.slip(id, documentTypeId));
    }

    @GetMapping(API + "/{id}/slip-sub")
    @ResponseBody
    public ResponseEntity<?> slipSub(@PathVariable int id) {
        return run(() -> service.slipSubReport(id));
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody PurchaseInvoiceStoreMgmtDto dto) {
        return run(() -> service.save(dto));
    }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) {
        return run(() -> service.delete(id));
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (org.springframework.security.access.AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(msg(e)));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(fail(msg(e)));
        }
    }

    private static String msg(Throwable e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String m = r.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? "Request failed." : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
