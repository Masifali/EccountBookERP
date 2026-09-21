package com.mst.controllers;
import com.mst.models.dto.InventoryItemCompanyAllocationRequest;
import com.mst.services.DesktopInventoryItemCompanyAllocationService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
@Controller
public class DesktopInventoryItemCompanyAllocationController {
 private final DesktopInventoryItemCompanyAllocationService service;
 public DesktopInventoryItemCompanyAllocationController(DesktopInventoryItemCompanyAllocationService service){this.service=service;}
 @GetMapping("/inventory/item-company-allocations") public String page(){return "inventory/item_company_allocations";}
 @GetMapping("/inventory/item-company-status") public String status(){return "inventory/item_company_status";}
 @GetMapping("/api/inventory/item-company-allocations/lookups") @ResponseBody public Map<String,Object> lookups(){return service.lookups();}
 @GetMapping("/api/inventory/item-company-allocations/rows") @ResponseBody public Map<String,Object> rows(@RequestParam int company,@RequestParam(defaultValue="false") boolean status){return service.rows(company,status);}
 @PostMapping("/api/inventory/item-company-allocations/{action:allocate|deallocate|activate|deactivate}") @ResponseBody public Map<String,Object> change(@PathVariable String action,@RequestBody InventoryItemCompanyAllocationRequest request){return service.change(action,request);}
 @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<Map<String,String>> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
 @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<Map<String,String>> forbidden(org.springframework.security.access.AccessDeniedException ex){return error(403,ex.getMessage());}
 @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<Map<String,String>> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Unable to change item allocation"));}
 private ResponseEntity<Map<String,String>> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
