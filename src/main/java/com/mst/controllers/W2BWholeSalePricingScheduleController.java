package com.mst.controllers;

import com.mst.services.W2BWholeSalePricingScheduleService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * frmPosWhSalePricing - ScreenDefinition 107 "Whole Sale Pricing" (Inventory hub, module 4).
 * Desktop class: Architecture.WinApp.WholeSale.frmItemPricingSchedule (recovered_source/.../Architecture.WinApp.WholeSale/frmItemPricingSchedule.cs).
 */
@Controller
@RequestMapping("/inventory/wholesale-pricing-schedule")
public class W2BWholeSalePricingScheduleController {

    private final W2BWholeSalePricingScheduleService service;
    public W2BWholeSalePricingScheduleController(W2BWholeSalePricingScheduleService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Whole Sale Pricing");
        return "inventory/w2b_wholesale_pricing_schedule";
    }

    /** frmItemPricingSchedule_Load :122 */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() { return run(service::init); }

    /** ItemFill :183 (New / refresh after save) */
    @GetMapping("/api/items") @ResponseBody
    public ResponseEntity<?> items() { return run(service::itemFill); }

    /** btnAdd_Click :667 / btnSearch_Click :640 -> GetPreviousPricesByItemId :271 */
    @PostMapping("/api/previous-prices") @ResponseBody
    public ResponseEntity<?> previousPrices(@RequestBody Map<String, Object> body) { return run(() -> service.previousPrices(body)); }

    /** tabControl1_SelectedIndexChanged :687 -> FormHistory :439 */
    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history() { return run(service::history); }

    /** DataGridHistory_ColumnButtonClick "Edit" :702 -> GetByItemId :498 */
    @GetMapping("/api/edit") @ResponseBody
    public ResponseEntity<?> edit(@RequestParam(defaultValue = "0") int itemId) { return run(() -> service.edit(itemId)); }

    /** Insert() :355 */
    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> service.save(body)); }

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
