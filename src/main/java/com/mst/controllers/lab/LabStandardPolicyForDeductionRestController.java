package com.mst.controllers.lab;

import com.mst.services.lab.LabStandardPolicyForDeductionService;
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
 * JSON for screen 166 "Lab Standard Policy For Deduction (Not Use)" (InvLabStandardPolicyForDeduction.cs).
 * No parameter carries tenancy: organization, company and user come from the session inside
 * {@link LabStandardPolicyForDeductionService}.
 *
 * There is no delete endpoint: the form has no Delete button and the BLL has no delete method. There is
 * no print endpoint: btnprint has no Click handler on the desktop.
 *
 * Business refusals are IllegalArgumentException (400 with the desktop's text, ApiExceptionAdvice);
 * rights are AccessDeniedException (403). A database error (the procedures' own RAISERROR texts) is what
 * the desktop shows in MessageBox.Show(ex.Message, "Database Error"), so it is answered as a 400 with
 * that text.
 */
@RestController
@RequestMapping("/api/lab/standard-policy-for-deduction")
public class LabStandardPolicyForDeductionRestController {

    private static final Logger LOG = LoggerFactory.getLogger(LabStandardPolicyForDeductionRestController.class);

    private final LabStandardPolicyForDeductionService service;

    public LabStandardPolicyForDeductionRestController(LabStandardPolicyForDeductionService service) {
        this.service = service;
    }

    /** Lab_Load (:163) — the five combo fills. */
    @GetMapping("/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    /** gridhistoryfill (:712). */
    @GetMapping("/history")
    public List<Map<String, Object>> history() { return service.history(); }

    /** getUpdate (:600). */
    @GetMapping("/{id}")
    public Map<String, Object> load(@PathVariable int id) { return service.load(id); }

    /** btnSave_Click (:586) / btnUpdate_Click (:500) -> Insert() (:516). */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody LabStandardPolicyForDeductionService.SaveRequest dto) {
        return service.save(dto);
    }

    /** The message the desktop shows in MessageBox.Show(ex.Message, "Database Error"). */
    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
        LOG.warn("Lab Standard Policy For Deduction database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }
}
