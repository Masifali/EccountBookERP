package com.mst.controllers;

import com.mst.services.Inv4aItemDiscountingScheduleService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** frmItemDiscountingScheduleForBooking — ScreenDefinition 99 "Item Discounting Schedule" (Inventory hub, module 4 Inventory Defination). */
@Controller
@RequestMapping("/inventory/item-discounting-schedule")
public class Inv4aItemDiscountingScheduleController {

    private final Inv4aItemDiscountingScheduleService service;
    public Inv4aItemDiscountingScheduleController(Inv4aItemDiscountingScheduleService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Item Discounting Schedule");
        return "inventory/inv4a_item_discounting_schedule";
    }

    /** frmItemPricingSchedule_Load :167 */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() { return run(service::init); }

    /** GetFilteredItems :525 (btnSearch) / ItemFill :404 */
    @GetMapping("/api/items") @ResponseBody
    public ResponseEntity<?> items(@RequestParam(defaultValue = "0") int itemCategoryId, @RequestParam(defaultValue = "0") int itemTypeId) {
        return run(() -> service.items(itemCategoryId, itemTypeId));
    }

    /** FormHistory :658 (history tab) */
    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history() { return run(service::history); }

    /** FormValiadation :836 — run before the confirm box, as the desktop does */
    @PostMapping("/api/validate") @ResponseBody
    public ResponseEntity<?> validate(@RequestBody Map<String, Object> body) { return run(() -> service.validate(body)); }

    /** Insert() :573 */
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
