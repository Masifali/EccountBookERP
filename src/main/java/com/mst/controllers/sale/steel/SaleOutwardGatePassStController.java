package com.mst.controllers.sale.steel;

import com.mst.controllers.sale.engr.SaleEngrControllerBase;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.steel.SaleOutwardGatePassStService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;

/** Screen 545 OutwardGatePass_St - /sale/steel/outward-gate-pass (page: SaleSteelViewController). */
@RestController
@RequestMapping("/sale/steel/outward-gate-pass/api")
public class SaleOutwardGatePassStController extends SaleEngrControllerBase {
    private final SaleOutwardGatePassStService service;

    public SaleOutwardGatePassStController(SaleOutwardGatePassStService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String, Object> initial() { return service.initial(); }

    @GetMapping("/refresh")
    public Map<String, Object> refresh() { return service.refresh(); }

    @GetMapping("/reset")
    public Map<String, Object> reset() { return service.resetData(); }

    @GetMapping("/items")
    public List<Map<String, Object>> items() { return service.items(); }

    @GetMapping("/customers")
    public List<Map<String, Object>> customers() { return service.customers(); }

    @GetMapping("/order")
    public Map<String, Object> order(@RequestParam String basedOn, @RequestParam(defaultValue = "") String orderNo, @RequestParam(required = false) String gpDate) {
        return service.orderLeave(basedOn, orderNo, gpDate);
    }

    @GetMapping("/open")
    public Map<String, Object> open() { return service.openRecords(); }

    @GetMapping("/history")
    public Map<String, Object> history(@RequestParam(defaultValue = "document") String dateType,
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
    public Map<String, Object> save(@RequestBody SaleOutwardGatePassStService.Request request) { return service.save(request); }

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
