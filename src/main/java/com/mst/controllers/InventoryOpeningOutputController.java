package com.mst.controllers;
import com.mst.services.InventoryOpeningOutputService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;
@RestController
@RequestMapping("/api/inventory/opening-stock")
public class InventoryOpeningOutputController {
 private final InventoryOpeningOutputService service;
 public InventoryOpeningOutputController(InventoryOpeningOutputService service){this.service=service;}
 @PostMapping("/grid-export") public ResponseEntity<byte[]> export(@RequestBody InventoryOpeningOutputService.Request r){return ResponseEntity.ok().contentType(MediaType.parseMediaType("application/vnd.ms-excel")).header("Content-Disposition","attachment; filename=Opening-Stock-History.xls").body(service.export(r));}
 @PostMapping("/grid-print") public Map<String,Boolean> print(){service.printRight();return Map.of("authorized",true);}
 @ExceptionHandler(IllegalArgumentException.class) public ResponseEntity<?> invalid(Exception ex){return ResponseEntity.badRequest().body(Map.of("message",ex.getMessage()));}
 @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) public ResponseEntity<?> denied(Exception ex){return ResponseEntity.status(403).body(Map.of("message",ex.getMessage()));}
 @ExceptionHandler({IllegalStateException.class,org.springframework.dao.DataAccessException.class}) public ResponseEntity<?> failure(Exception ex){return ResponseEntity.status(409).body(Map.of("message","History output could not be generated. Reload the history and retry."));}
}
