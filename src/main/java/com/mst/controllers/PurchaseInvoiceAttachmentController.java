package com.mst.controllers;

import com.mst.services.PurchaseInvoiceAttachmentService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/purchase/invoice-attachments")
public class PurchaseInvoiceAttachmentController {
    private final PurchaseInvoiceAttachmentService attachments;
    public PurchaseInvoiceAttachmentController(PurchaseInvoiceAttachmentService attachments){this.attachments=attachments;}
    @GetMapping("/{type}/{id}")
    public List<Map<String,Object>> list(@PathVariable int type,@PathVariable int id){return attachments.list(id,type);}
    @GetMapping("/{type}/{id}/{attachment}")
    public ResponseEntity<byte[]> download(@PathVariable int type,@PathVariable int id,@PathVariable int attachment){
        var file=attachments.download(id,type,attachment);
        return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM).header(HttpHeaders.CONTENT_DISPOSITION,ContentDisposition.attachment().filename(file.name(),java.nio.charset.StandardCharsets.UTF_8).build().toString()).body(file.bytes());
    }
}
