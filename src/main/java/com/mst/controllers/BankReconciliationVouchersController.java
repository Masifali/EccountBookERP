package com.mst.controllers;

import com.mst.services.BankReconciliationVouchersService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Screen 885 "Bank Reconciliation With Vouchers" (frmBankReconciliationWithVouchers). Page:
 * /accounts/banking/bank-reconciliation (AccountsModuleViewController).
 */
@RestController
@RequestMapping("/accounts/api/bank-reconciliation-vouchers")
public class BankReconciliationVouchersController {

    @Autowired
    private BankReconciliationVouchersService service;

    @GetMapping("/init")
    public ResponseEntity<?> init() { return run(service::init); }

    @GetMapping("/bank-accounts")
    public ResponseEntity<?> bankAccounts() { return run(service::bankAccounts); }

    @GetMapping("/history-banks")
    public ResponseEntity<?> historyBanks() { return run(service::historyBanks); }

    @GetMapping("/show")
    public ResponseEntity<?> show(@RequestParam("bankAccountId") int bankAccountId) { return run(() -> service.show(bankAccountId)); }

    @GetMapping("/read")
    public ResponseEntity<?> read(@RequestParam("bankAccountId") int bankAccountId, @RequestParam("voucherHeadId") int voucherHeadId) {
        return run(() -> service.read(bankAccountId, voucherHeadId));
    }

    @GetMapping("/slip")
    public ResponseEntity<?> slip(@RequestParam(value = "bankAccountId", defaultValue = "0") int bankAccountId,
                                  @RequestParam(value = "voucherHeadId", defaultValue = "0") int voucherHeadId) {
        return run(() -> service.slip(bankAccountId, voucherHeadId));
    }

    @GetMapping("/history")
    public ResponseEntity<?> history(@RequestParam(value = "dateType", defaultValue = "doc") String dateType,
                                     @RequestParam(value = "from", required = false) String from,
                                     @RequestParam(value = "to", required = false) String to,
                                     @RequestParam(value = "bankAccountId", defaultValue = "0") int bankAccountId) {
        return run(() -> service.history(dateType, from, to, bankAccountId));
    }

    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody BankReconciliationVouchersService.Request r) { return run(() -> service.save(r)); }

    @PostMapping("/delete")
    public ResponseEntity<?> delete(@RequestBody BankReconciliationVouchersService.Request r) { return run(() -> service.delete(r)); }

    private ResponseEntity<?> run(Supplier<Object> body) {
        try {
            return ResponseEntity.ok(body.get());
        } catch (BankReconciliationVouchersService.Refusal e) {
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
