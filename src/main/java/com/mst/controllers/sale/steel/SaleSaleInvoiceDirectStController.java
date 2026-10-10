package com.mst.controllers.sale.steel;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.steel.SaleSaleInvoiceDirectStService;
import com.mst.services.sale.steel.SaleStDirectCalc;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 544 SaleInvoiceDirect_St - /sale/steel/sale-invoice-direct (page: SaleSaleInvoiceDirectStViewController). */
@RestController
@RequestMapping("/sale/steel/sale-invoice-direct/api")
public class SaleSaleInvoiceDirectStController extends SaleEngrControllerBase {
    private final SaleSaleInvoiceDirectStService service;

    public SaleSaleInvoiceDirectStController(SaleSaleInvoiceDirectStService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos() { return service.historyCombos(); }

    @GetMapping("/uoms")
    public List<Map<String, Object>> uoms(@RequestParam int itemId) { return service.uoms(itemId); }

    @GetMapping("/stock")
    public Map<String, Object> stock(@RequestParam(defaultValue = "0") int warehouseId, @RequestParam(defaultValue = "0") int itemId,
                                     @RequestParam(defaultValue = "0") int jobLotId, @RequestParam(defaultValue = "0") int packingTypeId,
                                     @RequestParam(defaultValue = "0") int uomId, @RequestParam(required = false) String docDate) {
        return service.stock(warehouseId, itemId, jobLotId, packingTypeId, uomId, docDate);
    }

    @PostMapping("/calc")
    public SaleStDirectCalc.Inv calc(@RequestBody SaleSaleInvoiceDirectStService.CalcRequest request) { return service.calc(request); }

    @GetMapping("/loader/combos")
    public Map<String, Object> loaderCombos() { return service.loaderCombos(); }

    @GetMapping("/loader/pending")
    public List<Map<String, Object>> loaderPending() { return service.loaderPending(); }

    @PostMapping("/load")
    public SaleStDirectCalc.Inv load(@RequestBody SaleSaleInvoiceDirectStService.LoadRequest request) { return service.load(request); }

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

    @GetMapping("/{id}/voucher-head")
    public Map<String, Object> voucherHead(@PathVariable int id) { return Map.of("voucherHeadId", service.voucherHeadId(id)); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleSaleInvoiceDirectStService.SaveRequest request) { return service.save(request); }

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
