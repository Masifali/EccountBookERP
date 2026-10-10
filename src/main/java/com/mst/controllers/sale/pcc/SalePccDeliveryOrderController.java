package com.mst.controllers.sale.pcc;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.pcc.SalePccDeliveryOrderService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 549 DeliveryOrderConcrete - /sale/pcc/delivery-order (page: SalePccViewController). */
@RestController
@RequestMapping("/sale/pcc/delivery-order/api")
public class SalePccDeliveryOrderController extends SaleEngrControllerBase {
    private final SalePccDeliveryOrderService service;

    public SalePccDeliveryOrderController(SalePccDeliveryOrderService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    /** btnRefresh_Click */
    @GetMapping("/lists")
    public Map<String, Object> lists() { return service.lists(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/order-nos")
    public List<Map<String, Object>> orderNos(@RequestParam int customerId) { return service.orderNos(customerId); }

    @GetMapping("/order-items")
    public List<Map<String, Object>> orderItems(@RequestParam int orderId) { return service.orderItems(orderId); }

    @GetMapping("/varients")
    public List<Map<String, Object>> varients(@RequestParam int itemId) { return service.varients(itemId); }

    @GetMapping("/stock")
    public Map<String, Object> stock(@RequestParam int itemId, @RequestParam(required = false) String docDate, @RequestParam(defaultValue = "0") int warehouseId,
                                     @RequestParam(defaultValue = "0") int jobLotId, @RequestParam(defaultValue = "0") int varientId) {
        return service.stock(itemId, docDate, warehouseId, jobLotId, varientId);
    }

    @PostMapping("/stock-lines")
    public List<Double> stockLines(@RequestBody Map<String, Object> body) {
        @SuppressWarnings("unchecked") List<Map<String, Object>> lines = (List<Map<String, Object>>) body.get("lines");
        return service.stockLines(body.get("docDate") == null ? null : String.valueOf(body.get("docDate")), lines);
    }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos() { return service.historyCombos(); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(required = false) String vehicleNo, @RequestParam(defaultValue = "0") int customerId) {
        return service.history(fromDate, toDate, fromNo, toNo, vehicleNo, customerId);
    }

    @GetMapping("/loader/combos")
    public Map<String, Object> loaderCombos() { return service.loaderCombos(); }

    @GetMapping("/loader/orders")
    public List<Map<String, Object>> loaderOrders(@RequestParam Map<String, String> q) { return service.loaderOrders(q); }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SalePccDeliveryOrderService.Request request) { return service.save(request); }

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
