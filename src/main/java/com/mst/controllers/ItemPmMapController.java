package com.mst.controllers;

import com.mst.models.dto.ItemPmMapDto;
import com.mst.services.ItemPmMapService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

@Controller
public class ItemPmMapController {

    private static final String API = "/api/packing-material/item-pm-map";

    @Autowired private ItemPmMapService service;

    @GetMapping("/packing-material/item-pm-map")
    public String page(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        return "packing_material/item_pm_map";
    }

    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() {
        return run(service::lookups, "Could not load lookups.");
    }

    @GetMapping(API + "/uoms")
    @ResponseBody
    public ResponseEntity<?> uomsForItem(@RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.uomsForItem(itemId), "Could not load UOMs.");
    }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false) Integer itemId) {
        return run(() -> service.history(fromDate, toDate, itemId), "Could not load history.");
    }

    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> getById(@PathVariable int id) {
        return run(() -> service.getById(id), "Could not load record.");
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody ItemPmMapDto dto) {
        return write(() -> service.save(dto), "Save failed.");
    }

    @DeleteMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) {
        return run(() -> service.delete(id), "Delete failed.");
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e.getMessage() != null ? e.getMessage() : fallback));
        }
    }

    private static ResponseEntity<?> write(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e.getMessage() != null ? e.getMessage() : fallback));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
