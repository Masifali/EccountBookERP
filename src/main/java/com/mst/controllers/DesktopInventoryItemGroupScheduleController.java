package com.mst.controllers;
import com.mst.models.dto.InventoryItemGroupScheduleRequest;
import com.mst.services.DesktopInventoryItemGroupScheduleService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
@Controller
public class DesktopInventoryItemGroupScheduleController {
    private final DesktopInventoryItemGroupScheduleService service;
    public DesktopInventoryItemGroupScheduleController(DesktopInventoryItemGroupScheduleService service){this.service=service;}
    @GetMapping("/inventory/item-group-schedules") public String page(){return "inventory/item_group_schedules";}
    @GetMapping("/inventory/assign-item-group") public String assignment(){return "inventory/assign_item_group";}
    @GetMapping("/api/inventory/item-group-schedules/lookups") @ResponseBody public Map<String,Object> lookups(){return service.lookups();}
    @GetMapping("/api/inventory/item-group-schedules/history") @ResponseBody public List<Map<String,Object>> history(){return service.history();}
    @GetMapping("/api/inventory/item-group-schedules/{id:[0-9]+}") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @PostMapping("/api/inventory/item-group-schedules/save") @ResponseBody public Map<String,Object> save(@RequestBody InventoryItemGroupScheduleRequest request){return service.save(request);}
    @GetMapping("/api/inventory/assign-item-group/lookups") @ResponseBody public Map<String,Object> assignmentLookups(){return service.assignmentLookups();}
    @GetMapping("/api/inventory/assign-item-group/rows") @ResponseBody public List<Map<String,Object>> assignmentRows(@RequestParam(defaultValue="0") int parent,@RequestParam(defaultValue="0") int category,@RequestParam(defaultValue="0") int type,@RequestParam(defaultValue="0") int item){return service.assignmentRows(parent,category,type,item);}
    @PostMapping("/api/inventory/assign-item-group/save") @ResponseBody public Map<String,Object> assign(@RequestBody InventoryItemGroupScheduleRequest.Assignment request){return service.assign(request);}
    @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<Map<String,String>> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
    @ExceptionHandler(IllegalStateException.class) @ResponseBody public ResponseEntity<Map<String,String>> unavailable(IllegalStateException ex){return error(409,ex.getMessage());}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<Map<String,String>> forbidden(org.springframework.security.access.AccessDeniedException ex){return error(403,ex.getMessage());}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<Map<String,String>> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Unable to save"));}
    private ResponseEntity<Map<String,String>> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
