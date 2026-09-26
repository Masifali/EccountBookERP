package com.mst.controllers;
import com.mst.models.dto.InventoryGridColumn;
import com.mst.services.DesktopInventoryGridLayoutService;
import java.io.IOException;
import java.util.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;
@RestController
@RequestMapping("/api/inventory/{target:defined-items|store-item-ledger|opening-stock|uom-schedules|brands|pos-items|definition-categories|item_types|lots}/grid-layout")
public class DesktopInventoryGridLayoutController {
    private final DesktopInventoryGridLayoutService service;
    public DesktopInventoryGridLayoutController(DesktopInventoryGridLayoutService service){this.service=service;}
    @GetMapping public List<InventoryGridColumn> load(@PathVariable String target)throws IOException{return service.load(target);}
    @PostMapping public List<InventoryGridColumn> save(@PathVariable String target,@RequestBody List<InventoryGridColumn> columns)throws IOException{return service.save(target,columns);}
    @PostMapping("/remove") public Map<String,Boolean> remove(@PathVariable String target)throws IOException{service.remove(target);return Map.of("removed",true);}
    @ExceptionHandler(IllegalArgumentException.class) public ResponseEntity<?> invalid(IllegalArgumentException ex){return ResponseEntity.badRequest().body(Map.of("message",ex.getMessage()));}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) public ResponseEntity<?> denied(Exception ex){return ResponseEntity.status(403).body(Map.of("message",ex.getMessage()));}
    @ExceptionHandler(IOException.class) public ResponseEntity<?> file(IOException ex){return ResponseEntity.status(409).body(Map.of("message","The desktop layout file could not be accessed. No layout changes were committed."));}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) public ResponseEntity<?> database(Exception ex){return ResponseEntity.status(409).body(Map.of("message","The desktop layout registration could not be saved."));}
}
