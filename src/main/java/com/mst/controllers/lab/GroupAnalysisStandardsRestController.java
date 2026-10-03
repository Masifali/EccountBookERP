package com.mst.controllers.lab;

import com.mst.services.lab.GroupAnalysisStandardsService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.dao.DataAccessException;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * JSON for screen 158 "Group Analysis Standards" (InvLabGroupAnalysisStandards.cs). No parameter
 * carries tenancy. Business refusals are IllegalArgumentException (400, ApiExceptionAdvice), rights
 * are AccessDeniedException (403). A procedure's RAISERROR is what the desktop shows in its
 * MessageBox (catch (Exception ex) -> MessageBox.Show(ex.Message)), so it is answered as a 400 with
 * that text.
 */
@RestController
@RequestMapping("/api/lab/group-analysis-standards")
public class GroupAnalysisStandardsRestController {

    private static final Logger LOG = LoggerFactory.getLogger(GroupAnalysisStandardsRestController.class);

    private final GroupAnalysisStandardsService service;

    public GroupAnalysisStandardsRestController(GroupAnalysisStandardsService service) { this.service = service; }

    @GetMapping("/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/rows")
    public List<Map<String, Object>> rows() { return service.rows(); }

    @GetMapping("/{id:\\d+}")
    public Map<String, Object> byId(@PathVariable int id) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("row", service.byId(id));
        return out;
    }

    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody Map<String, Object> body) { return service.save(body); }

    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
        LOG.warn("Group Analysis Standards database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }
}
