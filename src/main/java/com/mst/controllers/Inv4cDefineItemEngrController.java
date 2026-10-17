package com.mst.controllers;

import com.mst.services.Inv4cDefineItemService;
import com.mst.services.Inv4cDefineItemService.Variant;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * frmAddItem_Engr - ScreenDefinition 777 "Define Item" (module 65 Mfg Definitions) and 776 "Item Definition_Engr" (module 8):
 * one form, one route. Errors: validation → 400 {message}; missing right → 403; anything else → 500.
 */
@Controller
@RequestMapping("/inventory/define-item-engr")
public class Inv4cDefineItemEngrController {

    private static final Variant V = Variant.ENGR;
    private final Inv4cDefineItemService service;
    public Inv4cDefineItemEngrController(Inv4cDefineItemService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Define Item");
        return "inventory/inv4c_define_item_engr";
    }

    /** InvDefrmAddItem_Load :337 / Refresh :1227. */
    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return call(() -> service.lookups(V), "Could not load the screen."); }

    /** cmbItemCategory_Leave :974. */
    @GetMapping("/api/category-leave") @ResponseBody
    public ResponseEntity<?> categoryLeave(@RequestParam int itemCategoryId) { return call(() -> service.categoryLeave(V, itemCategoryId), "Record Not Found"); }

    /** grdfrmfill :1586 — 50 rows on the History tab, 0 = all. The category id is accepted and ignored, as BLL 0583 FormHistory ignores it. */
    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "0") int noOfRecords,
                                     @RequestParam(defaultValue = "0") int itemCategoryId) {
        return call(() -> service.history(V, noOfRecords), "History failed.");
    }

    /** ReadById :1456. */
    @GetMapping("/api/{id}") @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(V, id);
            if (m == null) return fail(HttpStatus.NOT_FOUND, "Record Not Found");
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return fail(HttpStatus.INTERNAL_SERVER_ERROR, msg(e, "Could not open that item."));
        }
    }

    /** Insert :1260 up to "Are you sure to Save/Update?". */
    @PostMapping("/api/precheck") @ResponseBody
    public ResponseEntity<?> precheck(@RequestBody Map<String, Object> body) { return call(() -> service.precheck(V, body), "Save failed."); }

    /** Insert :1291 → Item.Save(items, true). */
    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> service.save(V, body), "Save failed."); }

    /** btnUomUpdate_Click :1752 — [{Id, ItemName, ItemCode}]. */
    @PostMapping("/api/update-names") @ResponseBody
    public ResponseEntity<?> updateNames(@RequestBody List<Map<String, Object>> rows) { return call(() -> service.updateItemNames(V, rows), "Update failed."); }

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
