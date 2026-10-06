package com.mst.controllers;

import com.mst.models.dto.InventoryTransactionsWithValueRequest;
import com.mst.services.InventoryTransactionsWithValueService;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

/** Screen 293 "Transaction Report (With Value)" - desktop InventoryEvaluationItemLedger. */
@Controller
public class InventoryTransactionsWithValueController {
    private final InventoryTransactionsWithValueService service;
    public InventoryTransactionsWithValueController(InventoryTransactionsWithValueService service) { this.service = service; }

    @GetMapping({"/inventory/stock-transactions-with-value", "/inventory/stock_transactions_with_value"})
    public String page() { return "inventory/stock_transactions_with_value"; }

    @GetMapping("/inventory/api/reports/stock-transactions-with-value/lookups") @ResponseBody
    public Map<String,Object> lookups() { return service.lookups(); }

    @GetMapping("/inventory/api/reports/stock-transactions-with-value/choices") @ResponseBody
    public List<Map<String,Object>> choices(@RequestParam(name = "branches", required = false) List<Integer> branches) { return service.choices(branches); }

    @PostMapping("/inventory/api/reports/stock-transactions-with-value") @ResponseBody
    public List<Map<String,Object>> load(@RequestBody InventoryTransactionsWithValueRequest request) { return service.load(request); }

    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody
    public org.springframework.http.ResponseEntity<?> denied(Exception ex){return org.springframework.http.ResponseEntity.status(403).body(Map.of("message",Objects.toString(ex.getMessage(),"Access denied")));}
    @ExceptionHandler(org.springframework.web.server.ResponseStatusException.class) @ResponseBody
    public org.springframework.http.ResponseEntity<?> invalid(org.springframework.web.server.ResponseStatusException ex){return org.springframework.http.ResponseEntity.status(ex.getStatus()).body(Map.of("message",Objects.toString(ex.getReason(),"Invalid report request")));}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody
    public org.springframework.http.ResponseEntity<?> database(org.springframework.dao.DataAccessException ex){return org.springframework.http.ResponseEntity.status(409).body(Map.of("message",Objects.toString(ex.getMostSpecificCause().getMessage(),"The Transaction Report (With Value) query failed. Please retry.")));}
}
