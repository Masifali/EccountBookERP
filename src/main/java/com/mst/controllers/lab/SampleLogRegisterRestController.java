package com.mst.controllers.lab;

import com.mst.services.DesktopInventoryItemFileService;
import com.mst.services.lab.SampleLogRegisterService;
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
 * JSON for screen 155 "Sample Log Register" (InvLabSampleLogRegister.cs, DocumentTypeId 301).
 * No parameter carries tenancy: organization, company and user come from the session inside
 * {@link SampleLogRegisterService}.
 *
 * Business refusals are IllegalArgumentException (400 with the desktop's text, ApiExceptionAdvice);
 * rights are AccessDeniedException (403). A database error is what the desktop shows in its
 * MessageBox.Show(ex.Message), so it is answered as a 400 with that text.
 */
@RestController
@RequestMapping("/api/lab/sample-log-register")
public class SampleLogRegisterRestController {

    private static final Logger LOG = LoggerFactory.getLogger(SampleLogRegisterRestController.class);

    private final SampleLogRegisterService service;

    public SampleLogRegisterRestController(SampleLogRegisterService service) { this.service = service; }

    /** InvLabSampleLogRegister_Load (:724). */
    @GetMapping("/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    /** refresh() (:444) — the next sample number and the Form grid. */
    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    /** cmbitem_Leave (:1019) -> bindPackSizeInput. */
    @GetMapping("/uoms")
    public List<Map<String, Object>> uoms(@RequestParam(defaultValue = "0") int itemId) { return service.uoms(itemId); }

    /** btnshow_Click (:1120). */
    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(required = false) String fromDate,
                                             @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int supplierId,
                                             @RequestParam(defaultValue = "0") int referencePartyId,
                                             @RequestParam(defaultValue = "0") int itemId,
                                             @RequestParam(defaultValue = "0") int cityId,
                                             @RequestParam(required = false) String cropYear) {
        return service.history(fromDate, toDate, supplierId, referencePartyId, itemId, cityId, cropYear);
    }

    /** ReadById (:752). */
    @GetMapping("/{id}")
    public Map<String, Object> load(@PathVariable int id) { return service.load(id); }

    /** btnSave_Click (:489) / btnUpdate_Click (:557). */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody SampleLogRegisterService.SaveRequest dto) { return service.save(dto); }

    /** picbox1 (:782) — the stored sample picture. */
    @GetMapping("/{id}/picture")
    public ResponseEntity<byte[]> picture(@PathVariable int id) {
        DesktopInventoryItemFileService.Download d = service.picture(id);
        if (d == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok()
                .contentType(MediaType.parseMediaType(d.type()))
                .header(HttpHeaders.CACHE_CONTROL, "no-store")
                .header("X-Content-Type-Options", "nosniff")
                .body(d.bytes());
    }

    /** grd_LinkClicked (:974) — the record's attachments. */
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
        LOG.warn("Sample Log Register database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }
}
