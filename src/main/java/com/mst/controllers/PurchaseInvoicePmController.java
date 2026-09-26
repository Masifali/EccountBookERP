package com.mst.controllers;

import com.mst.models.dto.PurchaseInvoicePmRequest;
import com.mst.services.DesktopInventoryItemFileService;
import com.mst.services.PurchaseInvoicePmService;
import java.util.*;
import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

/**
 * Screen 501 "Purchase Invoice PM" - Packing Material module 54.
 * Desktop {@code Architecture.WinApp.PackingMaterial_Store.PurchaseInvoicePackingMaterial}, DocumentTypeId 702.
 */
@Controller
public class PurchaseInvoicePmController {

    private static final String API = "/api/packing-material/purchase-invoice";

    private final PurchaseInvoicePmService service;

    public PurchaseInvoicePmController(PurchaseInvoicePmService service) {
        this.service = service;
    }

    @GetMapping({"/packing-material/purchase-invoice", "/packing-material/invoice", "/packing/purchase-invoice"})
    public String page(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        return "packing_material/purchase_invoice_pm";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/numbers")
    @ResponseBody
    public ResponseEntity<?> numbers() { return run(service::numbers, "Could not generate the document number."); }

    @GetMapping(API + "/pending-grns")
    @ResponseBody
    public ResponseEntity<?> pendingGrns(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate) {
        return run(() -> service.pendingGrns(fromDate, toDate), "Could not load GRNs.");
    }

    @PostMapping(API + "/load-grns")
    @ResponseBody
    public ResponseEntity<?> loadGrns(@RequestBody List<Integer> grnIds) {
        return run(() -> service.loadGrns(grnIds), "Could not load GRNs.");
    }

    @GetMapping(API + "/tax-options")
    @ResponseBody
    public ResponseEntity<?> taxOptions(@RequestParam(defaultValue = "0") int itemId, @RequestParam String docDate) {
        return run(() -> service.taxOptions(itemId, docDate), "Could not read the tax schedule.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int supplierId, @RequestParam(required = false) String branchIds) {
        return run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo, supplierId, branchIds), "History failed.");
    }

    @GetMapping(API + "/history-suppliers")
    @ResponseBody
    public ResponseEntity<?> historySuppliers(@RequestParam(required = false) String branchIds) {
        return run(() -> service.historySuppliers(branchIds), "Could not refresh.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> record(@PathVariable int id) { return run(() -> service.record(id), "Record Not Found"); }

    @GetMapping(API + "/{id}/voucher")
    @ResponseBody
    public ResponseEntity<?> voucher(@PathVariable int id) { return run(() -> service.voucherHead(id), "Record Not Found"); }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody PurchaseInvoicePmRequest request) { return run(() -> service.save(request), "Save failed."); }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) { return run(() -> service.delete(id), "Delete failed."); }

    @GetMapping(API + "/{id}/attachments/{attachmentId}")
    public ResponseEntity<byte[]> attachment(@PathVariable int id, @PathVariable int attachmentId) {
        DesktopInventoryItemFileService.Download d = service.attachment(id, attachmentId);
        HttpHeaders h = new HttpHeaders();
        h.setContentType(MediaType.parseMediaType(d.type()));
        h.setContentDisposition(ContentDisposition.attachment().filename(d.name()).build());
        return new ResponseEntity<>(d.bytes(), h, HttpStatus.OK);
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (ResponseStatusException e) {
            return ResponseEntity.status(e.getStatus()).body(fail(e.getReason()));
        } catch (Exception e) {
            String m = e.getMessage();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(m == null || m.isBlank() ? fallback : m));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
