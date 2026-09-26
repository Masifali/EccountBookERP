package com.mst.controllers;

import com.mst.models.dto.FreightVoucherDto;
import com.mst.services.FreightVoucherService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Freight Payment Voucher (FreightVoucher.cs, DocumentTypeId 28) — the page is
 * {@code /accounts/vouchers/freight}, served by AccountsModuleViewController.
 *
 * Its own namespace, like Contra's, so nothing here can change what the shared voucher API
 * writes for the other screens. The desktop form has no Delete, so there is no delete route.
 */
@RestController
@RequestMapping("/accounts/api/freight")
public class FreightVoucherController {

    @Autowired
    private FreightVoucherService service;

    @GetMapping("/init")
    public ResponseEntity<?> init() { return run(service::init); }

    @GetMapping("/document-no")
    public ResponseEntity<?> documentNo() {
        return run(() -> { Map<String, Object> m = new LinkedHashMap<>(); m.put("documentNo", service.generateCode()); return m; });
    }

    @GetMapping("/pending")
    public ResponseEntity<?> pending() { return run(service::pending); }

    @GetMapping("/history-combos")
    public ResponseEntity<?> historyCombos() { return run(service::historyCombos); }

    @GetMapping("/accounts")
    public ResponseEntity<?> accounts() { return run(service::globalAccounts); }

    @GetMapping("/cities")
    public ResponseEntity<?> cities() { return run(service::cities); }

    @GetMapping("/drivers")
    public ResponseEntity<?> drivers() { return run(service::drivers); }

    @GetMapping("/config")
    public ResponseEntity<?> config() { return run(service::config); }

    @GetMapping("/balance")
    public ResponseEntity<?> balance(@RequestParam("accountId") int accountId) {
        return run(() -> service.balance(accountId));
    }

    @GetMapping("/previous-city")
    public ResponseEntity<?> previousCity(@RequestParam("cityId") int cityId,
                                          @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return run(() -> service.previousCity(cityId, recId));
    }

    @GetMapping("/instrument-types")
    public ResponseEntity<?> instrumentTypes() { return run(service::instrumentTypes); }

    @GetMapping("/cheques")
    public ResponseEntity<?> cheques(@RequestParam("bankId") int bankId,
                                     @RequestParam(value = "voucherId", defaultValue = "0") int voucherId) {
        return run(() -> service.cheques(bankId, voucherId));
    }

    @PostMapping("/register")
    public ResponseEntity<?> register(@RequestBody FreightVoucherDto.RegisterFilter filter) {
        return run(() -> service.register(filter));
    }

    @GetMapping("/read/{id}")
    public ResponseEntity<?> read(@PathVariable("id") int id) { return run(() -> service.readById(id)); }

    @GetMapping("/slip241/{id}")
    public ResponseEntity<?> slip241(@PathVariable("id") int id) { return run(() -> service.slip241(id)); }

    @GetMapping("/voucher102/{voucherHeadId}")
    public ResponseEntity<?> voucher102(@PathVariable("voucherHeadId") int voucherHeadId) {
        return run(() -> service.voucher102(voucherHeadId));
    }

    /**
     * 200 {success, id, voucherHeadId, documentNo, message}
     * 400 a refusal in the desktop's words · 403 a missing Save/Update right ·
     * 500 a RAISERROR from a procedure, passed through verbatim (e.g. "Record already exist
     * against this GpNo Please Check!", "PaidAmount cannot be greater than Bilty FreightAmount…").
     */
    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody FreightVoucherDto dto) { return run(() -> service.save(dto)); }

    private ResponseEntity<?> run(Supplier<Object> body) {
        try {
            return ResponseEntity.ok(body.get());
        } catch (FreightVoucherService.Refusal e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(rootMessage(e)));
        }
    }

    /** A procedure's RAISERROR text sits at the bottom of Spring's exception chain. */
    private static String rootMessage(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return (m == null || m.trim().isEmpty()) ? "Request failed." : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
