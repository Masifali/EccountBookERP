package com.mst.controllers.sale.steel;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.steel.SaleInvGdnStService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 540 InvFrmGDN_St - /sale/steel/gdn (page: SaleSteelViewController). */
@RestController
@RequestMapping("/sale/steel/gdn/api")
public class SaleInvGdnStController extends SaleEngrControllerBase {
    private final SaleInvGdnStService service;

    public SaleInvGdnStController(SaleInvGdnStService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/pending")
    public Map<String, Object> pending() { return service.pending(); }

    @GetMapping("/uoms")
    public List<Map<String, Object>> uoms(@RequestParam int itemId) { return service.uoms(itemId); }

    @GetMapping("/stock")
    public Map<String, Object> stock(@RequestParam int itemId, @RequestParam(required = false) String docDate,
                                     @RequestParam(defaultValue = "0") int jobLotId, @RequestParam(defaultValue = "0") int warehouseId) {
        return Map.of("stock", service.stock(itemId, docDate, jobLotId, warehouseId));
    }

    @GetMapping("/order-items")
    public List<Map<String, Object>> orderItems(@RequestParam int orderId) { return service.orderItems(orderId); }

    @GetMapping("/gatepass/delivery-order")
    public Map<String, Object> gatePassDeliveryOrder(@RequestParam int gpId) { return service.gatePassDeliveryOrder(gpId); }

    @GetMapping("/gatepass/delivery-lines")
    public Map<String, Object> deliveryLines(@RequestParam int soOrderId, @RequestParam int customerId) { return service.deliveryLines(soOrderId, customerId); }

    @GetMapping("/gatepass/rows")
    public Map<String, Object> gatePassRows(@RequestParam int gpId, @RequestParam(defaultValue = "false") boolean netWeight) { return service.gatePassRows(gpId, netWeight); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId) {
        return service.history(dateType, fromDate, toDate, fromNo, toNo, customerId);
    }

    @GetMapping("/history-customers")
    public List<Map<String, Object>> historyCustomers() { return service.historyCustomers(); }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @GetMapping("/{id}/history-detail")
    public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleInvGdnStService.Request request) { return service.save(request); }

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
