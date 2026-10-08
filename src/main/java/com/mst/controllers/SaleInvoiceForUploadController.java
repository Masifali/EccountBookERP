package com.mst.controllers;

import com.mst.services.SaleInvoiceForUploadService;
import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;
import org.springframework.http.CacheControl;
import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

/**
 * 956 "Sale Invoice (Upload)" - Architecture.WinApp.DataSyncing.frmPendingSaleInvoiceForUpload (dbo.ScreenDefinition 956,
 * module 6 Customer Sales). The page and its API; the rules live in SaleInvoiceForUploadService. Business refusals are
 * IllegalArgumentException (400 with the desktop's text, ApiExceptionAdvice); rights / tenancy refusals are
 * AccessDeniedException (403). No parameter carries tenancy.
 */
@Controller
public class SaleInvoiceForUploadController {

    private static final String API = "/sale/api/sale-invoice-for-upload";

    private final SaleInvoiceForUploadService service;

    public SaleInvoiceForUploadController(SaleInvoiceForUploadService service) { this.service = service; }

    @GetMapping("/sale/sale-invoice-for-upload")
    public String page(Model model) {
        service.user();
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice (Upload)");
        model.addAttribute("screenId", SaleInvoiceForUploadService.SCREEN_ID);
        return "sale/sale_invoice_for_upload";
    }

    @GetMapping(API + "/init") @ResponseBody
    public Map<String, Object> init() { return service.init(); }

    @GetMapping(API + "/refresh") @ResponseBody
    public Map<String, Object> refresh() { return service.refresh(); }

    @PostMapping(API + "/rows") @ResponseBody
    public Map<String, Object> rows(@RequestBody Map<String, Object> body) { return service.rows(body); }

    @PostMapping(API + "/upload") @ResponseBody
    public Map<String, Object> upload(@RequestBody Map<String, Object> body) { return service.upload(body); }

    @GetMapping(API + "/attachments") @ResponseBody
    public List<Map<String, Object>> attachments(@RequestParam int id, @RequestParam int documentTypeId) {
        return service.attachments(id, documentTypeId);
    }

    @GetMapping(API + "/attachments/{attachmentId:[0-9]+}")
    public ResponseEntity<byte[]> attachment(@PathVariable int attachmentId, @RequestParam int id, @RequestParam int documentTypeId) {
        var file = service.attachment(id, documentTypeId, attachmentId);
        return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM)
                .header("X-Content-Type-Options", "nosniff")
                .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(file.name(), StandardCharsets.UTF_8).build().toString())
                .cacheControl(CacheControl.noStore()).body(file.bytes());
    }
}
