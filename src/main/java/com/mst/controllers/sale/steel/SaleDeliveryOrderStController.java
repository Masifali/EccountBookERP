package com.mst.controllers.sale.steel;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.steel.SaleDeliveryOrderStService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 541 DeliveryOrder_St - /sale/steel/delivery-order (page: SaleSteelViewController). */
@RestController
@RequestMapping("/sale/steel/delivery-order/api")
public class SaleDeliveryOrderStController extends SaleEngrControllerBase {
    private final SaleDeliveryOrderStService service;

    public SaleDeliveryOrderStController(SaleDeliveryOrderStService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/orders")
    public List<Map<String, Object>> orders(@RequestParam int customerId) { return service.orders(customerId); }

    @GetMapping("/order-items")
    public List<Map<String, Object>> orderItems(@RequestParam int orderId) { return service.orderItems(orderId); }

    @GetMapping("/uoms")
    public List<Map<String, Object>> uoms(@RequestParam int itemId) { return service.uoms(itemId); }

    @GetMapping("/stock")
    public Map<String, Object> stock(@RequestParam(defaultValue = "0") int warehouseId, @RequestParam(defaultValue = "0") int itemId,
                                     @RequestParam(defaultValue = "0") int jobLotId, @RequestParam(required = false) String docDate,
                                     @RequestParam(defaultValue = "0") int packingTypeId, @RequestParam(defaultValue = "0") int uomId) {
        return Map.of("stock", service.availableStock(warehouseId, itemId, jobLotId, docDate, packingTypeId, uomId));
    }

    @GetMapping("/balance")
    public Map<String, Object> balance(@RequestParam int orderId, @RequestParam int saleOrderDetailId, @RequestParam int itemId) {
        return Map.of("balWeight", service.orderBalanceWeight(orderId, saleOrderDetailId, itemId));
    }

    public static class StockRequest { public String docDate; public List<SaleDeliveryOrderStService.StockRow> rows; }

    @PostMapping("/current-stock")
    public Map<String, Object> currentStock(@RequestBody StockRequest r) { return Map.of("stocks", service.currentStocks(r.docDate, r.rows)); }

    @GetMapping("/loader/combos")
    public Map<String, Object> loaderCombos() { return service.loaderCombos(); }

    @GetMapping("/loader/pending")
    public Map<String, Object> loaderPending(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int itemId) {
        return service.loaderPending(fromDate, toDate, customerId, itemId);
    }

    @GetMapping("/loader/detail")
    public Map<String, Object> loaderDetail(@RequestParam(required = false) String ids, @RequestParam(required = false) String orderDetailIds) {
        return service.loaderDetail(ids, orderDetailIds);
    }

    @GetMapping("/loader/rows")
    public Map<String, Object> loaderRows(@RequestParam String detailIds) { return service.loadRows(detailIds); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo) {
        return service.history(dateType, fromDate, toDate, fromNo, toNo);
    }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @GetMapping("/{id}/lines")
    public List<Map<String, Object>> lines(@PathVariable int id) { return service.lines(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleDeliveryOrderStService.Request request) { return service.save(request); }

    public static class VehicleTypeRequest { public int id; public String description; }

    @PostMapping("/vehicle-type")
    public Map<String, Object> vehicleType(@RequestBody VehicleTypeRequest r) { return service.saveVehicleType(r.id, r.description); }

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
