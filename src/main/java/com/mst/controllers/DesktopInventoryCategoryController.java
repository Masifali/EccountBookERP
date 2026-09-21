package com.mst.controllers;
import com.mst.services.DesktopInventoryCategoryService;
import com.mst.models.dto.InventoryCategoryRequest;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import org.springframework.http.*;
@Controller
public class DesktopInventoryCategoryController {
 private final DesktopInventoryCategoryService service;
 public DesktopInventoryCategoryController(DesktopInventoryCategoryService service){this.service=service;}
 @GetMapping({"/inventory/item_categories","/inventory/item-categories"}) public String page(){return "inventory/item_categories";}
 @GetMapping("/inventory/item_categories/edit/{id}") public String edit(@PathVariable int id){service.record(id);return "redirect:/inventory/item_categories?id="+id;}
 @GetMapping(value="/api/inventory/definition-categories/lookups",produces="application/json") @ResponseBody public Map<String,Object> lookups(){return service.lookups();}
 @GetMapping(value="/api/inventory/definition-categories/translations",produces="application/json") @ResponseBody public Map<String,Object> translations(){return service.translations();}
 @PostMapping(value="/api/inventory/definition-categories/translations",produces="application/json") @ResponseBody public Map<String,Object> translate(@RequestBody InventoryCategoryRequest.Translation r){return service.saveTranslation(r);}
 @GetMapping(value="/api/inventory/definition-categories/history",produces="application/json") @ResponseBody public List<Map<String,Object>> history(){return service.history();}
 @GetMapping(value="/api/inventory/definition-categories/{id:[0-9]+}",produces="application/json") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
 @GetMapping(value="/api/inventory/definition-categories/code/{parent:[0-9]+}",produces="application/json") @ResponseBody public Map<String,Object> code(@PathVariable int parent){return Map.of("code",service.code(parent));}
 @PostMapping(value={"/api/inventory/definition-categories/save","/api/inventory/categories/save"},produces="application/json") @ResponseBody public Map<String,Object> save(@RequestBody InventoryCategoryRequest r){return service.save(r);}
 @GetMapping(value="/api/inventory/categories/list",produces="application/json") @ResponseBody public List<Map<String,Object>> legacyList(){return service.history().stream().map(r->{Map<String,Object> row=new LinkedHashMap<>();r.forEach((k,v)->row.put(Character.toLowerCase(k.charAt(0))+k.substring(1),v));return row;}).toList();}
 @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<?> invalid(IllegalArgumentException ex){return ResponseEntity.badRequest().contentType(MediaType.APPLICATION_JSON).body(Map.of("message",ex.getMessage()));}
 @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<?> database(org.springframework.dao.DataAccessException ex){return ResponseEntity.status(409).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",Objects.toString(ex.getMostSpecificCause().getMessage(),"Category save failed")));}
 @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<?> denied(Exception ex){return ResponseEntity.status(403).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",ex.getMessage()));}
}
