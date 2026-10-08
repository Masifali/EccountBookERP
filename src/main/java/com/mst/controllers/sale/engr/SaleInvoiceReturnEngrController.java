package com.mst.controllers.sale.engr;

import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleInvoiceReturnEngrService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 537 frmSaleInvoiceReturn_Engr - /sale/engr/sale-invoice-return (page: SaleEngrViewController). The desktop's btnDelete_Click is empty, so there is no delete endpoint. */
@RestController
@RequestMapping("/sale/engr/sale-invoice-return/api")
public class SaleInvoiceReturnEngrController extends SaleEngrControllerBase {
    private final SaleInvoiceReturnEngrService service;

    public SaleInvoiceReturnEngrController(SaleInvoiceReturnEngrService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh(@RequestParam(required = false) String date) { return service.refresh(date); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/tax-types")
    public List<Map<String, Object>> taxTypes(@RequestParam(required = false) String date) { return service.taxTypes(date); }

    @GetMapping("/tax-by-items")
    public List<Map<String, Object>> taxByItems(@RequestParam(required = false) String itemIds, @RequestParam(required = false) String date) {
        return service.taxByItems(itemIds, date);
    }

    @GetMapping("/uoms")
    public List<Map<String, Object>> uoms(@RequestParam int itemId) { return service.uoms(itemId); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate) {
        return service.history(dateType, fromDate, toDate);
    }

    @GetMapping("/{id}/history-detail")
    public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(id); }

    @PostMapping("/calc")
    public Map<String, Object> calc(@RequestBody SaleInvoiceReturnEngrService.Req request) { return service.calc(request); }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleInvoiceReturnEngrService.Req request) { return service.save(request); }

    @GetMapping("/{id}/attachments")
    public List<Map<String, Object>> attachments(@PathVariable int id) { return service.attachmentList(id); }

    @GetMapping("/{id}/attachments/{attachmentId}")
    public ResponseEntity<byte[]> download(@PathVariable int id, @PathVariable int attachmentId) {
        SaleEngrAttachments.Download d = service.download(id, attachmentId);
        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename*=UTF-8''" + java.net.URLEncoder.encode(d.name(), StandardCharsets.UTF_8).replace("+", "%20"))
                .contentType(MediaType.APPLICATION_OCTET_STREAM).body(d.bytes());
    }
}
