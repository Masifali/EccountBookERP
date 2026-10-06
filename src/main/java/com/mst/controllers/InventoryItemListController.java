package com.mst.controllers;

import com.mst.models.dto.InventoryItemListRequest;
import com.mst.services.InventoryItemListService;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

/** Screen 171 "Item List" - desktop Architecture.WinApp.Inventory_Reports.frmRptItemList. */
@Controller
public class InventoryItemListController {
    private final InventoryItemListService service;
    public InventoryItemListController(InventoryItemListService service) { this.service = service; }

    @GetMapping("/inventory/reports/item-list")
    public String page() { return "inventory/item_list_report"; }

    @GetMapping("/inventory/api/reports/item-list/lookups") @ResponseBody
    public Map<String,Object> lookups() { return service.lookups(); }

    @GetMapping("/inventory/api/reports/item-list") @ResponseBody
    public List<Map<String,Object>> load() { return service.load(); }

    @PostMapping("/inventory/api/reports/item-list/print-rows") @ResponseBody
    public List<Map<String,Object>> printRows(@RequestBody InventoryItemListRequest request) { return service.printRows(request); }

    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody
    public org.springframework.http.ResponseEntity<?> denied(Exception ex){return org.springframework.http.ResponseEntity.status(403).body(Map.of("message",Objects.toString(ex.getMessage(),"Access denied")));}
    @ExceptionHandler(org.springframework.web.server.ResponseStatusException.class) @ResponseBody
    public org.springframework.http.ResponseEntity<?> invalid(org.springframework.web.server.ResponseStatusException ex){return org.springframework.http.ResponseEntity.status(ex.getStatus()).body(Map.of("message",Objects.toString(ex.getReason(),"Invalid report request")));}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody
    public org.springframework.http.ResponseEntity<?> database(org.springframework.dao.DataAccessException ex){return org.springframework.http.ResponseEntity.status(409).body(Map.of("message",Objects.toString(ex.getMostSpecificCause().getMessage(),"The Item List query failed. Please retry.")));}
}
