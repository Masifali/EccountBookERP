package com.mst.controllers;

import com.mst.services.SaleInvoiceStoreWtService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.NoSuchElementException;

/**
 * Store Sale (module 109) - screen 327 "Sale Invoice Store Direct Without Tax" (InvfrmInvSaleInvoiceDirectPackingMaterial,
 * ScreenName frmSID_Store_WithoutTax: DocumentTypeId 126, BaseDocumentTypeId 0), at /store/sale-invoice-direct-without-tax.
 * Behaviour notes and deviations are on {@link SaleInvoiceStoreWtService}.
 */
@Controller
public class SaleInvoiceStoreWtController {


    private final SaleInvoiceStoreWtService service;
    public SaleInvoiceStoreWtController(SaleInvoiceStoreWtService service) { this.service = service; }

    @GetMapping("/store/sale-invoice-direct-without-tax")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/sale_invoice_store_wt";
    }

    @GetMapping("/store/api/sale-invoice-without-tax/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping("/store/api/sale-invoice-without-tax/numbers") @ResponseBody
    public ResponseEntity<?> numbers() { return run(service::numbers, "Could not number the document."); }

    @GetMapping("/store/api/sale-invoice-without-tax/tax-options") @ResponseBody
    public ResponseEntity<?> taxOptions(@RequestParam(defaultValue = "0") int itemId, @RequestParam(required = false) String docDate) {
        return run(() -> service.taxOptions(itemId, docDate), "Could not read the tax schedule.");
    }

    @GetMapping("/store/api/sale-invoice-without-tax/tax-by-items") @ResponseBody
    public ResponseEntity<?> taxByItems(@RequestParam(required = false) String itemIds, @RequestParam(required = false) String docDate) {
        return run(() -> service.taxByItems(itemIds, docDate), "Could not read the tax schedule.");
    }

    @GetMapping("/store/api/sale-invoice-without-tax/stock") @ResponseBody
    public ResponseEntity<?> stock(@RequestParam(defaultValue = "0") int itemId, @RequestParam(required = false) String docDate,
                                   @RequestParam(defaultValue = "0") int conditionId, @RequestParam(defaultValue = "0") int warehouseId,
                                   @RequestParam(defaultValue = "0") int rackId) {
        return run(() -> service.stock(itemId, docDate, conditionId, warehouseId, rackId), "Could not read the stock.");
    }

    @GetMapping("/store/api/sale-invoice-without-tax/gdn-branches") @ResponseBody
    public ResponseEntity<?> gdnBranches() { return run(service::gdnBranches, "Could not load branches."); }

    @GetMapping("/store/api/sale-invoice-without-tax/gdn-pending") @ResponseBody
    public ResponseEntity<?> gdnPending(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                        @RequestParam(required = false) String branchIds) {
        return write(() -> service.pendingGdns(fromDate, toDate, branchIds), "Could not load GDNs.");
    }

    @GetMapping("/store/api/sale-invoice-without-tax/gdn/{gdnId}") @ResponseBody
    public ResponseEntity<?> gdn(@PathVariable int gdnId) { return write(() -> service.loadGdn(gdnId), "Could not load the GDN."); }

    @GetMapping("/store/api/sale-invoice-without-tax/history-customers") @ResponseBody
    public ResponseEntity<?> historyCustomers() { return run(service::historyCustomers, "Could not load customers."); }

    @GetMapping("/store/api/sale-invoice-without-tax/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int customerId) {
        return run(() -> service.history(dateType, fromDate, toDate, fromDocNo, toDocNo, customerId), "History failed.");
    }

    @GetMapping("/store/api/sale-invoice-without-tax/{id}") @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            return ResponseEntity.ok(service.read(id));
        } catch (NoSuchElementException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found"));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(rootCause(e), "Could not open that document.")));
        }
    }

    @PostMapping("/store/api/sale-invoice-without-tax/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> req) { return write(() -> service.save(req), "Save failed."); }

    @PostMapping("/store/api/sale-invoice-without-tax/{id}/delete") @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) { return write(() -> service.delete(id), "Delete failed."); }

    // --------------------------------------------------------------------------- helpers

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(rootCause(e), fallback)));
        }
    }

    private static ResponseEntity<?> write(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException | NoSuchElementException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException | SecurityException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            /* RAISERRORs from the save procedures carry the desktop's own message. */
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(rootCause(e), fallback)));
        }
    }

    private static Throwable rootCause(Throwable t) {
        Throwable r = t;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        return r;
    }

    private static String msg(Throwable t, String fallback) {
        String m = t == null ? null : t.getMessage();
        return m == null || m.isBlank() ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
