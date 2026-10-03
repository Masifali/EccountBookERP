package com.mst.controllers;

import com.mst.repositories.PurchaseGrnRecordRepository;
import com.mst.services.GrnDirectService;
import com.mst.services.PurchaseDocAttachmentService;
import com.mst.services.StockInTransitService;
import java.nio.charset.StandardCharsets;
import java.util.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;

/**
 * The attachment list of a saved GRN (46), Grn (Sale Return) (143), GRN Direct (137) or Stock In Transit (251)
 * record - the desktop Attachment form (AT) and the History "NoOfAttachments" link
 * (CommonServices.GetNoofAttachmentsByScreenName / ...ByRefDocumentTypeID). Changes are not saved here: the page
 * stages them and posts them with the document's own Save / Update (see PurchaseDocAttachmentService).
 */
@RestController
@RequestMapping("/api/purchase/doc-attachments")
public class PurchaseDocAttachmentController {
    private final PurchaseDocAttachmentService attachments;
    private final PurchaseGrnRecordRepository grnRecords;
    private final GrnDirectService grnDirect;
    private final StockInTransitService stockInTransit;

    public PurchaseDocAttachmentController(PurchaseDocAttachmentService attachments, PurchaseGrnRecordRepository grnRecords,
                                           GrnDirectService grnDirect, StockInTransitService stockInTransit) {
        this.attachments = attachments; this.grnRecords = grnRecords; this.grnDirect = grnDirect; this.stockInTransit = stockInTransit;
    }

    /** The record must be one the user may open on its own form (tenancy, View, CanView AllRecord). */
    private void requireRecord(int type, int id) {
        switch (type) {
            case 46, 143 -> grnRecords.require(id, type);
            case 137 -> grnDirect.requireReadable(id);
            case 251 -> stockInTransit.requireReadable(id);
            default -> throw new IllegalArgumentException("Unsupported attachment form");
        }
    }

    @GetMapping("/{type}/{id}")
    public List<Map<String, Object>> list(@PathVariable int type, @PathVariable int id) {
        requireRecord(type, id);
        return attachments.list(id, type);
    }

    @GetMapping("/{type}/{id}/{attachment}")
    public ResponseEntity<byte[]> download(@PathVariable int type, @PathVariable int id, @PathVariable int attachment) {
        requireRecord(type, id);
        var file = attachments.download(id, type, attachment);
        return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM)
                .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(file.name(), StandardCharsets.UTF_8).build().toString())
                .body(file.bytes());
    }

    @ExceptionHandler(IllegalArgumentException.class)
    public ResponseEntity<?> invalid(IllegalArgumentException failure) {
        return ResponseEntity.badRequest().body(Map.of("success", false, "message", failure.getMessage()));
    }
}
