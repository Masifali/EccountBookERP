package com.mst.controllers;

import com.mst.services.AccountsReportService;
import com.mst.services.GeneralLedgerPrintService;
import com.mst.services.GeneralLedgerPrintService.Request;
import net.sf.jasperreports.engine.*;
import net.sf.jasperreports.engine.data.JRMapCollectionDataSource;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.*;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.time.LocalDate;
import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

@Controller
public class GeneralLedgerPrintController {
    private static final Logger log = LoggerFactory.getLogger(GeneralLedgerPrintController.class);
    private final GeneralLedgerPrintService service;
    private final AccountsReportService accountsReportService;

    public GeneralLedgerPrintController(GeneralLedgerPrintService service, AccountsReportService accountsReportService) {
        this.service = service;
        this.accountsReportService = accountsReportService;
    }

    @GetMapping("/accounts/reports/general-ledger/print")
    public String print(@RequestParam String format, @RequestParam int accountId,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate,
            @RequestParam(defaultValue = "false") boolean includeUnposted,
            @RequestParam(required = false) Integer languageId, Model model) {
        if (accountId <= 0 || fromDate.isAfter(toDate)) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Select an account and a valid date range");
        }
        try {
            var report = service.load(new Request(format, accountId, fromDate, toDate, includeUnposted, languageId));
            model.addAttribute("report", report);
            log.info("General Ledger Print-{} loaded {} rows", report.code(), report.rows().size());
            return "accounts/reports/general_ledger_print";
        } catch (IllegalArgumentException ex) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, ex.getMessage(), ex);
        } catch (IllegalStateException ex) {
            throw new ResponseStatusException(HttpStatus.CONFLICT, ex.getMessage(), ex);
        }
    }

    @GetMapping({"/accounts/reports/general-ledger/pdf", "/accounts/reports/general-ledger/pdf/{reportCode}"})
    @ResponseBody
    public ResponseEntity<byte[]> generateGeneralLedgerPdf(
            @RequestParam(required = false, defaultValue = "105") String format,
            @PathVariable(required = false) String reportCode,
            @RequestParam(required = false) String code,
            @RequestParam(required = false, defaultValue = "0") Integer accountId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(defaultValue = "false") boolean includeUnposted,
            @RequestParam(required = false) Integer languageId) {
        
        String effCode = (reportCode != null && !reportCode.isBlank()) ? reportCode 
                : (code != null && !code.isBlank()) ? code 
                : (format != null ? format : "105");

        List<Map<String, Object>> rows = accountsReportService.getGeneralLedgerReport(
                accountId, fromDate, toDate, null, null, null, languageId, includeUnposted);

        Map<String, Object> accountInfo = accountsReportService.getAccountInfoForLedger(accountId, fromDate, toDate);

        String accountTitle = "";
        String accountCodeStr = "";
        if (accountInfo != null && accountInfo.containsKey("accountCode") && accountInfo.get("accountCode") != null) {
            accountCodeStr = String.valueOf(accountInfo.get("accountCode"));
        }

        if (!rows.isEmpty()) {
            if (accountCodeStr.isBlank() && rows.get(0).get("AccountCode") != null) {
                accountCodeStr = String.valueOf(rows.get(0).get("AccountCode"));
            }
            if (rows.get(0).get("AccountTitle") != null) {
                accountTitle = String.valueOf(rows.get(0).get("AccountTitle"));
            }
        }
        String accountCodeTitle = (!accountCodeStr.isBlank() && !accountTitle.isBlank()) 
                ? (accountCodeStr + " - " + accountTitle) 
                : (!accountCodeStr.isBlank() ? accountCodeStr : accountTitle);

        String openingBalStr = formatBalanceStr(accountInfo != null ? accountInfo.get("openingBalance") : null);
        String closingBalStr = formatBalanceStr(accountInfo != null ? accountInfo.get("closingBalance") : null);

        Map<String, Object> params = new HashMap<>();
        params.put("CompanyName", "Golden Ace Rice Mills (Pvt) Ltd.");
        params.put("CompanyAddress", "Head Office / Factory Address");
        params.put("AccountCodeTitle", accountCodeTitle);
        params.put("FromDate", fromDate != null ? fromDate : "");
        params.put("ToDate", toDate != null ? toDate : "");
        params.put("OpeningBalanceStr", openingBalStr);
        params.put("ClosingBalanceStr", closingBalStr);

        String reportTitle = switch (effCode) {
            case "106" -> "GENERAL LEDGER WITH OFFSET ACCOUNTS";
            case "107" -> "QUANTITATIVE GENERAL LEDGER";
            case "108" -> "GENERAL LEDGER SUMMARY-I";
            case "105A" -> "GENERAL LEDGER SUMMARY-II";
            default -> "GENERAL LEDGER REPORT";
        };
        params.put("ReportTitle", reportTitle);

        List<Map<String, Object>> jasperRows = new ArrayList<>();
        for (Map<String, Object> row : rows) {
            Map<String, Object> jRow = new HashMap<>();

            Object vDate = row.get("VoucherDate");
            jRow.put("VoucherDate", vDate != null ? vDate.toString().split(" ")[0] : "");

            Object dt = getFirstNonEmptyString(row, "DocTypeCode", "DocType", "VoucherType");
            jRow.put("DocType", dt != null ? dt.toString() : "");

            Object vn = getFirstNonEmptyString(row, "VoucherCode", "VoucherNo");
            jRow.put("VoucherNo", vn != null ? vn.toString() : "");

            Object ac = row.get("AccountCode");
            jRow.put("AccountCode", ac != null ? ac.toString() : "");

            Object ot = getFirstNonEmptyString(row, "OffSetTitle", "OffsetAccountTitle", "OffSetAccountTitle", "OffsetAccount");
            jRow.put("OffSetTitle", ot != null ? ot.toString() : "");

            double debit = toDouble(row.get("DebitAmount"), row.get("Debit"));
            double credit = toDouble(row.get("CreditAmount"), row.get("Credit"));
            double balance = toDouble(row.get("Balance"), row.get("RunningBalance"));

            jRow.put("Debit", debit);
            jRow.put("Credit", credit);
            jRow.put("Balance", Math.abs(balance));

            Object bt = row.get("BalType");
            if (bt != null && !bt.toString().isBlank()) {
                jRow.put("BalType", bt.toString());
            } else {
                jRow.put("BalType", balance > 0 ? "Dr" : balance < 0 ? "Cr" : "");
            }

            Object cm = getFirstNonEmptyString(row, "Comments", "Remarks", "BookmarkRemarks");
            String commentsStr = cm != null ? cm.toString() : "";
            jRow.put("Comments", commentsStr);

            Object vh = getFirstNonEmptyString(row, "VehicleNo", "VehcileNo");
            String vehicleNo = vh != null ? vh.toString() : "";
            if (vehicleNo.isBlank() && !commentsStr.isBlank()) {
                vehicleNo = extractVehicleNoFromComments(commentsStr);
            }
            jRow.put("VehicleNo", vehicleNo);

            Object chk = row.get("ChequeNo");
            jRow.put("ChequeNo", chk != null ? chk.toString() : "");

            jasperRows.add(jRow);
        }

        try {
            InputStream reportStream = getClass().getResourceAsStream("/jasper/105-GeneralLedgerReport.jrxml");
            if (reportStream == null) {
                reportStream = getClass().getResourceAsStream("/jasper/105-GeneralLedgerReport.jasper");
            }
            if (reportStream == null) {
                return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                        .body(("Report template /jasper/105-GeneralLedgerReport.jrxml not found").getBytes(StandardCharsets.UTF_8));
            }

            JasperReport jasperReport = JasperCompileManager.compileReport(reportStream);
            JRMapCollectionDataSource dataSource = new JRMapCollectionDataSource((Collection) jasperRows);
            JasperPrint jasperPrint = JasperFillManager.fillReport(jasperReport, params, dataSource);
            byte[] pdfBytes = JasperExportManager.exportReportToPdf(jasperPrint);

            HttpHeaders headers = new HttpHeaders();
            headers.setContentType(MediaType.APPLICATION_PDF);
            headers.setContentDisposition(ContentDisposition.inline().filename("GeneralLedgerReport-" + effCode + ".pdf").build());

            return new ResponseEntity<>(pdfBytes, headers, HttpStatus.OK);
        } catch (Exception e) {
            log.error("PDF generation failed for code {}: {}", effCode, e.getMessage(), e);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(("PDF export failed: " + e.getMessage()).getBytes(StandardCharsets.UTF_8));
        }
    }

    private static Object getFirstNonEmptyString(Map<String, Object> map, String... keys) {
        for (String key : keys) {
            Object val = map.get(key);
            if (val != null && !val.toString().trim().isEmpty()) {
                return val;
            }
        }
        return null;
    }

    private static double toDouble(Object... vals) {
        for (Object val : vals) {
            if (val != null) {
                try {
                    if (val instanceof Number n) return n.doubleValue();
                    String s = val.toString().trim().replace(",", "");
                    if (!s.isEmpty()) return Double.parseDouble(s);
                } catch (Exception ignored) {}
            }
        }
        return 0.0;
    }

    private static String formatBalanceStr(Object balObj) {
        if (balObj == null) return "0.00";
        try {
            double val = toDouble(balObj);
            String formatted = String.format("%,.2f", Math.abs(val));
            if (val > 0) return formatted + " Dr";
            if (val < 0) return formatted + " Cr";
            return formatted;
        } catch (Exception e) {
            return balObj.toString();
        }
    }

    private static String extractVehicleNoFromComments(String text) {
        if (text == null || text.isBlank()) return "";
        Matcher m = Pattern.compile("(?:Vehicle\\s*No|Vehical\\s*No)[\\s:]*([A-Za-z0-9\\-]+)", Pattern.CASE_INSENSITIVE).matcher(text);
        if (m.find()) return m.group(1).trim();
        return "";
    }
}

