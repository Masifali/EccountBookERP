package com.mst.controllers.cmagt;

import com.mst.models.cmagt.dto.PurchaseOrderCmagtDto;
import com.mst.services.cmagt.PurchaseOrderCmagtService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/commission/purchase-order")
public class PurchaseOrderCmagtRestController {

    @Autowired
    private PurchaseOrderCmagtService service;

    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody PurchaseOrderCmagtDto dto) {
        return ResponseEntity.ok(service.saveOrUpdate(dto));
    }

    @GetMapping("/history")
    public ResponseEntity<List<Map<String, Object>>> getHistory(
            @RequestParam(required = false, defaultValue = "1") Integer companyId,
            @RequestParam(required = false, defaultValue = "1") Integer organizationId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate) {
        return ResponseEntity.ok(service.getHistory(companyId, organizationId, fromDate, toDate));
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getById(@PathVariable Integer id) {
        return ResponseEntity.ok(service.getById(id));
    }
}
