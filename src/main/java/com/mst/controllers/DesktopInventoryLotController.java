package com.mst.controllers;

import com.mst.models.dto.InventoryLotRequest;
import com.mst.services.DesktopInventoryLotService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping(value="/api/inventory/lots",produces=MediaType.APPLICATION_JSON_VALUE)
public class DesktopInventoryLotController {
    private final DesktopInventoryLotService service;
    public DesktopInventoryLotController(DesktopInventoryLotService service){this.service=service;}
    @GetMapping("/lookups") public Map<String,Object> lookups(){return service.lookups();}
    @GetMapping({"/list","/history"}) public List<Map<String,Object>> history(){return service.history();}
    @GetMapping("/{id:[0-9]+}") public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @GetMapping("/references") public List<Map<String,Object>> references(@RequestParam int type,@RequestParam(defaultValue="0") int record){return service.references(type,record);}
    @PostMapping("/save") public Map<String,Object> save(@RequestBody InventoryLotRequest request){return service.save(request);}
    @GetMapping("/allocations") public Map<String,Object> allocations(@RequestParam int branch){return service.allocations(branch);}
    @PostMapping("/allocations") public Map<String,Object> allocate(@RequestBody InventoryLotRequest.Allocation request){return service.allocate(request);}
    @ExceptionHandler(IllegalArgumentException.class) public ResponseEntity<?> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) public ResponseEntity<?> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Job lot operation failed"));}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) public ResponseEntity<?> denied(Exception ex){return error(403,ex.getMessage());}
    private ResponseEntity<?> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
