package com.mst.controllers;

import com.mst.services.Inv4aSalesPricingWithMarginService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** frmItemSalesPricingSchedule — ScreenDefinition 100 "Sales Pricing (With Margin)" (Inventory hub, module 4 Inventory Defination). */
@Controller
@RequestMapping("/inventory/sales-pricing-with-margin")
public class Inv4aSalesPricingWithMarginController {

    private final Inv4aSalesPricingWithMarginService service;
    public Inv4aSalesPricingWithMarginController(Inv4aSalesPricingWithMarginService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Sales Pricing (With Margin)");
        return "inventory/inv4a_sales_pricing_with_margin";
    }

    /** frmItemPricingSchedule_Load :215 */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() { return run(service::init); }

    /** ItemFill :268 (cmbDiscType_Leave :1133) */
    @GetMapping("/api/items") @ResponseBody
    public ResponseEntity<?> items(@RequestParam(defaultValue = "0") int discountTypeId) { return run(() -> service.items(discountTypeId)); }

    /** btnAdd_Click :685 "Show" */
    @PostMapping("/api/recipes") @ResponseBody
    public ResponseEntity<?> recipes(@RequestBody Map<String, Object> body) { return run(() -> service.recipes(body)); }

    /** BtnShowHistoryReport_Click :1001 / btnPrint_Click :964 / history Print :613 */
    @PostMapping("/api/final-price-list") @ResponseBody
    public ResponseEntity<?> finalPriceList(@RequestBody Map<String, Object> body) { return run(() -> service.finalPriceList(body)); }

    /** Insert() :455 */
    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> service.save(body)); }

    /** UpdateIsActiveFromHistory :1179 */
    @PostMapping("/api/active-inactive") @ResponseBody
    public ResponseEntity<?> activeInactive(@RequestBody Map<String, Object> body) { return run(() -> service.activeInactive(body)); }

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
