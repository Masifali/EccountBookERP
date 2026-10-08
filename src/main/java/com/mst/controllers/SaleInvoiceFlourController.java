package com.mst.controllers;

import com.mst.services.SaleInvoiceFlourService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.NoSuchElementException;

/**
 * Sale hub screens 137 "Sale Invoice Direct (Flour)" (frmSaleInvoiceDirect, DocumentTypeId 186) at /sale/sale-invoice-direct-flour
 * and 138 "Sale Invoice Direct Auto Rated" (frmSaleInvoiceDirectAutoRated, DocumentTypeId 184) at /sale/sale-invoice-direct-auto-rated.
 * Both forms share one backend; the document type is the path variable of the API.
 */
@Controller
public class SaleInvoiceFlourController {

    private static final String API = "/sale/api/sale-invoice-flour/{doc}";

    private final SaleInvoiceFlourService service;
    public SaleInvoiceFlourController(SaleInvoiceFlourService service) { this.service = service; }

    @GetMapping("/sale/sale-invoice-direct-flour")
    public String page137(Model model) {
        model.addAttribute("activeMenu", "sale");
        return "sale/sale_invoice_flour_137";
    }

    @GetMapping("/sale/sale-invoice-direct-auto-rated")
    public String page138(Model model) {
        model.addAttribute("activeMenu", "sale");
        return "sale/sale_invoice_flour_138";
    }

    private static boolean bad(int doc) { return doc != 184 && doc != 186; }
    private static ResponseEntity<?> badDoc() { return ResponseEntity.badRequest().body(fail("Unknown document type.")); }

    @GetMapping(API + "/lookups") @ResponseBody
    public ResponseEntity<?> lookups(@PathVariable int doc) { return bad(doc) ? badDoc() : run(() -> service.lookups(doc), "Could not load the screen."); }

    @GetMapping(API + "/numbers") @ResponseBody
    public ResponseEntity<?> numbers(@PathVariable int doc) { return bad(doc) ? badDoc() : run(() -> service.numbers(doc), "Could not read the document number."); }

    @GetMapping(API + "/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@PathVariable int doc, @RequestParam int itemId) { return bad(doc) ? badDoc() : run(() -> service.uoms(itemId), "Could not load the units."); }

    @GetMapping(API + "/rate-uoms") @ResponseBody
    public ResponseEntity<?> rateUoms(@PathVariable int doc, @RequestParam int itemId, @RequestParam(required = false) String docDate) {
        return bad(doc) ? badDoc() : run(() -> service.rateUoms(itemId, docDate), "Could not load the rate units.");
    }

    @GetMapping(API + "/rate") @ResponseBody
    public ResponseEntity<?> rate(@PathVariable int doc, @RequestParam int itemId, @RequestParam(required = false) String docDate,
                                  @RequestParam(defaultValue = "0") int rateUomId) {
        return bad(doc) ? badDoc() : run(() -> service.rate(itemId, docDate, rateUomId), "Could not read the rate.");
    }

    @GetMapping(API + "/stock") @ResponseBody
    public ResponseEntity<?> stock(@PathVariable int doc, @RequestParam int itemId, @RequestParam(required = false) String docDate,
                                   @RequestParam(defaultValue = "0") int warehouseId, @RequestParam(defaultValue = "0") int jobLotId,
                                   @RequestParam(required = false) String cropYear, @RequestParam(defaultValue = "0") int packingTypeId,
                                   @RequestParam(defaultValue = "0") int stockUom) {
        return bad(doc) ? badDoc() : run(() -> service.stock(itemId, docDate, warehouseId, jobLotId, cropYear, packingTypeId, stockUom), "Could not read the stock.");
    }

    @GetMapping(API + "/history-customers") @ResponseBody
    public ResponseEntity<?> historyCustomers(@PathVariable int doc) { return bad(doc) ? badDoc() : run(() -> service.historyCustomers(doc), "Could not load the customers."); }

    @GetMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@PathVariable int doc, @RequestParam(required = false) String dateMode, @RequestParam(required = false) String from,
                                     @RequestParam(required = false) String to, @RequestParam(defaultValue = "0") int fromDocNo,
                                     @RequestParam(defaultValue = "0") int toDocNo, @RequestParam(defaultValue = "0") int customerId) {
        return bad(doc) ? badDoc() : run(() -> service.history(doc, dateMode, from, to, fromDocNo, toDocNo, customerId), "History failed.");
    }

    @GetMapping(API + "/loader/setup") @ResponseBody
    public ResponseEntity<?> loaderSetup(@PathVariable int doc) { return bad(doc) ? badDoc() : run(service::loaderSetup, "Could not open the issuance loader."); }

    @GetMapping(API + "/loader/search") @ResponseBody
    public ResponseEntity<?> loaderSearch(@PathVariable int doc, @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                          @RequestParam(defaultValue = "0") int parentCategoryId, @RequestParam(defaultValue = "0") int itemCategoryId,
                                          @RequestParam(defaultValue = "0") int itemTypeId, @RequestParam(defaultValue = "0") int jobLotId,
                                          @RequestParam(required = false) String cropYear, @RequestParam(defaultValue = "0") int warehouseId,
                                          @RequestParam(defaultValue = "0") int refDocumentTypeId, @RequestParam(defaultValue = "0") int supplierCustomerId,
                                          @RequestParam(defaultValue = "0") int itemId) {
        return bad(doc) ? badDoc() : run(() -> service.loaderSearch(fromDate, toDate, parentCategoryId, itemCategoryId, itemTypeId, jobLotId, cropYear,
                warehouseId, refDocumentTypeId, supplierCustomerId, itemId), "Could not search the issuances.");
    }

    @GetMapping(API + "/{id:\\d+}") @ResponseBody
    public ResponseEntity<?> load(@PathVariable int doc, @PathVariable int id) {
        if (bad(doc)) return badDoc();
        try {
            return ResponseEntity.ok(service.read(doc, id));
        } catch (NoSuchElementException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found"));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(rootCause(e), "Could not open that document.")));
        }
    }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@PathVariable int doc, @RequestBody Map<String, Object> req) { return bad(doc) ? badDoc() : write(() -> service.save(doc, req), "Save failed."); }

    @DeleteMapping(API + "/{id:\\d+}") @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int doc, @PathVariable int id) { return bad(doc) ? badDoc() : write(() -> service.delete(doc, id), "Delete failed."); }

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
