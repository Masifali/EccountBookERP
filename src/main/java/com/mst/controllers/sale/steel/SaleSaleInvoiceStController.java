package com.mst.controllers.sale.steel;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.steel.SaleSaleInvoiceStService;
import com.mst.services.sale.steel.SaleStInvoiceCalc;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 543 SaleInvoice_St - /sale/steel/sale-invoice (page: SaleSaleInvoiceStViewController). */
@RestController
@RequestMapping("/sale/steel/sale-invoice/api")
public class SaleSaleInvoiceStController extends SaleEngrControllerBase {
    private final SaleSaleInvoiceStService service;

    public SaleSaleInvoiceStController(SaleSaleInvoiceStService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos() { return service.historyCombos(); }

    @PostMapping("/calc")
    public SaleStInvoiceCalc.Inv calc(@RequestBody SaleSaleInvoiceStService.CalcRequest request) { return service.calc(request); }

    @GetMapping("/loader/pending")
    public List<Map<String, Object>> loaderPending(@RequestParam(required = false) String from, @RequestParam(required = false) String to) { return service.loaderPending(from, to); }

    @GetMapping("/loader/detail")
    public List<Map<String, Object>> loaderDetail(@RequestParam int id) { return service.loaderDetail(id); }

    @PostMapping("/load")
    public SaleStInvoiceCalc.Inv load(@RequestBody SaleSaleInvoiceStService.LoadRequest request) { return service.load(request); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int paymentTermId,
                                             @RequestParam(required = false) String deliveryTerm) {
        return service.history(dateType, fromDate, toDate, fromNo, toNo, customerId, paymentTermId, deliveryTerm);
    }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @GetMapping("/{id}/history-detail")
    public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleSaleInvoiceStService.SaveRequest request) { return service.save(request); }

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
