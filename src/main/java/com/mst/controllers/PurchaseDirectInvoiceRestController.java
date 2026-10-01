package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.PurchaseDirectInvoiceService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/** InvfrmPurchasedirectInvoice (screen 117, DocumentTypeId 57). Tenancy always from the session. */
@RestController
@RequestMapping("/api/purchase/purchase-direct-invoice")
public class PurchaseDirectInvoiceRestController {

    @Autowired
    private PurchaseDirectInvoiceService purchaseDirectInvoiceService;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/dropdowns")
    public ResponseEntity<?> getDropdowns() {
        return ResponseEntity.ok(purchaseDirectInvoiceService.desktopDropdowns());
    }

    @PostMapping("/calculate-line")
    public ResponseEntity<?> calculateLine(@RequestBody Map<String,Object> input){return ResponseEntity.ok(purchaseDirectInvoiceService.calculateLine(input));}

    @PostMapping("/calculate-bill")
    public ResponseEntity<?> calculateBill(@RequestBody Map<String,Object> input){input.put("organizationId",currentUserContext.currentOrganizationId());input.put("companyId",currentUserContext.currentCompanyId());return ResponseEntity.ok(purchaseDirectInvoiceService.calculateBill(input));}

    @GetMapping("/next-code")
    public ResponseEntity<?> getNextCode() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();
        Map<String, Object> res = new HashMap<>();
        res.put("docNo", purchaseDirectInvoiceService.generateNextDocNo(orgId, compId, branchId, yearId));
        res.put("branchSrNo",purchaseDirectInvoiceService.generateNextBranchNo(orgId,compId,branchId,yearId));
        return ResponseEntity.ok(res);
    }

    /** GetAll :4676 - the ticked History branches, the date pair of the ticked radio, doc numbers and supplier. */
    @GetMapping("/history")
    public ResponseEntity<?> getHistory(@RequestParam(required = false) String branchIds,
                                         @RequestParam(required = false) String fromDate,
                                         @RequestParam(required = false) String toDate,
                                         @RequestParam(required = false) Integer supplierId,
                                         @RequestParam(required = false) Integer fromDocNo,
                                         @RequestParam(required = false) Integer toDocNo,
                                         @RequestParam(required = false) String dateType) {
        return ResponseEntity.ok(purchaseDirectInvoiceService.history(branchIds, fromDate, toDate, supplierId, fromDocNo, toDocNo, dateType));
    }

    @GetMapping("/history-detail/{id}")
    public ResponseEntity<?> historyDetail(@PathVariable("id") int id, @RequestParam(required = false) String branchIds) {
        return ResponseEntity.ok(purchaseDirectInvoiceService.historyDetail(id, branchIds));
    }

    @GetMapping("/history-suppliers")
    public ResponseEntity<?> historySuppliers(@RequestParam(required = false) String branchIds) {
        return ResponseEntity.ok(purchaseDirectInvoiceService.historySuppliers(branchIds));
    }

    @GetMapping("/history-refresh")
    public ResponseEntity<?> historyRefresh() {
        return ResponseEntity.ok(purchaseDirectInvoiceService.historyRefresh());
    }

    /** LoadPurchaseOrder dialog list (DocumentTypeId 41). */
    @GetMapping("/order-loader")
    public ResponseEntity<?> orderLoader(@RequestParam(required = false) Integer supplierId,
                                         @RequestParam(required = false) String fromDate,
                                         @RequestParam(required = false) String toDate,
                                         @RequestParam(required = false) Integer fromDocNo,
                                         @RequestParam(required = false) Integer toDocNo,
                                         @RequestParam(required = false) String excludeIds) {
        return ResponseEntity.ok(purchaseDirectInvoiceService.pendingOrders(supplierId, fromDate, toDate, fromDocNo, toDocNo, excludeIds));
    }

    /** LoadInGridDetail :5277 for the checked orders. */
    @GetMapping("/order-load")
    public ResponseEntity<?> orderLoad(@RequestParam(required = false) String orderIds) {
        return ResponseEntity.ok(purchaseDirectInvoiceService.loadOrders(orderIds));
    }

    /** GetEmptyBagsInformationFromOrder :1607. */
    @GetMapping("/order-empty-bags")
    public ResponseEntity<?> orderEmptyBags(@RequestParam(required = false) String orderIds) {
        return ResponseEntity.ok(purchaseDirectInvoiceService.orderEmptyBags(orderIds));
    }

    /** DeleteDetailrow :2240 - StockInReferenceValidationReferredOrNot for a stored detail row. */
    @PostMapping("/{id}/detail-row-check/{detailId}")
    public ResponseEntity<?> detailRowCheck(@PathVariable("id") int id, @PathVariable("detailId") int detailId) {
        purchaseDirectInvoiceService.detailRowCheck(id, detailId);
        return ResponseEntity.ok(Map.of("success", true));
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getById(@PathVariable("id") int id) {
        Map<String, Object> data = purchaseDirectInvoiceService.getById(id);
        if (data != null) return ResponseEntity.ok(data);
        return ResponseEntity.notFound().build();
    }

    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody Map<String, Object> payload) {
        payload.put("organizationId", currentUserContext.currentOrganizationId());
        payload.put("companyId", currentUserContext.currentCompanyId());
        payload.put("branchesId", currentUserContext.currentBranchId());
        payload.put("financialYearId", currentUserContext.currentFinancialYearId());

        Map<String, Object> res = purchaseDirectInvoiceService.saveDirectInvoice(payload);
        if (Boolean.TRUE.equals(res.get("success"))) {
            return ResponseEntity.ok(res);
        } else {
            return ResponseEntity.status(400).body(res);
        }
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> delete(@PathVariable("id") int id) {
        boolean ok = purchaseDirectInvoiceService.deleteDirectInvoice(id);
        Map<String, Object> res = new HashMap<>();
        res.put("success", ok);
        res.put("message", "Delete Voucher Successfully"); // btnDelete_Click :3726
        return ResponseEntity.ok(res);
    }
}
