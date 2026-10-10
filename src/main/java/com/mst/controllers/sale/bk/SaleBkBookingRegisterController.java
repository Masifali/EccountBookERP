package com.mst.controllers.sale.bk;

import com.mst.services.sale.bk.SaleBkBookingRegisterService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

/** Screen 765 BookingReport (Architecture.WinApp.PreBookingAndDelivery.PreBookingOrderRegister), module 2042. */
@Controller
public class SaleBkBookingRegisterController {
    private final SaleBkBookingRegisterService service;

    public SaleBkBookingRegisterController(SaleBkBookingRegisterService service) { this.service = service; }

    @GetMapping("/sale/reports/booking-report")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Booking Report");
        return "sale/bk/booking_report";
    }

    @GetMapping("/api/sale/bk/booking-report/lookups")
    @ResponseBody
    public Map<String, Object> lookups() { return service.lookups(); }

    @GetMapping("/api/sale/bk/booking-report/combos")
    @ResponseBody
    public Map<String, Object> combos(@RequestParam(value = "costCenterId", defaultValue = "0") int costCenterId) { return service.combos(costCenterId); }

    @GetMapping("/api/sale/bk/booking-report/rows")
    @ResponseBody
    public List<Map<String, Object>> rows(@RequestParam Map<String, String> q) { return service.show(q); }

    @ExceptionHandler(IllegalArgumentException.class)
    @ResponseBody
    public ResponseEntity<Map<String, String>> invalid(IllegalArgumentException e) {
        return ResponseEntity.badRequest().body(Map.of("message", String.valueOf(e.getMessage())));
    }
}
