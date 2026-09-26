package com.mst.controllers.sale;

import com.mst.models.dto.SaleDeliveryOrderRequest;
import com.mst.services.SaleDeliveryOrderService;
import java.util.Map;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/sale/delivery-order/api")
public class SaleDeliveryOrderController {
    private final SaleDeliveryOrderService service;

    public SaleDeliveryOrderController(SaleDeliveryOrderService service) { this.service = service; }

    @GetMapping("/initial")
    public Map<String,Object> initial() { return service.initial(); }

    @GetMapping("/sale-order/{id}/lines")
    public Object saleOrderLines(@PathVariable int id) { return service.orderLines(id); }

    @GetMapping("/history")
    public Object history() { return service.history(); }

    @GetMapping("/{id}")
    public Object record(@PathVariable int id) { return service.record(id); }

    @PostMapping
    public Map<String,Object> save(@RequestBody SaleDeliveryOrderRequest request) { return service.save(request); }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> delete(@PathVariable int id) {
        service.delete(id);
        return ResponseEntity.ok(Map.of("success", true, "message", "Record deleted successfully"));
    }
}
