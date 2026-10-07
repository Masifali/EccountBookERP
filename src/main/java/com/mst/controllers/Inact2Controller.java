package com.mst.controllers;

import com.mst.services.Inact2Service;
import java.time.LocalDate;
import java.util.*;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.web.bind.annotation.*;

/** Inactive Account_Reports group K data endpoints (54 / 55 / 57 / 58 / 59); the pages are in Inact2PageController. */
@RestController
@RequestMapping("/api/accounts/inact2")
public class Inact2Controller {
    private final Inact2Service service;

    public Inact2Controller(Inact2Service service) { this.service = service; }

    // ---- 54 GeneralLedgerStatment
    @GetMapping("/ledger-statement/lookups")
    public Map<String, Object> ledgerStatementLookups() { return service.ledgerStatementLookups(); }

    /** btnshow_Click -> VoucherReports.GeneralLedgerStatement (branchId / projectId 0 = not sent). */
    @GetMapping("/ledger-statement")
    public List<Map<String, Object>> ledgerStatement(@RequestParam(defaultValue = "0") int accountId,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate,
            @RequestParam(defaultValue = "0") int branchId, @RequestParam(defaultValue = "0") int projectId) {
        return service.ledgerStatement(accountId, fromDate, toDate, branchId, projectId);
    }

    // ---- 55 AccountsBalanceSheetStandardRpt
    @GetMapping("/balance-sheet/lookups")
    public Map<String, Object> balanceSheetLookups() { return service.balanceSheetLookups(); }

    /** GridBind -> VoucherReports.AccountsBalanceSheetStandardRpt. */
    @GetMapping("/balance-sheet")
    public List<Map<String, Object>> balanceSheet(@RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate) {
        return service.balanceSheet(toDate);
    }

    /** GridBind1 -> VoucherReports.AccountsProfitLoassStandard. */
    @GetMapping("/profit-loss")
    public List<Map<String, Object>> profitLoss(@RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate) {
        return service.profitLoss(fromDate, toDate);
    }

    // ---- 57 GeneralJournalSummeryRegister
    @GetMapping("/general-journal/lookups")
    public Map<String, Object> generalJournalLookups() { return service.generalJournalLookups(); }

    @GetMapping("/general-journal")
    public List<Map<String, Object>> generalJournal() { return service.generalJournal(); }

    // ---- 58 PayablesAging
    @GetMapping("/payables-aging/lookups")
    public Map<String, Object> payablesAgingLookups() { return service.payablesAgingLookups(); }

    @GetMapping("/payables-aging")
    public List<Map<String, Object>> payablesAging(@RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate,
            @RequestParam(defaultValue = "0") int agingDays, @RequestParam(defaultValue = "0") int intervalDays,
            @RequestParam(defaultValue = "false") boolean skipZero) {
        return Inact2Service.stripBinary(service.payablesAging(endDate, agingDays, intervalDays, skipZero));
    }

    // ---- 59 ReceivableAging
    @GetMapping("/receivable-aging/lookups")
    public Map<String, Object> receivableAgingLookups() { return service.receivableAgingLookups(); }

    @GetMapping("/receivable-aging")
    public List<Map<String, Object>> receivableAging(@RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate,
            @RequestParam(defaultValue = "0") int intervalDays) {
        return Inact2Service.stripBinary(service.receivableAging(endDate, intervalDays));
    }

    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class)
    public org.springframework.http.ResponseEntity<String> denied(org.springframework.security.access.AccessDeniedException e) {
        return org.springframework.http.ResponseEntity.status(403).contentType(org.springframework.http.MediaType.TEXT_PLAIN).body(e.getMessage());
    }

    @ExceptionHandler({IllegalArgumentException.class, IllegalStateException.class, org.springframework.dao.DataAccessException.class})
    public org.springframework.http.ResponseEntity<String> failed(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage() == null ? e.getMessage() : t.getMessage();
        return org.springframework.http.ResponseEntity.badRequest().contentType(org.springframework.http.MediaType.TEXT_PLAIN).body(m);
    }
}
