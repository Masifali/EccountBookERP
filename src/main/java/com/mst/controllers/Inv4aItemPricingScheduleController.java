package com.mst.controllers;

import com.mst.services.Inv4aItemPricingScheduleService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** frmItemPricingSchedule — ScreenDefinition 91 "Item Pricing Schedule" (Inventory hub, module 4 Inventory Defination). */
@Controller
@RequestMapping("/inventory/item-pricing-schedule")
public class Inv4aItemPricingScheduleController {

    private final Inv4aItemPricingScheduleService service;
    public Inv4aItemPricingScheduleController(Inv4aItemPricingScheduleService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Item Pricing Schedule");
        return "inventory/inv4a_item_pricing_schedule";
    }

    /** frmItemPricingSchedule_Load :189 */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() { return run(service::init); }

    /** btnrefresh_Click :886 */
    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::refresh); }

    /** ItemFill :217 */
    @GetMapping("/api/items") @ResponseBody
    public ResponseEntity<?> items(@RequestParam(defaultValue = "0") int parentId) { return run(() -> service.items(parentId)); }

    /** cmbItem_ValueChanged :546 */
    @GetMapping("/api/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam(defaultValue = "0") int itemId) { return run(() -> service.uoms(itemId)); }

    /** CmbRateUom_Leave :736 */
    @GetMapping("/api/last-rate") @ResponseBody
    public ResponseEntity<?> lastRate(@RequestParam(defaultValue = "0") int itemId, @RequestParam(defaultValue = "0") int rateUomId) {
        return run(() -> service.lastRate(itemId, rateUomId));
    }

    /** btnSearch_Click :770 */
    @PostMapping("/api/search") @ResponseBody
    public ResponseEntity<?> search(@RequestBody Map<String, Object> body) { return run(() -> service.search(body)); }

    /** FormHistory :492 */
    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history() { return run(service::history); }

    /** Insert() :432 */
    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> service.save(body)); }

    /** btnPrintHistory_Click :916 — rows for 325-ItemPricingList.rpt */
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
