package com.mst.controllers;

import com.mst.services.PackingMaterialFlowAllocationService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/packing-material/flow-allocation")
public class PackingMaterialFlowAllocationRestController {
    private final PackingMaterialFlowAllocationService service;

    public PackingMaterialFlowAllocationRestController(PackingMaterialFlowAllocationService service) {
        this.service = service;
    }

    @GetMapping("/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    @GetMapping("/items")
    public List<Map<String, Object>> items(@RequestParam int transactionFlowId,
                                            @RequestParam(defaultValue = "0") int itemTypeId,
                                            @RequestParam(defaultValue = "0") int itemCategoryId,
                                            @RequestParam int actionId) {
        return service.items(transactionFlowId, itemTypeId, itemCategoryId, actionId);
    }

    @PostMapping("/allocate")
    public ResponseEntity<Map<String, Object>> allocate(@RequestBody AllocationRequest request) {
        int count = service.allocate(request.transactionFlowId(), request.itemIds());
        return ResponseEntity.ok(Map.of("status", "SUCCESS", "count", count,
                "message", "Item's Allocated Successfully"));
    }

    @PostMapping("/deallocate")
    public ResponseEntity<Map<String, Object>> deallocate(@RequestBody AllocationRequest request) {
        int count = service.deallocate(request.transactionFlowId(), request.itemIds());
        return ResponseEntity.ok(Map.of("status", "SUCCESS", "count", count,
                "message", "Item's UnAllocated Successfully"));
    }

    public record AllocationRequest(int transactionFlowId, List<Integer> itemIds) {}
}
