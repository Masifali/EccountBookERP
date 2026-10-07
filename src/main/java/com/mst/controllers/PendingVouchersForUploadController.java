package com.mst.controllers;

import com.mst.services.PendingVouchersForUploadService;
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
 * 957 "Vouchers (Upload)" - Architecture.WinApp.DataSyncing.frmPendingVouchersForUpload (dbo.ScreenDefinition 957,
 * module 2 Accounts). The page and its API; the rules live in PendingVouchersForUploadService. Business refusals are
 * IllegalArgumentException (400 with the desktop's text, ApiExceptionAdvice); rights / tenancy refusals are
 * AccessDeniedException (403). No parameter carries tenancy.
 */
@Controller
public class PendingVouchersForUploadController {

    private static final String API = "/api/accounts/pending-vouchers-for-upload";

    private final PendingVouchersForUploadService service;

    public PendingVouchersForUploadController(PendingVouchersForUploadService service) { this.service = service; }

    @GetMapping("/accounts/pending-vouchers-for-upload")
    public String page(Model model) {
        service.user();
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", "Vouchers (Upload)");
        model.addAttribute("screenId", PendingVouchersForUploadService.SCREEN_ID);
        return "accounts/pending_vouchers_for_upload";
    }

    @GetMapping(API + "/init") @ResponseBody
    public Map<String, Object> init() { return service.init(); }

    @GetMapping(API + "/refresh") @ResponseBody
    public Map<String, Object> refresh() { return service.refresh(); }

    @PostMapping(API + "/rows") @ResponseBody
    public Map<String, Object> rows(@RequestBody Map<String, Object> body) { return service.rows(body); }

    @GetMapping(API + "/voucher/{id}") @ResponseBody
    public Map<String, Object> voucher(@PathVariable int id) { return service.voucher(id); }

    @PostMapping(API + "/upload") @ResponseBody
    public Map<String, Object> upload(@RequestBody Map<String, Object> body) { return service.upload(body); }

    @GetMapping(API + "/attachments") @ResponseBody
    public List<Map<String, Object>> attachments(@RequestParam int id, @RequestParam int documentTypeId) {
        return service.attachments(id, documentTypeId);
    }

    @GetMapping(API + "/attachments/{attachmentId:[0-9]+}")
    public ResponseEntity<byte[]> attachment(@PathVariable int attachmentId, @RequestParam int id, @RequestParam int documentTypeId) {
        PendingVouchersForUploadService.Download file = service.attachment(id, documentTypeId, attachmentId);
        return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM)
                .header("X-Content-Type-Options", "nosniff")
                .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(file.name, StandardCharsets.UTF_8).build().toString())
                .cacheControl(CacheControl.noStore()).body(file.bytes);
    }
}
