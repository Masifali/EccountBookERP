package com.mst.controllers;
import com.mst.models.dto.InventoryLookupDefinitionRequest;
import com.mst.services.DesktopInventoryLookupDefinitionService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
@Controller
public class DesktopInventoryLookupDefinitionController {
 private final DesktopInventoryLookupDefinitionService service;
 public DesktopInventoryLookupDefinitionController(DesktopInventoryLookupDefinitionService service){this.service=service;}
 @GetMapping({"/inventory/lookup-definitions", "/master-data-definition", "/lookups/master-data-definition", "/lookups/master-data"}) public String page(){return "inventory/lookup_definitions";}
 @GetMapping("/api/inventory/lookup-definitions/types") @ResponseBody public List<Map<String,Object>> types(){return service.types();}
 @GetMapping("/api/inventory/lookup-definitions/history") @ResponseBody public List<Map<String,Object>> history(){return service.history();}
 @GetMapping("/api/inventory/lookup-definitions/code") @ResponseBody public Map<String,Object> code(@RequestParam int type){return service.code(type);}
 @GetMapping("/api/inventory/lookup-definitions/{id:[0-9]+}") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
 @PostMapping("/api/inventory/lookup-definitions/save") @ResponseBody public Map<String,Object> save(@RequestBody InventoryLookupDefinitionRequest request){return service.save(request);}
 @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<Map<String,String>> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
 @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<Map<String,String>> forbidden(org.springframework.security.access.AccessDeniedException ex){return error(403,ex.getMessage());}
 @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<Map<String,String>> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Unable to save lookup"));}
 private ResponseEntity<Map<String,String>> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
