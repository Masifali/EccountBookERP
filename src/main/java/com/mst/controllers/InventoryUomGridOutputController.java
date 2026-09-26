package com.mst.controllers;
import com.mst.services.InventoryUomGridOutputService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;
@RestController
@RequestMapping("/api/inventory/uom-schedules")
public class InventoryUomGridOutputController {
 private final InventoryUomGridOutputService service;
 public InventoryUomGridOutputController(InventoryUomGridOutputService service){this.service=service;}
 @PostMapping("/grid-export") public ResponseEntity<byte[]> export(@RequestBody InventoryUomGridOutputService.Request r){return ResponseEntity.ok().contentType(MediaType.parseMediaType("application/vnd.ms-excel")).header("Content-Disposition","attachment; filename=Item-UOM-Schedule.xls").body(service.export(r));}
 @PostMapping("/grid-print") public ResponseEntity<byte[]> print(@RequestBody InventoryUomGridOutputService.Request r){return ResponseEntity.ok().contentType(MediaType.APPLICATION_PDF).header("Content-Disposition","inline; filename=Item-UOM-Schedule.pdf").body(service.print(r));}
 @ExceptionHandler(IllegalArgumentException.class) public ResponseEntity<?> invalid(Exception ex){return ResponseEntity.badRequest().body(Map.of("message",ex.getMessage()));}
 @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) public ResponseEntity<?> denied(Exception ex){return ResponseEntity.status(403).body(Map.of("message",ex.getMessage()));}
 @ExceptionHandler({IllegalStateException.class,org.springframework.dao.DataAccessException.class}) public ResponseEntity<?> failure(Exception ex){return ResponseEntity.status(409).body(Map.of("message","History output could not be generated. Reload and retry."));}
}
