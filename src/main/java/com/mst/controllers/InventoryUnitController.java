package com.mst.controllers;
import com.mst.models.dto.InventoryUnitRequest;
import com.mst.services.InventoryUnitService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class InventoryUnitController {
    private final InventoryUnitService service;
    public InventoryUnitController(InventoryUnitService service){this.service=service;}
    @GetMapping("/inventory/pos-define-item/units") public String page(){return "inventory/units";}
    @GetMapping(value="/api/inventory/pos-items/units",produces="application/json") @ResponseBody public Map<String,Object> list(){return service.history();}
    @GetMapping(value="/api/inventory/pos-items/units/{id:[0-9]+}",produces="application/json") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @PostMapping(value="/api/inventory/pos-items/units/save",produces="application/json") @ResponseBody public Map<String,Object> save(@RequestBody InventoryUnitRequest r){return service.save(r);}
    @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<?> invalid(IllegalArgumentException e){return ResponseEntity.badRequest().body(Map.of("message",e.getMessage()));}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<?> denied(Exception e){return ResponseEntity.status(403).body(Map.of("message",e.getMessage()));}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<?> database(org.springframework.dao.DataAccessException e){return ResponseEntity.status(409).body(Map.of("message",Objects.toString(e.getMostSpecificCause().getMessage(),"UOM save failed")));}
}
