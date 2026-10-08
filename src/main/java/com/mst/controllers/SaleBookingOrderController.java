package com.mst.controllers;

import com.mst.services.SaleBookingOrderService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/** Sale hub screen 140 "Booking Order" (BookingOrder, DocumentTypeId 127) at /sale/booking-order. */
@Controller
public class SaleBookingOrderController {
    private static final String API = "/sale/api/booking-order";
    private final SaleBookingOrderService service;
    public SaleBookingOrderController(SaleBookingOrderService service) { this.service = service; }

    @GetMapping("/sale/booking-order")
    public String page(Model model) { model.addAttribute("activeMenu", "sale"); return "sale/booking_order"; }

    @GetMapping(API + "/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }
    @GetMapping(API + "/numbers") @ResponseBody
    public ResponseEntity<?> numbers(@RequestParam(defaultValue = "0") int categoryId) { return run(() -> service.numbers(categoryId), "Could not read the document number."); }
    @GetMapping(API + "/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam int itemId) { return write(() -> service.uoms(itemId), "Could not load the units."); }
    @GetMapping(API + "/crop-years") @ResponseBody
    public ResponseEntity<?> cropYears(@RequestParam int itemId, @RequestParam(required = false) String date) { return run(() -> service.cropYears(itemId, date), "Could not load the crop years."); }
    @GetMapping(API + "/pricing") @ResponseBody
    public ResponseEntity<?> pricing(@RequestParam int itemId, @RequestParam int cropYearId, @RequestParam(defaultValue = "0") int customerId, @RequestParam(required = false) String date) {
        return run(() -> service.pricing(itemId, cropYearId, customerId, date), "Could not read the item rate.");
    }
    @GetMapping(API + "/customer-discount") @ResponseBody
    public ResponseEntity<?> customerDiscount(@RequestParam int customerId, @RequestParam int itemId, @RequestParam(required = false) String date) {
        return run(() -> service.customerDiscount(customerId, itemId, date), "Could not read the customer discount.");
    }
    @GetMapping(API + "/packing-add-less") @ResponseBody
    public ResponseEntity<?> packingAddLess(@RequestParam int uomId, @RequestParam(defaultValue = "0") int customerId, @RequestParam(required = false) String date) {
        return run(() -> service.packingAddLess(uomId, customerId, date), "Could not read the packing price.");
    }
    @GetMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String mode, @RequestParam(required = false) String from, @RequestParam(required = false) String to,
                                     @RequestParam(defaultValue = "0") double fromNo, @RequestParam(defaultValue = "0") double toNo, @RequestParam(defaultValue = "0") int customerId,
                                     @RequestParam(required = false) String branchIds) {
        return run(() -> service.history(mode, from, to, fromNo, toNo, customerId, branchIds), "Could not load the history.");
    }
    @GetMapping(API + "/history-customers") @ResponseBody
    public ResponseEntity<?> historyCustomers(@RequestParam(defaultValue = "") String branchIds) { return run(() -> service.historyCustomers(branchIds), "Could not load the customers."); }
    @GetMapping(API + "/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> record(@PathVariable int id) { return write(() -> service.record(id), "Record not found..."); }

    @PostMapping(API + "/checks") @ResponseBody
    public ResponseEntity<?> checks(@RequestBody Map<String, Object> body) { return write(() -> service.checks(body), "Could not check the booking order."); }
    @PostMapping(API) @ResponseBody
    public ResponseEntity<?> insert(@RequestBody Map<String, Object> body) { body.put("id", 0); return write(() -> service.save(body), "Could not save the record."); }
    @PutMapping(API + "/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> update(@PathVariable int id, @RequestBody Map<String, Object> body) { body.put("id", id); body.put("mode", "update"); return write(() -> service.save(body), "Could not update the record."); }

    private interface Call { Object get() throws Exception; }
    private static ResponseEntity<?> run(Call c, String fallback) {
        try { return ResponseEntity.ok(c.get()); }
        catch (Exception e) { return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(root(e), fallback))); }
    }
    private static ResponseEntity<?> write(Call c, String fallback) {
        try { return ResponseEntity.ok(c.get()); }
        catch (IllegalArgumentException | NoSuchElementException e) { return ResponseEntity.badRequest().body(fail(e.getMessage())); }
        catch (Exception e) { return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(root(e), fallback))); }
    }
    private static Throwable root(Throwable t) { Throwable r = t; while (r.getCause() != null && r.getCause() != r) r = r.getCause(); return r; }
    private static String msg(Throwable t, String fb) { String m = t == null ? null : t.getMessage(); return m == null || m.isBlank() ? fb : m; }
    private static Map<String, Object> fail(String m) { Map<String, Object> x = new LinkedHashMap<>(); x.put("success", false); x.put("message", m); return x; }
}
