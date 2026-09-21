package com.mst.controllers;

import com.mst.models.dto.InventoryItemLedgerRequest;
import com.mst.services.InventoryItemLedgerService;
import java.util.Map;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class InventoryItemLedgerController {
    private final InventoryItemLedgerService service;
    public InventoryItemLedgerController(InventoryItemLedgerService service){this.service=service;}
    @GetMapping({"/stocks/item_ledger","/stocks/item-ledger"})
    public String page(){return "stocks/item_ledger";}
    @GetMapping("/api/inventory/item-ledger/lookups") @ResponseBody
    public Map<String,Object> lookups(){return service.lookups();}
    @PostMapping("/api/inventory/item-ledger") @ResponseBody
    public Map<String,Object> load(@RequestBody InventoryItemLedgerRequest request){return service.load(request,"View");}
    @PostMapping("/api/inventory/item-ledger/{action:grid-print|grid-export}") @ResponseBody
    public Map<String,Object> output(@PathVariable String action){return service.authorizeOutput(action.equals("grid-print")?"Grid Print":"Grid Export");}
}
