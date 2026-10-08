package com.mst.controllers.sale.engr;

import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleInvoiceQtyEngrService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 139 SaleInvoiceQtyWithTax - /sale/engr/sale-invoice-direct (page: SaleEngrViewController). */
@RestController
@RequestMapping("/sale/engr/sale-invoice-direct/api")
public class SaleInvoiceQtyEngrController extends SaleEngrControllerBase {
    private final SaleInvoiceQtyEngrService service;

    public SaleInvoiceQtyEngrController(SaleInvoiceQtyEngrService service) { this.service = service; }

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

    @GetMapping("/last-rate")
    public List<Map<String, Object>> lastRate(@RequestParam int currencyId) { return service.lastRate(currencyId); }

    @GetMapping("/remaining")
    public Map<String, Object> remaining(@RequestParam(defaultValue = "0") int orderId, @RequestParam(defaultValue = "0") int recId) {
        return service.remaining(orderId, recId);
    }

    @GetMapping("/loader/combos")
    public Map<String, Object> loaderCombos() { return service.loaderCombos(); }

    @GetMapping("/loader/rows")
    public List<Map<String, Object>> loaderRows(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                                @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                                @RequestParam(defaultValue = "0") int customerId) {
        return service.loaderRows(fromDate, toDate, fromNo, toNo, customerId);
    }

    @GetMapping("/order-payments")
    public List<Map<String, Object>> orderPayments(@RequestParam String orderIds) { return service.orderPayments(orderIds); }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos() { return service.historyCombos(); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int paymentTermId) {
        return service.history(dateType, fromDate, toDate, fromNo, toNo, customerId, paymentTermId);
    }

    @GetMapping("/{id}/history-detail")
    public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(id); }

    @PostMapping("/calc")
    public Map<String, Object> calc(@RequestBody SaleInvoiceQtyEngrService.Req request) { return service.calc(request); }

    @PostMapping("/validate")
    public Map<String, Object> validate(@RequestBody SaleInvoiceQtyEngrService.Req request) { return service.validate(request); }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleInvoiceQtyEngrService.Req request) { return service.save(request); }

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
