package com.mst.controllers;

import com.mst.models.dto.InventoryStockEvaluationVehicleWiseHoldRequest;
import com.mst.models.dto.InventoryStockEvaluationVehicleWiseRequest;
import com.mst.services.InventoryStockEvaluationVehicleWiseService;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

/** Stock Evaluation Report Vehicle Wise (ScreenDefinition 292, desktop StockEvaluationReportVehicleWise). */
@Controller
public class InventoryStockEvaluationVehicleWiseController {
    private final InventoryStockEvaluationVehicleWiseService service;
    public InventoryStockEvaluationVehicleWiseController(InventoryStockEvaluationVehicleWiseService service){this.service=service;}
    @GetMapping({"/stocks/stock-evaluation-vehicle-wise","/stocks/stock_evaluation_vehicle_wise"})
    public String page(){return "stocks/stock_evaluation_vehicle_wise";}
    @GetMapping("/api/inventory/stock-evaluation-vehicle-wise/lookups") @ResponseBody
    public Map<String,Object> lookups(){return service.lookups();}
    @GetMapping("/api/inventory/stock-evaluation-vehicle-wise/uoms") @ResponseBody
    public List<Map<String,Object>> uoms(@RequestParam(name="itemId") int itemId){return service.uoms(itemId);}
    @PostMapping("/api/inventory/stock-evaluation-vehicle-wise") @ResponseBody
    public Map<String,Object> load(@RequestBody InventoryStockEvaluationVehicleWiseRequest request){return service.load(request);}
    @GetMapping("/api/inventory/stock-evaluation-vehicle-wise/transaction-detail") @ResponseBody
    public List<Map<String,Object>> transactionDetail(@RequestParam(name="refDocumentTypeId") int refDocumentTypeId,@RequestParam(name="refDocIdNo") int refDocIdNo,@RequestParam(name="refDocSubIdNo") int refDocSubIdNo){
        return service.transactionDetail(refDocumentTypeId,refDocIdNo,refDocSubIdNo);
    }
    @PostMapping("/api/inventory/stock-evaluation-vehicle-wise/stock-hold") @ResponseBody
    public Map<String,Object> stockHold(@RequestBody InventoryStockEvaluationVehicleWiseHoldRequest request){return service.stockHold(request);}
}
