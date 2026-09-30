package com.mst.controllers;

import com.mst.models.dto.InvoicesAdjustmentVoucherDto;
import com.mst.services.InvoicesAdjustmentVoucherService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Screens 861 (kind {@code payment}, frmInvoicesAdjustmentVoucher) and 863 (kind {@code receipt},
 * frmReceiptInvoicesAdjustmentVoucher). Pages: /accounts/vouchers/invoices-adjustment and
 * /accounts/vouchers/receipt-invoices-adjustment.
 */
@RestController
@RequestMapping("/accounts/api/adjustment-voucher/{kind}")
public class InvoicesAdjustmentVoucherController {

    @Autowired
    private InvoicesAdjustmentVoucherService service;

    @GetMapping("/init")
    public ResponseEntity<?> init(@PathVariable("kind") String kind) { return run(() -> service.init(kind)); }

    @GetMapping("/pending")
    public ResponseEntity<?> pending(@PathVariable("kind") String kind) { return run(() -> service.pending(kind)); }

    @GetMapping("/invoices")
    public ResponseEntity<?> invoices(@PathVariable("kind") String kind, @RequestParam("supplierCustomerId") int supplierCustomerId) {
        return run(() -> service.invoices(kind, supplierCustomerId));
    }

    @GetMapping("/by-voucher-head")
    public ResponseEntity<?> byVoucherHead(@PathVariable("kind") String kind,
                                           @RequestParam("voucherHeadId") int voucherHeadId,
                                           @RequestParam("paymentTypeId") int paymentTypeId,
                                           @RequestParam(value = "supplierCustomerId", defaultValue = "0") int supplierCustomerId,
                                           @RequestParam(value = "partyGlId", defaultValue = "0") int partyGlId) {
        return run(() -> service.byVoucherHead(kind, voucherHeadId, paymentTypeId, supplierCustomerId, partyGlId));
    }

    @GetMapping("/parties")
    public ResponseEntity<?> parties(@PathVariable("kind") String kind) { return run(() -> service.parties(kind)); }

    @GetMapping("/history")
    public ResponseEntity<?> history(@PathVariable("kind") String kind,
                                     @RequestParam(value = "dateType", defaultValue = "doc") String dateType,
                                     @RequestParam(value = "from", required = false) String from,
                                     @RequestParam(value = "to", required = false) String to,
                                     @RequestParam(value = "fromDocNo", defaultValue = "0") int fromDocNo,
                                     @RequestParam(value = "toDocNo", defaultValue = "0") int toDocNo,
                                     @RequestParam(value = "supplierCustomerId", defaultValue = "0") int supplierCustomerId,
                                     @RequestParam(value = "glAccountId", defaultValue = "0") int glAccountId) {
        return run(() -> service.history(kind, dateType, from, to, fromDocNo, toDocNo, supplierCustomerId, glAccountId));
    }

    /** 200 {success, message} · 400 a desktop refusal · 403 a missing right · 500 a RAISERROR, verbatim. */
    @PostMapping("/save")
    public ResponseEntity<?> save(@PathVariable("kind") String kind, @RequestBody InvoicesAdjustmentVoucherDto dto) {
        return run(() -> service.save(kind, dto));
    }

    @PostMapping("/delete")
    public ResponseEntity<?> delete(@PathVariable("kind") String kind, @RequestBody InvoicesAdjustmentVoucherDto dto) {
        return run(() -> service.delete(kind, dto));
    }

    private ResponseEntity<?> run(Supplier<Object> body) {
        try {
            return ResponseEntity.ok(body.get());
        } catch (InvoicesAdjustmentVoucherService.Refusal e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(DayBookVoucherController.rootMessage(e)));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
