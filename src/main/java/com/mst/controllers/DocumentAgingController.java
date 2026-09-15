package com.mst.controllers;

import com.mst.services.DocumentAgingService;
import java.time.LocalDate;
import java.util.*;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/accounts/document-aging")
public class DocumentAgingController {
    private final DocumentAgingService service;
    public DocumentAgingController(DocumentAgingService service) { this.service=service; }
    @GetMapping("/supplier/lookups")
    public Map<String,Object> lookups() { return service.supplierLookups(); }
    @GetMapping("/customer/lookups")
    public Map<String,Object> customerLookups() { return service.customerLookups(); }
    @GetMapping("/customer")
    public List<Map<String,Object>> customer(@RequestParam @DateTimeFormat(iso=DateTimeFormat.ISO.DATE) LocalDate asOnDate,
            @RequestParam(defaultValue="30") int agingDays,@RequestParam(defaultValue="0") int accountId,
            @RequestParam(defaultValue="0") int partyGroupId,@RequestParam(defaultValue="0") int customGroupId) {
        return service.customer(asOnDate,agingDays,accountId,partyGroupId,customGroupId);
    }
    @GetMapping("/supplier")
    public List<Map<String,Object>> supplier(@RequestParam @DateTimeFormat(iso=DateTimeFormat.ISO.DATE) LocalDate asOnDate,
            @RequestParam(defaultValue="30") int agingDays,@RequestParam(defaultValue="0") int accountId,
            @RequestParam(defaultValue="0") int partyGroupId,@RequestParam(defaultValue="0") int customGroupId) {
        return service.supplier(asOnDate,agingDays,accountId,partyGroupId,customGroupId);
    }
}
