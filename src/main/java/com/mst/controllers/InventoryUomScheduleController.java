package com.mst.controllers;

import com.mst.models.dto.InventoryUomScheduleRequest;
import com.mst.services.InventoryUomScheduleService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class InventoryUomScheduleController {
    private final InventoryUomScheduleService service;
    public InventoryUomScheduleController(InventoryUomScheduleService service){this.service=service;}
    @GetMapping({"/inventory/item_uom_schedule","/inventory/item-uom-schedule"})
    public String page(){return "inventory/item_uom_schedule";}
    @GetMapping(value="/api/inventory/uom-schedules/lookups",produces="application/json") @ResponseBody
    public Map<String,Object> lookups(){return service.lookups();}
    @GetMapping(value="/api/inventory/uom-schedules",produces="application/json") @ResponseBody
    public List<Map<String,Object>> history(@RequestParam(required=false) Integer itemId){return service.history(itemId);}
    @GetMapping(value="/api/inventory/uom-schedules/{id:[0-9]+}",produces="application/json") @ResponseBody
    public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @PostMapping(value="/api/inventory/uom-schedules/save",produces="application/json") @ResponseBody
    public Map<String,Object> save(@RequestBody InventoryUomScheduleRequest r){return service.save(r);}
    @ExceptionHandler(IllegalArgumentException.class) @ResponseBody
    public ResponseEntity<Map<String,String>> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody
    public ResponseEntity<Map<String,String>> forbidden(org.springframework.security.access.AccessDeniedException ex){return error(403,ex.getMessage());}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody
    public ResponseEntity<Map<String,String>> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Unable to save the UOM schedule"));}
    private ResponseEntity<Map<String,String>> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
