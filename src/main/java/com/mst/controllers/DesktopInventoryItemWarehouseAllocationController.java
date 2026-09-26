package com.mst.controllers;
import com.mst.models.dto.InventoryItemWarehouseAllocationRequest;
import com.mst.services.DesktopInventoryItemWarehouseAllocationService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
@Controller
public class DesktopInventoryItemWarehouseAllocationController {
 private final DesktopInventoryItemWarehouseAllocationService service;
 public DesktopInventoryItemWarehouseAllocationController(DesktopInventoryItemWarehouseAllocationService service){this.service=service;}
 @GetMapping("/inventory/item-warehouse-allocations") public String page(){return "inventory/item_warehouse_allocations";}
 @GetMapping("/api/inventory/item-warehouse-allocations/lookups") @ResponseBody public Map<String,Object> lookups(){return service.lookups();}
 @GetMapping("/api/inventory/item-warehouse-allocations/rows") @ResponseBody public Map<String,Object> rows(@RequestParam(defaultValue="0") int category,@RequestParam(defaultValue="0") int type){return service.rows(category,type);}
 @PostMapping("/api/inventory/item-warehouse-allocations/save") @ResponseBody public Map<String,Object> save(@RequestBody InventoryItemWarehouseAllocationRequest request){return service.save(request);}
 @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<Map<String,String>> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
 @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<Map<String,String>> forbidden(org.springframework.security.access.AccessDeniedException ex){return error(403,ex.getMessage());}
 @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<Map<String,String>> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Unable to save"));}
 private ResponseEntity<Map<String,String>> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
