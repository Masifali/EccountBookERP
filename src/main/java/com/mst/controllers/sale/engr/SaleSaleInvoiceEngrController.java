package com.mst.controllers.sale.engr;

import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleSaleInvoiceEngrService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 535 SaleInvoiceTrading_Engr - /sale/engr/sale-invoice (page: SaleEngrViewController). */
@RestController
@RequestMapping("/sale/engr/sale-invoice/api")
public class SaleSaleInvoiceEngrController extends SaleEngrControllerBase {
    private final SaleSaleInvoiceEngrService service;

    public SaleSaleInvoiceEngrController(SaleSaleInvoiceEngrService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/last-rate")
    public List<Map<String, Object>> lastRate(@RequestParam int currencyId) { return service.lastRate(currencyId); }

    @GetMapping("/remaining")
    public Map<String, Object> remaining(@RequestParam(defaultValue = "0") int orderId, @RequestParam(defaultValue = "0") int recId) {
        return service.remaining(orderId, recId);
    }

    @GetMapping("/loader/rows")
    public List<Map<String, Object>> loaderRows(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate) {
        return service.loaderRows(fromDate, toDate);
    }

    @GetMapping("/loader/detail")
    public List<Map<String, Object>> loaderDetail(@RequestParam int gdnId) { return service.loaderDetail(gdnId); }

    @GetMapping("/load-gdn")
    public Map<String, Object> loadGdn(@RequestParam String gdnIds, @RequestParam(defaultValue = "0") int deliveryTypeId,
                                       @RequestParam(defaultValue = "0") int customerId) {
        return service.loadGdn(gdnIds, deliveryTypeId, customerId);
    }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos() { return service.historyCombos(); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId) {
        return service.history(dateType, fromDate, toDate, fromNo, toNo, customerId);
    }

    @GetMapping("/{id}/history-detail")
    public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(id); }

    @PostMapping("/calc")
    public Map<String, Object> calc(@RequestBody SaleSaleInvoiceEngrService.Req request) { return service.calc(request); }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleSaleInvoiceEngrService.Req request) { return service.save(request); }

    @DeleteMapping("/{id}")
    public Map<String, Object> delete(@PathVariable int id) { return service.delete(id); }

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
