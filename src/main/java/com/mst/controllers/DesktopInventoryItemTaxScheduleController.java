package com.mst.controllers;

import com.mst.models.dto.InventoryItemTaxScheduleRequest;
import com.mst.services.DesktopInventoryItemTaxScheduleService;
import java.time.LocalDate;
import java.util.*;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class DesktopInventoryItemTaxScheduleController {
    private final DesktopInventoryItemTaxScheduleService service;
    public DesktopInventoryItemTaxScheduleController(DesktopInventoryItemTaxScheduleService service){this.service=service;}
    @GetMapping("/inventory/item-tax-schedules") public String page(){return "inventory/item_tax_schedules";}
    @GetMapping("/api/inventory/item-tax-schedules/lookups") @ResponseBody public Map<String,Object> lookups(){return service.lookups();}
    @GetMapping("/api/inventory/item-tax-schedules/{id:[0-9]+}") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @GetMapping("/api/inventory/item-tax-schedules/history") @ResponseBody public List<Map<String,Object>> history(@RequestParam(defaultValue="0") int category,@RequestParam(defaultValue="0") int type,@RequestParam(defaultValue="0") int item,@RequestParam(defaultValue="0") int tax,@RequestParam(required=false) @DateTimeFormat(iso=DateTimeFormat.ISO.DATE) LocalDate date,@RequestParam(required=false) Boolean active){return service.history(category,type,item,tax,date,active);}
    @PostMapping("/api/inventory/item-tax-schedules/save") @ResponseBody public Map<String,Object> save(@RequestBody InventoryItemTaxScheduleRequest request){return service.save(request);}
    @PostMapping("/api/inventory/item-tax-schedules/allocate") @ResponseBody public Map<String,Object> allocate(@RequestBody List<InventoryItemTaxScheduleRequest> rows){return service.allocate(rows);}
    @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<Map<String,String>> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<Map<String,String>> forbidden(org.springframework.security.access.AccessDeniedException ex){return error(403,ex.getMessage());}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<Map<String,String>> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Unable to save"));}
    private ResponseEntity<Map<String,String>> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
