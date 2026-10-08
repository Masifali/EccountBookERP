package com.mst.controllers;

import com.mst.services.SaleGdnAgainstOrderService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/** Sale hub screen 142 "Goods Dispatch Notes" (frmGdnAgainstSaleOrder, DocumentTypeId 170) at /sale/gdn-against-sale-order. */
@Controller
public class SaleGdnAgainstOrderController {
    private static final String API = "/sale/api/gdn-against-sale-order";
    private final SaleGdnAgainstOrderService service;
    public SaleGdnAgainstOrderController(SaleGdnAgainstOrderService service) { this.service = service; }

    @GetMapping("/sale/gdn-against-sale-order")
    public String page(Model model) { model.addAttribute("activeMenu", "sale"); return "sale/gdn_against_sale_order"; }

    @GetMapping(API + "/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }
    @GetMapping(API + "/next-no") @ResponseBody
    public ResponseEntity<?> nextNo() { return run(() -> Collections.singletonMap("nextNo", service.nextNo()), "Could not read the document number."); }
    @GetMapping(API + "/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam int itemId) { return run(() -> service.uoms(itemId), "Could not load the units."); }
    @GetMapping(API + "/stock") @ResponseBody
    public ResponseEntity<?> stock(@RequestParam int itemId, @RequestParam(required = false) String cropYear, @RequestParam(required = false) String docDate,
                                   @RequestParam(defaultValue = "0") int jobLotId, @RequestParam(defaultValue = "0") int warehouseId) {
        return run(() -> service.stock(itemId, cropYear, docDate, jobLotId, warehouseId), "Could not read the stock.");
    }
    @GetMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String mode, @RequestParam(required = false) String from, @RequestParam(required = false) String to,
                                     @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo, @RequestParam(defaultValue = "0") int customerId) {
        return run(() -> service.history(mode, from, to, fromNo, toNo, customerId), "Could not load the history.");
    }
    @GetMapping(API + "/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> record(@PathVariable int id) { return run(() -> service.record(id), "Record not found..."); }

    @GetMapping(API + "/loader/setup") @ResponseBody
    public ResponseEntity<?> loaderSetup() { return run(service::loaderSetup, "Could not open the sale order list."); }
    @GetMapping(API + "/loader/combos") @ResponseBody
    public ResponseEntity<?> loaderCombos(@RequestParam(defaultValue = "") String branchIds) { return run(() -> service.loaderCombos(branchIds), "Could not load the filters."); }
    @GetMapping(API + "/loader/search") @ResponseBody
    public ResponseEntity<?> loaderSearch(@RequestParam(defaultValue = "") String branchIds, @RequestParam(defaultValue = "0") int customerId,
                                          @RequestParam(defaultValue = "0") int itemId, @RequestParam(required = false) String from, @RequestParam(required = false) String to) {
        return write(() -> service.loaderSearch(branchIds, customerId, itemId, from, to), "Could not search the sale orders.");
    }
    @PostMapping(API + "/loader/load") @ResponseBody
    public ResponseEntity<?> loaderLoad(@RequestBody Map<String, Object> body) {
        @SuppressWarnings("unchecked") List<Number> ids = (List<Number>) body.getOrDefault("orderIds", List.of());
        return write(() -> service.loaderLoad(ids), "Could not load the sale order.");
    }

    @PostMapping(API) @ResponseBody
    public ResponseEntity<?> insert(@RequestBody Map<String, Object> body) { body.put("id", 0); return write(() -> service.save(body), "Could not save the record."); }
    @PutMapping(API + "/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> update(@PathVariable int id, @RequestBody Map<String, Object> body) { body.put("id", id); return write(() -> service.save(body), "Could not update the record."); }
    @DeleteMapping(API + "/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) {
        return write(() -> { service.delete(id); return Collections.singletonMap("message", "Delete Record Seccessfully"); }, "Could not delete the record.");
    }

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
