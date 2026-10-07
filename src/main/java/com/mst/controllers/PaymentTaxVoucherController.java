package com.mst.controllers;

import com.mst.models.dto.PaymentTaxVoucherDto;
import com.mst.services.PaymentTaxVoucherService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screens 850 PaymentVoucherWithTax (mode 0), 852 frmCashPaymentVoucherTax (mode 1) and
 * 851 frmBankPaymentVoucherTax (mode 2) - one desktop class, opened by its Tag.
 * {mode} is that Tag; the document type travels as "doc" where the screen holds both (850).
 *
 * No delete route: the form's btnDelete is hidden and has no handler that deletes.
 */
@RestController
@RequestMapping("/accounts/api/payment-tax")
public class PaymentTaxVoucherController {

    private static final Logger LOG = LoggerFactory.getLogger(PaymentTaxVoucherController.class);

    @Autowired
    private PaymentTaxVoucherService service;

    @GetMapping("/{mode}/lookups")
    public ResponseEntity<Map<String, Object>> lookups(@PathVariable("mode") int mode) {
        try {
            return ResponseEntity.ok(service.lookups(mode));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        }
    }

    /** VoucherNofill - CommonServices.GenerateVoucherCode(CmbVoucherType.Value). */
    @GetMapping("/{mode}/next-code")
    public ResponseEntity<Map<String, Object>> nextCode(@PathVariable("mode") int mode, @RequestParam("doc") int doc) {
        try {
            PaymentTaxVoucherService.requireDoc(mode, doc);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        }
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("voucherCode", service.nextCode(doc));
        return ResponseEntity.ok(r);
    }

    /** AccountCurrentBalance / DetailAccountCurrentBalance. */
    @GetMapping("/balance")
    public ResponseEntity<Map<String, Object>> balance(@RequestParam("accountId") int accountId,
                                                       @RequestParam("date") String date) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("balance", service.balance(accountId, date));
        return ResponseEntity.ok(r);
    }

    @GetMapping("/subsidiary-balance")
    public ResponseEntity<Map<String, Object>> subsidiaryBalance(@RequestParam("accountId") int accountId,
                                                                 @RequestParam("subsidiaryId") int subsidiaryId,
                                                                 @RequestParam("date") String date) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("balance", service.subsidiaryBalance(accountId, subsidiaryId, date));
        return ResponseEntity.ok(r);
    }

    /** The tax schedule in force on the voucher date. */
    @GetMapping("/tax-schedule")
    public ResponseEntity<Map<String, Object>> taxSchedule(@RequestParam("taxTypeId") int taxTypeId,
                                                           @RequestParam("date") String date) {
        Map<String, Object> r = service.taxSchedule(taxTypeId, date);
        Map<String, Object> body = new LinkedHashMap<>();
        body.put("found", r != null);
        if (r != null) body.putAll(r);
        return ResponseEntity.ok(body);
    }

    /** HistoryFillCpv / HistoryFillBpv - USP_VoucherFormHistory with this tab's own document type. */
    @GetMapping("/{mode}/history")
    public ResponseEntity<Map<String, Object>> history(@PathVariable("mode") int mode, @RequestParam("doc") int doc,
            @RequestParam(value = "dateType", required = false) String dateType,
            @RequestParam(value = "fromDate", required = false) String fromDate,
            @RequestParam(value = "toDate", required = false) String toDate,
            @RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
            @RequestParam(value = "toDocNo", required = false) Integer toDocNo,
            @RequestParam(value = "accountId", required = false) Integer accountId,
            @RequestParam(value = "approvedStatus", defaultValue = "notapproved") String approvedStatus) {
        try {
            return ResponseEntity.ok(service.history(mode, doc, dateType, fromDate, toDate, fromDocNo, toDocNo, accountId, approvedStatus));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        }
    }

    @GetMapping("/{mode}/history/accounts")
    public ResponseEntity<List<Map<String, Object>>> historyAccounts(@PathVariable("mode") int mode, @RequestParam("doc") int doc) {
        return ResponseEntity.ok(service.historyAccounts(mode, doc));
    }

    /** ReadById(ID). */
    @GetMapping("/{mode}/{id}")
    public ResponseEntity<Map<String, Object>> load(@PathVariable("mode") int mode, @PathVariable("id") int id) {
        Map<String, Object> r = service.load(mode, id);
        if (r == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Voucher not found"));
        return ResponseEntity.ok(r);
    }

    /** History grid SelectionChanged -> the detail grid under it. */
    @GetMapping("/{mode}/{id}/lines")
    public ResponseEntity<Map<String, Object>> lines(@PathVariable("mode") int mode, @PathVariable("id") int id) {
        Map<String, Object> r = service.lines(mode, id);
        if (r == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Voucher not found"));
        return ResponseEntity.ok(r);
    }

    /** cmbCurrency_Leave - the last exchange rate used for this currency on this voucher type. */
    @GetMapping("/{mode}/last-rate")
    public ResponseEntity<Map<String, Object>> lastRate(@PathVariable("mode") int mode, @RequestParam("doc") int doc,
                                                        @RequestParam("currencyId") int currencyId) {
        Map<String, Object> r = new LinkedHashMap<>();
        try {
            r.put("lastRate", service.lastRate(mode, doc, currencyId));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        }
        return ResponseEntity.ok(r);
    }

    /**
     * Save (Id = 0) or Update (Id = the loaded voucher).
     *   200 {success, id, voucherCode, voucherAmount, detailLines, costCentreLines, message}
     *   409 {success:false, confirm: whtMismatch|negativeBalance|duplicate:<AccountId>|glBalance:<AccountId>, message}
     *       - a desktop Yes/No; re-post with the kind added to "acknowledged" to answer Yes.
     *   400 validation refusal in the desktop's words; 403 missing Save/Update right;
     *   500 a RAISERROR from the procedure chain, passed through.
     */
    @PostMapping("/{mode}/save")
    public ResponseEntity<Map<String, Object>> save(@PathVariable("mode") int mode, @RequestBody PaymentTaxVoucherDto dto) {
        try {
            return ResponseEntity.ok(service.save(mode, dto));
        } catch (PaymentTaxVoucherService.ConfirmationRequiredException e) {
            Map<String, Object> b = fail(e.getMessage());
            b.put("confirm", e.kind);
            return ResponseEntity.status(HttpStatus.CONFLICT).body(b);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (SecurityException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            LOG.warn("Payment voucher with tax save failed (mode {})", mode, e);
            Throwable t = e;
            while (t.getCause() != null && t.getCause() != t) t = t.getCause();
            String m = t.getMessage() == null ? e.getMessage() : t.getMessage();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(m == null ? "Save failed." : m));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
