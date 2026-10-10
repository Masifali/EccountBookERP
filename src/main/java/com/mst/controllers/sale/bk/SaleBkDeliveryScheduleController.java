package com.mst.controllers.sale.bk;

import com.mst.services.sale.bk.SaleBkDeliveryScheduleService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

/** Screen 865 DeliveryScheduleCustomer (Architecture.WinApp.Sale.frmDeliveryScheduleCustomer), module 132 Booking Office / Customer Portal. */
@Controller
public class SaleBkDeliveryScheduleController {
    private final SaleBkDeliveryScheduleService service;

    public SaleBkDeliveryScheduleController(SaleBkDeliveryScheduleService service) { this.service = service; }

    @GetMapping("/sale/delivery-schedule-customer")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Delivery Schedule");
        return "sale/bk/delivery_schedule_customer";
    }

    @GetMapping("/api/sale/bk/delivery-schedule-customer/lookups")
    @ResponseBody
    public Map<String, Object> lookups() { return service.lookups(); }

    @GetMapping("/api/sale/bk/delivery-schedule-customer/rows")
    @ResponseBody
    public List<Map<String, Object>> rows(@RequestParam Map<String, String> q) { return service.rows(q); }

    @ExceptionHandler(IllegalArgumentException.class)
    @ResponseBody
    public ResponseEntity<Map<String, String>> invalid(IllegalArgumentException e) {
        return ResponseEntity.badRequest().body(Map.of("message", String.valueOf(e.getMessage())));
    }
}
