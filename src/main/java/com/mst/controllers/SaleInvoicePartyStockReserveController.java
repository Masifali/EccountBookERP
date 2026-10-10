package com.mst.controllers;

import com.mst.services.SaleInvoicePartyStockReserveService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.NoSuchElementException;

/**
 * Sale hub screen 143 "SaleInvoicepartyStockReserve" (desktop InvfrmInvSaleInvoiceDirect with FlagForm = 1, DocumentTypeId 99)
 * at /sale/sale-invoice-party-stock-reserve.
 */
@Controller
public class SaleInvoicePartyStockReserveController {

    private static final String API = "/sale/api/sale-invoice-party-stock-reserve";

    private final SaleInvoicePartyStockReserveService service;
    public SaleInvoicePartyStockReserveController(SaleInvoicePartyStockReserveService service) { this.service = service; }

    @GetMapping("/sale/sale-invoice-party-stock-reserve")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        return "sale/sale_invoice_party_stock_reserve";
    }

    @GetMapping(API + "/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/numbers") @ResponseBody
    public ResponseEntity<?> numbers() { return run(service::numbers, "Could not read the document number."); }

    @GetMapping(API + "/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam int itemId) { return run(() -> service.uoms(itemId), "Could not load the units."); }

    @GetMapping(API + "/items-by-warehouse") @ResponseBody
    public ResponseEntity<?> itemsByWarehouse(@RequestParam int warehouseId) { return run(() -> service.itemsByWarehouse(warehouseId), "Could not load the items."); }

    @GetMapping(API + "/stock") @ResponseBody
    public ResponseEntity<?> stock(@RequestParam int itemId, @RequestParam(required = false) String docDate,
                                   @RequestParam(defaultValue = "0") int warehouseId, @RequestParam(defaultValue = "0") int jobLotId,
                                   @RequestParam(required = false) String cropYear, @RequestParam(defaultValue = "0") int packingTypeId,
                                   @RequestParam(defaultValue = "0") int stockUom) {
        return run(() -> service.stock(itemId, docDate, warehouseId, jobLotId, cropYear, packingTypeId, stockUom), "Could not read the stock.");
    }

    @GetMapping(API + "/exchange-rate") @ResponseBody
    public ResponseEntity<?> exchangeRate(@RequestParam int currencyId) { return run(() -> service.exchangeRate(currencyId), "Could not read the exchange rate."); }

    @GetMapping(API + "/ledger") @ResponseBody
    public ResponseEntity<?> ledger(@RequestParam int customerId, @RequestParam(required = false) String docDate) {
        return run(() -> service.ledger(customerId, docDate), "Could not read the ledger balance.");
    }

    @GetMapping(API + "/history-branches") @ResponseBody
    public ResponseEntity<?> historyBranches() { return run(service::historyBranches, "Could not load the branches."); }

    @GetMapping(API + "/history-customers") @ResponseBody
    public ResponseEntity<?> historyCustomers() { return run(service::historyCustomers, "Could not load the customers."); }

    @GetMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String dateMode, @RequestParam(required = false) String from,
                                     @RequestParam(required = false) String to, @RequestParam(defaultValue = "0") int fromDocNo,
                                     @RequestParam(defaultValue = "0") int toDocNo, @RequestParam(defaultValue = "0") int customerId,
                                     @RequestParam(required = false) String branchIds) {
        return run(() -> service.history(dateMode, from, to, fromDocNo, toDocNo, customerId, branchIds), "History failed.");
    }

    @GetMapping(API + "/auto-update-ids") @ResponseBody
    public ResponseEntity<?> autoUpdateIds() { return run(service::autoUpdateIds, "Could not list the records."); }

    @GetMapping(API + "/{id:\\d+}") @ResponseBody
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

    @DeleteMapping(API + "/{id:\\d+}") @ResponseBody
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
