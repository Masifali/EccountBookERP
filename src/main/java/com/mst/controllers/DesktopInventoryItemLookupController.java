package com.mst.controllers;
import com.mst.models.dto.InventoryItemLookupRequest;
import com.mst.repositories.DesktopInventoryItemLookupRepository.Kind;
import com.mst.services.DesktopInventoryItemLookupService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
@Controller
public class DesktopInventoryItemLookupController {
    private final DesktopInventoryItemLookupService service;
    public DesktopInventoryItemLookupController(DesktopInventoryItemLookupService service){this.service=service;}
    @GetMapping("/inventory/crop-years") public String crops(){return "inventory/crop_years";}
    @GetMapping("/api/inventory/item-lookups/{kind}/history") @ResponseBody public List<Map<String,Object>> history(@PathVariable String kind){return service.history(kind(kind));}
    @GetMapping("/api/inventory/item-lookups/{kind}/{id:[0-9]+}") @ResponseBody public Map<String,Object> record(@PathVariable String kind,@PathVariable int id){return service.record(kind(kind),id);}
    @PostMapping("/api/inventory/item-lookups/{kind}/save") @ResponseBody public Map<String,Object> save(@PathVariable String kind,@RequestBody InventoryItemLookupRequest request){return service.save(kind(kind),request);}
    private Kind kind(String value){return switch(value){case "crop-years"->Kind.CROP_YEAR;case "uom-groups"->Kind.UOM_GROUP;default->throw new IllegalArgumentException("Unknown item lookup");};}
    @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<Map<String,String>> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
    @ExceptionHandler(IllegalStateException.class) @ResponseBody public ResponseEntity<Map<String,String>> unavailable(IllegalStateException ex){return error(409,ex.getMessage());}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<Map<String,String>> forbidden(org.springframework.security.access.AccessDeniedException ex){return error(403,ex.getMessage());}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<Map<String,String>> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Unable to save"));}
    private ResponseEntity<Map<String,String>> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
