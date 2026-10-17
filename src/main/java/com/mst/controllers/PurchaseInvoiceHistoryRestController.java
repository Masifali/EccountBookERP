package com.mst.controllers;

import com.mst.services.PurchaseInvoiceHistoryService;
import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;
import org.springframework.http.CacheControl;
import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

/** 476 frmPurchaseInvoiceHistory "Purchase Report" (Purchase Reports, module 52) API. Refusals are IllegalArgumentException (HTTP 400, desktop text). */
@RestController
@RequestMapping("/purchase/api/reports/invoice-history")
public class PurchaseInvoiceHistoryRestController {
    private final PurchaseInvoiceHistoryService service;

    public PurchaseInvoiceHistoryRestController(PurchaseInvoiceHistoryService service) { this.service = service; }

    @GetMapping("/init") public Map<String, Object> init() { return service.init(); }
    @GetMapping("/lookups") public Map<String, Object> lookups(@RequestParam(required = false) List<Integer> branchIds) { return service.lookups(branchIds); }
    @PostMapping("/rows") public Map<String, Object> rows(@RequestBody Map<String, Object> body) { return service.rows(body); }
    @PostMapping("/print-data") public Map<String, Object> printData(@RequestBody Map<String, Object> body) { return service.printData(body); }
    @GetMapping("/gl-account") public Map<String, Object> glAccount(@RequestParam int supplierCustomerId) { return service.glAccount(supplierCustomerId); }
    @GetMapping("/attachments") public List<Map<String, Object>> attachments(@RequestParam int id, @RequestParam int documentTypeId) { return service.attachments(id, documentTypeId); }

    @GetMapping("/attachments/{attachmentId:[0-9]+}")
    public ResponseEntity<byte[]> attachment(@PathVariable int attachmentId, @RequestParam int id, @RequestParam int documentTypeId) {
        var file = service.attachment(id, documentTypeId, attachmentId);
        return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM)
                .header("X-Content-Type-Options", "nosniff")
                .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(file.name(), StandardCharsets.UTF_8).build().toString())
                .cacheControl(CacheControl.noStore()).body(file.bytes());
    }
}
