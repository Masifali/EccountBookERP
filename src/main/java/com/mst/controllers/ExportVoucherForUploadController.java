package com.mst.controllers;

import com.mst.services.ExportVoucherForUploadService;
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
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

/**
 * 917 "Export Voucher (Upload)" - Architecture.WinApp.DataSyncing.frmPendingExportVoucherForUpload. The page and its API; the rules
 * live in ExportVoucherForUploadService. Business refusals are IllegalArgumentException (400 with the desktop's text,
 * ApiExceptionAdvice); rights / tenancy refusals are AccessDeniedException (403). No parameter carries tenancy.
 */
@Controller
public class ExportVoucherForUploadController {

    private static final String API = "/accounts/api/export-voucher-for-upload";

    private final ExportVoucherForUploadService service;

    public ExportVoucherForUploadController(ExportVoucherForUploadService service) { this.service = service; }

    @GetMapping("/accounts/export-voucher-for-upload")
    public String page(Model model) {
        service.user();
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", "Export Voucher (Upload)");
        model.addAttribute("screenId", ExportVoucherForUploadService.SCREEN_ID);
        return "accounts/export_voucher_for_upload";
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
        ExportVoucherForUploadService.Download file = service.attachment(id, documentTypeId, attachmentId);
        return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM)
                .header("X-Content-Type-Options", "nosniff")
                .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(file.name(), StandardCharsets.UTF_8).build().toString())
                .cacheControl(CacheControl.noStore()).body(file.bytes());
    }
}
