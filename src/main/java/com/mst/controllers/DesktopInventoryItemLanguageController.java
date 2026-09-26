package com.mst.controllers;
import com.mst.models.dto.InventoryItemLanguageRequest;
import com.mst.services.DesktopInventoryItemLanguageService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
@Controller
public class DesktopInventoryItemLanguageController {
 private final DesktopInventoryItemLanguageService service;
 public DesktopInventoryItemLanguageController(DesktopInventoryItemLanguageService service){this.service=service;}
 @GetMapping("/inventory/item-languages") public String page(){return "inventory/item_languages";}
 @GetMapping("/api/inventory/item-languages/lookups") @ResponseBody public Map<String,Object> lookups(){return service.lookups();}
 @GetMapping("/api/inventory/item-languages/history") @ResponseBody public List<Map<String,Object>> history(){return service.history();}
 @GetMapping("/api/inventory/item-languages/{id:[0-9]+}") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
 @PostMapping("/api/inventory/item-languages/save") @ResponseBody public Map<String,Object> save(@RequestBody InventoryItemLanguageRequest request){return service.save(request);}
 @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<Map<String,String>> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
 @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<Map<String,String>> forbidden(org.springframework.security.access.AccessDeniedException ex){return error(403,ex.getMessage());}
 @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<Map<String,String>> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Unable to save translation"));}
 private ResponseEntity<Map<String,String>> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
