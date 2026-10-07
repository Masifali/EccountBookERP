package com.mst.controllers;

import com.mst.services.ExportInvoiceVoucherService;
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
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

/**
 * 40 "Export Voucher" - Architecture.WinApp.Account_Definition.frmExportInvoiceVoucher. The page and its API; the rules live in
 * ExportInvoiceVoucherService. Business refusals are IllegalArgumentException (400 with the desktop's text); rights / tenancy refusals
 * are AccessDeniedException (403). No parameter carries tenancy.
 */
@Controller
@RequestMapping("/accounts/export-invoice-voucher")
public class ExportInvoiceVoucherController {

    private final ExportInvoiceVoucherService service;

    public ExportInvoiceVoucherController(ExportInvoiceVoucherService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "accounts");
        return "accounts/export_invoice_voucher";
    }

    @GetMapping("/api/init") @ResponseBody
    public Map<String, Object> init() { return service.init(); }

    @GetMapping("/api/refresh") @ResponseBody
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/api/pending") @ResponseBody
    public List<Map<String, Object>> pending() { return service.pending(); }

    @GetMapping("/api/next-code") @ResponseBody
    public Map<String, Object> nextCode() { return Map.of("code", service.nextCode()); }

    @GetMapping("/api/bind") @ResponseBody
    public Map<String, Object> bind(@RequestParam int id, @RequestParam(required = false) String voucherDate) { return service.bindInvoice(id, voucherDate); }

    @GetMapping("/api/voucher/{id:[0-9]+}") @ResponseBody
    public Map<String, Object> read(@PathVariable int id) { return service.read(id); }

    @PostMapping("/api/rates") @ResponseBody
    public List<Map<String, Object>> rates(@RequestBody Map<String, Object> body) { return service.rates(body); }

    @PostMapping("/api/history") @ResponseBody
    public List<Map<String, Object>> history(@RequestBody Map<String, Object> body) { return service.history(body); }

    @GetMapping("/api/history/detail/{id:[0-9]+}") @ResponseBody
    public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(id); }

    @GetMapping("/api/history-customers") @ResponseBody
    public List<Map<String, Object>> historyCustomers() { return service.historyCustomers(); }

    @GetMapping("/api/auto-update-ids") @ResponseBody
    public List<Integer> autoUpdateIds() { return service.autoUpdateIds(); }

    @PostMapping("/api/save") @ResponseBody
    public Map<String, Object> save(@RequestBody Map<String, Object> body) { return service.save(body); }

    @PostMapping("/api/delete") @ResponseBody
    public Map<String, Object> delete(@RequestBody Map<String, Object> body) { return service.delete(body); }

    @GetMapping("/api/attachments") @ResponseBody
    public List<Map<String, Object>> attachments(@RequestParam int id) { return service.attachments(id); }

    @GetMapping("/api/attachments/{attachmentId:[0-9]+}")
    public ResponseEntity<byte[]> attachment(@PathVariable int attachmentId, @RequestParam int id) {
        ExportInvoiceVoucherService.Download file = service.attachment(id, attachmentId);
        return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM)
                .header("X-Content-Type-Options", "nosniff")
                .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(file.name(), StandardCharsets.UTF_8).build().toString())
                .cacheControl(CacheControl.noStore()).body(file.bytes());
    }
}
