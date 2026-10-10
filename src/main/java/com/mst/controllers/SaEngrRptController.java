package com.mst.controllers;

import com.mst.services.SaEngrRptService;
import java.time.LocalDate;
import java.util.*;
import javax.servlet.http.HttpServletResponse;
import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;

/**
 * Group E2 data endpoints for the Sale Engr report pages 555 / 554 / 556 (pages: SaEngrRptPageController, prints: SaEngrRptPrintController).
 *   GET  /api/sale/saengr/{report}/lookups        the form's Load lists
 *   GET  /api/sale/saengr/{report}/rows           the Show button (query = the form's filters)
 *   GET  /api/sale/saengr/order-history/summary   554 Summary tab Show
 *   GET  /api/sale/saengr/mfg-order-history/{lookups|rows|summary}   848 (module 135)
 *   POST /api/sale/saengr/{order-history|mfg-order-history}/action    554 / 848 Complete / Cancel / UpdateExpiryDate
 *   GET  /api/sale/saengr/{report}/gl-account     CommonServices.GetGlAccountIdBySupplierCustomerId
 *   GET  /api/sale/saengr/{report}/attachments    CommonServices.GetNoofAttachmentsByRefDocumentTypeID (+ /download)
 * {report} = gdn-history (555), order-history (554), sales-activities (556).
 */
@RestController
@RequestMapping("/api/sale/saengr")
public class SaEngrRptController {
    private final SaEngrRptService service;

    public SaEngrRptController(SaEngrRptService service) { this.service = service; }

    @GetMapping("/{report}/lookups")
    public Map<String, Object> lookups(@PathVariable("report") String report, @RequestParam(value = "branchIds", required = false) String branchIds) {
        return service.lookups(report, branchIds);
    }

    @GetMapping("/{report}/rows")
    public List<Map<String, Object>> rows(@PathVariable("report") String report, @RequestParam Map<String, String> q) { return service.rows(report, q); }

    @GetMapping("/order-history/summary")
    public List<Map<String, Object>> orderSummary(@RequestParam Map<String, String> q) { return service.orderSummaryRows(q); }

    /** 848 Summary tab Show. */
    @GetMapping("/mfg-order-history/summary")
    public List<Map<String, Object>> mfgOrderSummary(@RequestParam Map<String, String> q) { return service.mfgOrderSummaryRows(q); }

    /** Body: {action: Complete | Cancel | UpdateExpiryDate, id, expiryDate?}; report = order-history (554) or mfg-order-history (848). */
    @PostMapping("/{report}/action")
    public Map<String, Object> orderAction(@PathVariable("report") String report, @RequestBody Map<String, Object> body) {
        Object id = body.get("id");
        Object ex = body.get("expiryDate");
        LocalDate expiry = ex == null || ex.toString().length() < 10 ? null : LocalDate.parse(ex.toString().substring(0, 10));
        return service.orderAction(report, String.valueOf(body.get("action")), id instanceof Number ? ((Number) id).intValue() : 0, expiry);
    }

    /** frmApprovalCommentory rows (query = the report's own filters + id, documentTypeId, idColumn, typeColumn). */
    @GetMapping("/{report}/approval-history")
    public List<Map<String, Object>> approvalHistory(@PathVariable("report") String report, @RequestParam Map<String, String> q) {
        String idColumn = q.getOrDefault("idColumn", "Id");
        String typeColumn = q.getOrDefault("typeColumn", "DocumentTypeId");
        if (!idColumn.matches("[A-Za-z]{1,30}") || !typeColumn.matches("[A-Za-z]{1,30}")) throw new IllegalArgumentException("Invalid column");
        int id = q.get("id") == null ? 0 : Integer.parseInt(q.get("id").trim());
        int type = q.get("documentTypeId") == null ? 0 : Integer.parseInt(q.get("documentTypeId").trim());
        return service.approvalHistory(report, id, type, q, idColumn, typeColumn);
    }

    @GetMapping("/{report}/gl-account")
    public Map<String, Object> glAccount(@PathVariable("report") String report, @RequestParam int supplierCustomerId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("glAccountId", service.glAccountOfParty(report, supplierCustomerId));
        return out;
    }

    @GetMapping("/{report}/attachments")
    public List<Map<String, Object>> attachments(@PathVariable("report") String report, @RequestParam int id, @RequestParam int documentTypeId) {
        return service.attachments(report, id, documentTypeId);
    }

    @GetMapping("/{report}/attachments/download")
    public void download(@PathVariable("report") String report, @RequestParam int id, @RequestParam int documentTypeId, @RequestParam int attachmentId,
            HttpServletResponse response) throws java.io.IOException {
        SaEngrRptService.Download d = service.download(report, id, documentTypeId, attachmentId);
        response.setContentType(MediaType.APPLICATION_OCTET_STREAM_VALUE);
        response.setHeader(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(d.name, java.nio.charset.StandardCharsets.UTF_8).build().toString());
        response.getOutputStream().write(d.bytes);
    }

    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class)
    public org.springframework.http.ResponseEntity<String> denied(org.springframework.security.access.AccessDeniedException e) {
        return org.springframework.http.ResponseEntity.status(403).contentType(MediaType.TEXT_PLAIN).body(e.getMessage());
    }

    @ExceptionHandler({IllegalArgumentException.class, IllegalStateException.class, org.springframework.dao.DataAccessException.class})
    public org.springframework.http.ResponseEntity<String> failed(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage() == null ? e.getMessage() : t.getMessage();
        return org.springframework.http.ResponseEntity.badRequest().contentType(MediaType.TEXT_PLAIN).body(m);
    }
}
