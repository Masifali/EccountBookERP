package com.mst.controllers;

import com.mst.models.dto.ExpenseTaxVoucherDto;
import com.mst.services.ContraVoucherService;
import com.mst.services.ExpenseTaxVoucherService;
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
 * Screen 862 ExpenseVoucherNew ("Expense Voucher New"). No delete route: the form's btnDelete handler is empty.
 */
@RestController
@RequestMapping("/accounts/api/expense-tax")
public class ExpenseTaxVoucherController {

    private static final Logger LOG = LoggerFactory.getLogger(ExpenseTaxVoucherController.class);

    @Autowired
    private ExpenseTaxVoucherService service;

    @GetMapping("/lookups")
    public ResponseEntity<Map<String, Object>> lookups() {
        return ResponseEntity.ok(service.lookups());
    }

    /** VoucherNofill - CommonServices.GenerateVoucherCode(26). */
    @GetMapping("/next-code")
    public ResponseEntity<Map<String, Object>> nextCode() {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("voucherCode", service.nextCode());
        return ResponseEntity.ok(r);
    }

    /** combcreditac_Leave / combactitle_Leave - the account balance on the voucher date. */
    @GetMapping("/balance")
    public ResponseEntity<Map<String, Object>> balance(@RequestParam("accountId") int accountId, @RequestParam("date") String date) {
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

    /** BindSubsidiaryAccount for the detail account picked. */
    @GetMapping("/subsidiaries")
    public ResponseEntity<List<Map<String, Object>>> subsidiaries(@RequestParam("accountId") int accountId) {
        return ResponseEntity.ok(service.subsidiaries(accountId));
    }

    /** CheqNoFill - outstanding cheques of the credit (bank) account. */
    @GetMapping("/cheques")
    public ResponseEntity<List<Map<String, Object>>> cheques(@RequestParam("bankId") int bankId,
                                                             @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return ResponseEntity.ok(service.cheques(bankId, recId));
    }

    /** cmbCurrency_Leave - the last exchange rate used for this currency on a type-26 voucher. */
    @GetMapping("/last-rate")
    public ResponseEntity<Map<String, Object>> lastRate(@RequestParam("currencyId") int currencyId) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("lastRate", service.lastRate(currencyId));
        return ResponseEntity.ok(r);
    }

    /** HistoryFill - USP_VoucherFormHistory for document type 26. */
    @GetMapping("/history")
    public ResponseEntity<Map<String, Object>> history(@RequestParam(value = "dateType", required = false) String dateType,
            @RequestParam(value = "fromDate", required = false) String fromDate,
            @RequestParam(value = "toDate", required = false) String toDate,
            @RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
            @RequestParam(value = "toDocNo", required = false) Integer toDocNo,
            @RequestParam(value = "accountId", required = false) Integer accountId,
            @RequestParam(value = "approvedStatus", defaultValue = "notapproved") String approvedStatus) {
        try {
            return ResponseEntity.ok(service.history(dateType, fromDate, toDate, fromDocNo, toDocNo, accountId, approvedStatus));
        } catch (Exception e) {
            LOG.warn("Expense voucher new history failed", e);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(rootMessage(e, "History could not be loaded.")));
        }
    }

    /** ReadById(ID). */
    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> load(@PathVariable("id") int id) {
        Map<String, Object> r = service.load(id);
        if (r == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Voucher not found"));
        return ResponseEntity.ok(r);
    }

    /** VoucherDetailByHeaderId - the detail grid under the history grid. */
    @GetMapping("/{id}/lines")
    public ResponseEntity<Map<String, Object>> lines(@PathVariable("id") int id) {
        Map<String, Object> r = service.lines(id);
        if (r == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Voucher not found"));
        return ResponseEntity.ok(r);
    }

    /**
     * Save (Id = 0) or Update (Id = the loaded voucher).
     *   200 {success, id, voucherCode, voucherAmount, warnings, message}
     *   409 {success:false, confirm:"negativeBalance"|"duplicate", message} - a desktop Yes/No; re-post with
     *       NegativeBalanceAcknowledged / DuplicateAcknowledged true to answer Yes.
     *   400 validation refusal in the desktop's words; 403 missing Save/Update right; 500 a RAISERROR passed through.
     */
    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody ExpenseTaxVoucherDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
        } catch (ContraVoucherService.ConfirmationRequiredException e) {
            Map<String, Object> b = fail(e.getMessage());
            b.put("confirm", e.kind);
            return ResponseEntity.status(HttpStatus.CONFLICT).body(b);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (SecurityException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            LOG.warn("Expense voucher new save failed", e);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(rootMessage(e, "Save failed.")));
        }
    }

    private static String rootMessage(Exception e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage() == null ? e.getMessage() : t.getMessage();
        return m == null ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
