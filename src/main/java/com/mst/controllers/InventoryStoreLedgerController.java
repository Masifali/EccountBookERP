package com.mst.controllers;
import com.mst.models.dto.InventoryStoreLedgerRequest;
import com.mst.services.InventoryStoreLedgerService;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import org.springframework.http.*;
@Controller
public class InventoryStoreLedgerController {
 private final InventoryStoreLedgerService service;
 public InventoryStoreLedgerController(InventoryStoreLedgerService service){this.service=service;}
 @GetMapping("/stocks/store-item-ledger") public String page(){return "stocks/store_item_ledger";}
 @PostMapping("/api/inventory/store-item-ledger") @ResponseBody public List<Map<String,Object>> load(@RequestBody InventoryStoreLedgerRequest r){return service.load(r);}
 @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<?> invalid(Exception ex){return ResponseEntity.badRequest().body(Map.of("message",ex.getMessage()));}
 @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<?> denied(Exception ex){return ResponseEntity.status(403).body(Map.of("message",ex.getMessage()));}
 @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<?> database(org.springframework.dao.DataAccessException ex){return ResponseEntity.status(409).body(Map.of("message",Objects.toString(ex.getMostSpecificCause().getMessage(),"The Store Item Ledger query failed")));}
}
