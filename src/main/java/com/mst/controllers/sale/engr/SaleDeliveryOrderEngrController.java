package com.mst.controllers.sale.engr;

import com.mst.services.sale.engr.SaleDeliveryOrderEngrService;
import com.mst.services.sale.engr.SaleEngrAttachments;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** Screen 538 DeliveryOrder_Engr - /sale/engr/delivery-order (page: SaleEngrViewController). */
@RestController
@RequestMapping("/sale/engr/delivery-order/api")
public class SaleDeliveryOrderEngrController extends SaleEngrControllerBase {
    private final SaleDeliveryOrderEngrService service;

    public SaleDeliveryOrderEngrController(SaleDeliveryOrderEngrService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    /** DocumentNoFill + GetOutStandingOrderAndParties + SupplierNameFill / ItemDetailFillWithoutOrderId (Refresh and Reset). */
    @GetMapping("/refresh")
    public Map<String, Object> refresh(@RequestParam(defaultValue = "0") int recId) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("customers", service.customers());
        m.put("items", service.itemsWithoutOrder());
        m.put("outstanding", service.outstanding(recId));
        m.put("lookups", service.lookups());
        m.put("warehouses", service.warehouses());
        m.put("jobLots", service.jobLots());
        m.put("assets", service.assets());
        return m;
    }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/outstanding")
    public List<Map<String, Object>> outstanding(@RequestParam(defaultValue = "0") int recId) { return service.outstanding(recId); }

    @GetMapping("/uoms")
    public List<Map<String, Object>> uoms(@RequestParam int itemId) { return service.uoms(itemId); }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos() { return service.historyCombos(); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int deliveryTypeId, @RequestParam(defaultValue = "0") int customerId,
                                             @RequestParam(defaultValue = "0") int requestedById, @RequestParam(defaultValue = "0") int approvedById) {
        return service.history(dateType, fromDate, toDate, fromNo, toNo, deliveryTypeId, customerId, requestedById, approvedById);
    }

    @GetMapping("/loader/combos")
    public Map<String, Object> loaderCombos() {
        Map<String, Object> m = new LinkedHashMap<>(service.loaderCombos());
        m.put("fromDate", service.fyStart());
        return m;
    }

    @GetMapping("/loader/orders")
    public List<Map<String, Object>> loaderOrders(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                                  @RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int itemId) {
        return service.loaderOrders(fromDate, toDate, customerId, itemId);
    }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleDeliveryOrderEngrService.Request request) { return service.save(request); }

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
