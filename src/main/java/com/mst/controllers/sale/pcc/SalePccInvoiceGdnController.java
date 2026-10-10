package com.mst.controllers.sale.pcc;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.pcc.SalePccInvoiceGdnService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 547 SaleInvoiceAgainstGDNConcrete (document type 1861) - page /sale/pcc/sale-invoice-against-gdn (SalePccInvoiceGdnViewController). */
@RestController
@RequestMapping("/sale/pcc/sale-invoice-against-gdn/api")
public class SalePccInvoiceGdnController extends SaleEngrControllerBase {
    private final SalePccInvoiceGdnService service;

    public SalePccInvoiceGdnController(SalePccInvoiceGdnService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    /** btnRefresh_Click */
    @GetMapping("/lists")
    public Map<String, Object> lists() { return service.lists(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/last-rate")
    public Map<String, Object> lastRate(@RequestParam int currencyId) { return service.lastRate(currencyId); }

    @GetMapping("/accounts")
    public Map<String, Object> accounts() { return service.accounts(); }

    /** Load Gdn dialog: filter combos. */
    @GetMapping("/loader/combos")
    public Map<String, Object> loaderCombos() { return service.loaderCombos(); }

    /** Load Gdn dialog: PendingGDNForInvoice. */
    @GetMapping("/loader/pending")
    public List<Map<String, Object>> pending(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int categoryId,
                                             @RequestParam(defaultValue = "0") int itemId, @RequestParam(defaultValue = "0") int typeId,
                                             @RequestParam(defaultValue = "0") int parentId) {
        return service.pending(fromDate, toDate, fromNo, toNo, customerId, categoryId, itemId, typeId, parentId);
    }

    /** LoadExpData: GdnWagesDetailByGdnIds. */
    @GetMapping("/loader/wages")
    public List<Map<String, Object>> gdnWages(@RequestParam String ids) { return service.gdnWages(ids); }

    /** DocDate_ValueChanged -> UpdateWagesRateForAllRows. */
    @GetMapping("/wages-rates")
    public List<Map<String, Object>> wagesRates(@RequestParam int itemId, @RequestParam(required = false) String docDate, @RequestParam(defaultValue = "0") int contractorId) {
        return service.wagesRates(itemId, docDate, contractorId);
    }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos() { return service.historyCombos(); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int agentId) {
        return service.history(fromDate, toDate, fromNo, toNo, customerId, agentId);
    }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SalePccInvoiceGdnService.Request request) { return service.save(request); }

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
