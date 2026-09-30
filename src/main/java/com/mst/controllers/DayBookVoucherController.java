package com.mst.controllers;

import com.mst.models.dto.DayBookVoucherDto;
import com.mst.services.DayBookVoucherService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Screens 15 "Day Book" (DayBook.cs, kind {@code cash}) and 24 "Day Book (Off Set)"
 * (frmDayBook.cs, kind {@code offset}). Pages: /accounts/vouchers/day-book and
 * /accounts/vouchers/day-book-offset (AccountsModuleViewController).
 *
 * Neither desktop form has a Delete, so there is no delete route.
 */
@RestController
@RequestMapping("/accounts/api/day-book")
public class DayBookVoucherController {

    @Autowired
    private DayBookVoucherService service;

    @GetMapping("/{kind}/init")
    public ResponseEntity<?> init(@PathVariable("kind") String kind) { return run(() -> service.init(kind)); }

    @GetMapping("/{kind}/accounts")
    public ResponseEntity<?> accounts(@PathVariable("kind") String kind) { return run(() -> service.accounts(kind, true)); }

    @GetMapping("/{kind}/code")
    public ResponseEntity<?> code(@PathVariable("kind") String kind) {
        return run(() -> { Map<String, Object> m = new LinkedHashMap<>(); m.put("voucherCode", service.nextCode(DayBookVoucherService.docType(kind))); return m; });
    }

    @GetMapping("/{kind}/voucher-dates")
    public ResponseEntity<?> voucherDates(@PathVariable("kind") String kind) {
        return run(() -> service.voucherDates(DayBookVoucherService.docType(kind)));
    }

    @GetMapping("/{kind}/history")
    public ResponseEntity<?> history(@PathVariable("kind") String kind,
                                     @RequestParam(value = "voucherDate", required = false) String voucherDate,
                                     @RequestParam(value = "cashAccountId", defaultValue = "0") int cashAccountId) {
        return run(() -> service.history(kind, voucherDate, cashAccountId));
    }

    @GetMapping("/balance")
    public ResponseEntity<?> balance(@RequestParam("accountId") int accountId, @RequestParam("date") String date) {
        return run(() -> service.balance(accountId, date));
    }

    @GetMapping("/subsidiary-accounts")
    public ResponseEntity<?> subsidiaryAccounts() { return run(service::subsidiaryAccounts); }

    @GetMapping("/cheques")
    public ResponseEntity<?> cheques(@RequestParam("bankId") int bankId) { return run(() -> service.cheques(bankId)); }

    @GetMapping("/last-rate")
    public ResponseEntity<?> lastRate(@RequestParam("currencyId") int currencyId) { return run(() -> service.lastRate(currencyId)); }

    @GetMapping("/read/{id}")
    public ResponseEntity<?> read(@PathVariable("id") int id) { return run(() -> service.read(id)); }

    @GetMapping("/slip144")
    public ResponseEntity<?> slip144(@RequestParam(value = "documentTypeId", defaultValue = "0") int documentTypeId,
                                     @RequestParam(value = "id", defaultValue = "0") int id,
                                     @RequestParam(value = "accountId", defaultValue = "0") int accountId,
                                     @RequestParam(value = "skipCash", defaultValue = "false") boolean skipCash) {
        return run(() -> service.daybookSlip(documentTypeId, id, accountId, skipCash));
    }

    @GetMapping("/slip102/{id}")
    public ResponseEntity<?> slip102(@PathVariable("id") int id) {
        return run(() -> service.voucherSlip102(id, DayBookVoucherService.DOC_OFFSET));
    }

    /**
     * 200 {success, id, message} · 400 a desktop refusal · 403 a missing right ·
     * 409 {confirm} a Yes/No the operator has to answer · 500 a RAISERROR, verbatim.
     */
    @PostMapping("/cash/save")
    public ResponseEntity<?> saveCash(@RequestBody DayBookVoucherDto dto) { return run(() -> service.saveCash(dto)); }

    @PostMapping("/offset/save")
    public ResponseEntity<?> saveOffset(@RequestBody DayBookVoucherDto dto) { return run(() -> service.saveOffset(dto)); }

    private ResponseEntity<?> run(Supplier<Object> body) {
        try {
            return ResponseEntity.ok(body.get());
        } catch (DayBookVoucherService.Refusal e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (DayBookVoucherService.Confirm e) {
            Map<String, Object> m = fail(e.getMessage());
            m.put("confirm", e.kind);
            return ResponseEntity.status(HttpStatus.CONFLICT).body(m);
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(rootMessage(e)));
        }
    }

    static String rootMessage(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return (m == null || m.trim().isEmpty()) ? "Request failed." : m;
    }

    static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
