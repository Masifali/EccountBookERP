package com.mst.controllers.cmagt;

import com.mst.models.cmagt.dto.GoodsDispatchingNoteCmagtDto;
import com.mst.services.cmagt.GoodsDispatchingNoteCmagtService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/commission/goods-dispatching-note")
public class GoodsDispatchingNoteCmagtRestController {

    @Autowired
    private GoodsDispatchingNoteCmagtService service;

    /* Company, organization, branch, year and user are NEVER taken from the request; the
       service reads them from the session (desktop UserAccount / clsGlobalVariables.ActiveYr). */

    /** btnsave_Click :2072 - always a new document (RecId = 0). */
    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody GoodsDispatchingNoteCmagtDto dto) {
        return ResponseEntity.ok(service.saveOrUpdate(dto, false));
    }

    /** btnUpdate_Click :2085 - updates the loaded document; refused without an id. */
    @PostMapping("/update")
    public ResponseEntity<Map<String, Object>> update(@RequestBody GoodsDispatchingNoteCmagtDto dto) {
        return ResponseEntity.ok(service.saveOrUpdate(dto, true));
    }

    /** btnDelete_Click :2191 -> BLL DeleteByID. */
    @DeleteMapping("/{id}")
    public ResponseEntity<Map<String, Object>> delete(@PathVariable Integer id) {
        return ResponseEntity.ok(service.delete(id));
    }

    /** GenerateCode (:677) for the Doc No box on New. */
    @GetMapping("/generate-code")
    public ResponseEntity<Map<String, Object>> generateCode() {
        return ResponseEntity.ok(service.generateCode());
    }

    @GetMapping("/history")
    public ResponseEntity<List<Map<String, Object>>> getHistory(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate) {
        return ResponseEntity.ok(service.getHistory(fromDate, toDate));
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getById(@PathVariable Integer id) {
        return ResponseEntity.ok(service.getById(id));
    }
}
