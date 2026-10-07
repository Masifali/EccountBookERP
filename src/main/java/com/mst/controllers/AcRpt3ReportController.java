package com.mst.controllers;

import com.mst.services.AcRpt3ReportService;
import com.mst.services.AccountReportsHSupport;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

/** R3 account reports 60 / 67 / 85 / 86 - REST endpoints used by the four new pages (tenancy from CurrentUserContext). */
@RestController
@RequestMapping("/api/accounts/acrpt3")
public class AcRpt3ReportController {
    private final AcRpt3ReportService service;

    public AcRpt3ReportController(AcRpt3ReportService service) { this.service = service; }

    // 60 FcyGeneralLedgerRpt
    @GetMapping("/fcygl/lookups")
    public ResponseEntity<?> fcyGlLookups() { return run(() -> service.fcyGlLookups()); }

    @GetMapping("/fcygl/data")
    public ResponseEntity<?> fcyGlData(@RequestParam String mode, @RequestParam(defaultValue = "0") int accountId,
                                       @RequestParam String fromDate, @RequestParam String toDate) {
        return run(() -> service.fcyGlData(mode, accountId, fromDate, toDate));
    }

    // 85 FcyBankCharges_Register
    @GetMapping("/bankcharges/lookups")
    public ResponseEntity<?> bankChargesLookups() { return run(() -> service.bankChargesLookups()); }

    @GetMapping("/bankcharges/data")
    public ResponseEntity<?> bankChargesData(@RequestParam(defaultValue = "0") int bankId,
                                             @RequestParam(defaultValue = "0") int chargesAccountId,
                                             @RequestParam(defaultValue = "") String fdbcNo) {
        return run(() -> service.bankChargesData(bankId, chargesAccountId, fdbcNo));
    }

    // 86 ProfitLoss
    @GetMapping("/pl/lookups")
    public ResponseEntity<?> plLookups() { return run(() -> service.profitLossLookups()); }

    @GetMapping("/pl/data")
    public ResponseEntity<?> plData(@RequestParam String fromDate, @RequestParam String toDate,
                                    @RequestParam(defaultValue = "") String branchText,
                                    @RequestParam(required = false) Integer branchId) {
        return run(() -> service.profitLossData(fromDate, toDate, branchText, branchId));
    }

    @GetMapping("/pl/print-rows")
    public ResponseEntity<?> plPrintRows(@RequestParam String fromDate, @RequestParam String toDate,
                                         @RequestParam(defaultValue = "") String branchText,
                                         @RequestParam(required = false) Integer branchId) {
        return run(() -> service.profitLossPrintRows(fromDate, toDate, branchText, branchId));
    }

    // 67 CostomerWiseVoucherSlip
    @GetMapping("/cv/lookups")
    public ResponseEntity<?> cvLookups() { return run(() -> service.voucherSlipLookups()); }

    @GetMapping("/cv/cashbank")
    public ResponseEntity<?> cvCashBank(@RequestParam String date) { return run(() -> service.cashBanks(date)); }

    @GetMapping("/cv/parties")
    public ResponseEntity<?> cvParties(@RequestParam String date) { return run(() -> service.parties(date)); }

    @GetMapping("/cv/data")
    public ResponseEntity<?> cvData(@RequestParam String date, @RequestParam(defaultValue = "0") int partyId,
                                    @RequestParam(defaultValue = "") String slipFlag,
                                    @RequestParam(defaultValue = "false") boolean cash,
                                    @RequestParam(defaultValue = "false") boolean bank,
                                    @RequestParam(defaultValue = "") String accountIds) {
        return run(() -> service.voucherSlipData(date, partyId, slipFlag, cash, bank, accountIds));
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c) {
        try {
            Object body = c.get();
            return ResponseEntity.ok(body);
        } catch (IllegalArgumentException | AccountReportsHSupport.Refusal e) {
            return fail(400, e);
        } catch (SecurityException e) {
            return fail(403, e);
        } catch (Exception e) {
            Throwable t = e;
            while (t.getCause() != null && t.getCause() != t) t = t.getCause();
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("message", t.getMessage() == null ? t.getClass().getSimpleName() : t.getMessage());
            return ResponseEntity.status(500).body(m);
        }
    }

    private static ResponseEntity<?> fail(int status, Exception e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("message", e.getMessage() == null ? e.getClass().getSimpleName() : e.getMessage());
        return ResponseEntity.status(status).body(m);
    }
}
