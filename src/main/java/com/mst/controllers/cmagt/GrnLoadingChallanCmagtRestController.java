package com.mst.controllers.cmagt;

import com.mst.models.cmagt.dto.GrnLoadingChallanCmagtDto;
import com.mst.services.cmagt.GrnLoadingChallanCmagtService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/commission/grn-loading-challan")
public class GrnLoadingChallanCmagtRestController {

    @Autowired
    private GrnLoadingChallanCmagtService service;

    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody GrnLoadingChallanCmagtDto dto) {
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
