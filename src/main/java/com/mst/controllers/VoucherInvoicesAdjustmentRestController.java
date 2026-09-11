package com.mst.controllers;

import com.mst.models.dto.VoucherInvoicesAdjustmentDto;
import com.mst.services.VoucherInvoicesAdjustmentService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDate;
import java.util.Map;

/**
 * REST endpoints for the Voucher Invoices Adjustment screen - ditto
 * Architecture.WinApp.Account_Definition.AdjustmentVouchers.frmInvoicesAdjustmentVoucher.cs.
 * See VoucherInvoicesAdjustmentService for the real stored-procedure mapping and evidence.
 */
@RestController
@RequestMapping("/accounts/api/vouchers/invoices-adjustment")
public class VoucherInvoicesAdjustmentRestController {

    @Autowired
    private VoucherInvoicesAdjustmentService service;

    @GetMapping("/outstanding")
    public ResponseEntity<?> getOutstanding(@RequestParam(value = "accountId", required = false) Integer accountId) {
        return ResponseEntity.ok(service.getOutstandingForAdjustment(accountId));
    }

    @GetMapping("/purchase-invoices")
    public ResponseEntity<?> getPurchaseInvoices(@RequestParam("supplierCustomerId") int supplierCustomerId,
            @RequestParam(value = "partyGlId", required = false) Integer partyGlId) {
        return ResponseEntity.ok(service.getPurchaseInvoicesForAdjustment(supplierCustomerId, partyGlId));
    }

    @GetMapping("/by-voucher-head")
    public ResponseEntity<?> getByVoucherHead(@RequestParam("voucherHeadId") int voucherHeadId,
            @RequestParam("paymentTypeId") int paymentTypeId,
            @RequestParam(value = "supplierCustomerId", required = false) Integer supplierCustomerId,
            @RequestParam(value = "partyGlId", required = false) Integer partyGlId) {
        return ResponseEntity.ok(service.getByVoucherHeadId(voucherHeadId, paymentTypeId, supplierCustomerId, partyGlId));
    }

    @GetMapping("/party-dropdown")
    public ResponseEntity<?> getPartyDropDown() {
        return ResponseEntity.ok(service.getPartyDropDown());
    }

    @GetMapping("/history")
    public ResponseEntity<?> getHistory(
            @RequestParam(value = "dateType", required = false) String dateType,
            @RequestParam(value = "fromDate", required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam(value = "toDate", required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate,
            @RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
            @RequestParam(value = "toDocNo", required = false) Integer toDocNo,
            @RequestParam(value = "supplierCustomerId", required = false) Integer supplierCustomerId,
            @RequestParam(value = "partyGlId", required = false) Integer partyGlId) {
        return ResponseEntity.ok(service.getFormHistory(dateType, fromDate, toDate, fromDocNo, toDocNo, supplierCustomerId, partyGlId));
    }

    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody VoucherInvoicesAdjustmentDto dto) {
        Map<String, Object> res = service.save(dto);
        if (Boolean.TRUE.equals(res.get("success"))) {
            return ResponseEntity.ok(res);
        }
        return ResponseEntity.status(400).body(res);
    }

    @PostMapping("/delete")
    public ResponseEntity<?> delete(@RequestBody VoucherInvoicesAdjustmentDto dto) {
        Map<String, Object> res = service.deleteAll(dto);
        if (Boolean.TRUE.equals(res.get("success"))) {
            return ResponseEntity.ok(res);
        }
        return ResponseEntity.status(400).body(res);
    }
}
