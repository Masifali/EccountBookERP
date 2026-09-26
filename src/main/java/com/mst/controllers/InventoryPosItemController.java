package com.mst.controllers;

import com.mst.models.dto.InventoryPosItemRequest;
import com.mst.services.InventoryPosItemService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class InventoryPosItemController {
    private final InventoryPosItemService service;
    public InventoryPosItemController(InventoryPosItemService service){this.service=service;}
    @GetMapping({"/inventory/pos_define_item","/inventory/pos-define-item"}) public String page(){return "inventory/pos_item";}
    @GetMapping(value="/api/inventory/pos-items/lookups",produces="application/json") @ResponseBody public Map<String,Object> lookups(){return service.lookups();}
    @GetMapping(value="/api/inventory/pos-items/history",produces="application/json") @ResponseBody public List<Map<String,Object>> history(@RequestParam(defaultValue="false") boolean all,@RequestParam(required=false) Integer category){return service.history(all,category);}
    @GetMapping(value="/api/inventory/pos-items/{id:[0-9]+}",produces="application/json") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @GetMapping(value="/api/inventory/pos-items/category/{id:[0-9]+}",produces="application/json") @ResponseBody public Map<String,Object> category(@PathVariable int id){return service.category(id);}
    @PostMapping(value="/api/inventory/pos-items/save",produces="application/json") @ResponseBody public Map<String,Object> save(@RequestBody InventoryPosItemRequest r){return service.save(r);}
    @PostMapping(value="/api/inventory/pos-items/barcode",produces="application/json") @ResponseBody public Map<String,Object> barcode(@RequestBody Map<String,String> r){return service.barcode(r.get("code"));}
    @GetMapping("/api/inventory/pos-items/{id:[0-9]+}/attachments/{attachment:[0-9]+}") public ResponseEntity<byte[]> attachment(@PathVariable int id,@PathVariable int attachment){var file=service.attachment(id,attachment);return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM).header("X-Content-Type-Options","nosniff").header(HttpHeaders.CONTENT_DISPOSITION,ContentDisposition.attachment().filename(file.name(),java.nio.charset.StandardCharsets.UTF_8).build().toString()).body(file.bytes());}
    @GetMapping("/api/inventory/pos-items/{id:[0-9]+}/product-image") public ResponseEntity<byte[]> product(@PathVariable int id){var file=service.product(id);return ResponseEntity.ok().contentType(MediaType.parseMediaType(file.type())).header("X-Content-Type-Options","nosniff").cacheControl(CacheControl.noStore()).body(file.bytes());}
    @ExceptionHandler(IllegalStateException.class) @ResponseBody public ResponseEntity<Map<String,String>> unavailable(IllegalStateException ex){return error(409,ex.getMessage());}
    @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<Map<String,String>> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody public ResponseEntity<Map<String,String>> forbidden(org.springframework.security.access.AccessDeniedException ex){return error(403,ex.getMessage());}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<Map<String,String>> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Unable to save the item"));}
    private ResponseEntity<Map<String,String>> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",message));}
}
