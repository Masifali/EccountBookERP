package com.mst.controllers;

import com.mst.models.dto.PurchaseInvoiceDirectStoreVariantDto;
import com.mst.services.PurchaseInvoiceDirectStoreVariantService;
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
import java.util.List;
import java.util.Map;

/**
 * Supplier Portal, ModuleId 131 — screen 904 "Purchase Invoice Direct Store With Variant",
 * {@code frmPurchaseInvoiceDirectStoreWithVariant.cs}, DocumentTypeId 61, at /purchase/purchase-invoice-direct-store-with-variant.
 * See {@link PurchaseInvoiceDirectStoreVariantService} for the desktop trace, reproduced behaviour and deviations.
 */
@Controller
public class PurchaseInvoiceDirectStoreVariantController {


    private final PurchaseInvoiceDirectStoreVariantService service;
    public PurchaseInvoiceDirectStoreVariantController(PurchaseInvoiceDirectStoreVariantService service) { this.service = service; }

    @GetMapping("/purchase/purchase-invoice-direct-store-with-variant")
    public String page(Model model) {
        model.addAttribute("activeMenu", "purchase");
        return "purchase/purchase_invoice_direct_store_with_variant";
    }

    @GetMapping("/api/purchase/purchase-invoice-direct-store-with-variant/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/purchase/purchase-invoice-direct-store-with-variant/numbers")
    @ResponseBody
    public ResponseEntity<?> numbers() { return run(service::numbers, "Could not generate the Doc No."); }

    @GetMapping("/api/purchase/purchase-invoice-direct-store-with-variant/stock")
    @ResponseBody
    public ResponseEntity<?> stock(@RequestParam(defaultValue = "0") int itemId,
                                   @RequestParam(required = false) String docDate,
                                   @RequestParam(defaultValue = "0") int warehouseId) {
        return run(() -> service.stock(itemId, docDate, warehouseId), "Could not read the stock.");
    }

    @GetMapping("/api/purchase/purchase-invoice-direct-store-with-variant/uoms")
    @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam(defaultValue = "0") int itemId) { return run(() -> service.uoms(itemId), "Could not load UOMs."); }

    @GetMapping("/api/purchase/purchase-invoice-direct-store-with-variant/variants")
    @ResponseBody
    public ResponseEntity<?> variants(@RequestParam(defaultValue = "0") int itemId) { return run(() -> service.variants(itemId), "Could not load variants."); }

    @GetMapping("/api/purchase/purchase-invoice-direct-store-with-variant/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo,
                                     @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int supplierCustomerId) {
        return run(() -> service.history(dateType, fromDate, toDate, fromDocNo, toDocNo, supplierCustomerId),
                "History failed.");
    }

    @GetMapping("/api/purchase/purchase-invoice-direct-store-with-variant/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that document.")));
        }
    }

    @GetMapping("/api/purchase/purchase-invoice-direct-store-with-variant/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id) { return report(() -> service.slip(id)); }

    @GetMapping("/api/purchase/purchase-invoice-direct-store-with-variant/{id}/voucher")
    @ResponseBody
    public ResponseEntity<?> voucher(@PathVariable int id) { return report(() -> service.voucher118(id)); }

    @GetMapping("/api/purchase/purchase-invoice-direct-store-with-variant/{id}/voucher-slip")
    @ResponseBody
    public ResponseEntity<?> voucherSlip(@PathVariable int id) { return report(() -> service.voucher103(id)); }

    @PostMapping("/api/purchase/purchase-invoice-direct-store-with-variant/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody PurchaseInvoiceDirectStoreVariantDto dto) { return write(() -> service.save(dto)); }

    @PostMapping("/api/purchase/purchase-invoice-direct-store-with-variant/{id}/delete")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) { return write(() -> service.delete(id)); }

    @PostMapping("/api/purchase/purchase-invoice-direct-store-with-variant/{id}/detail/{detailId}/check-delete")
    @ResponseBody
    public ResponseEntity<?> checkDetailDelete(@PathVariable int id, @PathVariable int detailId) {
        return write(() -> service.checkDetailDelete(id, detailId));
    }

    // ------------------------------------------------------------------------------ plumbing

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> report(Call c) {
        try {
            Object rows = c.get();
            if (rows == null || (rows instanceof List && ((List<?>) rows).isEmpty())) {
                return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found For Display"));
            }
            return ResponseEntity.ok(rows);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(root(e), "Print failed.")));
        }
    }

    private static ResponseEntity<?> write(Call c) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(root(e), "Save failed.")));
        }
    }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(root(e), fallback)));
        }
    }

    private static Throwable root(Throwable e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        return r;
    }

    private static String msg(Throwable e, String fallback) {
        return (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
