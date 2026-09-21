package com.mst.controllers;
import com.mst.models.dto.InventoryItemManufacturerRequest;
import com.mst.services.DesktopInventoryItemManufacturerService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
@Controller
public class DesktopInventoryItemManufacturerController {
 private final DesktopInventoryItemManufacturerService service;
 public DesktopInventoryItemManufacturerController(DesktopInventoryItemManufacturerService service){this.service=service;}
 @GetMapping("/inventory/item-manufacturers") public String page(){return "inventory/item_manufacturers";}
 @GetMapping("/api/inventory/item-manufacturers/lookups") @ResponseBody public Map<String,Object> lookups(){return service.lookups();}
 @GetMapping("/api/inventory/item-manufacturers/history") @ResponseBody public List<Map<String,Object>> history(){return service.history();}
 @GetMapping("/api/inventory/item-manufacturers/{id:[0-9]+}") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
 @PostMapping("/api/inventory/item-manufacturers/save") @ResponseBody public Map<String,Object> save(@RequestBody InventoryItemManufacturerRequest request){return service.save(request);}
 @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<Map<String,String>> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
 @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<Map<String,String>> forbidden(org.springframework.security.access.AccessDeniedException ex){return error(403,ex.getMessage());}
 @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<Map<String,String>> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Unable to save manufacturer allocation"));}
 private ResponseEntity<Map<String,String>> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}

