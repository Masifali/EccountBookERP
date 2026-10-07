package com.mst.controllers;

import com.mst.models.dto.ItemPmMapDto;
import com.mst.services.ItemPmMapService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
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

    @GetMapping(value = API + "/lookups", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ResponseEntity<?> lookups() {
        return run(service::lookups, "Could not load lookups.");
    }

    @GetMapping(value = API + "/uoms", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ResponseEntity<?> uomsForItem(@RequestParam(name = "itemId", defaultValue = "0") int itemId) {
        return run(() -> service.uomsForItem(itemId), "Could not load UOMs.");
    }

    @GetMapping(value = API + "/history", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(name = "fromDate", required = false) String fromDate,
                                     @RequestParam(name = "toDate", required = false) String toDate,
                                     @RequestParam(name = "itemId", required = false) Integer itemId) {
        return run(() -> service.history(fromDate, toDate, itemId), "Could not load history.");
    }

    @GetMapping(value = API + "/{id}", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ResponseEntity<?> getById(@PathVariable("id") int id) {
        return run(() -> service.getById(id), "Could not load record.");
    }

    @PostMapping(value = API + "/save", consumes = MediaType.APPLICATION_JSON_VALUE,
            produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody ItemPmMapDto dto) {
        return write(() -> service.save(dto), "Save failed.");
    }

    @DeleteMapping(value = API + "/{id}", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ResponseEntity<?> delete(@PathVariable("id") int id) {
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
