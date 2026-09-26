package com.mst.controllers;

import com.mst.models.dto.InventoryWarehouseRequest;
import com.mst.services.DesktopInventoryWarehouseService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping(value="/api/inventory/warehouses",produces=MediaType.APPLICATION_JSON_VALUE)
public class DesktopInventoryWarehouseController {
    private final DesktopInventoryWarehouseService service;
    public DesktopInventoryWarehouseController(DesktopInventoryWarehouseService service){this.service=service;}
    @GetMapping("/lookups") public Map<String,Object> lookups(){return service.lookups();}
    @GetMapping({"/list","/history"}) public List<Map<String,Object>> history(){return service.history();}
    @GetMapping("/{id:[0-9]+}") public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @PostMapping("/save") public Map<String,Object> save(@RequestBody InventoryWarehouseRequest request){return service.save(request);}
    @GetMapping("/allocations") public Map<String,Object> allocations(@RequestParam int branch){return service.allocations(branch);}
    @PostMapping("/allocations") public Map<String,Object> allocate(@RequestBody InventoryWarehouseRequest.Allocation request){return service.allocate(request);}
    @GetMapping("/racks/lookups") public Map<String,Object> rackLookups(){return service.rackLookups();}
    @GetMapping("/racks/history") public List<Map<String,Object>> rackHistory(){return service.rackHistory();}
    @GetMapping("/racks/{id:[0-9]+}") public Map<String,Object> rack(@PathVariable int id){return service.rack(id);}
    @PostMapping("/racks/save") public Map<String,Object> saveRack(@RequestBody InventoryWarehouseRequest.Rack request){return service.saveRack(request);}
    @GetMapping("/racks/items") public Map<String,Object> rackItems(@RequestParam int rack){return service.rackItems(rack);}
    @PostMapping("/racks/items") public Map<String,Object> saveRackItems(@RequestBody InventoryWarehouseRequest.RackItems request){return service.saveRackItems(request);}
    @ExceptionHandler(IllegalArgumentException.class) public ResponseEntity<?> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) public ResponseEntity<?> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Warehouse operation failed"));}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) public ResponseEntity<?> denied(Exception ex){return error(403,ex.getMessage());}
    private ResponseEntity<?> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
