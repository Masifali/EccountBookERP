package com.mst.controllers;

import com.mst.services.ExportReceiptInvoicesAdjustmentVoucherService;
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
 * 916 "Fcy Receipt Adjustment Voucher" - Architecture.WinApp.Account_Definition.AdjustmentVouchers.frmExportReceiptInvoicesAdjustmentVoucher.
 * The page and its API; the rules live in ExportReceiptInvoicesAdjustmentVoucherService. Business refusals are IllegalArgumentException
 * (400 with the desktop's text, ApiExceptionAdvice); rights / tenancy refusals are AccessDeniedException (403). No parameter carries tenancy.
 */
@Controller
@RequestMapping("/accounts/export-receipt-invoices-adjustment-voucher")
public class ExportReceiptInvoicesAdjustmentVoucherController {

    private final ExportReceiptInvoicesAdjustmentVoucherService service;

    public ExportReceiptInvoicesAdjustmentVoucherController(ExportReceiptInvoicesAdjustmentVoucherService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "accounts");
        return "accounts/export_receipt_invoices_adjustment_voucher";
    }

    @GetMapping("/api/load") @ResponseBody
    public Map<String, Object> load() { return service.load(); }

    @GetMapping("/api/pending") @ResponseBody
    public List<Map<String, Object>> pending() { return service.pending(); }

    @GetMapping("/api/invoices") @ResponseBody
    public List<Map<String, Object>> invoices(@RequestParam int supplierCustomerId, @RequestParam(defaultValue = "0") int fcyId) {
        return service.invoices(supplierCustomerId, fcyId);
    }

    @GetMapping("/api/by-voucher-head") @ResponseBody
    public List<Map<String, Object>> byVoucherHead(@RequestParam int voucherHeadId, @RequestParam(defaultValue = "0") int paymentTypeId,
                                                   @RequestParam(defaultValue = "0") int supplierCustomerId, @RequestParam(defaultValue = "0") int partyGlId) {
        return service.byVoucherHead(voucherHeadId, paymentTypeId, supplierCustomerId, partyGlId);
    }

    @GetMapping("/api/history-parties") @ResponseBody
    public List<Map<String, Object>> historyParties() { return service.historyParties(); }

    @PostMapping("/api/history") @ResponseBody
    public List<Map<String, Object>> history(@RequestBody Map<String, Object> body) { return service.history(body); }

    @PostMapping("/api/save") @ResponseBody
    public Map<String, Object> save(@RequestBody Map<String, Object> body) { return service.save(body); }

    @PostMapping("/api/delete") @ResponseBody
    public Map<String, Object> delete(@RequestBody Map<String, Object> body) { return service.delete(body); }

    @GetMapping("/api/attachments") @ResponseBody
    public List<Map<String, Object>> attachments(@RequestParam int id, @RequestParam int documentTypeId) {
        return service.attachments(id, documentTypeId);
    }

    @GetMapping("/api/attachments/{attachmentId:[0-9]+}")
    public ResponseEntity<byte[]> attachment(@PathVariable int attachmentId, @RequestParam int id, @RequestParam int documentTypeId) {
        ExportReceiptInvoicesAdjustmentVoucherService.Download file = service.attachment(id, documentTypeId, attachmentId);
        return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM)
                .header("X-Content-Type-Options", "nosniff")
                .header(HttpHeaders.CONTENT_DISPOSITION, ContentDisposition.attachment().filename(file.name(), StandardCharsets.UTF_8).build().toString())
                .cacheControl(CacheControl.noStore()).body(file.bytes());
    }
}
