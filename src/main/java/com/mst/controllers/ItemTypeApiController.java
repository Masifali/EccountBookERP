package com.mst.controllers;
import com.mst.models.dto.InventoryTypeRequest;
import com.mst.services.DesktopInventoryTypeService;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import org.springframework.http.*;
@Controller
public class ItemTypeApiController {
 private final DesktopInventoryTypeService service;
 public ItemTypeApiController(DesktopInventoryTypeService service){this.service=service;}
 @GetMapping({"/inventory/item_types","/inventory/item-types"}) public String page(){return "inventory/item_types";}
 @GetMapping("/inventory/item_types/edit/{id}") public String edit(@PathVariable int id){service.record(id);return "redirect:/inventory/item_types?id="+id;}
 @GetMapping(value="/api/inventory/item_types/lookups",produces="application/json") @ResponseBody public Map<String,Object> lookups(){return service.lookups();}
 @GetMapping(value="/api/inventory/item_types/history",produces="application/json") @ResponseBody public List<Map<String,Object>> history(){return service.history();}
 @GetMapping(value="/api/inventory/item_types/generate-code",produces="application/json") @ResponseBody public Map<String,String> code(){return Map.of("code",service.code());}
 @GetMapping(value="/api/inventory/item_types/{id:[0-9]+}",produces="application/json") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
 @PostMapping(value={"/api/inventory/item_types/save","/api/inventory/types/save"},produces="application/json") @ResponseBody public Map<String,Object> save(@RequestBody InventoryTypeRequest r){return service.save(r);}
 @GetMapping(value="/api/inventory/types/list",produces="application/json") @ResponseBody public List<Map<String,Object>> legacyList(){return service.history().stream().map(r->{Map<String,Object> row=new LinkedHashMap<>();r.forEach((k,v)->row.put(Character.toLowerCase(k.charAt(0))+k.substring(1),v));return row;}).toList();}
 @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<?> invalid(IllegalArgumentException ex){return ResponseEntity.badRequest().contentType(MediaType.APPLICATION_JSON).body(Map.of("message",ex.getMessage()));}
 @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<?> database(org.springframework.dao.DataAccessException ex){return ResponseEntity.status(409).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",Objects.toString(ex.getMostSpecificCause().getMessage(),"Item type save failed")));}
 @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<?> denied(Exception ex){return ResponseEntity.status(403).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",ex.getMessage()));}
 @GetMapping(value="/api/inventory/item_types/translations",produces="application/json") @ResponseBody public Map<String,Object> translations(){return service.translations();}
 @PostMapping(value="/api/inventory/item_types/translations",produces="application/json") @ResponseBody public Map<String,Object> translate(@RequestBody InventoryTypeRequest.Translation r){return service.saveTranslation(r);}}
