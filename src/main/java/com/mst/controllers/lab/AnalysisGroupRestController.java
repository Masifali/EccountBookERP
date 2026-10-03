package com.mst.controllers.lab;

import com.mst.services.lab.AnalysisGroupService;
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
 * JSON for screen 157 "Analysis Group" (InvLabAnalysisGroup.cs). No parameter carries tenancy.
 * There is no delete and no print endpoint: the desktop form has neither button
 * (toolstrip items :688 = btnnew, btnupdate, btnsave).
 *
 * Validation refusals are IllegalArgumentException (400, ApiExceptionAdvice); rights are
 * AccessDeniedException (403). A database error is what the desktop shows as MessageBox(ex.Message)
 * (:129-132), so it is answered as a 400 with that text.
 */
@RestController
@RequestMapping("/api/lab/analysis-group")
public class AnalysisGroupRestController {

    private static final Logger LOG = LoggerFactory.getLogger(AnalysisGroupRestController.class);

    private final AnalysisGroupService service;

    public AnalysisGroupRestController(AnalysisGroupService service) {
        this.service = service;
    }

    /** InvLabAnalysisGroup_Load_1: rights + Group Type list + grid rows. */
    @GetMapping("/init")
    public Map<String, Object> init() { return service.init(); }

    /** gridfill(). */
    @GetMapping("/grid")
    public List<Map<String, Object>> grid() { return service.grid(); }

    /** grdfrm_DoubleClick. */
    @GetMapping("/{id}")
    public Map<String, Object> byId(@PathVariable int id) { return service.byId(id); }

    /** btnsave_Click (id 0) / btnupdate_Click (id = RecId). */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody SaveRequest body) {
        return service.save(body.getId(), body.getGroupType(), body.getAnalysisGroupCode(), body.getAnalysisGroupDescription());
    }

    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
        LOG.warn("Analysis Group database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }

    /** The posted body — the model's own fields (Model 0615) minus tenancy. */
    public static class SaveRequest {
        private Integer id;
        private Integer groupType;
        private String analysisGroupCode;
        private String analysisGroupDescription;

        public Integer getId() { return id; }
        public void setId(Integer id) { this.id = id; }
        public Integer getGroupType() { return groupType; }
        public void setGroupType(Integer groupType) { this.groupType = groupType; }
        public String getAnalysisGroupCode() { return analysisGroupCode; }
        public void setAnalysisGroupCode(String analysisGroupCode) { this.analysisGroupCode = analysisGroupCode; }
        public String getAnalysisGroupDescription() { return analysisGroupDescription; }
        public void setAnalysisGroupDescription(String analysisGroupDescription) { this.analysisGroupDescription = analysisGroupDescription; }
    }
}
