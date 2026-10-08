package com.mst.controllers.sale.engr;

import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleOutwardGatePassEngrService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 534 OutwardGatePassTrading - /sale/engr/outward-gate-pass (page: SaleEngrViewController). */
@RestController
@RequestMapping("/sale/engr/outward-gate-pass/api")
public class SaleOutwardGatePassEngrController extends SaleEngrControllerBase {
    private final SaleOutwardGatePassEngrService service;

    public SaleOutwardGatePassEngrController(SaleOutwardGatePassEngrService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/type-code")
    public Map<String, Object> typeCode(@RequestParam String type) { return service.typeCode(type); }

    @GetMapping("/customers")
    public List<Map<String, Object>> customers(@RequestParam(defaultValue = "0") int orderTypeId) { return service.customersFor(orderTypeId); }

    @GetMapping("/order")
    public Map<String, Object> order(@RequestParam int orderTypeId, @RequestParam(defaultValue = "") String orderNo,
                                     @RequestParam(required = false) String gpDate, @RequestParam(defaultValue = "") String gatePassType) {
        return service.orderLeave(orderTypeId, orderNo, gpDate, gatePassType);
    }

    @GetMapping("/open")
    public List<Map<String, Object>> open() { return service.openRecords(); }

    @GetMapping("/do-rows")
    public List<Map<String, Object>> doRows() { return service.deliveryOrderRows(); }

    @GetMapping("/so-rows")
    public List<Map<String, Object>> soRows() { return service.saleOrderRows(); }

    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(defaultValue = "document") String dateType,
                                             @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                             @RequestParam(defaultValue = "0") int fromNo, @RequestParam(defaultValue = "0") int toNo,
                                             @RequestParam(defaultValue = "0") int customerId) {
        return service.history(dateType, fromDate, toDate, fromNo, toNo, customerId);
    }

    @GetMapping("/{id}")
    public Map<String, Object> record(@PathVariable int id) { return service.record(id); }

    @GetMapping("/{id}/wb")
    public Map<String, Object> wb(@PathVariable int id) { return service.wbInfo(id); }

    @PostMapping
    public Map<String, Object> save(@RequestBody SaleOutwardGatePassEngrService.Request request) { return service.save(request); }

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
