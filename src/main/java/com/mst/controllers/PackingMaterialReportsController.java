package com.mst.controllers;

import com.mst.services.PackingMaterialReportsService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

@Controller
public class PackingMaterialReportsController {

    private static final String API = "/api/packing-material/reports";

    @Autowired private PackingMaterialReportsService service;
    @Autowired private com.mst.services.PackingMaterialReportExportService exports;

    /* 1. Goods Receipt Notes PM Register */
    @GetMapping("/packing-material/reports/grn-register")
    public String grnRegisterPage(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        model.addAttribute("reportTitle", "Goods Receipt Notes PM Register");
        model.addAttribute("reportType", "grn-register");
        return "packing_material/reports/grn_register";
    }

    /* 2. Requirement Planning Detail */
    @GetMapping("/packing-material/reports/requirement-planning-detail")
    public String requirementPlanningPage(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        model.addAttribute("reportTitle", "Packing Material Requirement Planning Detail");
        model.addAttribute("reportType", "requirement-planning-detail");
        return "packing_material/reports/requirement_planning_detail";
    }

    /* 3. Purchase Order Register PM */
    @GetMapping("/packing-material/reports/purchase-order-register")
    public String purchaseOrderRegisterPage(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        model.addAttribute("reportTitle", "Purchase Order Register PM");
        model.addAttribute("reportType", "purchase-order-register");
        return "packing_material/reports/purchase_order_register";
    }

    /* 4. Inventory Transaction Report */
    @GetMapping("/packing-material/reports/inventory-transaction-report")
    public String inventoryTransactionPage(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        model.addAttribute("reportTitle", "Inventory Transaction Report (PM)");
        model.addAttribute("reportType", "inventory-transaction-report");
        return "packing_material/reports/inventory_transaction_report";
    }

    /* 5. Stock With Supplier Report */
    @GetMapping("/packing-material/reports/stock-with-supplier")
    public String stockWithSupplierPage(Model model) {
        model.addAttribute("activeMenu", "packing-material");
        model.addAttribute("reportTitle", "Stock With Supplier Report (PM)");
        model.addAttribute("reportType", "stock-with-supplier");
        return "packing_material/reports/stock_with_supplier";
    }

    @GetMapping(API + "/{report}/initial") @ResponseBody
    public Object initial(@PathVariable("report") String report) { return service.initial(report); }
    @GetMapping(API + "/{report}/lookups") @ResponseBody
    public Object lookups(@PathVariable("report") String report,@RequestParam(name="branches",defaultValue="") String branches) { return service.lookups(report,branches); }
    @PostMapping(API + "/{report}/data") @ResponseBody
    public Object data(@PathVariable("report") String report,@RequestBody com.mst.models.PackingReportFilter filter) { return service.data(report,filter); }
    @PostMapping(API + "/purchase-order-register/action") @ResponseBody
    public Object action(@RequestBody PackingMaterialReportsService.Action action) { return service.action(action); }
    @PostMapping(API + "/purchase-order-register/approval-history") @ResponseBody
    public Object approvals(@RequestBody PackingMaterialReportsService.Action action) { return service.approvalHistory(action); }
    @PostMapping(API + "/stock-with-supplier/document") @ResponseBody
    public Object document(@RequestBody PackingMaterialReportsService.Document request) { return service.document(request); }
    @PostMapping(API + "/inventory-transaction-report/document") @ResponseBody
    public Object transactionDocument(@RequestBody PackingMaterialReportsService.TransactionDocument request) { return service.transactionDocuments(request); }
    @PostMapping(API + "/{report}/export") @ResponseBody
    public ResponseEntity<byte[]> export(@PathVariable("report") String report,@RequestBody com.mst.services.PackingMaterialReportExportService.Request request) throws Exception {
        return ResponseEntity.ok().header("Content-Type","application/vnd.ms-excel").header("Content-Disposition","attachment; filename="+report+".xls").body(exports.export(report,request));
    }
    @PostMapping(API + "/{report}/print") @ResponseBody
    public ResponseEntity<byte[]> print(@PathVariable("report") String report,@RequestBody com.mst.models.PackingReportFilter filter,@RequestParam(name="format",defaultValue="main") String format) throws Exception {
        return ResponseEntity.ok().header("Content-Type","application/pdf").header("Content-Disposition","inline; filename="+report+".pdf").body(service.print(report,filter,format));
    }
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) @ResponseBody
    public ResponseEntity<?> denied(Exception e) {return ResponseEntity.status(403).body(Map.of("message",e.getMessage()));}
    @ExceptionHandler(IllegalArgumentException.class) @ResponseBody
    public ResponseEntity<?> invalid(Exception e) {return ResponseEntity.badRequest().body(Map.of("message",e.getMessage()));}
    @ExceptionHandler(Exception.class) @ResponseBody
    public ResponseEntity<?> failed(Exception e) {return ResponseEntity.status(500).body(Map.of("message",e.getMessage()==null?"The report request failed":e.getMessage()));}
}
