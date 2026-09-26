package com.mst.controllers;

import com.mst.models.dto.StockAdjustmentPmDto;
import com.mst.services.StockAdjustmentPmService;
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
import java.util.Map;

/**
 * Packing Material, ModuleId 54 - screen 492 "Stock Adjustment For PM", {@code StockAdjustmentForPM.cs}
 * (ScreenName "StockAdjustmentForPM"), DocumentTypeId 215, at /packing-material/stock-adjustment.
 * Behaviour notes and deviations are on {@link StockAdjustmentPmService}.
 */
@Controller
public class StockAdjustmentPmController {

    private static final String API = "/api/packing-material/stock-adjustment";

    private final StockAdjustmentPmService service;
    public StockAdjustmentPmController(StockAdjustmentPmService service) { this.service = service; }

    @GetMapping("/packing-material/stock-adjustment")
    public String page(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        return "packing_material/stock_adjustment_pm";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/doc-no")
    @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> Collections.singletonMap("docNo", service.docNo()), "Could not number the document."); }

    @GetMapping(API + "/accounts")
    @ResponseBody
    public ResponseEntity<?> accounts(@RequestParam(defaultValue = "0") int entryTypeId) { return run(() -> service.accounts(entryTypeId), "Could not load accounts."); }

    @GetMapping(API + "/stock")
    @ResponseBody
    public ResponseEntity<?> stock(@RequestParam(defaultValue = "0") int recId, @RequestParam(defaultValue = "0") int itemId,
                                   @RequestParam(defaultValue = "0") int conditionId, @RequestParam(defaultValue = "0") int warehouseId,
                                   @RequestParam(defaultValue = "0") int rackId, @RequestParam(defaultValue = "0") int entryTypeId,
                                   @RequestParam(required = false) String docDate) {
        return run(() -> service.stock(recId, itemId, conditionId, warehouseId, rackId, entryTypeId, docDate), "Could not read the stock.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int adjustmentTypeId) {
        return run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo, adjustmentTypeId), "History failed.");
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
    public ResponseEntity<?> save(@RequestBody StockAdjustmentPmDto dto) { return write(() -> service.save(dto), "Save failed."); }

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
