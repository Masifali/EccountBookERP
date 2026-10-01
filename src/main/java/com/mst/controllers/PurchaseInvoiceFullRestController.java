package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.PurchaseInvoiceFullService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/purchase/purchase-invoice-full")
public class PurchaseInvoiceFullRestController {

    @Autowired
    private PurchaseInvoiceFullService purchaseInvoiceFullService;

    @Autowired
    private CurrentUserContext currentUserContext;

    @Autowired private com.mst.services.PurchaseInvoiceGrnService grnTransfer;

    @PostMapping("/load-grns")
    public Map<String,Object> loadGrns(@RequestBody List<Map<String,Object>> selected){return grnTransfer.preview(selected);}

    /** txtGrnNo_Leave:6002 - the GRN (type 46) with this DocNo in the active year that is not on an invoice yet. */
    @GetMapping("/grn-by-no")
    public Map<String,Object> grnByNo(@RequestParam int docNo){purchaseInvoiceFullService.requireView();return grnTransfer.previewByDocNo(docNo);}

    @PostMapping("/payment-row")
    @SuppressWarnings("unchecked")
    public Map<String,Object> paymentRow(@RequestBody Map<String,Object> body){return purchaseInvoiceFullService.paymentRow(body);}

    @GetMapping("/dropdowns")
    public ResponseEntity<?> getDropdowns() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return ResponseEntity.ok(purchaseInvoiceFullService.getDropdowns(orgId, compId));
    }

    @GetMapping("/next-code")
    public ResponseEntity<?> getNextCode(@RequestParam(required = false, defaultValue = "56") int docTypeId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();
        int nextNo = purchaseInvoiceFullService.generateNextDocNo(orgId, compId, branchId, yearId, docTypeId);
        Map<String, Object> res = new HashMap<>();
        res.put("docNo", nextNo);
        res.put("branchSrNo", purchaseInvoiceFullService.generateNextBranchNo());
        return ResponseEntity.ok(res);
    }

    @PostMapping("/calculate-line")
    public Map<String,Object> calculateLine(@RequestBody Map<String,Object> body){return purchaseInvoiceFullService.calculateLine(body);}

    @PostMapping("/calculate-bill")
    public Map<String,Object> calculateBill(@RequestBody Map<String,Object> body){return purchaseInvoiceFullService.calculateBill(body);}

    @PostMapping("/supplement-row")
    public Map<String,Object> supplementRow(@RequestBody Map<String,Object> body){return purchaseInvoiceFullService.supplementRow(body);}

    /** History tab "Show" (GetAll:4932): branches from the Branch Name combo, dates by the Doc/Entry/Modify/Approved radio. */
    @GetMapping("/history")
    public ResponseEntity<?> getHistory(@RequestParam(required = false) String branchIds,
                                        @RequestParam(required = false) String fromDate,
                                        @RequestParam(required = false) String toDate,
                                        @RequestParam(required = false) Integer supplierId,
                                        @RequestParam(required = false) Integer fromDocNo,
                                        @RequestParam(required = false) Integer toDocNo,
                                        @RequestParam(required = false) String dateType) {
        return ResponseEntity.ok(purchaseInvoiceFullService.history(branchIds, fromDate, toDate, supplierId, fromDocNo, toDocNo, dateType));
    }

    @GetMapping("/history-detail/{id}")
    public List<Map<String, Object>> historyDetail(@PathVariable("id") int id, @RequestParam(required = false) String branchIds) {
        return purchaseInvoiceFullService.historyDetail(id, branchIds);
    }

    @GetMapping("/history-suppliers")
    public List<Map<String, Object>> historySuppliers(@RequestParam(required = false) String branchIds) {
        return purchaseInvoiceFullService.historySuppliers(branchIds);
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getById(@PathVariable("id") int id) {
        Map<String, Object> data = purchaseInvoiceFullService.getById(id);
        if (data != null) return ResponseEntity.ok(data);
        return ResponseEntity.notFound().build();
    }

    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody Map<String, Object> payload) {
        payload.put("organizationId", currentUserContext.currentOrganizationId());
        payload.put("companyId", currentUserContext.currentCompanyId());
        payload.put("branchesId", currentUserContext.currentBranchId());
        payload.put("financialYearId", currentUserContext.currentFinancialYearId());

        Map<String, Object> res = purchaseInvoiceFullService.savePurchaseInvoice(payload);
        if (Boolean.TRUE.equals(res.get("success"))) {
            return ResponseEntity.ok(res);
        } else {
            return ResponseEntity.status(400).body(res);
        }
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> delete(@PathVariable("id") int id) {
        boolean ok = purchaseInvoiceFullService.deletePurchaseInvoice(id);
        Map<String, Object> res = new HashMap<>();
        res.put("success", ok);
        res.put("message", ok ? "Delete Record Successfully" : "Failed to delete");
        return ResponseEntity.ok(res);
    }
}
