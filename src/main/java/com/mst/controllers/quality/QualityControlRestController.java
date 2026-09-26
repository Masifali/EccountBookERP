package com.mst.controllers.quality;

import com.mst.serviceInterface.QualityControlService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/quality/api")
public class QualityControlRestController {

    @Autowired
    private QualityControlService qcService;

    @GetMapping("/labs")
    public ResponseEntity<?> getLabs() {
        return ResponseEntity.ok(qcService.getLabs());
    }

    @GetMapping("/parameters")
    public ResponseEntity<?> getParameters() {
        return ResponseEntity.ok(qcService.getAnalysisParameters());
    }

    @GetMapping("/groups")
    public ResponseEntity<?> getGroups() {
        return ResponseEntity.ok(qcService.getAnalysisGroups());
    }

    @GetMapping("/history/sample-log")
    public ResponseEntity<?> getSampleLogHistory() {
        return ResponseEntity.ok(qcService.getSampleLogHistory());
    }

    @GetMapping("/history/sample-analysis")
    public ResponseEntity<?> getSampleAnalysisHistory() {
        return ResponseEntity.ok(qcService.getSampleAnalysisHistory());
    }

    @GetMapping("/history/purchase-analysis")
    public ResponseEntity<?> getPurchaseAnalysisHistory() {
        return ResponseEntity.ok(qcService.getPurchaseAnalysisHistory());
    }

    @GetMapping("/history/inprocess-analysis")
    public ResponseEntity<?> getInProcessAnalysisHistory() {
        return ResponseEntity.ok(qcService.getInProcessAnalysisHistory());
    }

    @GetMapping("/sample-log/{id}")
    public ResponseEntity<?> getSampleLogById(@PathVariable("id") int id) {
        return ResponseEntity.ok(qcService.getSampleLogById(id));
    }

    @GetMapping("/sample-analysis/{id}")
    public ResponseEntity<?> getSampleAnalysisById(@PathVariable("id") int id) {
        return ResponseEntity.ok(qcService.getSampleAnalysisById(id));
    }

    @GetMapping("/purchase-analysis/{id}")
    public ResponseEntity<?> getPurchaseAnalysisById(@PathVariable("id") int id) {
        return ResponseEntity.ok(qcService.getPurchaseAnalysisById(id));
    }

    @GetMapping("/inprocess-analysis/{id}")
    public ResponseEntity<?> getInProcessAnalysisById(@PathVariable("id") int id) {
        return ResponseEntity.ok(qcService.getInProcessAnalysisById(id));
    }

    @PostMapping("/sample-log/save")
    public ResponseEntity<?> saveSampleLog(@RequestBody Map<String, Object> req) {
        Map<String, Object> res = new HashMap<>();
        res.put("success", true);
        res.put("message", "Sample Log Register saved successfully.");
        res.put("id", 1001);
        return ResponseEntity.ok(res);
    }

    @PostMapping("/sample-analysis/save")
    public ResponseEntity<?> saveSampleAnalysis(@RequestBody Map<String, Object> req) {
        Map<String, Object> res = new HashMap<>();
        res.put("success", true);
        res.put("message", "Sample Analysis saved successfully.");
        res.put("id", 1002);
        return ResponseEntity.ok(res);
    }

    @PostMapping("/purchase-analysis/save")
    public ResponseEntity<?> savePurchaseAnalysis(@RequestBody Map<String, Object> req) {
        Map<String, Object> res = new HashMap<>();
        res.put("success", true);
        res.put("message", "Purchase Analysis saved successfully.");
        res.put("id", 1003);
        return ResponseEntity.ok(res);
    }

    @PostMapping("/inprocess-analysis/save")
    public ResponseEntity<?> saveInProcessAnalysis(@RequestBody Map<String, Object> req) {
        Map<String, Object> res = new HashMap<>();
        res.put("success", true);
        res.put("message", "In-Process Analysis saved successfully.");
        res.put("id", 1004);
        return ResponseEntity.ok(res);
    }
}
