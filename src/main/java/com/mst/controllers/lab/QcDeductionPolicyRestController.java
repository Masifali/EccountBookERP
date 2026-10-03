package com.mst.controllers.lab;

import com.mst.services.DesktopInventoryItemFileService;
import com.mst.services.lab.QcDeductionPolicyService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.dao.DataAccessException;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * JSON for screen 164 "Lab Deduction Policy For Purchase" (frmQcDeductionPolicy.cs).
 * No parameter carries tenancy: organization, company and user come from the session inside
 * {@link QcDeductionPolicyService}.
 *
 * Business refusals are IllegalArgumentException (400 with the desktop's text, ApiExceptionAdvice);
 * rights are AccessDeniedException (403). A database error is what the desktop shows in its
 * MessageBox.Show(ex.Message), so it is answered as a 400 with that text.
 */
@RestController
@RequestMapping("/api/lab/qc-deduction-policy")
public class QcDeductionPolicyRestController {

    private static final Logger LOG = LoggerFactory.getLogger(QcDeductionPolicyRestController.class);

    private final QcDeductionPolicyService service;

    public QcDeductionPolicyRestController(QcDeductionPolicyService service) { this.service = service; }

    /** frmQcDeductionPolicy_Load (:224). */
    @GetMapping("/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    /** btnRefresh_Click (:988) — AnalysisGroup() + UOM(). */
    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    /** CmbAnalysisGroup_Leave (:400) -> AnalysisParamerterByGroup. */
    @GetMapping("/parameters")
    public List<Map<String, Object>> parameters(@RequestParam(defaultValue = "0") int groupId) {
        return service.parameters(groupId);
    }

    /** btnShow_Click (:1147) -> gridhistoryfill. An unchecked date picker is sent as an empty value. */
    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "entry") String dateType,
                                             @RequestParam(required = false) String fromDate,
                                             @RequestParam(required = false) String toDate) {
        return service.history(dateType, fromDate, toDate);
    }

    /** ReadById (:910). */
    @GetMapping("/{id}")
    public Map<String, Object> load(@PathVariable int id) { return service.load(id); }

    /** grdhistory_SelectionChanged (:1355) -> GridDetailBind. */
    @GetMapping("/{id}/details")
    public List<Map<String, Object>> details(@PathVariable int id) { return service.details(id); }

    /** btnSave_Click (:896) / btnUpdate_Click (:772). */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody QcDeductionPolicyService.SaveRequest dto) { return service.save(dto); }

    /** btnDelete_Click (:948). */
    @PostMapping("/{id}/delete")
    public Map<String, Object> delete(@PathVariable int id) { return service.delete(id); }

    /** grdhistory_LinkClicked (:1319) — the record's attachments. */
    @GetMapping("/{id}/attachments")
    public List<Map<String, Object>> attachments(@PathVariable int id) { return service.attachments(id); }

    @GetMapping("/{id}/attachments/{attachmentId}")
    public ResponseEntity<byte[]> attachment(@PathVariable int id, @PathVariable int attachmentId) {
        DesktopInventoryItemFileService.Download d = service.attachment(id, attachmentId);
        if (d == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        String name = d.name().replaceAll("[\\r\\n\"]", "_");
        return ResponseEntity.ok()
                .contentType(MediaType.APPLICATION_OCTET_STREAM)
                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename=\"" + name + "\"")
                .header("X-Content-Type-Options", "nosniff")
                .body(d.bytes());
    }

    /** The message the desktop shows in MessageBox.Show(ex.Message). */
    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
        LOG.warn("Lab Deduction Policy database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }
}
