package com.mst.controllers.sale.pcc;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.pcc.SalePccSaleOrderService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 548 SaleOrderConcrete - /sale/pcc/sale-order (page: SalePccP2ViewController). */
@RestController
@RequestMapping("/sale/pcc/sale-order/api")
public class SalePccSaleOrderController extends SaleEngrControllerBase {
    private final SalePccSaleOrderService service;

    public SalePccSaleOrderController(SalePccSaleOrderService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/lists")
    public Map<String, Object> lists(@RequestParam(required = false) String docDate) { return service.lists(docDate); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/items")
    public List<Map<String, Object>> items(@RequestParam(required = false) String docDate) { return service.items(docDate); }

    @GetMapping("/varients")
    public List<Map<String, Object>> varients(@RequestParam int itemId) { return service.varients(itemId); }

    @GetMapping("/ref-salesmen")
    public List<Map<String, Object>> refSalesMen(@RequestParam(defaultValue = "0") int agentId) { return service.refSalesMen(agentId); }

    @GetMapping("/supplier-gl")
    public Map<String, Object> supplierGl(@RequestParam int glAccountId) { return Map.of("supplierCustomerId", service.supplierGl(glAccountId)); }

    @GetMapping("/last-rate")
    public List<Map<String, Object>> lastRate(@RequestParam int currencyId) { return service.lastRate(currencyId); }

    @GetMapping("/service-activities")
    public List<Map<String, Object>> serviceActivities(@RequestParam int itemId, @RequestParam(required = false) String date) { return service.serviceActivities(itemId, date); }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos() { return service.historyCombos(); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int customerId, @RequestParam(required = false) String status,
                                             @RequestParam(required = false) String approved) {
        return service.history(fromDate, toDate, customerId, status, approved);
    }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SalePccSaleOrderService.Request request) { return service.save(request); }

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
