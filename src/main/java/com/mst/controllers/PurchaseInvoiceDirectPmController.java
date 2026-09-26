package com.mst.controllers;

import com.mst.models.dto.PurchaseInvoiceDirectPmRequest;
import com.mst.services.DesktopInventoryItemFileService;
import com.mst.services.PurchaseInvoiceDirectPmService;
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
 * Screen 495 "Purchase Invoice Direct PM" - Packing Material module 54.
 * Desktop {@code Architecture.WinApp.PackingMaterial_Store.frmPurchaseInvoiceDirectPM}, DocumentTypeId 245.
 */
@Controller
public class PurchaseInvoiceDirectPmController {

    private static final String API = "/api/packing-material/purchase-invoice-direct";

    private final PurchaseInvoiceDirectPmService service;

    public PurchaseInvoiceDirectPmController(PurchaseInvoiceDirectPmService service) {
        this.service = service;
    }

    @GetMapping({"/packing-material/purchase-invoice-direct", "/packing-material/invoice-direct", "/packing/purchase-invoice-direct"})
    public String page(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        return "packing_material/purchase_invoice_direct_pm";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/numbers")
    @ResponseBody
    public ResponseEntity<?> numbers() { return run(service::numbers, "Could not generate the document number."); }

    @GetMapping(API + "/tax-options")
    @ResponseBody
    public ResponseEntity<?> taxOptions(@RequestParam(defaultValue = "0") int itemId, @RequestParam String docDate) {
        return run(() -> service.taxOptions(itemId, docDate), "Could not read the tax schedule.");
    }

    @GetMapping(API + "/tax-by-items")
    @ResponseBody
    public ResponseEntity<?> taxByItems(@RequestParam(required = false) String itemIds, @RequestParam String docDate) {
        return run(() -> service.taxByItems(itemIds, docDate), "Could not read the tax schedule.");
    }

    @GetMapping(API + "/stock")
    @ResponseBody
    public ResponseEntity<?> stock(@RequestParam int itemId, @RequestParam String docDate, @RequestParam(defaultValue = "0") int conditionId,
                                   @RequestParam(defaultValue = "0") int recId, @RequestParam(defaultValue = "0") int warehouseId,
                                   @RequestParam(defaultValue = "0") int rackId) {
        return run(() -> service.stock(itemId, docDate, conditionId, recId, warehouseId, rackId), "Could not read the stock.");
    }

    @GetMapping(API + "/ref-docs")
    @ResponseBody
    public ResponseEntity<?> refDocs(@RequestParam(defaultValue = "0") int refDocumentTypeId, @RequestParam(defaultValue = "0") int recId) {
        return run(() -> service.refDocs(refDocumentTypeId, recId), "Could not load reference documents.");
    }

    @GetMapping(API + "/exchange-rate")
    @ResponseBody
    public ResponseEntity<?> exchangeRate(@RequestParam(defaultValue = "0") int currencyId) {
        return run(() -> service.exchangeRate(currencyId), "Could not read the exchange rate.");
    }

    @PostMapping(API + "/{id}/detail/{detailId}/check-delete")
    @ResponseBody
    public ResponseEntity<?> checkDelete(@PathVariable int id, @PathVariable int detailId) {
        return run(() -> service.checkDetailDelete(id, detailId), "Could not delete the row.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int supplierId, @RequestParam(required = false) String branchIds) {
        return run(() -> service.history(dateType, fromDate, toDate, fromDocNo, toDocNo, supplierId, branchIds), "History failed.");
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
    public ResponseEntity<?> save(@RequestBody PurchaseInvoiceDirectPmRequest request) { return run(() -> service.save(request), "Save failed."); }

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
