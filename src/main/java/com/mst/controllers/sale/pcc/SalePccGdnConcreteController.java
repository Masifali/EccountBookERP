package com.mst.controllers.sale.pcc;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.pcc.SalePccGdnConcreteService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 551 GoodsDispatchNotesConcrete - /sale/pcc/goods-dispatch-note (page: SalePccP2ViewController). */
@RestController
@RequestMapping("/sale/pcc/goods-dispatch-note/api")
public class SalePccGdnConcreteController extends SaleEngrControllerBase {
    private final SalePccGdnConcreteService service;

    public SalePccGdnConcreteController(SalePccGdnConcreteService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/items")
    public java.util.List<Map<String, Object>> items(@RequestParam(required = false) String docDate) { return service.items(docDate); }

    @GetMapping("/varients")
    public java.util.List<Map<String, Object>> varients(@RequestParam int itemId) { return service.varients(itemId); }

    @GetMapping("/lists")
    public Map<String, Object> lists() { return service.lists(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/pending-orders")
    public List<Map<String, Object>> pendingOrders() { return service.pendingOrders(); }

    @GetMapping("/pending-customers")
    public List<Map<String, Object>> pendingCustomers(@RequestParam int doId) { return service.pendingCustomers(doId); }

    @GetMapping("/pending-order-nos")
    public List<Map<String, Object>> pendingOrderNos(@RequestParam int doId, @RequestParam int customerId, @RequestParam(defaultValue = "0") int recId) {
        return service.pendingOrdersOf(doId, customerId, recId);
    }

    @GetMapping("/delivery-order-data")
    public Map<String, Object> deliveryOrderData(@RequestParam int customerId, @RequestParam int orderId, @RequestParam int doId, @RequestParam(required = false) String docDate) {
        return service.deliveryOrderData(customerId, orderId, doId, docDate);
    }

    @PostMapping("/stock-lines")
    public List<Double> stockLines(@RequestParam(required = false) String docDate, @RequestBody List<Map<String, Object>> lines) { return service.stockLines(docDate, lines); }

    @GetMapping("/wage-activities")
    public List<Map<String, Object>> wageActivities(@RequestParam int itemId, @RequestParam(required = false) String date, @RequestParam(defaultValue = "0") int contractorId) {
        return service.wageActivities(itemId, date, contractorId);
    }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos() { return service.historyCombos(); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId) {
        return service.history(fromDate, toDate, fromNo, toNo, customerId);
    }

    @GetMapping("/auto-update-ids")
    public List<Integer> autoUpdateIds() { return service.autoUpdateIds(); }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @GetMapping("/{id}/detail")
    public List<Map<String, Object>> detail(@PathVariable int id) { return service.detailOf(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SalePccGdnConcreteService.Request request) { return service.save(request); }

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
