package com.mst.controllers;

import com.mst.models.dto.ItemCategoryStoreDto;
import com.mst.services.ItemCategoryStoreService;
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
import java.util.Map;

/**
 * Store Management, ModuleId 24 — screen 336 "Item Category Store" (ScreenName ItemCategoryStore),
 * desktop class Architecture.WinApp.Inventory_Definition.InvDeffrmItemCatagory (ScreenDefinition
 * TargetUrl), at /store/item-category-store. See {@link ItemCategoryStoreService} for the port notes.
 */
@Controller
public class ItemCategoryStoreController {

    private static final String API = "/api/store/item-category-store";

    private final ItemCategoryStoreService service;

    public ItemCategoryStoreController(ItemCategoryStoreService service) { this.service = service; }

    @GetMapping("/store/item-category-store")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/item_category_store";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping(API + "/code")
    @ResponseBody
    public ResponseEntity<?> code(@RequestParam(defaultValue = "0") int parentCategoryId) {
        return run(() -> service.generateCode(parentCategoryId), "Could not generate the code.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history() { return run(service::history, "History failed."); }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            return ResponseEntity.ok(service.load(id));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that record.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody ItemCategoryStoreDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
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
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
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
