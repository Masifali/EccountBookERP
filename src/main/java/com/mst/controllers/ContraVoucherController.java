package com.mst.controllers;

import com.mst.models.dto.ContraVoucherDto;
import com.mst.services.ContraVoucherService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Screen 22 "Contra Voucher" — the desktop-parity write path.
 *
 * ---------------------------------------------------------------------------------------------
 * A SEPARATE NAMESPACE, ON PURPOSE
 * ---------------------------------------------------------------------------------------------
 * The generic voucher API lives at {@code /accounts/api/vouchers/...} and is shared by ten
 * screens. This one is {@code /accounts/api/contra/...} so that moving Contra onto the desktop's
 * stored-procedure chain cannot change what any of the other nine writes. Each of those gets its
 * own trace and its own migration; until then they keep the path they have.
 *
 * The Contra page keeps using the shared endpoints for its READ side (history, lookups) — only
 * the SAVE moves here.
 *
 * ---------------------------------------------------------------------------------------------
 * THERE IS NO DELETE ROUTE, AND THAT IS THE PARITY
 * ---------------------------------------------------------------------------------------------
 * {@code ContraVoucher.btnDelete_Click} is an empty method on the desktop — the form's Delete
 * button does nothing at all. Offering a delete here would be a capability the desktop does not
 * have, on live accounting documents. So there is none.
 */
@RestController
@RequestMapping("/accounts/api/contra")
public class ContraVoucherController {

    @Autowired
    private ContraVoucherService service;

    /**
     * Save or update. The body carries GRID ROWS, not ledger lines — the debit/credit pair is
     * derived server-side from the desktop's rule.
     *
     * Responses:
     *   200 {success:true, id, voucherCode, voucherAmount, detailLines, costCentreLines, message}
     *   409 {success:false, confirm:"duplicate"|"negativeBalance", message}
     *       — a desktop Yes/No the operator has to answer. Re-post with the matching
     *         acknowledgement flag set to proceed, exactly as clicking Yes does on the desktop.
     *   400 {success:false, message} — a validation refusal, in the desktop's own wording.
     *   403 {success:false, message} — the Save or Update right is missing.
     */
    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody ContraVoucherDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
        } catch (ContraVoucherService.ConfirmationRequiredException e) {
            Map<String, Object> body = fail(e.getMessage());
            body.put("confirm", e.kind);
            return ResponseEntity.status(HttpStatus.CONFLICT).body(body);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            /* A RAISERROR out of USP_VoucherBalanceCheck or Sp_VoucherHead_Update arrives here.
               Its text is the desktop's own message — "Debit Amount not equal to Credit Amount…",
               "Record cannot be Updated because record has approved" — so it is passed through
               rather than replaced with something of my own invention. */
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(msg(e, "Save failed.")));
        }
    }

    /** CommonServices.GenerateVoucherCode(10). */
    @GetMapping("/next-code")
    public ResponseEntity<Map<String, Object>> nextCode() {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("voucherCode", service.nextVoucherCode());
        return ResponseEntity.ok(r);
    }

    private static String msg(Exception e, String fallback) {
        String m = e.getMessage();
        return (m == null || m.trim().isEmpty()) ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
