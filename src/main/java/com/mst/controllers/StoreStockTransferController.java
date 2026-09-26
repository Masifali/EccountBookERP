package com.mst.controllers;

import com.mst.models.dto.StoreStockTransferDto;
import com.mst.services.StoreStockTransferService;
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

import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Store Management, ModuleId 24 — screen 339 "Stock Transfer", {@code frmStockTransfer.cs}
 * (ScreenName "frmStockTransfer"), DocumentTypeId 68, at /store/stock-transfer.
 * All behaviour notes and deviations are on {@link StoreStockTransferService}.
 */
@Controller
public class StoreStockTransferController {

    private static final String API = "/api/store/stock-transfer";

    private final StoreStockTransferService service;
    public StoreStockTransferController(StoreStockTransferService service) { this.service = service; }

    @GetMapping("/store/stock-transfer")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/stock_transfer";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/refresh")
    @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::reloadCombos, "Refresh failed."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> Collections.singletonMap("docNo", service.docNo()), "Could not number the document."); }

    @GetMapping(API + "/items")
    @ResponseBody
    public ResponseEntity<?> items() { return run(service::items, "Could not load items."); }

    @GetMapping(API + "/pending")
    @ResponseBody
    public ResponseEntity<?> pending() { return run(service::pending, "Could not load pending entries."); }

    @GetMapping(API + "/gate-passes")
    @ResponseBody
    public ResponseEntity<?> gatePasses(@RequestParam(defaultValue = "") String transferType,
                                        @RequestParam(defaultValue = "0") int refDocumentTypeId) {
        return run(() -> service.gatePasses(transferType, refDocumentTypeId), "Could not load gate passes.");
    }

    @GetMapping(API + "/tickets")
    @ResponseBody
    public ResponseEntity<?> tickets(@RequestParam int gatePassId, @RequestParam(defaultValue = "") String transferType,
                                     @RequestParam(defaultValue = "0") int refDocumentTypeId) {
        return run(() -> service.tickets(gatePassId, transferType, refDocumentTypeId), "Could not load tickets.");
    }

    @GetMapping(API + "/move-order-tickets")
    @ResponseBody
    public ResponseEntity<?> moveOrderTickets() { return run(service::moveOrderTickets, "Could not load tickets."); }

    @GetMapping(API + "/ticket-weight")
    @ResponseBody
    public ResponseEntity<?> ticketWeight(@RequestParam(defaultValue = "0") int ticketId) {
        return run(() -> service.ticketWeight(ticketId), "Could not read the ticket.");
    }

    @GetMapping(API + "/delivery-order-items")
    @ResponseBody
    public ResponseEntity<?> doItems(@RequestParam(defaultValue = "0") int gatePassId) {
        return run(() -> service.itemsFromDeliveryOrder(gatePassId), "Could not load items.");
    }

    @GetMapping(API + "/branch-data")
    @ResponseBody
    public ResponseEntity<?> branchData(@RequestParam(defaultValue = "0") int gatePassId) {
        return run(() -> service.branchData(gatePassId), "Could not load the transfer.");
    }

    @GetMapping(API + "/uoms")
    @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.uoms(itemId), "Could not load units.");
    }

    @GetMapping(API + "/available-stock")
    @ResponseBody
    public ResponseEntity<?> availableStock(@RequestParam(defaultValue = "0") int recId, @RequestParam(defaultValue = "0") int itemId,
                                            @RequestParam(defaultValue = "0") int warehouseId, @RequestParam(defaultValue = "0") int jobLotId,
                                            @RequestParam(required = false) String cropYear, @RequestParam(required = false) String docDate) {
        return run(() -> Collections.singletonMap("availableStock",
                service.availableStock(recId, itemId, warehouseId, jobLotId, cropYear, docDate)), "Could not read the stock.");
    }

    @GetMapping(API + "/avg-rate")
    @ResponseBody
    public ResponseEntity<?> avgRate(@RequestParam(defaultValue = "0") int recId, @RequestParam(defaultValue = "0") int itemId,
                                     @RequestParam(required = false) String docDate, @RequestParam(defaultValue = "0") int jobLotId,
                                     @RequestParam(defaultValue = "0") int cropYearId, @RequestParam(required = false) String cropYear,
                                     @RequestParam(defaultValue = "0") int warehouseId) {
        return run(() -> Collections.singletonMap("rate",
                service.avgRate(recId, itemId, docDate, jobLotId, cropYearId, cropYear, warehouseId)), "Could not read the rate.");
    }

    @GetMapping(API + "/loader/lookups")
    @ResponseBody
    public ResponseEntity<?> loaderLookups() { return run(service::loaderLookups, "Could not open the loader."); }

    @GetMapping(API + "/loader/search")
    @ResponseBody
    public ResponseEntity<?> loaderSearch(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                          @RequestParam(defaultValue = "0") int parentCategoryId, @RequestParam(defaultValue = "0") int itemCategoryId,
                                          @RequestParam(defaultValue = "0") int itemTypeId, @RequestParam(defaultValue = "0") int jobLotId,
                                          @RequestParam(required = false) String cropYear, @RequestParam(defaultValue = "0") int warehouseId,
                                          @RequestParam(defaultValue = "0") int refDocumentTypeId, @RequestParam(defaultValue = "0") int supplierId,
                                          @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.loaderSearch(fromDate, toDate, parentCategoryId, itemCategoryId, itemTypeId, jobLotId,
                cropYear, warehouseId, refDocumentTypeId, supplierId, itemId), "Search failed.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(required = false) String transferType) {
        return run(() -> service.history(dateType, fromDate, toDate, fromDocNo, toDocNo, transferType), "History failed.");
    }

    @GetMapping(API + "/{id}/history-detail")
    @ResponseBody
    public ResponseEntity<?> historyDetail(@PathVariable int id) {
        return run(() -> {
            List<Map<String, Object>> r = service.historyDetail(id);
            return r == null ? Collections.emptyList() : r;
        }, "Could not read the detail.");
    }

    @GetMapping(API + "/{id}")
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

    @GetMapping(API + "/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id) {
        try {
            List<Map<String, Object>> rows = service.slip(id);
            if (rows == null || rows.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found For Display"));
            return ResponseEntity.ok(rows);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody StoreStockTransferDto dto) { return write(() -> service.save(dto), "Save failed."); }

    @PostMapping(API + "/{id}/delete")
    @ResponseBody
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
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
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
