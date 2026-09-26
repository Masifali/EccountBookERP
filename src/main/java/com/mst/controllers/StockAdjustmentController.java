package com.mst.controllers;

import com.mst.models.dto.StockAdjustmentDto;
import com.mst.services.StockAdjustmentService;
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
 * Store Management, ModuleId 24 — screen 320 "Stock Adjustment", {@code frmStockAdjustment.cs}
 * (ScreenName "frmStockAdjustment"), DocumentTypeId 70, at /store/stock-adjustment.
 * API base /api/store/stock-adjustment. Behaviour notes and deviations: StockAdjustmentService.
 */
@Controller
public class StockAdjustmentController {

    private static final String API = "/api/store/stock-adjustment";

    private final StockAdjustmentService service;
    public StockAdjustmentController(StockAdjustmentService service) { this.service = service; }

    @GetMapping("/store/stock-adjustment")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/stock_adjustment";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> java.util.Collections.singletonMap("docNo", service.docNo()), "Could not read the doc no."); }

    @GetMapping(API + "/refresh")
    @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::refresh, "Refresh failed."); }

    @GetMapping(API + "/history-types")
    @ResponseBody
    public ResponseEntity<?> historyTypes() { return run(service::historyTypes, "Could not load the types."); }

    @GetMapping(API + "/uoms")
    @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.uoms(itemId), "Could not load the units.");
    }

    @GetMapping(API + "/accounts")
    @ResponseBody
    public ResponseEntity<?> accounts(@RequestParam(defaultValue = "0") int entryTypeId) {
        return run(() -> service.accounts(entryTypeId), "Could not load the accounts.");
    }

    @GetMapping(API + "/stock-balance")
    @ResponseBody
    public ResponseEntity<?> stockBalance(@RequestParam(defaultValue = "0") int recId,
                                          @RequestParam(defaultValue = "0") int itemId,
                                          @RequestParam(required = false) String docDate,
                                          @RequestParam(defaultValue = "0") int jobLotId,
                                          @RequestParam(defaultValue = "0") int warehouseId,
                                          @RequestParam(required = false) String cropYear) {
        return run(() -> service.stockBalance(recId, itemId, docDate, jobLotId, warehouseId, cropYear), "Could not read the stock.");
    }

    @GetMapping(API + "/rate")
    @ResponseBody
    public ResponseEntity<?> rate(@RequestParam(defaultValue = "entry") String mode,
                                  @RequestParam(defaultValue = "0") int recId,
                                  @RequestParam(defaultValue = "0") int itemId,
                                  @RequestParam(required = false) String docDate,
                                  @RequestParam(defaultValue = "0") int packUomId,
                                  @RequestParam(defaultValue = "0") int warehouseId,
                                  @RequestParam(defaultValue = "0") int packingTypeId,
                                  @RequestParam(defaultValue = "0") int jobLotId,
                                  @RequestParam(defaultValue = "0") int cropYearId,
                                  @RequestParam(defaultValue = "0") double qty,
                                  @RequestParam(defaultValue = "0") double weight) {
        return run(() -> service.rate(mode, recId, itemId, docDate, packUomId, warehouseId, packingTypeId, jobLotId,
                cropYearId, qty, weight), "Could not read the rate.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int docNoFrom,
                                     @RequestParam(defaultValue = "0") int docNoTo,
                                     @RequestParam(defaultValue = "0") int adjustmentTypeId) {
        return run(() -> service.history(fromDate, toDate, docNoFrom, docNoTo, adjustmentTypeId), "History failed.");
    }

    @GetMapping(API + "/loader/lookups")
    @ResponseBody
    public ResponseEntity<?> loaderLookups() { return run(service::loaderLookups, "Could not open the loader."); }

    @GetMapping(API + "/loader/search")
    @ResponseBody
    public ResponseEntity<?> loaderSearch(@RequestParam(required = false) String fromDate,
                                          @RequestParam(required = false) String toDate,
                                          @RequestParam(defaultValue = "0") int parentCategoryId,
                                          @RequestParam(defaultValue = "0") int itemCategoryId,
                                          @RequestParam(defaultValue = "0") int itemTypeId,
                                          @RequestParam(defaultValue = "0") int jobLotId,
                                          @RequestParam(required = false) String cropYear,
                                          @RequestParam(defaultValue = "0") int warehouseId,
                                          @RequestParam(defaultValue = "0") int refDocumentTypeId,
                                          @RequestParam(defaultValue = "0") int supplierCustomerId,
                                          @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.loaderSearch(fromDate, toDate, parentCategoryId, itemCategoryId, itemTypeId, jobLotId,
                cropYear, warehouseId, refDocumentTypeId, supplierCustomerId, itemId), "Search failed.");
    }

    @GetMapping(API + "/voucher/{voucherHeadId}/slip")
    @ResponseBody
    public ResponseEntity<?> voucherSlip(@PathVariable int voucherHeadId) {
        try {
            List<Map<String, Object>> rows = service.voucherSlip(voucherHeadId);
            if (rows == null || rows.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found For Display"));
            return ResponseEntity.ok(rows);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
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
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody StockAdjustmentDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.badRequest().body(fail(msg(r, "Save failed.")));
        }
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(fail(msg(e, fallback)));
        }
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
