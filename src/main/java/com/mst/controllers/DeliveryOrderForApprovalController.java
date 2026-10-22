package com.mst.controllers;

import com.mst.services.DeliveryOrderForApprovalService;
import com.mst.services.DeliveryOrderForApprovalService.*;
import java.time.LocalDate;
import java.util.Map;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class DeliveryOrderForApprovalController {
    private final DeliveryOrderForApprovalService service;
    public DeliveryOrderForApprovalController(DeliveryOrderForApprovalService service){this.service=service;}
    @GetMapping("/dashboard/delivery-order-for-approval")
    public String page(){return "dashboard/delivery_order_for_approval";}
    @GetMapping("/api/dashboard/delivery-order-approval/history") @ResponseBody
    public Map<String,Object> history(@RequestParam Map<String,String> p){
        return service.history(new Filter(date(p.get("fromDate")),date(p.get("toDate")),p.getOrDefault("status","Not Approved")));
    }
    @PostMapping("/api/dashboard/delivery-order-approval/approve") @ResponseBody
    public Map<String,Object> approve(@RequestBody Approval request){return service.approve(request);}
    private static LocalDate date(String value){return value==null||value.isBlank()?null:LocalDate.parse(value);}
    @ExceptionHandler({IllegalArgumentException.class,AccessDeniedException.class}) @ResponseBody
    public ResponseEntity<Map<String,String>> error(RuntimeException e){
        return ResponseEntity.status(e instanceof AccessDeniedException?403:400).body(Map.of("message",e.getMessage()));
    }
}
