package com.mst.controllers;

import com.mst.services.PurchaseInvoiceForUploadService;
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
 * 910 "Purchase Invoice For Upload" - Architecture.WinApp.DataSyncing.frmPendingPurchaseInvoiceForUpload
 * (dbo.ScreenDefinition 910, module 5 Supplier Purchases). The page and its API; the rules live in
 * PurchaseInvoiceForUploadService. Business refusals are IllegalArgumentException (400 with the desktop's text,
 * ApiExceptionAdvice); rights / tenancy refusals are AccessDeniedException (403). No parameter carries tenancy.
 */
@Controller
public class PurchaseInvoiceForUploadController {

    private static final String API = "/purchase/api/purchase-invoice-for-upload";

    private final PurchaseInvoiceForUploadService service;

    public PurchaseInvoiceForUploadController(PurchaseInvoiceForUploadService service) { this.service = service; }

    @GetMapping("/purchase/purchase-invoice-for-upload")
    public String page(Model model) {
        service.user();
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice For Upload");
        model.addAttribute("screenId", PurchaseInvoiceForUploadService.SCREEN_ID);
        return "purchase/purchase_invoice_for_upload";
    }

    @GetMapping("/purchase/api/purchase-invoice-for-upload/init") @ResponseBody
    public Map<String, Object> init() { return service.init(); }

    @GetMapping("/purchase/api/purchase-invoice-for-upload/refresh") @ResponseBody
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/purchase/api/purchase-invoice-for-upload/suppliers") @ResponseBody
    public List<Map<String, Object>> suppliers(@RequestParam(value = "branchIds", required = false) List<Integer> branchIds) {
        return service.suppliers(branchIds);
    }

    @PostMapping("/purchase/api/purchase-invoice-for-upload/rows") @ResponseBody
    public Map<String, Object> rows(@RequestBody Map<String, Object> body) { return service.rows(body); }

    @PostMapping("/purchase/api/purchase-invoice-for-upload/upload") @ResponseBody
    public Map<String, Object> upload(@RequestBody Map<String, Object> body) { return service.upload(body); }

    @GetMapping("/purchase/api/purchase-invoice-for-upload/attachments") @ResponseBody
    public List<Map<String, Object>> attachments(@RequestParam int id, @RequestParam int documentTypeId) {
        return service.attachments(id, documentTypeId);
    }

    @GetMapping("/purchase/api/purchase-invoice-for-upload/attachments/{attachmentId:[0-9]+}")
    public ResponseEntity<byte[]> attachment(@PathVariable int attachmentId, @RequestParam int id, @RequestParam int documentTypeId) {
        var file = service.attachment(id, documentTypeId, attachmentId);
        return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM)
                .header("X-Content-Type-Options", "nosniff")
                .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(file.name(), StandardCharsets.UTF_8).build().toString())
                .cacheControl(CacheControl.noStore()).body(file.bytes());
    }
}
