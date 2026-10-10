package com.mst.controllers.sale.bk;

import com.mst.services.sale.bk.SaleBkOrderStatusService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

/** Screen 762 OrderDashboard (Architecture.WinApp.PreBookingAndDelivery.MyOrdersStatus), module 2042. */
@Controller
public class SaleBkOrderStatusController {
    private final SaleBkOrderStatusService service;

    public SaleBkOrderStatusController(SaleBkOrderStatusService service) { this.service = service; }

    @GetMapping("/sale/reports/order-dashboard")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Order Dashboard");
        return "sale/bk/order_status";
    }

    @GetMapping("/api/sale/bk/order-status/init")
    @ResponseBody
    public Map<String, Object> init() { return service.init(); }

    @GetMapping("/api/sale/bk/order-status/cards")
    @ResponseBody
    public List<Map<String, Object>> cards(@RequestParam(value = "costCenterId", defaultValue = "0") int costCenterId) { return service.cards(costCenterId); }

    @GetMapping("/api/sale/bk/order-status/booking")
    @ResponseBody
    public Map<String, Object> booking(@RequestParam Map<String, String> q) { return service.detail(true, q); }

    @GetMapping("/api/sale/bk/order-status/pending")
    @ResponseBody
    public Map<String, Object> pending(@RequestParam Map<String, String> q) { return service.detail(false, q); }

    @GetMapping("/api/sale/bk/order-status/transit")
    @ResponseBody
    public List<Map<String, Object>> transit(@RequestParam Map<String, String> q) { return service.transit(q); }

    @GetMapping("/api/sale/bk/order-status/bills")
    @ResponseBody
    public List<Map<String, Object>> bills(@RequestParam Map<String, String> q) { return service.bills(q); }

    @PostMapping("/api/sale/bk/order-status/confirm")
    @ResponseBody
    public Map<String, Object> confirm(@RequestBody Map<String, Object> body) {
        boolean bill = "bill".equals(String.valueOf(body.get("kind")));
        int id = body.get("id") == null ? 0 : Integer.parseInt(String.valueOf(body.get("id")));
        String remarks = body.get("remarks") == null ? "" : String.valueOf(body.get("remarks"));
        return Map.of("message", service.confirm(bill, id, remarks));
    }

    @ExceptionHandler(IllegalArgumentException.class)
    @ResponseBody
    public ResponseEntity<Map<String, String>> invalid(IllegalArgumentException e) {
        return ResponseEntity.badRequest().body(Map.of("message", String.valueOf(e.getMessage())));
    }
}
