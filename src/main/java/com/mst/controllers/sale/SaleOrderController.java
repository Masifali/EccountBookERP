package com.mst.controllers.sale;

import com.mst.models.sale.dto.SaleOrderDto;
import com.mst.services.sale.SaleOrderService;
import com.mst.services.sale.SaleOrderAttachmentService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.Collections;
import java.util.List;
import java.util.Map;

/**
 * Controller for Sale Order module (desktop DocumentTypeId = 81),
 * rendering Thymeleaf page and exposing all necessary REST APIs.
 */
@Controller
@RequestMapping("/sale/sale-order")
public class SaleOrderController {

    @Autowired
    private SaleOrderService saleOrderService;

    @Autowired
    private SaleOrderAttachmentService saleOrderAttachmentService;

    @Autowired
    private com.mst.security.CurrentUserContext currentUserContext;

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
        // Desktop shows a disabled txtBranchSrNo filled by BranchSrNoFill() from the signed-in
        // user's own branch (SaleOrder.cs :917-924) - there is no branch picker on this screen.
        model.addAttribute("branchSrNo",
                saleOrderService.generateNextSaleOrderBranchSrNo(currentUserContext.currentBranchId()));
        model.addAllAttributes(saleOrderService.getMasterLookups());
        return "sale/sale_order";
    }

    /**
     * Branch serial for the signed-in user's own branch. The branch is NOT a request parameter:
     * the desktop reads UserAccount.BranchesId (SaleOrder.cs :917), so accepting one from the
     * client would let a caller pull a serial belonging to a branch they are not in.
     */
    @GetMapping("/api/next-branch-sr-no")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> getNextBranchSrNo() {
        int srNo = saleOrderService.generateNextSaleOrderBranchSrNo(currentUserContext.currentBranchId());
        return ResponseEntity.ok(Map.of("branchSrNo", srNo));
    }

    @GetMapping("/api/master-lookups")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> getMasterLookups() {
        return ResponseEntity.ok(saleOrderService.getMasterLookups());
    }

    @GetMapping("/api/customer-balance/{customerId}")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> getCustomerBalanceSummary(@PathVariable("customerId") int customerId,
            @RequestParam(value="saleOrderId", required=false) Integer saleOrderId) {
        return ResponseEntity.ok(saleOrderService.getCustomerBalanceSummary(customerId, saleOrderId));
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
        return ResponseEntity.ok(saleOrderService.getOutstandingPreBookingOrders());
    }

    @PostMapping("/api/save")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> saveSaleOrder(@RequestBody SaleOrderDto dto) {
        Map<String, Object> resp = saleOrderService.saveSaleOrder(dto);
        return ResponseEntity.ok(resp);
    }

    @GetMapping("/api/history")
    @ResponseBody
    public ResponseEntity<List<Map<String, Object>>> getHistory(
            @RequestParam(value="fromDate", required=false) String fromDate,
            @RequestParam(value="toDate", required=false) String toDate,
            @RequestParam(value="customerId", required=false) Integer customerId) {
        return ResponseEntity.ok(saleOrderService.getSaleOrdersHistory(fromDate, toDate, customerId));
    }

    @GetMapping("/api/{id}")
    @ResponseBody
    public ResponseEntity<?> getById(@PathVariable("id") int id) {
        Map<String, Object> data = saleOrderService.getSaleOrderById(id);
        if (data != null) return ResponseEntity.ok(data);
        return ResponseEntity.notFound().build();
    }

    @GetMapping("/api/{id:[0-9]+}/attachments")
    @ResponseBody
    public List<Map<String, Object>> attachments(@PathVariable int id) {
        return saleOrderAttachmentService.list(id);
    }

    @PostMapping("/api/{id:[0-9]+}/attachments")
    @ResponseBody
    public List<Map<String, Object>> saveAttachments(@PathVariable int id,
            @RequestBody SaleOrderAttachmentService.Request request) {
        return saleOrderAttachmentService.save(id, request);
    }

    @GetMapping("/api/{id:[0-9]+}/attachments/{attachmentId:[0-9]+}")
    public ResponseEntity<byte[]> downloadAttachment(@PathVariable int id, @PathVariable int attachmentId) {
        var file = saleOrderAttachmentService.download(id, attachmentId);
        return ResponseEntity.ok()
                .header(org.springframework.http.HttpHeaders.CONTENT_DISPOSITION,
                        org.springframework.http.ContentDisposition.attachment()
                                .filename(file.name(), java.nio.charset.StandardCharsets.UTF_8).build().toString())
                .contentType(org.springframework.http.MediaType.APPLICATION_OCTET_STREAM).body(file.bytes());
    }
}
