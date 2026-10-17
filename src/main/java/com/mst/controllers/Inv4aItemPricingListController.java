package com.mst.controllers;

import com.mst.services.Inv4aItemPricingListService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** frmGetItemPricingList — ScreenDefinition 803 "Item Pricing List" (Inventory hub, module 8 Inventory Reports). */
@Controller
@RequestMapping("/inventory/reports/item-pricing-list")
public class Inv4aItemPricingListController {

    private final Inv4aItemPricingListService service;
    public Inv4aItemPricingListController(Inv4aItemPricingListService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Item Pricing List");
        return "inventory/inv4a_item_pricing_list";
    }

    /** frmGetItemPricingList_Load :98 / Refresh :363 */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() {
        try { return ResponseEntity.ok(service.init()); }
        catch (AccessDeniedException e) { return fail(HttpStatus.FORBIDDEN, msg(e)); }
        catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** gridHisory :273 (btnshow) */
    @PostMapping("/api/show") @ResponseBody
    public ResponseEntity<?> show(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.show(body)); }
        catch (AccessDeniedException e) { return fail(HttpStatus.FORBIDDEN, msg(e)); }
        catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
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
