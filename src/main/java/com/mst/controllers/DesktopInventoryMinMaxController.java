package com.mst.controllers;
import com.mst.models.dto.InventoryMinMaxRequest;
import com.mst.services.DesktopInventoryMinMaxService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;
@RestController
@RequestMapping(value="/api/inventory/min-max",produces=MediaType.APPLICATION_JSON_VALUE)
public class DesktopInventoryMinMaxController {
    private final DesktopInventoryMinMaxService service;
    public DesktopInventoryMinMaxController(DesktopInventoryMinMaxService service){this.service=service;}
    @GetMapping("/lookups") public Map<String,Object> lookups(){return service.lookups();}
    @GetMapping("/items") public List<Map<String,Object>> items(@RequestParam int parent){return service.items(parent);}
    @GetMapping("/uoms") public List<Map<String,Object>> uoms(@RequestParam int item){return service.uoms(item);}
    @GetMapping("/last") public List<Map<String,Object>> last(@RequestParam int item,@RequestParam(defaultValue="0") int unit){return service.last(item,unit);}
    @PostMapping("/search") public List<Map<String,Object>> search(@RequestBody InventoryMinMaxRequest.Filter r){return service.query(r,false);}
    @PostMapping("/history") public List<Map<String,Object>> history(@RequestBody InventoryMinMaxRequest.Filter r){return service.query(r,true);}
    @GetMapping("/{id:[0-9]+}") public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @PostMapping("/save") public Map<String,Object> save(@RequestBody InventoryMinMaxRequest r){return service.save(r);}
    @PostMapping("/delete") public Map<String,Object> delete(@RequestBody InventoryMinMaxRequest.Delete r){return service.delete(r);}
    @ExceptionHandler(IllegalArgumentException.class) public ResponseEntity<?> invalid(Exception e){return error(400,e.getMessage());}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) public ResponseEntity<?> denied(Exception e){return error(403,e.getMessage());}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) public ResponseEntity<?> database(org.springframework.dao.DataAccessException e){return error(409,Objects.toString(e.getMostSpecificCause().getMessage(),"Rate operation failed"));}
    private ResponseEntity<?> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
