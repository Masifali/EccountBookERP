package com.mst.controllers;
import com.mst.models.dto.InventoryConsumptionRequest;
import com.mst.services.DesktopInventoryConsumptionService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;
@RestController
@RequestMapping(value="/api/inventory/consumption",produces=MediaType.APPLICATION_JSON_VALUE)
public class DesktopInventoryConsumptionController {
    private final DesktopInventoryConsumptionService service;
    public DesktopInventoryConsumptionController(DesktopInventoryConsumptionService service){this.service=service;}
    @GetMapping("/lookups") public Map<String,Object> lookups(){return service.lookups();}
    @GetMapping({"/show","/list"}) public Map<String,Object> show(@RequestParam(defaultValue="0") int category,@RequestParam(defaultValue="0") int type){return service.show(category,type);}
    @GetMapping("/{id:[0-9]+}") public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @PostMapping("/save") public Map<String,Object> save(@RequestBody InventoryConsumptionRequest r){return service.save(r,false);}
    @PostMapping("/update") public Map<String,Object> update(@RequestBody InventoryConsumptionRequest r){return service.save(r,true);}
    @ExceptionHandler(IllegalArgumentException.class) public ResponseEntity<?> invalid(Exception e){return error(400,e.getMessage());}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) public ResponseEntity<?> denied(Exception e){return error(403,e.getMessage());}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) public ResponseEntity<?> database(org.springframework.dao.DataAccessException e){return error(409,Objects.toString(e.getMostSpecificCause().getMessage(),"Consumption allocation failed"));}
    private ResponseEntity<?> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
