package com.mst.controllers;

import com.mst.models.dto.StockTransferManualDto;
import com.mst.services.StockTransferManualService;
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
 * Store Management, ModuleId 24 — screen 332 "Stock Transfer Manual", {@code frmStockTransferManual.cs}
 * (ScreenName "frmStockTransferManual"), DocumentTypeId 806, at /store/stock-transfer-manual.
 * All behaviour notes and deviations are on {@link StockTransferManualService}.
 */
@Controller
public class StockTransferManualController {

    private static final String API = "/api/store/stock-transfer-manual";

    private final StockTransferManualService service;
    public StockTransferManualController(StockTransferManualService service) { this.service = service; }

    @GetMapping("/store/stock-transfer-manual")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/stock_transfer_manual";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/refresh")
    @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::combos, "Refresh failed."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> Collections.singletonMap("docNo", service.docNo()), "Could not number the document."); }

    @GetMapping(API + "/history-transfer-types")
    @ResponseBody
    public ResponseEntity<?> historyTransferTypes() { return run(service::historyTransferTypes, "Could not load transfer types."); }

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
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(rootCause(e), "Could not open that document.")));
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
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(rootCause(e), "Print failed.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody StockTransferManualDto dto) { return write(() -> service.save(dto), "Save failed."); }

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
