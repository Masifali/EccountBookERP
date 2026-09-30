package com.mst.controllers;

import com.mst.models.dto.PaymentReceiptVoucherDto;
import com.mst.services.PaymentReceiptVoucherService;
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
import java.util.Map;

/**
 * Screens 28-31: Cash Payment (1), Bank Payment (2), Cash Receipt (3), Bank Receipt Voucher (4).
 * {doc} is the DocumentTypeId the page is (fixed per page, as CmbVoucherType / combvtype holds a
 * single row for the Tag the form was opened with).
 *
 * A separate namespace from /accounts/api/vouchers so that moving these four onto the desktop's
 * procedure chain cannot change what any other voucher writes. History keeps the shared
 * /accounts/api/vouchers/{cpv|bpv|crv|brv}/history endpoints (USP_VoucherFormHistory).
 *
 * No delete route: PaymentVoucherNew.btnDelete_Click and ReceiptsVoucherNew.btnDelete_Click are
 * empty methods on the desktop.
 */
@RestController
@RequestMapping("/accounts/api/payment-receipt")
public class PaymentReceiptVoucherController {

    private static final Logger LOG = LoggerFactory.getLogger(PaymentReceiptVoucherController.class);

    @Autowired
    private PaymentReceiptVoucherService service;

    @GetMapping("/{doc}/lookups")
    public ResponseEntity<Map<String, Object>> lookups(@PathVariable("doc") int doc) {
        try {
            return ResponseEntity.ok(service.lookups(doc));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        }
    }

    @GetMapping("/{doc}/next-code")
    public ResponseEntity<Map<String, Object>> nextCode(@PathVariable("doc") int doc) {
        PaymentReceiptVoucherService.requireDoc(doc);
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

    /** checkBox1_CheckedChanged / CmbTaxType_Leave: the tax schedule in force on the voucher date. */
    @GetMapping("/tax-schedule")
    public ResponseEntity<Map<String, Object>> taxSchedule(@RequestParam("taxTypeId") int taxTypeId,
                                                           @RequestParam("date") String date) {
        Map<String, Object> r = service.taxSchedule(taxTypeId, date);
        Map<String, Object> body = new LinkedHashMap<>();
        body.put("found", r != null);
        if (r != null) body.putAll(r);
        return ResponseEntity.ok(body);
    }

    /** HistoryFillCpv / Bpv / Crv / Brv - USP_VoucherFormHistory with this form's own DocumentTypeName. */
    @GetMapping("/{doc}/history")
    public ResponseEntity<Map<String, Object>> history(@PathVariable("doc") int doc,
            @RequestParam(value = "dateType", required = false) String dateType,
            @RequestParam(value = "fromDate", required = false) String fromDate,
            @RequestParam(value = "toDate", required = false) String toDate,
            @RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
            @RequestParam(value = "toDocNo", required = false) Integer toDocNo,
            @RequestParam(value = "accountId", required = false) Integer accountId,
            @RequestParam(value = "approvedStatus", defaultValue = "notapproved") String approvedStatus) {
        return ResponseEntity.ok(service.history(doc, dateType, fromDate, toDate, fromDocNo, toDocNo, accountId, approvedStatus));
    }

    /** ComboBindForXxxHistory - the history "Account Title" filter. */
    @GetMapping("/{doc}/history/accounts")
    public ResponseEntity<java.util.List<Map<String, Object>>> historyAccounts(@PathVariable("doc") int doc) {
        return ResponseEntity.ok(service.historyAccounts(doc));
    }

    /** ReadById(). */
    @GetMapping("/{doc}/{id}")
    public ResponseEntity<Map<String, Object>> load(@PathVariable("doc") int doc, @PathVariable("id") int id) {
        Map<String, Object> r = service.load(doc, id);
        if (r == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Voucher not found"));
        return ResponseEntity.ok(r);
    }

    /**
     * Save (Id = 0) or Update (Id = the loaded voucher).
     *   200 {success, id, voucherCode, voucherAmount, detailLines, costCentreLines, message}
     *   409 {success:false, confirm: whtMismatch|negativeBalance|duplicate|glBalance, message}
     *       - a desktop Yes/No; re-post with the kind added to "acknowledged" to answer Yes.
     *   400 validation refusal in the desktop's words; 403 missing Save/Update right;
     *   500 a RAISERROR from the procedure chain (e.g. USP_VoucherBalanceCheck, approved record), passed through.
     */
    @PostMapping("/{doc}/save")
    public ResponseEntity<Map<String, Object>> save(@PathVariable("doc") int doc,
                                                    @RequestBody PaymentReceiptVoucherDto dto) {
        try {
            return ResponseEntity.ok(service.save(doc, dto));
        } catch (PaymentReceiptVoucherService.ConfirmationRequiredException e) {
            Map<String, Object> b = fail(e.getMessage());
            b.put("confirm", e.kind);
            return ResponseEntity.status(HttpStatus.CONFLICT).body(b);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (SecurityException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            LOG.warn("Payment/receipt voucher save failed (doc {})", doc, e);
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
