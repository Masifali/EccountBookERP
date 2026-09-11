package com.mst.controllers.sale;

import com.mst.models.sale.dto.SaleOrderDto;
import com.mst.services.sale.SaleOrderService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.Collections;
import java.util.List;
import java.util.Map;

/**
 * Controller for Sale Order module (DocumentTypeId = 20),
 * rendering Thymeleaf page and exposing all necessary REST APIs.
 */
@Controller
@RequestMapping("/sale/sale-order")
public class SaleOrderController {

    @Autowired
    private SaleOrderService saleOrderService;

    @Autowired(required = false)
    private com.mst.services.BranchService branchService;

    @GetMapping
    public String showSaleOrderPage(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Order");
        // Real DocumentTypeId = 81 (verified against desktop DocumentNoFill/BranchSrNoFill and the
        // Sp_SaleOrder_GetAllMethod proc's own usage-example comment) - NOT 20.
        model.addAttribute("documentTypeId", 81);
        int nextDocNo = saleOrderService.generateNextSaleOrderDocNo();
        model.addAttribute("nextCode", nextDocNo);
        model.addAttribute("nextDocNo", nextDocNo);
        if (branchService != null) {
            model.addAttribute("branches", branchService.getAllBranches());
        }
        model.addAllAttributes(saleOrderService.getMasterLookups());
        return "sale/sale_order";
    }

    @GetMapping("/api/next-branch-sr-no/{branchId}")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> getNextBranchSrNo(@PathVariable("branchId") int branchId) {
        int srNo = saleOrderService.generateNextSaleOrderBranchSrNo(branchId);
        return ResponseEntity.ok(Map.of("branchSrNo", srNo));
    }

    @GetMapping("/api/master-lookups")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> getMasterLookups() {
        return ResponseEntity.ok(saleOrderService.getMasterLookups());
    }

    @GetMapping("/api/customer-balance/{customerId}")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> getCustomerBalanceSummary(@PathVariable("customerId") int customerId) {
        return ResponseEntity.ok(saleOrderService.getCustomerBalanceSummary(customerId));
    }

    @GetMapping("/api/item-uoms/{itemId}")
    @ResponseBody
    public ResponseEntity<List<Map<String, Object>>> getItemUomSchedule(@PathVariable("itemId") int itemId) {
        return ResponseEntity.ok(saleOrderService.getItemUomSchedule(itemId));
    }

    @GetMapping("/api/stock-report")
    @ResponseBody
    public ResponseEntity<List<Map<String, Object>>> getStockReport(
            @RequestParam(value = "itemId", required = false) Integer itemId,
            @RequestParam(value = "cropYear", required = false) String cropYear,
            @RequestParam(value = "jobLotId", required = false) Integer jobLotId,
            @RequestParam(value = "warehouseId", required = false) Integer warehouseId,
            @RequestParam(value = "date", required = false) String date) {
        return ResponseEntity.ok(saleOrderService.getStockReportData(itemId, cropYear, jobLotId, warehouseId, date));
    }

    @GetMapping("/api/stock-report-with-value")
    @ResponseBody
    public ResponseEntity<List<Map<String, Object>>> getStockReportWithValue(
            @RequestParam(value = "itemId", required = false) Integer itemId,
            @RequestParam(value = "cropYear", required = false) String cropYear,
            @RequestParam(value = "jobLotId", required = false) Integer jobLotId,
            @RequestParam(value = "warehouseId", required = false) Integer warehouseId,
            @RequestParam(value = "date", required = false) String date) {
        return ResponseEntity.ok(saleOrderService.getStockReportWithValueData(itemId, cropYear, jobLotId, warehouseId, date));
    }

    @GetMapping("/api/pre-booking-orders")
    @ResponseBody
    public ResponseEntity<List<Map<String, Object>>> getPreBookingOrders() {
        return ResponseEntity.ok(Collections.emptyList());
    }

    @PostMapping("/api/save")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> saveSaleOrder(@RequestBody SaleOrderDto dto) {
        Map<String, Object> resp = saleOrderService.saveSaleOrder(dto);
        return ResponseEntity.ok(resp);
    }

    @GetMapping("/api/history")
    @ResponseBody
    public ResponseEntity<List<Map<String, Object>>> getHistory() {
        return ResponseEntity.ok(saleOrderService.getSaleOrdersHistory());
    }

    @GetMapping("/api/{id}")
    @ResponseBody
    public ResponseEntity<?> getById(@PathVariable("id") int id) {
        Map<String, Object> data = saleOrderService.getSaleOrderById(id);
        if (data != null) return ResponseEntity.ok(data);
        return ResponseEntity.notFound().build();
    }
}
