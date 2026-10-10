package com.mst.controllers.sale.engr;

import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleInvoiceDirectMfgEngrService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 780 frmSaleInvoiceDirectEngr (Architecture.WinApp.Mfg.Sale) - Sale Invoice Direct, document type 1661. Page: SaleInvoiceDirectMfgEngrViewController. */
@RestController
@RequestMapping("/sale/engr/mfg/sale-invoice-direct/api")
public class SaleInvoiceDirectMfgEngrController extends SaleEngrControllerBase {
    private final SaleInvoiceDirectMfgEngrService service;

    public SaleInvoiceDirectMfgEngrController(SaleInvoiceDirectMfgEngrService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo(), "branchSrNo", service.nextBranchSrNo()); }

    @GetMapping("/last-rate")
    public List<Map<String, Object>> lastRate(@RequestParam int currencyId) { return service.lastRate(currencyId); }

    @GetMapping("/ledger-balance")
    public Map<String, Object> ledgerBalance(@RequestParam(defaultValue = "0") int customerId, @RequestParam(required = false) String docDate) {
        return service.ledgerBalance(customerId, docDate);
    }

    @GetMapping("/tax-by-date")
    public List<Map<String, Object>> taxByDate(@RequestParam String itemIds, @RequestParam String docDate) { return service.taxByDate(itemIds, docDate); }

    @GetMapping("/remaining")
    public Map<String, Object> remaining(@RequestParam int orderId, @RequestParam(defaultValue = "0") int recId) { return service.remaining(orderId, recId); }

    @GetMapping("/loader/init")
    public Map<String, Object> loaderInit() { return service.loaderInit(); }

    @GetMapping("/loader/rows")
    public List<Map<String, Object>> loaderRows(@RequestParam(required = false) String branchIds, @RequestParam(required = false) String fromDate,
                                                @RequestParam(required = false) String toDate, @RequestParam(defaultValue = "0") int fromNo,
                                                @RequestParam(defaultValue = "0") int toNo, @RequestParam(defaultValue = "0") int customerId) {
        return service.loaderRows(branchIds, fromDate, toDate, fromNo, toNo, customerId);
    }

    @GetMapping("/loader/customers")
    public List<Map<String, Object>> loaderCustomers(@RequestParam(required = false) String branchIds) { return service.loaderCustomers(branchIds); }

    @GetMapping("/load-orders")
    public Map<String, Object> loadOrders(@RequestParam int orderId, @RequestParam String detailIds, @RequestParam(defaultValue = "0") int loadedOrderId) {
        return service.loadOrders(orderId, detailIds, loadedOrderId);
    }

    @GetMapping("/stock")
    public Map<String, Object> stock(@RequestParam(required = false) String docDate, @RequestParam(defaultValue = "0") int warehouseId, @RequestParam(defaultValue = "0") int itemId,
                                     @RequestParam(defaultValue = "0") int variantId, @RequestParam(defaultValue = "0") int productionStageId,
                                     @RequestParam(defaultValue = "0") int uomId, @RequestParam(defaultValue = "0") int castingTypeId) {
        return service.stock(docDate, warehouseId, itemId, variantId, productionStageId, uomId, castingTypeId);
    }

    @GetMapping("/tax-types")
    public List<Map<String, Object>> taxTypes(@RequestParam(required = false) String docDate) { return service.taxTypes(docDate); }

    @GetMapping("/uoms")
    public List<Map<String, Object>> uoms(@RequestParam(defaultValue = "0") int itemId) { return service.uoms(itemId); }

    @GetMapping("/variants")
    public List<Map<String, Object>> variants(@RequestParam(defaultValue = "0") int itemId) { return service.variants(itemId); }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos(@RequestParam(required = false) String branchIds) { return service.historyCombos(branchIds); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(required = false) String branchIds, @RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int paymentTermId) {
        return service.history(branchIds, dateType, fromDate, toDate, fromNo, toNo, customerId, paymentTermId);
    }

    @GetMapping("/{id}/history-detail")
    public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(id); }

    @PostMapping("/calc")
    public Map<String, Object> calc(@RequestBody SaleInvoiceDirectMfgEngrService.Req request) { return service.calc(request); }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleInvoiceDirectMfgEngrService.Req request) { return service.save(request); }

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
