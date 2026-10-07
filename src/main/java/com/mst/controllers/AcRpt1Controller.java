package com.mst.controllers;

import com.mst.services.AcRpt1Service;
import java.time.LocalDate;
import java.util.*;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.web.bind.annotation.*;

/** Group R1 data endpoints for the four payables / receivables report pages (the pages are in AcRpt1PageController). */
@RestController
@RequestMapping("/api/accounts/acrpt1")
public class AcRpt1Controller {
    private final AcRpt1Service service;

    public AcRpt1Controller(AcRpt1Service service) { this.service = service; }

    // ---- 48 Payables
    @GetMapping("/payables/lookups")
    public Map<String, Object> payablesLookups() { return service.payablesLookups(); }

    @GetMapping("/payables")
    public List<Map<String, Object>> payables(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate,
            @RequestParam(defaultValue = "0") int actionId, @RequestParam(defaultValue = "0") int cityId,
            @RequestParam(defaultValue = "") String branchesIds, @RequestParam(defaultValue = "") String controlAccountIds,
            @RequestParam(defaultValue = "0") int customGroupId, @RequestParam(defaultValue = "") String customerGroupIds,
            @RequestParam(defaultValue = "0") double balanceFrom, @RequestParam(defaultValue = "0") double balanceTo,
            @RequestParam(defaultValue = "0") int showAssetLiability) {
        return service.payables(fromDate, toDate, actionId, cityId, branchesIds, controlAccountIds, customGroupId, customerGroupIds, balanceFrom, balanceTo, showAssetLiability);
    }

    // ---- 63 PayablesByDueDate
    @GetMapping("/payables-due/lookups")
    public Map<String, Object> payablesDueLookups() { return service.payablesDueLookups(); }

    /** btnRefresh_Click: CustomerGroupFill + LanguageDropdownBind. */
    @GetMapping("/payables-due/refresh")
    public Map<String, Object> payablesDueRefresh() { return service.payablesDueRefresh(); }

    @GetMapping("/payables-due")
    public List<Map<String, Object>> payablesDue(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dueDateTo,
            @RequestParam(defaultValue = "") String ids, @RequestParam(defaultValue = "0") int fromDocNo,
            @RequestParam(defaultValue = "0") int toDocNo, @RequestParam(defaultValue = "0") int languageId) {
        return service.payablesDue(dueDateTo, ids, fromDocNo, toDocNo, languageId);
    }

    // ---- 70 ReceiveablesByDueDate
    @GetMapping("/receivables-due/lookups")
    public Map<String, Object> receivablesDueLookups() { return service.receivablesDueLookups(); }

    @GetMapping("/receivables-due")
    public List<Map<String, Object>> receivablesDue(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dueDateTo,
            @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
            @RequestParam(defaultValue = "0") int languageId, @RequestParam(defaultValue = "0") int customGroupId,
            @RequestParam(defaultValue = "") String ids, @RequestParam(defaultValue = "") String parentAccountCode) {
        return service.receivablesDue(dueDateTo, fromDocNo, toDocNo, languageId, customGroupId, ids, parentAccountCode);
    }

    // ---- 68 ReceivablesByDueDatesNew (lookups are also its btnRefresh_Click)
    @GetMapping("/receivables-new/lookups")
    public Map<String, Object> receivablesNewLookups() { return service.receivablesNewLookups(); }

    @GetMapping("/receivables-new")
    public List<Map<String, Object>> receivablesNew(
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate,
            @RequestParam(defaultValue = "0") int customGroupId, @RequestParam(defaultValue = "0") int parentId) {
        return service.receivablesNew(fromDate, toDate, customGroupId, parentId);
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
