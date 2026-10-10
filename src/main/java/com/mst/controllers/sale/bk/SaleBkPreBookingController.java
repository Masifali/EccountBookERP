package com.mst.controllers.sale.bk;

import com.mst.services.sale.bk.SaleBkPreBookingService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

/** Screen 761 PreBookingOrder (Architecture.WinApp.PreBookingAndDelivery.PreBookingOrder), module 132 Booking Office / Customer Portal. */
@Controller
public class SaleBkPreBookingController {
    private final SaleBkPreBookingService service;

    public SaleBkPreBookingController(SaleBkPreBookingService service) { this.service = service; }

    @GetMapping("/sale/booking/pre-booking-order")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Pre Booking Order");
        return "sale/bk/pre_booking_order";
    }

    @GetMapping("/api/sale/bk/pre-booking/init")
    @ResponseBody
    public Map<String, Object> init() { return service.init(); }

    @GetMapping("/api/sale/bk/pre-booking/doc-no")
    @ResponseBody
    public Map<String, Object> docNo() { return Map.of("docNo", service.docNo()); }

    @GetMapping("/api/sale/bk/pre-booking/customers")
    @ResponseBody
    public List<Map<String, Object>> customers(@RequestParam(value = "costCenterId", defaultValue = "0") int costCenterId) { return service.customers(costCenterId); }

    @GetMapping("/api/sale/bk/pre-booking/refresh")
    @ResponseBody
    public Map<String, Object> refresh(@RequestParam(value = "costCenterId", defaultValue = "0") int costCenterId) {
        return Map.of("customers", service.customers(costCenterId), "paymentTerms", service.paymentTerms(), "items", service.items());
    }

    @GetMapping("/api/sale/bk/pre-booking/history-customers")
    @ResponseBody
    public List<Map<String, Object>> historyCustomers(@RequestParam(value = "costCenterId", defaultValue = "0") int costCenterId) { return service.historyCustomers(costCenterId); }

    @GetMapping("/api/sale/bk/pre-booking/item")
    @ResponseBody
    public Map<String, Object> item(@RequestParam("itemId") int itemId, @RequestParam("docDate") String docDate) {
        return Map.of("uoms", service.uoms(itemId), "cropYears", service.cropYears(itemId, docDate));
    }

    @GetMapping("/api/sale/bk/pre-booking/rate")
    @ResponseBody
    public Map<String, Object> rate(@RequestParam("itemId") int itemId, @RequestParam("cropYearId") int cropYearId, @RequestParam("docDate") String docDate) {
        return service.rate(itemId, cropYearId, docDate);
    }

    @GetMapping("/api/sale/bk/pre-booking/history")
    @ResponseBody
    public List<Map<String, Object>> history(@RequestParam Map<String, String> q) { return service.history(q); }

    @GetMapping("/api/sale/bk/pre-booking/get")
    @ResponseBody
    public Map<String, Object> get(@RequestParam("id") int id) { return service.get(id); }

    @PostMapping("/api/sale/bk/pre-booking/save")
    @ResponseBody
    public Map<String, Object> save(@RequestBody Map<String, Object> body) { return service.save(body); }

    @ExceptionHandler(IllegalArgumentException.class)
    @ResponseBody
    public ResponseEntity<Map<String, String>> invalid(IllegalArgumentException e) {
        return ResponseEntity.badRequest().body(Map.of("message", String.valueOf(e.getMessage())));
    }
}
