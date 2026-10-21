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
    public Object saleOrderLines(@PathVariable int id, @RequestParam(defaultValue="0") int saleTypeId) { return service.orderLines(id, saleTypeId); }

    @GetMapping("/orders")
    public Object orders(@RequestParam int customerId, @RequestParam(defaultValue="0") int recId) { return service.ordersOfCustomer(customerId, recId); }

    @GetMapping("/order-items")
    public Object orderItems(@RequestParam int orderId) { return service.orderItems(orderId); }

    @GetMapping("/order-balance")
    public Object orderBalance(@RequestParam int orderId, @RequestParam int detailId, @RequestParam int itemId) { return Map.of("balWeight", service.orderBalanceWeight(orderId, detailId, itemId)); }

    @GetMapping("/order-expenses")
    public Object orderExpenses(@RequestParam String ids) { return service.orderExpenses(ids); }


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
