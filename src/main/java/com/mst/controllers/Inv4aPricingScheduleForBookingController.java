package com.mst.controllers;

import com.mst.services.Inv4aPricingScheduleForBookingService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** frmItemPricingSchedule_New — ScreenDefinition 796 "Pricing Schedule For Booking" (Inventory hub, module 4 Inventory Defination). */
@Controller
@RequestMapping("/inventory/pricing-schedule-for-booking")
public class Inv4aPricingScheduleForBookingController {

    private final Inv4aPricingScheduleForBookingService service;
    public Inv4aPricingScheduleForBookingController(Inv4aPricingScheduleForBookingService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Pricing Schedule For Booking");
        return "inventory/inv4a_pricing_schedule_for_booking";
    }

    /** frmItemPricingSchedule_Load :220 */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() { return run(service::init); }

    /** btnrefresh_Click :1072 */
    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::refresh); }

    /** btnRefreshHistory_Click :1194 → HistoryComboFill :399 */
    @GetMapping("/api/history-lookups") @ResponseBody
    public ResponseEntity<?> historyLookups() { return run(service::historyLookups); }

    /** ItemFill :251 */
    @GetMapping("/api/items") @ResponseBody
    public ResponseEntity<?> items(@RequestParam(defaultValue = "0") int parentId) { return run(() -> service.items(parentId)); }

    /** cmbItem_ValueChanged :703 */
    @GetMapping("/api/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam(defaultValue = "0") int itemId) { return run(() -> service.uoms(itemId)); }

    /** CmbRateUom_Leave :922 */
    @GetMapping("/api/last-rate") @ResponseBody
    public ResponseEntity<?> lastRate(@RequestParam(defaultValue = "0") int itemId, @RequestParam(defaultValue = "0") int rateUomId) {
        return run(() -> service.lastRate(itemId, rateUomId));
    }

    /** btnSearch_Click :956 */
    @PostMapping("/api/search") @ResponseBody
    public ResponseEntity<?> search(@RequestBody Map<String, Object> body) { return run(() -> service.search(body)); }

    /** FormHistory :624 (btnShow) */
    @PostMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return run(() -> service.history(body)); }

    /** Insert() :558 */
    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> service.save(body)); }

    /** btnPrint325_Click :1152 — rows for 325-ItemPricingList.rpt */
    @PostMapping("/api/print-rows") @ResponseBody
    public ResponseEntity<?> printRows(@RequestBody Map<String, Object> body) { return run(() -> service.printRows(body)); }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c) {
        try { return ResponseEntity.ok(c.get()); }
        catch (IllegalArgumentException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
        catch (IllegalStateException e) { return fail(HttpStatus.FORBIDDEN, msg(e)); }
        catch (Exception e) { return fail(HttpStatus.INTERNAL_SERVER_ERROR, msg(e)); }
    }
    private static ResponseEntity<Map<String, Object>> fail(HttpStatus status, String message) {
        Map<String, Object> m = new LinkedHashMap<>(); m.put("success", false); m.put("message", message);
        return ResponseEntity.status(status).body(m);
    }
    private static String msg(Exception e) {
        Throwable t = e; while (t.getCause() != null && (t.getMessage() == null || t.getMessage().isBlank())) t = t.getCause();
        return t.getMessage() == null || t.getMessage().isBlank() ? "Request failed." : t.getMessage();
    }
}
