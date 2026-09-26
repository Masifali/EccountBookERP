package com.mst.controllers;

import com.mst.services.StoreStockConversionService;
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
 * Packing Material, ModuleId 54 - screen 502 "Store Stock Conversion", {@code StoreStockConversion.cs}
 * (ScreenName "StoreStockConversion"), DocTypeId 808, at /packing-material/store-stock-conversion.
 * Behaviour notes and deviations are on {@link StoreStockConversionService}.
 */
@Controller
public class StoreStockConversionController {

    private static final String API = "/api/packing-material/store-stock-conversion";

    private final StoreStockConversionService service;
    public StoreStockConversionController(StoreStockConversionService service) { this.service = service; }

    @GetMapping("/packing-material/store-stock-conversion")
    public String page(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        return "packing_material/store_stock_conversion";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/refresh")
    @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::refresh, "Could not refresh."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> Collections.singletonMap("docNo", service.docNo()), "Could not number the document."); }

    @GetMapping(API + "/stock")
    @ResponseBody
    public ResponseEntity<?> stock(@RequestParam(defaultValue = "0") int recId, @RequestParam(defaultValue = "0") int itemId,
                                   @RequestParam(defaultValue = "0") int conditionId, @RequestParam(defaultValue = "0") int warehouseId,
                                   @RequestParam(defaultValue = "0") int rackId, @RequestParam(required = false) String docDate) {
        return run(() -> service.stock(recId, itemId, conditionId, warehouseId, rackId, docDate), "Could not read the stock.");
    }

    @SuppressWarnings("unchecked")
    @PostMapping(API + "/rates")
    @ResponseBody
    public ResponseEntity<?> rates(@RequestBody Map<String, Object> body) {
        return run(() -> service.rates(body.get("recId") instanceof Number ? ((Number) body.get("recId")).intValue() : 0,
                (String) body.get("docDate"), (List<Map<String, Object>>) body.get("rows")), "Could not read the rates.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateType,
                                     @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo) {
        return run(() -> service.history(dateType, fromDate, toDate, fromDocNo, toDocNo), "History failed.");
    }

    @GetMapping(API + "/loader/lookups")
    @ResponseBody
    public ResponseEntity<?> loaderLookups() { return run(service::loaderLookups, "Could not load the loader."); }

    @GetMapping(API + "/loader/search")
    @ResponseBody
    public ResponseEntity<?> loaderSearch(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                          @RequestParam(defaultValue = "0") int parentCategoryId, @RequestParam(defaultValue = "0") int itemCategoryId,
                                          @RequestParam(defaultValue = "0") int itemTypeId, @RequestParam(defaultValue = "0") int warehouseId,
                                          @RequestParam(defaultValue = "0") int itemId, @RequestParam(defaultValue = "0") int itemConditionId) {
        return run(() -> service.loaderSearch(fromDate, toDate, parentCategoryId, itemCategoryId, itemTypeId, warehouseId, itemId, itemConditionId),
                "Could not load the stock.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record not found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(rootCause(e), "Could not open that document.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return write(() -> service.save(body), "Save failed."); }

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
