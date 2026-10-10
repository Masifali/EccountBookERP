package com.mst.controllers.sale.steel;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.steel.SaleSaleOrderStService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 542 SaleOrder_St - /sale/steel/sale-order (page: SaleSteelViewController). */
@RestController
@RequestMapping("/sale/steel/sale-order/api")
public class SaleSaleOrderStController extends SaleEngrControllerBase {
    private final SaleSaleOrderStService service;

    public SaleSaleOrderStController(SaleSaleOrderStService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() {
        Map<String, Object> m = service.initial();
        m.putAll(service.statics());
        return m;
    }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.lists(); }

    @GetMapping("/next-no")
    public Map<String, Object> nextNo() { return Map.of("nextNo", service.nextNo()); }

    @GetMapping("/category-no")
    public Map<String, Object> categoryNo(@RequestParam int categoryId) { return Map.of("catSrNo", service.categoryNo(categoryId)); }

    @GetMapping("/uoms")
    public List<Map<String, Object>> uoms(@RequestParam int itemId) { return service.uoms(itemId); }

    @GetMapping("/limit")
    public Map<String, Object> limit(@RequestParam int customerId, @RequestParam double amount) { return Map.of("availableLimit", service.limit(customerId, amount)); }

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

    @GetMapping("/{id}/lines")
    public List<Map<String, Object>> lines(@PathVariable int id) { return service.lines(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleSaleOrderStService.Request request) { return service.save(request); }

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
