package com.mst.controllers.lab;

import com.mst.services.lab.InProcessAnalysisStepsService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.dao.DataAccessException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * JSON for screen 163 "InProcess Analysis Steps Schedule"
 * (LabInProcessAnalysisStepAndParameterSchedule.cs) and its helper form DefineProcessStep.cs.
 * No parameter carries tenancy. Business refusals are IllegalArgumentException (400), rights are
 * AccessDeniedException (403). The desktop's Yes/No prompt is answered as HTTP 409
 * {confirm:true,message}; the page asks and resends with confirm=true.
 */
@RestController
@RequestMapping("/api/lab/inprocess-analysis-steps")
public class InProcessAnalysisStepsRestController {

    private static final Logger LOG = LoggerFactory.getLogger(InProcessAnalysisStepsRestController.class);

    private final InProcessAnalysisStepsService service;

    public InProcessAnalysisStepsRestController(InProcessAnalysisStepsService service) { this.service = service; }

    @GetMapping("/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    @GetMapping("/combos")
    public Map<String, Object> combos() { return service.combos(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/{id:\\d+}")
    public Map<String, Object> byId(@PathVariable int id) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("row", service.byId(id));
        return out;
    }

    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody Map<String, Object> body) {
        return answer(service.save(body));
    }

    @GetMapping("/process-steps")
    public Map<String, Object> processSteps() { return service.processSteps(); }

    @GetMapping("/process-steps/{id:\\d+}")
    public Map<String, Object> processStep(@PathVariable int id) { return service.processStep(id); }

    @PostMapping("/process-steps/save")
    public ResponseEntity<Map<String, Object>> saveProcessStep(@RequestBody Map<String, Object> body) {
        return answer(service.saveProcessStep(body));
    }

    private static ResponseEntity<Map<String, Object>> answer(Map<String, Object> result) {
        if (Boolean.TRUE.equals(result.get("confirm"))) return ResponseEntity.status(HttpStatus.CONFLICT).body(result);
        return ResponseEntity.ok(result);
    }

    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
        LOG.warn("InProcess Analysis Steps Schedule database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }
}
