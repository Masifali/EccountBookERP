package com.mst.controllers;
import com.mst.models.dto.InventoryStoreStockRequest;
import com.mst.services.InventoryStoreStockService;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import org.springframework.http.*;
@Controller
public class InventoryStoreStockController {
 private final InventoryStoreStockService service;
 public InventoryStoreStockController(InventoryStoreStockService service){this.service=service;}
 @GetMapping("/stocks/store-stock-report") public String page(){return "stocks/store_stock_report";}
 @GetMapping("/api/inventory/store-stock/lookups") @ResponseBody public Map<String,Object> lookups(){return service.lookups();}
 @GetMapping("/api/inventory/store-stock/choices") @ResponseBody public List<Map<String,Object>> choices(@RequestParam List<Integer> branches){return service.choices(branches);}
 @PostMapping("/api/inventory/store-stock") @ResponseBody public Map<String,Object> load(@RequestBody InventoryStoreStockRequest r){return service.load(r);}
 @PostMapping("/api/inventory/store-stock/update") @ResponseBody public Map<String,Object> update(@RequestBody InventoryStoreStockRequest r){return service.update(r);}
 @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<?> invalid(Exception ex){return ResponseEntity.badRequest().body(Map.of("message",ex.getMessage()));}
 @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<?> denied(Exception ex){return ResponseEntity.status(403).body(Map.of("message",ex.getMessage()));}
 @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<?> database(org.springframework.dao.DataAccessException ex){return ResponseEntity.status(409).body(Map.of("message",Objects.toString(ex.getMostSpecificCause().getMessage(),"The Store Stock query failed")));}
}
