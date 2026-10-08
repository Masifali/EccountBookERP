package com.mst.controllers.sale.engr;

import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleGdnEngrService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 539 GoodsDispatchNotes_Engr - /sale/engr/gdn (page: SaleEngrViewController). */
@RestController
@RequestMapping("/sale/engr/gdn/api")
public class SaleGdnEngrController extends SaleEngrControllerBase {
    private final SaleGdnEngrService service;

    public SaleGdnEngrController(SaleGdnEngrService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/reset-data")
    public Map<String, Object> resetData() { return service.resetData(); }

    @GetMapping("/outstanding")
    public List<Map<String, Object>> outstanding(@RequestParam(defaultValue = "0") int recId) { return service.outstanding(recId); }

    @GetMapping("/uoms")
    public List<Map<String, Object>> uoms(@RequestParam int itemId) { return service.uoms(itemId); }

    @GetMapping("/do-data")
    public List<Map<String, Object>> doData(@RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int deliveryOrderId,
                                            @RequestParam(defaultValue = "0") int deliveryTypeId) {
        return service.deliveryOrderData(customerId, deliveryOrderId, deliveryTypeId);
    }

    @GetMapping("/loader/combos")
    public Map<String, Object> loaderCombos() {
        Map<String, Object> m = new java.util.LinkedHashMap<>(service.loaderCombos());
        m.put("fromDate", service.fyStart());
        return m;
    }

    @GetMapping("/loader/rows")
    public List<Map<String, Object>> loaderRows(@RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int deliveryTypeId,
                                                @RequestParam(defaultValue = "0") int itemId) {
        return service.loaderRows(customerId, deliveryTypeId, itemId);
    }

    @GetMapping("/history-combos")
    public Map<String, Object> historyCombos() { return service.historyCombos(); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int requestedById,
                                             @RequestParam(defaultValue = "all") String referred) {
        return service.history(dateType, fromDate, toDate, fromNo, toNo, customerId, requestedById, referred);
    }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @GetMapping("/{id}/details")
    public List<Map<String, Object>> details(@PathVariable int id) { return service.detailOf(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleGdnEngrService.Request request) { return service.save(request); }

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
