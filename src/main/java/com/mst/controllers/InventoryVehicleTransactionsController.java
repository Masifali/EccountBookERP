package com.mst.controllers;

import com.mst.models.dto.InventoryVehicleTransactionsRequest;
import com.mst.services.InventoryVehicleTransactionsService;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
public class InventoryVehicleTransactionsController {
    private final InventoryVehicleTransactionsService service;
    public InventoryVehicleTransactionsController(InventoryVehicleTransactionsService service){this.service=service;}
    @GetMapping({"/stocks/transaction-vehicle-wise","/stocks/stock_movement","/stocks/stock-movement"})
    public String page(){return "stocks/transaction_vehicle_wise";}
    @GetMapping("/api/inventory/transaction-vehicle-wise/lookups") @ResponseBody
    public Map<String,Object> lookups(){return service.lookups();}
    @GetMapping("/api/inventory/transaction-vehicle-wise/uoms") @ResponseBody
    public List<Map<String,Object>> uoms(@RequestParam int itemId){return service.uoms(itemId);}
    @PostMapping("/api/inventory/transaction-vehicle-wise") @ResponseBody
    public Map<String,Object> load(@RequestBody InventoryVehicleTransactionsRequest request){return service.load(request);}
    @PostMapping("/api/inventory/transaction-vehicle-wise/{action:grid-print|grid-export}") @ResponseBody
    public Map<String,Object> output(@PathVariable String action){return service.authorizeOutput(action.equals("grid-print")?"Grid Print":"Grid Export");}
}
