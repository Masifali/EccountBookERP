package com.mst.controllers;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.mst.models.ItemType;
import com.mst.serviceInterface.IItemTypeService;

@RestController
@RequestMapping("/api/inventory/item_types")
public class ItemTypeApiController {

    @Autowired
    private IItemTypeService itemTypeService;

    @GetMapping("/lookups")
    public ResponseEntity<Map<String, Object>> getLookups() {
        Map<String, Object> result = new HashMap<>();
        result.put("parentCategories", itemTypeService.getParentCategoriesLookup());
        result.put("itemTypes", itemTypeService.getItemTypeLookups());
        return ResponseEntity.ok(result);
    }

    @GetMapping("/generate-code")
    public ResponseEntity<Map<String, String>> generateCode() {
        String code = itemTypeService.generateItemTypeCode();
        return ResponseEntity.ok(Map.of("code", code));
    }

    @GetMapping("/history")
    public ResponseEntity<List<Map<String, Object>>> getHistory() {
        List<Map<String, Object>> history = itemTypeService.getItemTypeHistory();
        return ResponseEntity.ok(history);
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getById(@PathVariable("id") int id) {
        Map<String, Object> record = itemTypeService.getByIdSp(id);
        if (record != null) {
            return ResponseEntity.ok(record);
        }
        return ResponseEntity.notFound().build();
    }

    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody ItemType itemType) {
        Map<String, Object> saved = itemTypeService.saveSp(itemType);
        return ResponseEntity.ok(saved);
    }
}
