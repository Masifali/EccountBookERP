package com.mst.controllers;

import com.mst.services.AccountsReportService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/accounts/api/reports")
public class AccountsReportRestController {

    @Autowired
    private AccountsReportService accountsReportService;

    private static Integer intOrNull(Object v) {
        if (v == null) return null;
        String s = v.toString().trim();
        if (s.isEmpty()) return null;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) { return null; }
    }

    private static String strOrNull(Object v) {
        if (v == null) return null;
        String s = v.toString().trim();
        return s.isEmpty() ? null : s;
    }

    @PostMapping("/general-ledger")
    public ResponseEntity<?> getGeneralLedger(@RequestBody Map<String, Object> req) {
        try {
            Integer accountId = intOrNull(req.get("accountId"));
            String fromDate = strOrNull(req.get("fromDate"));
            String toDate = strOrNull(req.get("toDate"));
            Integer subsidiaryAccountId = intOrNull(req.get("subsidiaryAccountId"));
            Integer branchId = intOrNull(req.get("branchId"));
            Integer costCenterId = intOrNull(req.get("costCenterId"));
            Integer languageId = intOrNull(req.get("languageId"));
            boolean includeUnposted = Boolean.parseBoolean(String.valueOf(req.getOrDefault("includeUnposted", false)));

            List<Map<String, Object>> data = accountsReportService.getGeneralLedgerReport(accountId, fromDate, toDate,
                    subsidiaryAccountId, branchId, costCenterId, languageId, includeUnposted);
            Map<String, Object> info = accountsReportService.getAccountInfoForLedger(accountId, fromDate, toDate);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("accountInfo", info);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading General Ledger: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }

    @PostMapping("/customer-ledger")
    public ResponseEntity<?> getCustomerLedger(@RequestBody Map<String, Object> req) {
        try {
            Integer supplierCustomerId = intOrNull(req.get("supplierCustomerId"));
            String fromDate = strOrNull(req.get("fromDate"));
            String toDate = strOrNull(req.get("toDate"));

            List<Map<String, Object>> data = accountsReportService.getCustomerLedgerReport(supplierCustomerId, fromDate, toDate);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading Customer Ledger: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }

    @PostMapping("/account-info")
    public ResponseEntity<?> getAccountInfo(@RequestBody Map<String, Object> req) {
        Integer accountId = intOrNull(req.get("accountId"));
        String fromDate = strOrNull(req.get("fromDate"));
        String toDate = strOrNull(req.get("toDate"));
        return ResponseEntity.ok(accountsReportService.getAccountInfoForLedger(accountId, fromDate, toDate));
    }

    @GetMapping("/supplier-customers")
    public ResponseEntity<?> getSupplierCustomers() {
        return ResponseEntity.ok(accountsReportService.getSupplierCustomersForCombo());
    }

    @GetMapping("/date-types")
    public ResponseEntity<?> getDateTypes() {
        return ResponseEntity.ok(accountsReportService.getDateTypes());
    }

    @GetMapping("/cost-centers")
    public ResponseEntity<?> getCostCenters() {
        return ResponseEntity.ok(accountsReportService.getCostCenters());
    }

    @GetMapping("/subsidiary-accounts")
    public ResponseEntity<?> getSubsidiaryAccounts(@RequestParam(value = "costCenterId", required = false) Integer costCenterId) {
        return ResponseEntity.ok(accountsReportService.getSubsidiaryAccounts(costCenterId));
    }

    @GetMapping("/branches")
    public ResponseEntity<?> getBranches() {
        return ResponseEntity.ok(accountsReportService.getBranchesForReports());
    }

    @GetMapping("/languages")
    public ResponseEntity<?> getLanguages() {
        return ResponseEntity.ok(accountsReportService.getLanguages());
    }

    @GetMapping("/account-groups")
    public ResponseEntity<?> getAccountGroups() {
        return ResponseEntity.ok(accountsReportService.getAccountGroups());
    }

    @GetMapping("/document-types")
    public ResponseEntity<?> getDocumentTypes() {
        return ResponseEntity.ok(accountsReportService.getDocumentTypesForReports());
    }

    @PostMapping("/general-ledger-statement")
    public ResponseEntity<?> getGeneralLedgerStatement(@RequestBody Map<String, Object> req) {
        try {
            Integer accountId = req.get("accountId") != null ? Integer.parseInt(req.get("accountId").toString()) : null;
            String fromDate = req.get("fromDate") != null ? req.get("fromDate").toString() : null;
            String toDate = req.get("toDate") != null ? req.get("toDate").toString() : null;
            Integer branchId = req.get("branchId") != null ? Integer.parseInt(req.get("branchId").toString()) : null;
            Integer projectId = req.get("projectId") != null ? Integer.parseInt(req.get("projectId").toString()) : null;

            List<Map<String, Object>> data = accountsReportService.getGeneralLedgerStatementReport(accountId, fromDate, toDate, branchId, projectId);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading General Ledger Statement: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }

    @PostMapping("/trial-balance")
    public ResponseEntity<?> getTrialBalance(@RequestBody Map<String, Object> req) {
        try {
            String fromDate = strOrNull(req.get("fromDate"));
            String toDate = strOrNull(req.get("toDate"));
            Boolean skipZeroBalance = req.get("skipZeroBalance") != null ? Boolean.parseBoolean(String.valueOf(req.get("skipZeroBalance"))) : null;
            Double closingDebitFilter = req.get("closingDebitFilter") != null && !req.get("closingDebitFilter").toString().isBlank() ? Double.parseDouble(req.get("closingDebitFilter").toString()) : null;
            Double closingCreditFilter = req.get("closingCreditFilter") != null && !req.get("closingCreditFilter").toString().isBlank() ? Double.parseDouble(req.get("closingCreditFilter").toString()) : null;
            boolean includeUnposted = Boolean.parseBoolean(String.valueOf(req.getOrDefault("includeUnposted", false)));
            Integer languageId = intOrNull(req.get("languageId"));
            String documentTypeIds = strOrNull(req.get("documentTypeIds"));
            String branchesIds = strOrNull(req.get("branchesIds"));
            String accountTypeIds = strOrNull(req.get("accountTypeIds"));
            Integer customGroupId = intOrNull(req.get("customGroupId"));
            Integer groupAccountId = intOrNull(req.get("groupAccountId"));
            Boolean skipCgsAccounts = req.get("skipCgsAccounts") != null ? Boolean.parseBoolean(String.valueOf(req.get("skipCgsAccounts"))) : null;

            List<Map<String, Object>> data = accountsReportService.getTrialBalanceReport(fromDate, toDate, skipZeroBalance,
                    closingDebitFilter, closingCreditFilter, includeUnposted, languageId, documentTypeIds, branchesIds,
                    accountTypeIds, customGroupId, groupAccountId, skipCgsAccounts);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading Trial Balance: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }

    @PostMapping("/selected-trial-balance")
    public ResponseEntity<?> getSelectedTrialBalance(@RequestBody Map<String, Object> req) {
        try {
            String fromDate = strOrNull(req.get("fromDate"));
            String toDate = strOrNull(req.get("toDate"));
            Integer cityId = intOrNull(req.get("cityId"));
            Integer groupAccountId = intOrNull(req.get("groupAccountId"));
            Integer customGroupId = intOrNull(req.get("customGroupId"));
            Integer languageId = intOrNull(req.get("languageId"));
            boolean skipZeroBalance = Boolean.parseBoolean(String.valueOf(req.getOrDefault("skipZeroBalance", false)));
            boolean skipCgsAccounts = Boolean.parseBoolean(String.valueOf(req.getOrDefault("skipCgsAccounts", false)));
            boolean includeUnposted = Boolean.parseBoolean(String.valueOf(req.getOrDefault("includeUnposted", false)));
            String documentTypeIds = strOrNull(req.get("documentTypeIds"));
            String branchesIds = strOrNull(req.get("branchesIds"));
            Double closingDebitFilter = req.get("closingDebitFilter") != null && !req.get("closingDebitFilter").toString().isBlank() ? Double.parseDouble(req.get("closingDebitFilter").toString()) : null;
            Double closingCreditFilter = req.get("closingCreditFilter") != null && !req.get("closingCreditFilter").toString().isBlank() ? Double.parseDouble(req.get("closingCreditFilter").toString()) : null;

            List<Map<String, Object>> data = accountsReportService.getSelectedTrialBalanceReport(fromDate, toDate,
                    cityId, groupAccountId, customGroupId, languageId, skipZeroBalance, skipCgsAccounts,
                    includeUnposted, documentTypeIds, branchesIds, closingDebitFilter, closingCreditFilter);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading Selected Trial Balance: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }

    @PostMapping("/trial-balance-all-level")
    public ResponseEntity<?> getTrialBalanceAllLevel(@RequestBody Map<String, Object> req) {
        try {
            String fromDate = strOrNull(req.get("fromDate"));
            String toDate = strOrNull(req.get("toDate"));
            boolean skipZero = Boolean.parseBoolean(String.valueOf(req.getOrDefault("skipZero", false)));

            List<Map<String, Object>> data = accountsReportService.getTrialBalanceAllLevelsReport(fromDate, toDate, skipZero);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading Trial Balances All Level: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }

    @GetMapping("/bank-balances")
    public ResponseEntity<?> getBankBalances(@RequestParam(value = "isCashOnly", defaultValue = "false") boolean isCashOnly) {
        List<Map<String, Object>> data = accountsReportService.getBankOrCashBalancesReport(isCashOnly);
        return ResponseEntity.ok(data);
    }

    @GetMapping("/party-aging")
    public ResponseEntity<?> getPartyAging(@RequestParam(value = "isPayables", defaultValue = "true") boolean isPayables,
                                           @RequestParam(value = "toDate", required = false) String toDate) {
        List<Map<String, Object>> data = accountsReportService.getPartyAgingReport(isPayables, toDate);
        return ResponseEntity.ok(data);
    }

    @GetMapping("/accounts-by-parent")
    public ResponseEntity<?> getAccountsByParent(@RequestParam(value = "parentCode", required = false) String parentCode) {
        return ResponseEntity.ok(accountsReportService.getAccountsByParent(parentCode));
    }

    @PostMapping("/activity-summary")
    public ResponseEntity<?> getActivitySummary(@RequestBody Map<String, Object> req) {
        try {
            Integer accountId = req.get("accountId") != null && !req.get("accountId").toString().isEmpty() ? Integer.parseInt(req.get("accountId").toString()) : null;
            String fromDate = req.get("fromDate") != null ? req.get("fromDate").toString() : null;
            String toDate = req.get("toDate") != null ? req.get("toDate").toString() : null;
            String dateType = req.get("dateType") != null ? req.get("dateType").toString() : "DocDate";
            Integer reportTypeId = req.get("reportTypeId") != null ? Integer.parseInt(req.get("reportTypeId").toString()) : 1;
            Boolean approvedOnly = req.get("approvedOnly") != null ? Boolean.parseBoolean(req.get("approvedOnly").toString()) : true;

            List<Map<String, Object>> data = accountsReportService.getActivitySummaryReport(accountId, fromDate, toDate, dateType, reportTypeId, approvedOnly);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading Activity Summary: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }
}

