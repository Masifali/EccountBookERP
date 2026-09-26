package com.mst.controllers;

import com.mst.models.dto.InventoryTransactionsRequest;
import com.mst.services.InventoryTransactionsService;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class InventoryTransactionsController {
    private final InventoryTransactionsService service;
    public InventoryTransactionsController(InventoryTransactionsService service) { this.service=service; }
    @GetMapping({"/inventory/stock-transactions","/inventory/stock_transactions"})
    public String page() { return "inventory/stock_transactions"; }
    @GetMapping("/inventory/api/reports/stock-transactions/lookups") @ResponseBody
    public Map<String,Object> lookups() { return service.lookups(); }
    @PostMapping("/inventory/api/reports/stock-transactions") @ResponseBody
    public List<Map<String,Object>> load(@RequestBody InventoryTransactionsRequest request) { return service.load(request); }
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody
    public org.springframework.http.ResponseEntity<?> denied(Exception ex){return org.springframework.http.ResponseEntity.status(403).body(Map.of("message",ex.getMessage()));}
    @ExceptionHandler(org.springframework.web.server.ResponseStatusException.class) @ResponseBody
    public org.springframework.http.ResponseEntity<?> invalid(org.springframework.web.server.ResponseStatusException ex){return org.springframework.http.ResponseEntity.status(ex.getStatus()).body(Map.of("message",Objects.toString(ex.getReason(),"Invalid report request")));}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody
    public org.springframework.http.ResponseEntity<?> database(Exception ex){return org.springframework.http.ResponseEntity.status(409).body(Map.of("message","The Inventory Stock Transactions report query failed. Please retry."));}
}
