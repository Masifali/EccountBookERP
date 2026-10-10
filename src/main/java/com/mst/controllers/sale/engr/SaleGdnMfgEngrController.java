package com.mst.controllers.sale.engr;

import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleGdnMfgEngrService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 844 frmGoodsDispatchNotesEngr (Architecture.WinApp.Mfg, module 134) - API under /sale/engr/mfg/gdn/api (page: SaleGdnMfgEngrPageController). */
@RestController
@RequestMapping("/sale/engr/mfg/gdn/api")
public class SaleGdnMfgEngrController extends SaleEngrControllerBase {
    private final SaleGdnMfgEngrService service;

    public SaleGdnMfgEngrController(SaleGdnMfgEngrService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    /** btnRefresh_Click */
    @GetMapping("/lists")
    public Map<String, Object> lists() { return service.lists(); }

    /** Reset tail: FillGrdPendingOrders + GenerateCode */
    @GetMapping("/reset-data")
    public Map<String, Object> resetData() { return service.resetData(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    /** BindPendingCustomer */
    @GetMapping("/pending-customers")
    public List<Map<String, Object>> pendingCustomers(@RequestParam int doId) { return service.pendingCustomers(doId); }

    /** GetDeliveryOrderDataByPartyDoAndOrderId */
    @GetMapping("/do-data")
    public List<Map<String, Object>> doData(@RequestParam int customerId, @RequestParam int doId) { return service.deliveryOrderData(customerId, doId); }

    /** grd_CellUpdated: current stock of the grid rows */
    @PostMapping("/stocks")
    public List<Double> stocks(@RequestParam(required = false) String docDate, @RequestBody List<SaleGdnMfgEngrService.StockReq> rows) { return service.stocks(docDate, rows); }

    @GetMapping("/variants")
    public List<Map<String, Object>> variants(@RequestParam int itemId) { return service.variants(itemId); }

    @GetMapping("/history-customers")
    public List<Map<String, Object>> historyCustomers() { return service.historyCustomers(); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId) {
        return service.history(dateType, fromDate, toDate, fromNo, toNo, customerId);
    }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    /** History selection: the detail of the highlighted row */
    @GetMapping("/{id}/detail")
    public List<Map<String, Object>> detail(@PathVariable int id) { return service.detailOf(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleGdnMfgEngrService.Request request) { return service.save(request); }

    @DeleteMapping("/{id}")
    public Map<String, Object> delete(@PathVariable int id) { return service.delete(id); }

    /** GenerateCustomerSlip: "Not Record Found For Display" check */
    @GetMapping("/{id}/slip-check")
    public Map<String, Object> slipCheck(@PathVariable int id) { return service.slipRows(id); }

    @GetMapping("/attachments/{docType}/{id}")
    public List<Map<String, Object>> attachments(@PathVariable int docType, @PathVariable int id) { return service.attachmentList(docType, id); }

    @GetMapping("/attachments/{docType}/{id}/{attachmentId}")
    public ResponseEntity<byte[]> download(@PathVariable int docType, @PathVariable int id, @PathVariable int attachmentId) {
        SaleEngrAttachments.Download d = service.download(docType, id, attachmentId);
        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename*=UTF-8''" + java.net.URLEncoder.encode(d.name(), StandardCharsets.UTF_8).replace("+", "%20"))
                .contentType(MediaType.APPLICATION_OCTET_STREAM).body(d.bytes());
    }
}
