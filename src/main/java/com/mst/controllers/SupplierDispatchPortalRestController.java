package com.mst.controllers;

import com.mst.services.SupplierDispatchAttachmentService;
import com.mst.services.SupplierDispatchPortalService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.dao.DataAccessException;
import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpHeaders;
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

import java.nio.charset.StandardCharsets;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** JSON for screen 788 "Supplier Dispatch" (frmSupplierDispatch, DocumentTypeId 246). */
@RestController
public class SupplierDispatchPortalRestController {
    private static final Logger LOG = LoggerFactory.getLogger(SupplierDispatchPortalRestController.class);

    private final SupplierDispatchPortalService service;
    private final SupplierDispatchAttachmentService attachments;

    public SupplierDispatchPortalRestController(SupplierDispatchPortalService service, SupplierDispatchAttachmentService attachments) {
        this.service = service;
        this.attachments = attachments;
    }

    @GetMapping("/api/purchase/supplier-dispatch/lookups")
    public Map<String, Object> lookups() { return service.lookups(); }

    @GetMapping("/api/purchase/supplier-dispatch/refresh")
    public Map<String, Object> refresh() { return service.formRefresh(); }

    @GetMapping("/api/purchase/supplier-dispatch/numbers")
    public Map<String, Object> numbers() { return service.numbers(); }

    @GetMapping("/api/purchase/supplier-dispatch/history-refresh")
    public Map<String, Object> historyRefresh() { return service.historyRefresh(); }

    @GetMapping("/api/purchase/supplier-dispatch/uoms")
    public List<Map<String, Object>> uoms(@RequestParam(defaultValue = "0") int itemId) { return service.uoms(itemId); }

    @GetMapping("/api/purchase/supplier-dispatch/orders")
    public List<Map<String, Object>> orders(@RequestParam(defaultValue = "0") int supplierId) { return service.orders(supplierId); }

    @GetMapping("/api/purchase/supplier-dispatch/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "") String branchIds,
                                             @RequestParam(defaultValue = "doc") String dateType,
                                             @RequestParam(defaultValue = "true") boolean fromChecked,
                                             @RequestParam(defaultValue = "") String fromDate,
                                             @RequestParam(defaultValue = "true") boolean toChecked,
                                             @RequestParam(defaultValue = "") String toDate,
                                             @RequestParam(defaultValue = "") String fromDocNo,
                                             @RequestParam(defaultValue = "") String toDocNo) {
        return service.history(branchIds, dateType, fromChecked, fromDate, toChecked, toDate, fromDocNo, toDocNo);
    }

    @GetMapping("/api/purchase/supplier-dispatch/{id}")
    public Map<String, Object> load(@PathVariable int id) { return service.load(id); }

    @GetMapping("/api/purchase/supplier-dispatch/{id}/image/{which}")
    public ResponseEntity<byte[]> image(@PathVariable int id, @PathVariable String which) {
        Map<String, Object> m = service.image(id, which);
        if (m == null) return ResponseEntity.notFound().build();
        return ResponseEntity.ok().contentType(MediaType.parseMediaType(String.valueOf(m.get("type")))).body((byte[]) m.get("bytes"));
    }

    @GetMapping("/api/purchase/supplier-dispatch/{id}/print-check")
    public Map<String, Object> printCheck(@PathVariable int id) { return service.checkPrint(id); }

    @PostMapping("/api/purchase/supplier-dispatch/save")
    public Map<String, Object> save(@RequestBody Map<String, Object> dto) { return service.save(dto); }

    @PostMapping("/api/purchase/supplier-dispatch/{id}/delete")
    public Map<String, Object> delete(@PathVariable int id) { return service.delete(id); }

    /** The Attachment form's list of a saved record (and the History NoOfAttachments link). */
    @GetMapping("/api/purchase/supplier-dispatch-attachments/{type}/{id}")
    public List<Map<String, Object>> attachmentList(@PathVariable int type, @PathVariable int id) {
        service.requireReadable(id);
        return attachments.list(id, type);
    }

    @GetMapping("/api/purchase/supplier-dispatch-attachments/{type}/{id}/{attachment}")
    public ResponseEntity<byte[]> attachmentDownload(@PathVariable int type, @PathVariable int id, @PathVariable int attachment) {
        service.requireReadable(id);
        var file = attachments.download(id, type, attachment);
        return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM)
                .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(file.name(), StandardCharsets.UTF_8).build().toString())
                .body(file.bytes());
    }

    @ExceptionHandler(IllegalArgumentException.class)
    public ResponseEntity<Map<String, Object>> invalid(IllegalArgumentException e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", e.getMessage());
        return ResponseEntity.badRequest().body(m);
    }

    /** A procedure's RAISERROR text - the message the desktop's "Database Error" box shows. */
    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
        LOG.warn("Supplier Dispatch database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }
}
