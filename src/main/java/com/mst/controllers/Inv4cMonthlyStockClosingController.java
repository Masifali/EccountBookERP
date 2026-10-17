package com.mst.controllers;

import com.mst.services.Inv4cStockClosingService;
import com.mst.services.Inv4cStockClosingService.Variant;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * MonthlyStockClosing - ScreenDefinition 103 "Stock Closing" (Inventory hub, module 4). DocumentTypeId 300.
 * Errors: validation → 400 {message}; missing right → 403; anything else → 500.
 */
@Controller
@RequestMapping("/inventory/monthly-stock-closing")
public class Inv4cMonthlyStockClosingController {

    private static final Variant V = Variant.MONTHLY_STOCK_CLOSING;
    private final Inv4cStockClosingService service;
    public Inv4cMonthlyStockClosingController(Inv4cStockClosingService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock Closing");
        return "inventory/inv4c_monthly_stock_closing";
    }

    /** MonthlyStockClosing_Load :101 — rights + Difference Account combo. */
    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return call(() -> service.lookups(V), "Could not load the screen."); }

    /** btnShow_Click :154. */
    @GetMapping("/api/show") @ResponseBody
    public ResponseEntity<?> show(@RequestParam String asOnDate) { return call(() -> service.show(V, asOnDate), "Show failed."); }

    /** btnSave_Click :405 up to the confirmation box. */
    @PostMapping("/api/precheck") @ResponseBody
    public ResponseEntity<?> precheck(@RequestBody Map<String, Object> body) { return call(() -> service.precheck(V, body), "Save failed."); }

    /** btnSave_Click :442 → StockMonthlyClosingHeader.Save. */
    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> service.save(V, body), "Save failed."); }

    /** btnSlip_Click :590 — 300-StockMonthlyClosingSlip rows. */
    @GetMapping("/api/slip") @ResponseBody
    public ResponseEntity<?> slip(@RequestParam(defaultValue = "0") int recId, @RequestParam String asOnDate,
                                  @RequestParam(defaultValue = "false") boolean afterSave) {
        return call(() -> service.slip(V, recId, asOnDate, afterSave), "Print failed.");
    }

    /** btnprint_Click :514 — 103-AcRptPurchaseSalesVoucherSlip rows. */
    @GetMapping("/api/voucher") @ResponseBody
    public ResponseEntity<?> voucher(@RequestParam(defaultValue = "0") int recId) { return call(() -> service.voucher(V, recId), "Print failed."); }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> call(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return fail(HttpStatus.BAD_REQUEST, msg(e, fallback));
        } catch (IllegalStateException e) {
            return fail(HttpStatus.FORBIDDEN, msg(e, fallback));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return fail(HttpStatus.INTERNAL_SERVER_ERROR, msg(r, fallback));
        }
    }

    private static String msg(Throwable e, String fallback) {
        return (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
    }

    private static ResponseEntity<Map<String, Object>> fail(HttpStatus status, String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return ResponseEntity.status(status).body(m);
    }
}
