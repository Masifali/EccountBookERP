package com.mst.controllers;

import com.mst.services.SaleInvoiceDirect139Service;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.NoSuchElementException;

/**
 * Sale hub screen 145 "Sale Invoice Direct 139" (Architecture.WinApp.Sale.frmSaleInvoiceDirect139), DocumentTypeId 139,
 * at /sale/sale-invoice-direct-139. Behaviour notes and deviations are on {@link SaleInvoiceDirect139Service}.
 */
@Controller
public class SaleInvoiceDirect139Controller {

    private static final String API = "/sale/api/sale-invoice-direct-139";

    private final SaleInvoiceDirect139Service service;
    public SaleInvoiceDirect139Controller(SaleInvoiceDirect139Service service) { this.service = service; }

    @GetMapping("/sale/sale-invoice-direct-139")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        return "sale/sale_invoice_direct_139";
    }

    @GetMapping(API + "/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/numbers") @ResponseBody
    public ResponseEntity<?> numbers() { return run(service::numbers, "Could not read the document number."); }

    @GetMapping(API + "/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam int itemId) { return run(() -> service.uoms(itemId), "Could not load the units."); }

    @GetMapping(API + "/stock") @ResponseBody
    public ResponseEntity<?> stock(@RequestParam int itemId, @RequestParam(required = false) String docDate,
                                   @RequestParam(defaultValue = "0") int packingTypeId, @RequestParam(defaultValue = "0") int stockUom) {
        return run(() -> service.stock(itemId, docDate, packingTypeId, stockUom), "Could not read the stock.");
    }

    @GetMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "0") int noOfRecords) {
        return run(() -> service.history(noOfRecords), "History failed.");
    }

    @GetMapping(API + "/{id}") @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            return ResponseEntity.ok(service.read(id));
        } catch (NoSuchElementException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found"));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(rootCause(e), "Could not open that document.")));
        }
    }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> req) { return write(() -> service.save(req), "Save failed."); }

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
