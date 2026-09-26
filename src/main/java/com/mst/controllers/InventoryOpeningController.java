package com.mst.controllers;

import com.mst.models.dto.InventoryOpeningRequest;
import com.mst.services.InventoryOpeningService;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class InventoryOpeningController {
    private final InventoryOpeningService service;
    public InventoryOpeningController(InventoryOpeningService service){this.service=service;}
    @GetMapping({"/stocks/stock_opening_form","/stocks/stock-opening-form"})
    public String page(){return "stocks/opening_stock";}
    @GetMapping(value="/api/inventory/opening-stock/lookups", produces="application/json") @ResponseBody public Map<String,Object> lookups(){return service.lookups();}
    @GetMapping(value="/api/inventory/opening-stock/defaults", produces="application/json") @ResponseBody public Map<String,Object> defaults(){return service.defaults();}
    @GetMapping(value="/api/inventory/opening-stock/history-choices", produces="application/json") @ResponseBody public List<Map<String,Object>> historyChoices(){return service.historyChoices();}
    @GetMapping(value="/api/inventory/opening-stock/uoms/{item}", produces="application/json") @ResponseBody public List<Map<String,Object>> uoms(@PathVariable int item){return service.uoms(item);}
    @GetMapping(value="/api/inventory/opening-stock/{id:[0-9]+}", produces="application/json") @ResponseBody public Map<String,Object> record(@PathVariable int id){return service.record(id);}
    @GetMapping(value="/api/inventory/opening-stock/{id:[0-9]+}/attachments", produces="application/json") @ResponseBody public List<Map<String,Object>> attachments(@PathVariable int id){return service.attachments(id);}
    @GetMapping("/api/inventory/opening-stock/{id:[0-9]+}/attachments/{attachment:[0-9]+}") @ResponseBody public org.springframework.http.ResponseEntity<byte[]> download(@PathVariable int id,@PathVariable int attachment){var file=service.download(id,attachment);return org.springframework.http.ResponseEntity.ok().header("Content-Disposition",org.springframework.http.ContentDisposition.attachment().filename(file.name(),java.nio.charset.StandardCharsets.UTF_8).build().toString()).contentType(org.springframework.http.MediaType.APPLICATION_OCTET_STREAM).body(file.bytes());}
    @PostMapping(value="/api/inventory/opening-stock/history", produces="application/json") @ResponseBody public List<Map<String,Object>> history(@RequestBody InventoryOpeningRequest.History r){return service.history(r);}
    @PostMapping(value="/api/inventory/opening-stock", produces="application/json") @ResponseBody public Map<String,Object> save(@RequestBody InventoryOpeningRequest r){return service.save(r);}
    @ExceptionHandler(IllegalArgumentException.class) @ResponseBody @ResponseStatus(org.springframework.http.HttpStatus.BAD_REQUEST)
    public org.springframework.http.ResponseEntity<Map<String,String>> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody @ResponseStatus(org.springframework.http.HttpStatus.FORBIDDEN)
    public org.springframework.http.ResponseEntity<Map<String,String>> forbidden(org.springframework.security.access.AccessDeniedException ex){return error(403,ex.getMessage());}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody @ResponseStatus(org.springframework.http.HttpStatus.CONFLICT)
    public org.springframework.http.ResponseEntity<Map<String,String>> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"The database could not complete this request"));}
    private org.springframework.http.ResponseEntity<Map<String,String>> error(int status,String message){return org.springframework.http.ResponseEntity.status(status).contentType(org.springframework.http.MediaType.APPLICATION_JSON).body(Map.of("message",message));}
    @ExceptionHandler(IllegalStateException.class) @ResponseBody public org.springframework.http.ResponseEntity<Map<String,String>> unavailable(IllegalStateException ex){return error(409,ex.getMessage());}
}
