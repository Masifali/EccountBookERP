package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.AccountsPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import com.mst.models.GeneralLedgerSummaryRequest;
import com.mst.reports.jasper.CR;
import com.mst.reports.jasper.CrystalJasperPrinter;
import com.mst.services.GeneralLedgerSummaryService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import net.sf.jasperreports.engine.JRException;
import net.sf.jasperreports.engine.JRParameter;
import net.sf.jasperreports.engine.JasperCompileManager;
import net.sf.jasperreports.engine.JasperExportManager;
import net.sf.jasperreports.engine.JasperFillManager;
import net.sf.jasperreports.engine.JasperPrint;
import net.sf.jasperreports.engine.JasperReport;
import net.sf.jasperreports.engine.data.JRMapCollectionDataSource;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.ModelAttribute;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import javax.servlet.http.HttpServletResponse;
import javax.validation.Valid;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Objects;
import java.util.Collection;
import java.util.Collections;
import java.nio.charset.StandardCharsets;
import java.time.LocalDate;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import com.mst.models.Company;
import com.mst.services.GeneralLedgerPrintService;
import com.mst.services.GeneralLedgerPrintService.Request;
import com.mst.services.AccountsReportService;
import org.springframework.ui.Model;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.http.*;

/** Accounts print actions. Existing print URLs are preserved. */
@Controller
public class AccountsPrintController extends ReportPrintSupport {
    @Autowired private GeneralLedgerPrintService generalLedgerPrintService;
    @Autowired private AccountsReportService accountsReportService;
    @Autowired private com.mst.services.TradeReportService tradeReportService;
    @Autowired private CrystalJasperPrinter crystalJasperPrinter;
    @Autowired private org.springframework.jdbc.core.JdbcTemplate jdbcTemplate;
    @Autowired private com.mst.repositories.IAccountOpeningBalanceRepository accountOpeningBalanceRepository;
    @Autowired private com.mst.repositories.ICompanyRepository companyRepository;
    @Autowired private com.mst.services.OpeningBalanceDesktopService openingBalanceDesktop;
    private static final org.slf4j.Logger log = org.slf4j.LoggerFactory.getLogger(AccountsPrintController.class);
    private final GeneralLedgerSummaryService generalLedgerSummaryService;

    public AccountsPrintController(GeneralLedgerSummaryService generalLedgerSummaryService) {
        this.generalLedgerSummaryService = generalLedgerSummaryService;
    }

    /** TradeDebitorsReport.cs:1194-1230 prints the TradeDebtorsAndCreditors data, not ReceivablesReport. */
    @GetMapping("/reports/print/trade-accounts")
    public void printTradeAccounts(HttpServletResponse response,
            @ModelAttribute com.mst.models.dto.TradeReportRequest request,
            @RequestParam(name = "cityWise", defaultValue = "false") boolean cityWise) throws Exception {
        currentUserContext.currentUserId();
        List<Map<String, Object>> rows = new ArrayList<>(tradeReportService.load(request));
        if (cityWise) rows.sort(java.util.Comparator
                .comparing((Map<String, Object> row) -> Objects.toString(row.get("CityName"), ""), String.CASE_INSENSITIVE_ORDER)
                .thenComparing(row -> Objects.toString(row.get("AccountTitle"), ""), String.CASE_INSENSITIVE_ORDER));
        Map<String, Object> parameters = new HashMap<>();
        Company company = companyRepository.findById(currentUserContext.currentCompanyId()).orElse(null);
        parameters.put("CompanyName", company == null ? "" : Objects.toString(company.getCompName(), ""));
        parameters.put("CompanyAddress", company == null ? "" : Objects.toString(company.getCompAddress(), ""));
        parameters.put("FromDate", request.getFromDate().toString());
        parameters.put("ToDate", request.getToDate().toString());
        String template = cityWise ? "122A-CityWise-AcRptAccounts_ReceivablesWithStatus.rpt"
                                  : "122A-AcRptAccounts_ReceivablesWithStatus.rpt";
        printReportData(response, template, Map.of("rows", rows, "reportParameters", parameters));
    }

    /** Browser URL: /reports/general-ledger-summary?accountId=23967&fromDate=2026-08-01&toDate=2026-10-03 */
    @RequestMapping(value = "/reports/general-ledger-summary", method = RequestMethod.GET)
    public void generalLedgerSummaryGet(HttpServletResponse response,
            @Valid @ModelAttribute GeneralLedgerSummaryRequest request) throws IOException, JRException {
        generalLedgerSummaryReport(response, request);
    }

    /** JSON body: {"accountId":23967,"fromDate":"2026-08-01","toDate":"2026-10-03"} */
    @RequestMapping(value = "/reports/general-ledger-summary", method = RequestMethod.POST)
    public void generalLedgerSummaryReport(HttpServletResponse response,
            @Valid @RequestBody GeneralLedgerSummaryRequest request) throws IOException, JRException {
        String fileName = "108-GeneralLedgerSummary.pdf";

        // 1. Get the report data. Inspect this list when debugging missing or incorrect rows.
        List<Map<String, Object>> ledgerRows = generalLedgerSummaryService.getGeneralLedgerSummary(request);
        if (ledgerRows.isEmpty()) {
            response.sendError(HttpServletResponse.SC_NOT_FOUND, "No Record Found For Display");
            return;
        }

        // 2. Load and compile this report's JRXML. Classpath loading also works from a packaged JAR.
        JasperReport jasperReport;
        try (InputStream reportStream = getClass().getResourceAsStream(
                "/jasper/converted/108-AcRptGeneralLedgerSummery.jrxml")) {
            if (reportStream == null) {
                throw new IOException("Report template 108-AcRptGeneralLedgerSummery.jrxml was not found");
            }
            jasperReport = JasperCompileManager.compileReport(reportStream);
        }

        // 3. Set the template parameters from the company columns returned by the procedure.
        Map<String, Object> firstRow = ledgerRows.get(0);
        Map<String, Object> parameters = new HashMap<>();
        parameters.put("CompanyName", Objects.toString(firstRow.get("CompName"), ""));
        parameters.put("CompanyAddress", Objects.toString(firstRow.get("CompAddress"), ""));
        parameters.put("CR_VARS", new CR.Vars());
        parameters.put(JRParameter.REPORT_LOCALE, Locale.US);

        // 4. The procedure returns maps, so use a map data source instead of a bean data source.
        // normalize matches database column names/types to the existing converted template fields.
        List<Map<String, Object>> reportRows = CrystalJasperPrinter.normalize(jasperReport, ledgerRows);
        JRMapCollectionDataSource dataSource =
                new JRMapCollectionDataSource(new ArrayList<Map<String, ?>>(reportRows));
        JasperPrint jasperPrint = JasperFillManager.fillReport(jasperReport, parameters, dataSource);

        // 5. Export and send the PDF. Each request owns its bytes; no shared output file is needed.
        byte[] pdf = JasperExportManager.exportReportToPdf(jasperPrint);
        response.setContentType("application/pdf");
        response.setHeader("Content-Disposition", "inline; filename=\"" + fileName + "\"");
        response.setContentLength(pdf.length);
        response.getOutputStream().write(pdf);
    }

    private static final Map<String, String> DESKTOP_TEMPLATES = Map.of(
            "105", "105-AcRptGeneralLedger.rpt",
            "106", "106-AcRptGeneralLedgerB.rpt",
            "107", "107-AcRptQuantativeLedger.rpt");

    private ResponseEntity<byte[]> printDesktopTemplate(String code, String rpt, Integer accountId, String fromDate,
            String toDate, Integer subsidiaryAccountId, Integer branchId, Integer costCenterId, Integer languageId,
            boolean includeUnposted) {
        if (accountId == null || accountId <= 0) {
            return plain(HttpStatus.BAD_REQUEST, "Account Title Field is Required");
        }
        try {
            List<Map<String, Object>> rows = accountsReportService.getGeneralLedgerReport(accountId, fromDate, toDate,
                    subsidiaryAccountId, branchId, costCenterId, languageId, includeUnposted);
            if (rows == null || rows.isEmpty()) {
                return plain(HttpStatus.NOT_FOUND, "Not Record Found For Display");   // the desktop's message
            }
            /* @CompanyName/@CompanyAddress = UserAccount.CompName/CompAddress on the desktop; the
               procedure returns the same Company row (co.CompName, co.CompAddress). */
            Map<String, Object> first = rows.get(0);
            Map<String, Object> reportParams = new HashMap<>();
            reportParams.put("@CompanyName", first.get("CompName"));
            reportParams.put("@CompanyAddress", first.get("CompAddress"));
            byte[] pdf = crystalJasperPrinter.printPdf(rpt, rows, Collections.emptyMap(), reportParams);

            HttpHeaders headers = new HttpHeaders();
            headers.setContentType(MediaType.APPLICATION_PDF);
            headers.setContentDisposition(ContentDisposition.inline()
                    .filename(rpt.replace(".rpt", "") + "-" + accountId + ".pdf").build());
            log.info("General Ledger Print-{} ({}) rendered {} rows", code, rpt, rows.size());
            return new ResponseEntity<>(pdf, headers, HttpStatus.OK);
        } catch (Exception e) {
            log.error("General Ledger Print-{} failed: {}", code, e.getMessage(), e);
            return plain(HttpStatus.INTERNAL_SERVER_ERROR, "Print-" + code + " failed: " + e.getMessage());
        }
    }

    /** Errors on a PDF endpoint are readable text, never a broken PDF. */
    private static ResponseEntity<byte[]> plain(HttpStatus status, String message) {
        HttpHeaders h = new HttpHeaders();
        h.setContentType(new MediaType("text", "plain", StandardCharsets.UTF_8));
        return new ResponseEntity<>(message.getBytes(StandardCharsets.UTF_8), h, status);
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
            var report = generalLedgerPrintService.load(new Request(format, accountId, fromDate, toDate, includeUnposted, languageId));
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
            @RequestParam(required = false) Integer languageId,
            @RequestParam(required = false) Integer branchId,
            @RequestParam(required = false) Integer subsidiaryAccountId,
            @RequestParam(required = false) Integer costCenterId) {

        String effCode = (reportCode != null && !reportCode.isBlank()) ? reportCode
                : (code != null && !code.isBlank()) ? code
                : (format != null ? format : "105");

        String desktopRpt = DESKTOP_TEMPLATES.get(effCode);
        if (desktopRpt != null && crystalJasperPrinter.hasTemplate(desktopRpt)) {
            return printDesktopTemplate(effCode, desktopRpt, accountId, fromDate, toDate, subsidiaryAccountId,
                    branchId, costCenterId, languageId, includeUnposted);
        }

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


    /**
     * 103-AcRptPurchaseSalesVoucherSlip.rpt (Voucher Slip)
     */
    @GetMapping({"/accounts/reports/voucher-slip/pdf", "/accounts/reports/voucher-slip/pdf/{id}"})
    public ResponseEntity<byte[]> generateVoucherSlipPdf(
            @PathVariable(required = false) Integer id,
            @RequestParam(required = false) Integer voucherHeadId,
            HttpServletResponse response) {
        int targetId = id != null ? id : (voucherHeadId != null ? voucherHeadId : 0);
        if (targetId <= 0) {
            return ResponseEntity.badRequest().body("VoucherHead ID is required".getBytes(StandardCharsets.UTF_8));
        }

        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @AccountId=?, @Activity=?",
                orgId, compId, targetId, "ReadVoucherDetailByHeaderId");

        if (rows == null || rows.isEmpty()) {
            rows = jdbcTemplate.queryForList(
                    "SELECT h.VoucherCode as VoucherNo, h.VoucherDate, dt.DocumentTypeDescription as DocType, " +
                    "coa.AccountCode, coa.AccountTitle, d.DebitAmount as Debit, d.CreditAmount as Credit, " +
                    "d.Comments, h.ChequeNo, h.PayeeTitle " +
                    "FROM VoucherDetail d " +
                    "JOIN VoucherHead h ON d.VoucherHeadId = h.ID " +
                    "JOIN ChartofAccount coa ON d.AccountId = coa.ID " +
                    "LEFT JOIN DocumentType dt ON h.DocumentTypeId = dt.ID " +
                    "WHERE h.ID = ? AND h.OrganizationId = ? AND h.CompanyId = ?",
                    targetId, orgId, compId);
        }

        Map<String, Object> parameters = new HashMap<>();
        parameters.put("CompanyName", "Golden Ace Rice Mills (Pvt) Ltd.");
        parameters.put("CompanyAddress", "Head Office / Factory Address");
        parameters.put("ReportTitle", "103-PURCHASE/SALES VOUCHER SLIP");

        return renderVoucherPdf("/jasper/purchaseVoucher.jrxml", rows, parameters, "VoucherSlip-" + targetId + ".pdf");
    }

    /**
     * 240-FreightVoucherRegister.rpt (Freight Voucher Register)
     */
    @GetMapping("/accounts/reports/freight-voucher-register/pdf")
    public ResponseEntity<byte[]> generateFreightVoucherRegisterPdf(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_InvFreightVoucher_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                orgId, compId, "ReadAll");

        Map<String, Object> parameters = new HashMap<>();
        parameters.put("CompanyName", "Golden Ace Rice Mills (Pvt) Ltd.");
        parameters.put("CompanyAddress", "Head Office / Factory Address");
        parameters.put("ReportTitle", "240-FREIGHT VOUCHER REGISTER");
        parameters.put("FromDate", fromDate != null ? fromDate : "");
        parameters.put("ToDate", toDate != null ? toDate : "");

        return renderVoucherPdf("/jasper/105-GeneralLedgerReport.jrxml", rows, parameters, "FreightVoucherRegister.pdf");
    }

    /**
     * 241-FreightVoucherSlip.rpt (Freight Voucher Slip)
     */
    @GetMapping({"/accounts/reports/freight-voucher-slip/pdf", "/accounts/reports/freight-voucher-slip/pdf/{id}"})
    public ResponseEntity<byte[]> generateFreightVoucherSlipPdf(
            @PathVariable(required = false) Integer id,
            @RequestParam(required = false) Integer freightVoucherId) {
        int targetId = id != null ? id : (freightVoucherId != null ? freightVoucherId : 0);
        if (targetId <= 0) {
            return ResponseEntity.badRequest().body("Freight Voucher ID is required".getBytes(StandardCharsets.UTF_8));
        }

        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_InvFreightVoucher_GetAllMethod @OrganizationId=?, @CompanyId=?, @Id=?, @Activity=?",
                orgId, compId, targetId, "ReadByHeaderId");

        Map<String, Object> parameters = new HashMap<>();
        parameters.put("CompanyName", "Golden Ace Rice Mills (Pvt) Ltd.");
        parameters.put("CompanyAddress", "Head Office / Factory Address");
        parameters.put("ReportTitle", "241-FREIGHT VOUCHER SLIP");

        return renderVoucherPdf("/jasper/purchaseVoucher.jrxml", rows, parameters, "FreightVoucherSlip-" + targetId + ".pdf");
    }

    private ResponseEntity<byte[]> renderVoucherPdf(String jrxmlPath, List<Map<String, Object>> rows, Map<String, Object> parameters, String filename) {
        try {
            InputStream reportStream = getClass().getResourceAsStream(jrxmlPath);
            if (reportStream == null) {
                reportStream = getClass().getResourceAsStream("/jasper/105-GeneralLedgerReport.jrxml");
            }

            JasperReport jasperReport = JasperCompileManager.compileReport(reportStream);
            JRMapCollectionDataSource dataSource = new JRMapCollectionDataSource((Collection) (rows != null ? rows : Collections.emptyList()));
            JasperPrint jasperPrint = JasperFillManager.fillReport(jasperReport, parameters, dataSource);
            byte[] pdfBytes = JasperExportManager.exportReportToPdf(jasperPrint);

            HttpHeaders headers = new HttpHeaders();
            headers.setContentType(MediaType.APPLICATION_PDF);
            headers.setContentDisposition(ContentDisposition.inline().filename(filename).build());
            return new ResponseEntity<>(pdfBytes, headers, HttpStatus.OK);
        } catch (Exception e) {
            log.error("Failed to generate PDF for {}: {}", filename, e.getMessage(), e);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(("PDF export failed: " + e.getMessage()).getBytes(StandardCharsets.UTF_8));
        }
    }


	@GetMapping({"/accounts/opening_balance/pdf", "/accounts/opening_balance/129-OpeningBalanceRpt.pdf"})
	public org.springframework.http.ResponseEntity<byte[]> exportOpeningBalancePdf() {
		/* Session-scoped, never from the request. See the note on the doc-number endpoint: a
		   tenancy id taken from a query string lets a caller read another company's data, and when
		   it is omitted the "1" default reports on a company that does not exist here. */
		Integer organizationId = currentUserContext.currentOrganizationId();
		Integer companyId      = currentUserContext.currentCompanyId();
		Integer financialYearId = currentUserContext.currentFinancialYearId();


		try {
			List<java.util.Map<String, Object>> reportData = accountOpeningBalanceRepository
					.getOpeningBalanceReportData(organizationId, companyId, financialYearId);

			if (reportData == null) {
				reportData = new ArrayList<>();
			}

			String companyName = "Golden Ace Rice Mills (Pvt) Ltd.";
			String companyAddress = "Factory / Head Office Address";
			try {
				Company comp = companyRepository.findById(companyId).orElse(null);
				if (comp != null && comp.getCompanyName() != null) {
					companyName = comp.getCompanyName();
				}
			} catch (Exception ignored) {}

			if (!reportData.isEmpty()) {
				java.util.Map<String, Object> firstRow = reportData.get(0);
				if (firstRow.containsKey("CompAddress") && firstRow.get("CompAddress") != null) {
					companyAddress = firstRow.get("CompAddress").toString();
				}
			}

			java.util.Map<String, Object> params = new java.util.HashMap<>();
			params.put("CompanyName", companyName);
			params.put("CompanyAddress", companyAddress);

			net.sf.jasperreports.engine.data.JRMapCollectionDataSource dataSource =
					new net.sf.jasperreports.engine.data.JRMapCollectionDataSource((java.util.Collection) reportData);

			java.io.InputStream reportStream = getClass().getResourceAsStream("/jasper/129-OpeningBalanceRpt.jrxml");
			if (reportStream == null) {
				reportStream = getClass().getResourceAsStream("/jasper/129-OpeningBalanceRpt.jasper");
			}

			byte[] pdfBytes;
			if (reportStream != null) {
				net.sf.jasperreports.engine.JasperReport jasperReport =
						net.sf.jasperreports.engine.JasperCompileManager.compileReport(reportStream);
				net.sf.jasperreports.engine.JasperPrint jasperPrint =
						net.sf.jasperreports.engine.JasperFillManager.fillReport(jasperReport, params, dataSource);
				pdfBytes = net.sf.jasperreports.engine.JasperExportManager.exportReportToPdf(jasperPrint);
			} else {
				pdfBytes = "Report template 129-OpeningBalanceRpt.jrxml not found".getBytes(java.nio.charset.StandardCharsets.UTF_8);
			}

			org.springframework.http.HttpHeaders headers = new org.springframework.http.HttpHeaders();
			headers.setContentType(org.springframework.http.MediaType.APPLICATION_PDF);
			headers.setContentDispositionFormData("inline", "129-OpeningBalanceRpt.pdf");

			return new org.springframework.http.ResponseEntity<>(pdfBytes, headers, org.springframework.http.HttpStatus.OK);
		} catch (Exception e) {
			org.slf4j.LoggerFactory.getLogger(AccountsPrintController.class)
					.error("[129-PRINT PDF ERROR] Jasper export failed: {}", e.getMessage(), e);
			return org.springframework.http.ResponseEntity.status(org.springframework.http.HttpStatus.INTERNAL_SERVER_ERROR)
					.body(("Error generating PDF report: " + e.getMessage()).getBytes(java.nio.charset.StandardCharsets.UTF_8));
		}
	}

	@GetMapping("/accounts/opening_balance/print")
	@ResponseBody
	public java.util.Map<String, Object> printOpeningBalance() {
		/* Session-scoped, never from the request. See the note on the doc-number endpoint: a
		   tenancy id taken from a query string lets a caller read another company's data, and when
		   it is omitted the "1" default reports on a company that does not exist here. */
		Integer organizationId = currentUserContext.currentOrganizationId();
		Integer companyId      = currentUserContext.currentCompanyId();
		Integer financialYearId = currentUserContext.currentFinancialYearId();


		java.util.Map<String, Object> res = new java.util.HashMap<>();
		org.slf4j.Logger logger = org.slf4j.LoggerFactory.getLogger(AccountsPrintController.class);

		try {
			/* BtnPrint.Enabled = formrights.DoHavePrintRights */
			if (!openingBalanceDesktop.hasPrintRight()) {
				res.put("success", false);
				res.put("resultCount", 0);
				res.put("message", "You do not have the Print right for this screen.");
				return res;
			}
			List<java.util.Map<String, Object>> data = accountOpeningBalanceRepository
					.getOpeningBalanceReportData(organizationId, companyId, financialYearId);

			int resultCount = (data != null) ? data.size() : 0;

			logger.info("[129-PRINT] Report: 129-OpeningBalanceRpt.rpt, StoredProc: Sp_AccountsOpeningBalance_Slip, OrgId: {}, CompId: {}, FinancialYrId: {}, ResultCount: {}",
					organizationId, companyId, financialYearId, resultCount);

			if (data == null || data.isEmpty()) {
				res.put("success", false);
				res.put("resultCount", 0);
				res.put("message", "Record Not Found For Display");
				return res;
			}

			res.put("success", true);
			res.put("resultCount", resultCount);
			res.put("message", "Success");
			res.put("printUrl", "/accounts/opening_balance/print-view?organizationId=" + organizationId
					+ "&companyId=" + companyId + "&financialYearId=" + financialYearId);
		} catch (Exception e) {
			logger.error("[129-PRINT ERROR] Error executing Sp_AccountsOpeningBalance_Slip: {}", e.getMessage(), e);
			res.put("success", false);
			res.put("resultCount", 0);
			res.put("message", "Database Error: " + e.getMessage());
		}
		return res;
	}

	@GetMapping("/accounts/opening_balance/print-view")
	public String viewOpeningBalanceReport(
			Model model) {
		/* Session-scoped, never from the request. See the note on the doc-number endpoint: a
		   tenancy id taken from a query string lets a caller read another company's data, and when
		   it is omitted the "1" default reports on a company that does not exist here. */
		Integer organizationId = currentUserContext.currentOrganizationId();
		Integer companyId      = currentUserContext.currentCompanyId();
		Integer financialYearId = currentUserContext.currentFinancialYearId();


		List<java.util.Map<String, Object>> reportData = new ArrayList<>();
		try {
			List<java.util.Map<String, Object>> rawData = accountOpeningBalanceRepository
					.getOpeningBalanceReportData(organizationId, companyId, financialYearId);
			if (rawData != null) {
				reportData = rawData;
			}
		} catch (Exception e) {
			org.slf4j.LoggerFactory.getLogger(AccountsPrintController.class)
					.error("[129-PRINT VIEW ERROR] Error: {}", e.getMessage());
		}

		double totalDebit = 0.0;
		double totalCredit = 0.0;
		String companyName = "Golden Ace Rice Mills (Pvt) Ltd.";
		String companyAddress = "Factory / Head Office Address";
		String reportingRemarks = "System Generated Opening Balance Slip";

		if (!reportData.isEmpty()) {
			java.util.Map<String, Object> firstRow = reportData.get(0);
			if (firstRow.containsKey("CompAddress") && firstRow.get("CompAddress") != null) {
				companyAddress = firstRow.get("CompAddress").toString();
			}
			if (firstRow.containsKey("ReportingRemarks") && firstRow.get("ReportingRemarks") != null) {
				reportingRemarks = firstRow.get("ReportingRemarks").toString();
			}
			for (java.util.Map<String, Object> row : reportData) {
				if (row.get("YearObDebit") != null) {
					totalDebit += ((Number) row.get("YearObDebit")).doubleValue();
				}
				if (row.get("YearObCredit") != null) {
					totalCredit += ((Number) row.get("YearObCredit")).doubleValue();
				}
			}
		}

		try {
			Company comp = companyRepository.findById(companyId).orElse(null);
			if (comp != null && comp.getCompanyName() != null) {
				companyName = comp.getCompanyName();
			}
		} catch (Exception ignored) {}

		model.addAttribute("organizationId", organizationId);
		model.addAttribute("companyId", companyId);
		model.addAttribute("financialYearId", financialYearId);
		model.addAttribute("reportData", reportData);
		model.addAttribute("totalDebit", totalDebit);
		model.addAttribute("totalCredit", totalCredit);
		model.addAttribute("companyName", companyName);
		model.addAttribute("companyAddress", companyAddress);
		model.addAttribute("reportingRemarks", reportingRemarks);

		return "accounts/reports/129_opening_balance_report";
	}




    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 114-AcRptChartOfAccounts.rpt
     * Procedure: Sp_ChartOfAccounts_Rpt
     * Desktop: VoucherReports.ChartofAccount
     */
    @RequestMapping(value = "/reports/print/114-chart-of-accounts", method = RequestMethod.POST)
    public void print114ChartOfAccounts(HttpServletResponse response, @RequestBody(required = false) Rpt114ChartOfAccountsRequest request) throws Exception {
        if (request == null) request = new Rpt114ChartOfAccountsRequest();
        printReport(response, "114-AcRptChartOfAccounts.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/114-chart-of-accounts", method = RequestMethod.GET)
    public void print114ChartOfAccountsGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print114ChartOfAccounts(response, objectMapper.convertValue(query, Rpt114ChartOfAccountsRequest.class));
    }

    /**
     * Template: 129-OpeningBalanceRpt.rpt
     * Procedure: Sp_AccountsOpeningBalance_Slip
     * Desktop: VoucherReports.OpeningBalanceReport
     */
    @RequestMapping(value = "/reports/print/129-opening-balance", method = RequestMethod.POST)
    public void print129OpeningBalance(HttpServletResponse response, @RequestBody(required = false) Rpt129OpeningBalanceRequest request) throws Exception {
        if (request == null) request = new Rpt129OpeningBalanceRequest();
        printReport(response, "129-OpeningBalanceRpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/129-opening-balance", method = RequestMethod.GET)
    public void print129OpeningBalanceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print129OpeningBalance(response, objectMapper.convertValue(query, Rpt129OpeningBalanceRequest.class));
    }

    /**
     * Template: 920_BankReconciliation_Slip.rpt
     * Procedure: [dbo].[USP_BankReconciliation_SlipAndRegister]
     * Desktop: BankReconciliation.BankReconciliation_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/920-bank-reconciliation-slip", method = RequestMethod.POST)
    public void print920BankReconciliationSlip(HttpServletResponse response, @RequestBody(required = false) Rpt920BankReconciliationSlipRequest request) throws Exception {
        if (request == null) request = new Rpt920BankReconciliationSlipRequest();
        printReport(response, "920_BankReconciliation_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/920-bank-reconciliation-slip", method = RequestMethod.GET)
    public void print920BankReconciliationSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print920BankReconciliationSlip(response, objectMapper.convertValue(query, Rpt920BankReconciliationSlipRequest.class));
    }

    /**
     * Template: 110-AcRptActivitySummery.rpt
     * Procedure: Sp_ActivityReportSummery_Rpt
     * Desktop: VoucherReports.ActivitySummaryReport
     */
    @RequestMapping(value = "/reports/print/110-activity-summery", method = RequestMethod.POST)
    public void print110ActivitySummery(HttpServletResponse response, @RequestBody(required = false) Rpt110ActivitySummeryRequest request) throws Exception {
        if (request == null) request = new Rpt110ActivitySummeryRequest();
        printReport(response, "110-AcRptActivitySummery.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/110-activity-summery", method = RequestMethod.GET)
    public void print110ActivitySummeryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print110ActivitySummery(response, objectMapper.convertValue(query, Rpt110ActivitySummeryRequest.class));
    }

    /**
     * Template: 110A-AcRptActivitySummery.rpt
     * Procedure: Sp_ActivityReportSummery_Rpt
     * Desktop: VoucherReports.ActivitySummaryReport
     */
    @RequestMapping(value = "/reports/print/110a-activity-summery", method = RequestMethod.POST)
    public void print110AActivitySummery(HttpServletResponse response, @RequestBody(required = false) Rpt110AActivitySummeryRequest request) throws Exception {
        if (request == null) request = new Rpt110AActivitySummeryRequest();
        printReport(response, "110A-AcRptActivitySummery.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/110a-activity-summery", method = RequestMethod.GET)
    public void print110AActivitySummeryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print110AActivitySummery(response, objectMapper.convertValue(query, Rpt110AActivitySummeryRequest.class));
    }

    /**
     * Template: 110B-AcRptActivitySummery.rpt
     * Procedure: Sp_ActivityReportSummeryDocumentTypeWise_Rpt
     * Desktop: VoucherReports.ActivitySummaryDocumentTypeWise
     */
    @RequestMapping(value = "/reports/print/110b-activity-summery", method = RequestMethod.POST)
    public void print110BActivitySummery(HttpServletResponse response, @RequestBody(required = false) Rpt110BActivitySummeryRequest request) throws Exception {
        if (request == null) request = new Rpt110BActivitySummeryRequest();
        printReport(response, "110B-AcRptActivitySummery.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/110b-activity-summery", method = RequestMethod.GET)
    public void print110BActivitySummeryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print110BActivitySummery(response, objectMapper.convertValue(query, Rpt110BActivitySummeryRequest.class));
    }

    /**
     * Template: 153-BalanceSheetStatementRpt.rpt
     * Procedure: SpAccounts_BalanceSheetFormatA_Report
     * Desktop: VoucherReports.AccountsBalanceSheetStandardFormatII
     */
    @RequestMapping(value = "/reports/print/153-balance-sheet-statement", method = RequestMethod.POST)
    public void print153BalanceSheetStatement(HttpServletResponse response, @RequestBody(required = false) Rpt153BalanceSheetStatementRequest request) throws Exception {
        if (request == null) request = new Rpt153BalanceSheetStatementRequest();
        printReport(response, "153-BalanceSheetStatementRpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/153-balance-sheet-statement", method = RequestMethod.GET)
    public void print153BalanceSheetStatementGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print153BalanceSheetStatement(response, objectMapper.convertValue(query, Rpt153BalanceSheetStatementRequest.class));
    }

    /**
     * Template: 593-CashBankBalancesSummery_Rpt.rpt
     * Procedure: Sp_Accounts_CashBankBalancesSummery_Rpt
     * Desktop: VoucherReports.CashandBankBalancesSummery
     */
    @RequestMapping(value = "/reports/print/593-cash-bank-balances-summery", method = RequestMethod.POST)
    public void print593CashBankBalancesSummery(HttpServletResponse response, @RequestBody(required = false) Rpt593CashBankBalancesSummeryRequest request) throws Exception {
        if (request == null) request = new Rpt593CashBankBalancesSummeryRequest();
        printReport(response, "593-CashBankBalancesSummery_Rpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/593-cash-bank-balances-summery", method = RequestMethod.GET)
    public void print593CashBankBalancesSummeryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print593CashBankBalancesSummery(response, objectMapper.convertValue(query, Rpt593CashBankBalancesSummeryRequest.class));
    }

    /**
     * Template: 593_01_CashBankBalancesSummeryReport.rpt
     * Procedure: Sp_Accounts_CashBankBalancesSummery_Rpt
     * Desktop: VoucherReports.CashandBankBalancesSummery
     */
    @RequestMapping(value = "/reports/print/593-01-cash-bank-balances-summery-report", method = RequestMethod.POST)
    public void print59301CashBankBalancesSummeryReport(HttpServletResponse response, @RequestBody(required = false) Rpt59301CashBankBalancesSummeryReportRequest request) throws Exception {
        if (request == null) request = new Rpt59301CashBankBalancesSummeryReportRequest();
        printReport(response, "593_01_CashBankBalancesSummeryReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/593-01-cash-bank-balances-summery-report", method = RequestMethod.GET)
    public void print59301CashBankBalancesSummeryReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print59301CashBankBalancesSummeryReport(response, objectMapper.convertValue(query, Rpt59301CashBankBalancesSummeryReportRequest.class));
    }

    /**
     * Template: 590-BankBalances.rpt
     * Procedure: Sp_Accounts_BankBalances_Rpt
     * Desktop: VoucherReports.BankBalances
     */
    @RequestMapping(value = "/reports/print/590-bank-balances", method = RequestMethod.POST)
    public void print590BankBalances(HttpServletResponse response, @RequestBody(required = false) Rpt590BankBalancesRequest request) throws Exception {
        if (request == null) request = new Rpt590BankBalancesRequest();
        printReport(response, "590-BankBalances.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/590-bank-balances", method = RequestMethod.GET)
    public void print590BankBalancesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print590BankBalances(response, objectMapper.convertValue(query, Rpt590BankBalancesRequest.class));
    }

    /**
     * Template: 590_01_BankBalancesWithSummary.rpt
     * Procedure: Sp_Accounts_BankBalances_Rpt
     * Desktop: VoucherReports.BankBalances
     */
    @RequestMapping(value = "/reports/print/590-01-bank-balances-with-summary", method = RequestMethod.POST)
    public void print59001BankBalancesWithSummary(HttpServletResponse response, @RequestBody(required = false) Rpt59001BankBalancesWithSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt59001BankBalancesWithSummaryRequest();
        printReport(response, "590_01_BankBalancesWithSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/590-01-bank-balances-with-summary", method = RequestMethod.GET)
    public void print59001BankBalancesWithSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print59001BankBalancesWithSummary(response, objectMapper.convertValue(query, Rpt59001BankBalancesWithSummaryRequest.class));
    }

    /**
     * Template: 141-AccountPayablesWithLastBillAmountAndDate.rpt
     * Procedure: Sp_Accounts_PayablesWithLastBillAndPaidAmount_Rpt
     * Desktop: VoucherReports.PayablesWithLastBillAndPaidAmount
     */
    @RequestMapping(value = "/reports/print/141-account-payables-with-last-bill-amount-and-date", method = RequestMethod.POST)
    public void print141AccountPayablesWithLastBillAmountAndDate(HttpServletResponse response, @RequestBody(required = false) Rpt141AccountPayablesWithLastBillAmountAndDateRequest request) throws Exception {
        if (request == null) request = new Rpt141AccountPayablesWithLastBillAmountAndDateRequest();
        printReport(response, "141-AccountPayablesWithLastBillAmountAndDate.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/141-account-payables-with-last-bill-amount-and-date", method = RequestMethod.GET)
    public void print141AccountPayablesWithLastBillAmountAndDateGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print141AccountPayablesWithLastBillAmountAndDate(response, objectMapper.convertValue(query, Rpt141AccountPayablesWithLastBillAmountAndDateRequest.class));
    }

    /**
     * Template: 592-CashBalances.rpt
     * Procedure: Sp_Accounts_CashBalances_Rpt
     * Desktop: VoucherReports.CashBalances
     */
    @RequestMapping(value = "/reports/print/592-cash-balances", method = RequestMethod.POST)
    public void print592CashBalances(HttpServletResponse response, @RequestBody(required = false) Rpt592CashBalancesRequest request) throws Exception {
        if (request == null) request = new Rpt592CashBalancesRequest();
        printReport(response, "592-CashBalances.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/592-cash-balances", method = RequestMethod.GET)
    public void print592CashBalancesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print592CashBalances(response, objectMapper.convertValue(query, Rpt592CashBalancesRequest.class));
    }

    /**
     * Template: 157-CommissionAndBrokary.rpt
     * Procedure: Sp_CommissionAndBrokery_Report
     * Desktop: GeneralReprots.GeneralLedgerCommissionAndBrokery
     */
    @RequestMapping(value = "/reports/print/157-commission-and-brokary", method = RequestMethod.POST)
    public void print157CommissionAndBrokary(HttpServletResponse response, @RequestBody(required = false) Rpt157CommissionAndBrokaryRequest request) throws Exception {
        if (request == null) request = new Rpt157CommissionAndBrokaryRequest();
        printReport(response, "157-CommissionAndBrokary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/157-commission-and-brokary", method = RequestMethod.GET)
    public void print157CommissionAndBrokaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print157CommissionAndBrokary(response, objectMapper.convertValue(query, Rpt157CommissionAndBrokaryRequest.class));
    }

    /**
     * Template: 001-ContractorWagesRegister.rpt
     * Procedure: USP_GetSummaryWagesByRefDocumentsAndActivities
     * Desktop: InvContractorWagesBillHeader.GetSummaryWagesByRefDocumentsAndActivities
     */
    @RequestMapping(value = "/reports/print/001-contractor-wages-register", method = RequestMethod.POST)
    public void print001ContractorWagesRegister(HttpServletResponse response, @RequestBody(required = false) Rpt001ContractorWagesRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt001ContractorWagesRegisterRequest();
        printReport(response, "001-ContractorWagesRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/001-contractor-wages-register", method = RequestMethod.GET)
    public void print001ContractorWagesRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print001ContractorWagesRegister(response, objectMapper.convertValue(query, Rpt001ContractorWagesRegisterRequest.class));
    }

    /**
     * Template: 130-RptAcSupplierCustomerQuantativeGL.rpt
     * Procedure: Sp_GeneralLedger_SupplierCustomerQuantative
     * Desktop: InventoryStockEvalautionDetail.GeneralLedgerSupplierCustomerQuantative
     */
    @RequestMapping(value = "/reports/print/130-ac-supplier-customer-quantative-gl", method = RequestMethod.POST)
    public void print130AcSupplierCustomerQuantativeGL(HttpServletResponse response, @RequestBody(required = false) Rpt130AcSupplierCustomerQuantativeGLRequest request) throws Exception {
        if (request == null) request = new Rpt130AcSupplierCustomerQuantativeGLRequest();
        printReport(response, "130-RptAcSupplierCustomerQuantativeGL.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/130-ac-supplier-customer-quantative-gl", method = RequestMethod.GET)
    public void print130AcSupplierCustomerQuantativeGLGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print130AcSupplierCustomerQuantativeGL(response, objectMapper.convertValue(query, Rpt130AcSupplierCustomerQuantativeGLRequest.class));
    }

    /**
     * Template: 105-AcRptGeneralLedger.rpt
     * Procedure: Sp_Accounts_GeneralLedger_Rpt
     * Desktop: VoucherReports.GeneralLedgerWithOffsetAccount
     */
    @RequestMapping(value = "/reports/print/105-general-ledger", method = RequestMethod.POST)
    public void print105GeneralLedger(HttpServletResponse response, @RequestBody(required = false) Rpt105GeneralLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt105GeneralLedgerRequest();
        printReport(response, "105-AcRptGeneralLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/105-general-ledger", method = RequestMethod.GET)
    public void print105GeneralLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print105GeneralLedger(response, objectMapper.convertValue(query, Rpt105GeneralLedgerRequest.class));
    }

    /**
     * Template: 106-AcRptGeneralLedgerB.rpt
     * Procedure: Sp_Accounts_GeneralLedger_Rpt
     * Desktop: VoucherReports.GeneralLedgerWithOffsetAccount
     */
    @RequestMapping(value = "/reports/print/106-general-ledger-b", method = RequestMethod.POST)
    public void print106GeneralLedgerB(HttpServletResponse response, @RequestBody(required = false) Rpt106GeneralLedgerBRequest request) throws Exception {
        if (request == null) request = new Rpt106GeneralLedgerBRequest();
        printReport(response, "106-AcRptGeneralLedgerB.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/106-general-ledger-b", method = RequestMethod.GET)
    public void print106GeneralLedgerBGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print106GeneralLedgerB(response, objectMapper.convertValue(query, Rpt106GeneralLedgerBRequest.class));
    }

    /**
     * Template: 107-AcRptQuantativeLedger.rpt
     * Procedure: Sp_Accounts_GeneralLedger_Rpt
     * Desktop: VoucherReports.GeneralLedgerWithOffsetAccount
     */
    @RequestMapping(value = "/reports/print/107-quantative-ledger", method = RequestMethod.POST)
    public void print107QuantativeLedger(HttpServletResponse response, @RequestBody(required = false) Rpt107QuantativeLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt107QuantativeLedgerRequest();
        printReport(response, "107-AcRptQuantativeLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/107-quantative-ledger", method = RequestMethod.GET)
    public void print107QuantativeLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print107QuantativeLedger(response, objectMapper.convertValue(query, Rpt107QuantativeLedgerRequest.class));
    }

    /**
     * Template: 108-AcRptGeneralLedgerSummery.rpt
     * Procedure: Sp_VouchersAccountsGeneralLedgerSummery
     * Desktop: VoucherReports.GeneralLedgerSummery
     */
    @RequestMapping(value = "/reports/print/108-general-ledger-summery", method = RequestMethod.POST)
    public void print108GeneralLedgerSummery(HttpServletResponse response, @RequestBody(required = false) Rpt108GeneralLedgerSummeryRequest request) throws Exception {
        if (request == null) request = new Rpt108GeneralLedgerSummeryRequest();
        printReport(response, "108-AcRptGeneralLedgerSummery.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/108-general-ledger-summery", method = RequestMethod.GET)
    public void print108GeneralLedgerSummeryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print108GeneralLedgerSummery(response, objectMapper.convertValue(query, Rpt108GeneralLedgerSummeryRequest.class));
    }

    /**
     * Template: 105A-GeneralLedgerSummary2.rpt
     * Procedure: SpAccounts_GeneralLedger2Format_Rpt
     * Desktop: VoucherReports.GeneralLedger2Format_Rpt
     */
    @RequestMapping(value = "/reports/print/105a-general-ledger-summary-2", method = RequestMethod.POST)
    public void print105AGeneralLedgerSummary2(HttpServletResponse response, @RequestBody(required = false) Rpt105AGeneralLedgerSummary2Request request) throws Exception {
        if (request == null) request = new Rpt105AGeneralLedgerSummary2Request();
        printReport(response, "105A-GeneralLedgerSummary2.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/105a-general-ledger-summary-2", method = RequestMethod.GET)
    public void print105AGeneralLedgerSummary2Get(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print105AGeneralLedgerSummary2(response, objectMapper.convertValue(query, Rpt105AGeneralLedgerSummary2Request.class));
    }

    /**
     * Template: 109-AcRptGeneralLedgerStatement.rpt
     * Procedure: Sp_GeneralLedgerStatement_Rpt
     * Desktop: VoucherReports.GeneralLedgerStatement
     */
    @RequestMapping(value = "/reports/print/109-general-ledger-statement", method = RequestMethod.POST)
    public void print109GeneralLedgerStatement(HttpServletResponse response, @RequestBody(required = false) Rpt109GeneralLedgerStatementRequest request) throws Exception {
        if (request == null) request = new Rpt109GeneralLedgerStatementRequest();
        printReport(response, "109-AcRptGeneralLedgerStatement.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/109-general-ledger-statement", method = RequestMethod.GET)
    public void print109GeneralLedgerStatementGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print109GeneralLedgerStatement(response, objectMapper.convertValue(query, Rpt109GeneralLedgerStatementRequest.class));
    }

    /**
     * Template: 134-InventoryPayablesandReceivables.rpt
     * Procedure: SpAccounts_InventoryPayablesandReceivables_Rpt
     * Desktop: GeneralReprots.InventoryPayablesandReceivables
     */
    @RequestMapping(value = "/reports/print/134-inventory-payablesand-receivables", method = RequestMethod.POST)
    public void print134InventoryPayablesandReceivables(HttpServletResponse response, @RequestBody(required = false) Rpt134InventoryPayablesandReceivablesRequest request) throws Exception {
        if (request == null) request = new Rpt134InventoryPayablesandReceivablesRequest();
        printReport(response, "134-InventoryPayablesandReceivables.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/134-inventory-payablesand-receivables", method = RequestMethod.GET)
    public void print134InventoryPayablesandReceivablesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print134InventoryPayablesandReceivables(response, objectMapper.convertValue(query, Rpt134InventoryPayablesandReceivablesRequest.class));
    }

    /**
     * Template: 134A-InventoryPayablesandReceivables.rpt
     * Procedure: SpAccounts_InventoryPayablesandReceivables_Rpt
     * Desktop: GeneralReprots.InventoryPayablesandReceivables
     */
    @RequestMapping(value = "/reports/print/134a-inventory-payablesand-receivables", method = RequestMethod.POST)
    public void print134AInventoryPayablesandReceivables(HttpServletResponse response, @RequestBody(required = false) Rpt134AInventoryPayablesandReceivablesRequest request) throws Exception {
        if (request == null) request = new Rpt134AInventoryPayablesandReceivablesRequest();
        printReport(response, "134A-InventoryPayablesandReceivables.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/134a-inventory-payablesand-receivables", method = RequestMethod.GET)
    public void print134AInventoryPayablesandReceivablesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print134AInventoryPayablesandReceivables(response, objectMapper.convertValue(query, Rpt134AInventoryPayablesandReceivablesRequest.class));
    }

    /**
     * Template: 121_01-ReceivablesByDueDates.rpt
     * Procedure: [dbo].[Usp_ReceivablesByDueDates]
     * Desktop: VoucherReports.ReceivablesByDueDates
     */
    @RequestMapping(value = "/reports/print/121-01-receivables-by-due-dates", method = RequestMethod.POST)
    public void print12101ReceivablesByDueDates(HttpServletResponse response, @RequestBody(required = false) Rpt12101ReceivablesByDueDatesRequest request) throws Exception {
        if (request == null) request = new Rpt12101ReceivablesByDueDatesRequest();
        printReport(response, "121_01-ReceivablesByDueDates.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/121-01-receivables-by-due-dates", method = RequestMethod.GET)
    public void print12101ReceivablesByDueDatesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print12101ReceivablesByDueDates(response, objectMapper.convertValue(query, Rpt12101ReceivablesByDueDatesRequest.class));
    }

    /**
     * Template: 122-AcRptAccounts_ReceivablesWithStatus.rpt
     * Procedure: Sp_Accounts_Receivables_Rpt
     * Desktop: VoucherReports.ReceivablesReport
     */
    @RequestMapping(value = "/reports/print/122-accounts-receivables-with-status", method = RequestMethod.POST)
    public void print122AccountsReceivablesWithStatus(HttpServletResponse response, @RequestBody(required = false) Rpt122AccountsReceivablesWithStatusRequest request) throws Exception {
        if (request == null) request = new Rpt122AccountsReceivablesWithStatusRequest();
        printReport(response, "122-AcRptAccounts_ReceivablesWithStatus.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/122-accounts-receivables-with-status", method = RequestMethod.GET)
    public void print122AccountsReceivablesWithStatusGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print122AccountsReceivablesWithStatus(response, objectMapper.convertValue(query, Rpt122AccountsReceivablesWithStatusRequest.class));
    }

    /**
     * Template: 124_Payables_New.rpt
     * Procedure: UPS_PayablesAging_New
     * Desktop: VoucherReports.PayablesAging_New
     */
    @RequestMapping(value = "/reports/print/124-payables-new", method = RequestMethod.POST)
    public void print124PayablesNew(HttpServletResponse response, @RequestBody(required = false) Rpt124PayablesNewRequest request) throws Exception {
        if (request == null) request = new Rpt124PayablesNewRequest();
        printReport(response, "124_Payables_New.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/124-payables-new", method = RequestMethod.GET)
    public void print124PayablesNewGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print124PayablesNew(response, objectMapper.convertValue(query, Rpt124PayablesNewRequest.class));
    }

    /**
     * Template: 127-SupplierAgingReport_DocumentWise.rpt
     * Procedure: usp_SupplierAgingReport_DocumentWise
     * Desktop: VoucherReports.SupplierAgingDocumentWise
     */
    @RequestMapping(value = "/reports/print/127-supplier-aging-report-document-wise", method = RequestMethod.POST)
    public void print127SupplierAgingReportDocumentWise(HttpServletResponse response, @RequestBody(required = false) Rpt127SupplierAgingReportDocumentWiseRequest request) throws Exception {
        if (request == null) request = new Rpt127SupplierAgingReportDocumentWiseRequest();
        printReport(response, "127-SupplierAgingReport_DocumentWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/127-supplier-aging-report-document-wise", method = RequestMethod.GET)
    public void print127SupplierAgingReportDocumentWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print127SupplierAgingReportDocumentWise(response, objectMapper.convertValue(query, Rpt127SupplierAgingReportDocumentWiseRequest.class));
    }

    /**
     * Template: 123_Receivables_New.rpt
     * Procedure: UPS_ReceivablesAging_New
     * Desktop: VoucherReports.ReceivablesAging_New
     */
    @RequestMapping(value = "/reports/print/123-receivables-new", method = RequestMethod.POST)
    public void print123ReceivablesNew(HttpServletResponse response, @RequestBody(required = false) Rpt123ReceivablesNewRequest request) throws Exception {
        if (request == null) request = new Rpt123ReceivablesNewRequest();
        printReport(response, "123_Receivables_New.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/123-receivables-new", method = RequestMethod.GET)
    public void print123ReceivablesNewGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print123ReceivablesNew(response, objectMapper.convertValue(query, Rpt123ReceivablesNewRequest.class));
    }

    /**
     * Template: 126-CustomerAgingReport_DocumentWise.rpt
     * Procedure: usp_CustomerAgingReport_DocumentWise
     * Desktop: VoucherReports.CustomerAgingDocumentWise
     */
    @RequestMapping(value = "/reports/print/126-customer-aging-report-document-wise", method = RequestMethod.POST)
    public void print126CustomerAgingReportDocumentWise(HttpServletResponse response, @RequestBody(required = false) Rpt126CustomerAgingReportDocumentWiseRequest request) throws Exception {
        if (request == null) request = new Rpt126CustomerAgingReportDocumentWiseRequest();
        printReport(response, "126-CustomerAgingReport_DocumentWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/126-customer-aging-report-document-wise", method = RequestMethod.GET)
    public void print126CustomerAgingReportDocumentWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print126CustomerAgingReportDocumentWise(response, objectMapper.convertValue(query, Rpt126CustomerAgingReportDocumentWiseRequest.class));
    }

    /**
     * Template: 113A-AcRptTrialBalanceSelectedGroupAc.rpt
     * Procedure: SpAccounts_TrialBalanceSelectedNew_Report
     * Desktop: VoucherReports.SelectedTrialBalanceNew
     */
    @RequestMapping(value = "/reports/print/113a-trial-balance-selected-group-ac", method = RequestMethod.POST)
    public void print113ATrialBalanceSelectedGroupAc(HttpServletResponse response, @RequestBody(required = false) Rpt113ATrialBalanceSelectedGroupAcRequest request) throws Exception {
        if (request == null) request = new Rpt113ATrialBalanceSelectedGroupAcRequest();
        printReport(response, "113A-AcRptTrialBalanceSelectedGroupAc.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/113a-trial-balance-selected-group-ac", method = RequestMethod.GET)
    public void print113ATrialBalanceSelectedGroupAcGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print113ATrialBalanceSelectedGroupAc(response, objectMapper.convertValue(query, Rpt113ATrialBalanceSelectedGroupAcRequest.class));
    }

    /**
     * Template: 375-GetSubsidiaryAccountFromVouchers.rpt
     * Procedure: USP_GetSubsidiaryPayablesReceivables
     * Desktop: VoucherHead.GetSubsidiaryPayablesReceivables
     */
    @RequestMapping(value = "/reports/print/375-get-subsidiary-account-from-vouchers", method = RequestMethod.POST)
    public void print375GetSubsidiaryAccountFromVouchers(HttpServletResponse response, @RequestBody(required = false) Rpt375GetSubsidiaryAccountFromVouchersRequest request) throws Exception {
        if (request == null) request = new Rpt375GetSubsidiaryAccountFromVouchersRequest();
        printReport(response, "375-GetSubsidiaryAccountFromVouchers.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/375-get-subsidiary-account-from-vouchers", method = RequestMethod.GET)
    public void print375GetSubsidiaryAccountFromVouchersGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print375GetSubsidiaryAccountFromVouchers(response, objectMapper.convertValue(query, Rpt375GetSubsidiaryAccountFromVouchersRequest.class));
    }

    /**
     * Template: 111A-AcRptTrialBalances.rpt
     * Procedure: Sp_TrialBalance_Rpt
     * Desktop: VoucherReports.TrialBalanceReport
     */
    @RequestMapping(value = "/reports/print/111a-trial-balances", method = RequestMethod.POST)
    public void print111ATrialBalances(HttpServletResponse response, @RequestBody(required = false) Rpt111ATrialBalancesRequest request) throws Exception {
        if (request == null) request = new Rpt111ATrialBalancesRequest();
        Map<String, Object> report = reportDataService.run("111a-acrpttrialbalances", request.toArgs());
        @SuppressWarnings("unchecked")
        List<Map<String, Object>> rows = (List<Map<String, Object>>) report.getOrDefault("rows", List.of());

        /* TrialBalance.btntrialMultisubPrint_Click reuses dtTrial and builds these two in-memory
           tables itself; there are no separate stored-procedure calls for either subreport. */
        List<Map<String, Object>> debitRows = new ArrayList<>();
        List<Map<String, Object>> creditRows = new ArrayList<>();
        for (Map<String, Object> row : rows) {
            if (toDouble(row.get("ClosingDr")) > 0.0) debitRows.add(row);
            if (toDouble(row.get("ClosingCr")) > 0.0) creditRows.add(row);
        }
        report.put("subReports", List.of(
                Map.of("template", "TrialBalanceDebitSubReport.rpt", "rows", debitRows),
                Map.of("template", "TrialBalanceCreditSubReport.rpt", "rows", creditRows)));
        printReportData(response, "111A-AcRptTrialBalances.rpt", report);
    }

    @RequestMapping(value = "/reports/print/111a-trial-balances", method = RequestMethod.GET)
    public void print111ATrialBalancesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print111ATrialBalances(response, objectMapper.convertValue(query, Rpt111ATrialBalancesRequest.class));
    }

    /**
     * Template: 118-AcRptVoucherSlip.rpt
     * Procedure: Sp_Accounts_VouchersValidation_Rpt
     * Desktop: VoucherReports.VoucherValidationReport
     */
    @RequestMapping(value = "/reports/print/118-voucher-slip", method = RequestMethod.POST)
    public void print118VoucherSlip(HttpServletResponse response, @RequestBody(required = false) Rpt118VoucherSlipRequest request) throws Exception {
        if (request == null) request = new Rpt118VoucherSlipRequest();
        printReport(response, "118-AcRptVoucherSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/118-voucher-slip", method = RequestMethod.GET)
    public void print118VoucherSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print118VoucherSlip(response, objectMapper.convertValue(query, Rpt118VoucherSlipRequest.class));
    }

    /**
     * Template: 118A-VoucherReport.rpt
     * Procedure: Sp_Accounts_VouchersValidation_Rpt
     * Desktop: VoucherReports.VoucherValidationReport
     */
    @RequestMapping(value = "/reports/print/118a-voucher-report", method = RequestMethod.POST)
    public void print118AVoucherReport(HttpServletResponse response, @RequestBody(required = false) Rpt118AVoucherReportRequest request) throws Exception {
        if (request == null) request = new Rpt118AVoucherReportRequest();
        printReport(response, "118A-VoucherReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/118a-voucher-report", method = RequestMethod.GET)
    public void print118AVoucherReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print118AVoucherReport(response, objectMapper.convertValue(query, Rpt118AVoucherReportRequest.class));
    }

    /**
     * Template: 102-BankPaymentVoucher.rpt
     * Procedure: SpVouchers_PaymentReceiptVoucherSlipNew_Rpt
     * Desktop: VoucherReports.NewVoucherReprot
     */
    @RequestMapping(value = "/reports/print/102-bank-payment-voucher", method = RequestMethod.POST)
    public void print102BankPaymentVoucher(HttpServletResponse response, @RequestBody(required = false) Rpt102BankPaymentVoucherRequest request) throws Exception {
        if (request == null) request = new Rpt102BankPaymentVoucherRequest();
        printReport(response, "102-BankPaymentVoucher.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/102-bank-payment-voucher", method = RequestMethod.GET)
    public void print102BankPaymentVoucherGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print102BankPaymentVoucher(response, objectMapper.convertValue(query, Rpt102BankPaymentVoucherRequest.class));
    }

    /**
     * Template: 901_VoucherInvoicesAdjustment_Slip.rpt
     * Procedure: [dbo].[USP_VoucherInvoicesAdjustment_ReadByVoucherHeadId]
     * Desktop: VoucherInvoicesAdjustment.GetByVoucherHeadId
     */
    @RequestMapping(value = "/reports/print/901-voucher-invoices-adjustment-slip", method = RequestMethod.POST)
    public void print901VoucherInvoicesAdjustmentSlip(HttpServletResponse response, @RequestBody(required = false) Rpt901VoucherInvoicesAdjustmentSlipRequest request) throws Exception {
        if (request == null) request = new Rpt901VoucherInvoicesAdjustmentSlipRequest();
        printReport(response, "901_VoucherInvoicesAdjustment_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/901-voucher-invoices-adjustment-slip", method = RequestMethod.GET)
    public void print901VoucherInvoicesAdjustmentSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print901VoucherInvoicesAdjustmentSlip(response, objectMapper.convertValue(query, Rpt901VoucherInvoicesAdjustmentSlipRequest.class));
    }

    /**
     * Template: 102_01_BillsPayablesOrReceiveableVoucher.rpt
     * Procedure: SpVouchers_PaymentReceiptVoucherSlipNew_Rpt
     * Desktop: VoucherReports.NewVoucherReprot
     */
    @RequestMapping(value = "/reports/print/102-01-bills-payables-or-receiveable-voucher", method = RequestMethod.POST)
    public void print10201BillsPayablesOrReceiveableVoucher(HttpServletResponse response, @RequestBody(required = false) Rpt10201BillsPayablesOrReceiveableVoucherRequest request) throws Exception {
        if (request == null) request = new Rpt10201BillsPayablesOrReceiveableVoucherRequest();
        printReport(response, "102_01_BillsPayablesOrReceiveableVoucher.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/102-01-bills-payables-or-receiveable-voucher", method = RequestMethod.GET)
    public void print10201BillsPayablesOrReceiveableVoucherGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print10201BillsPayablesOrReceiveableVoucher(response, objectMapper.convertValue(query, Rpt10201BillsPayablesOrReceiveableVoucherRequest.class));
    }

    /**
     * Template: 144-DayBookSlip.rpt
     * Procedure: Sp_DayBookSlip
     * Desktop: GeneralReprots.DayBookSlip
     */
    @RequestMapping(value = "/reports/print/144-day-book-slip", method = RequestMethod.POST)
    public void print144DayBookSlip(HttpServletResponse response, @RequestBody(required = false) Rpt144DayBookSlipRequest request) throws Exception {
        if (request == null) request = new Rpt144DayBookSlipRequest();
        printReport(response, "144-DayBookSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/144-day-book-slip", method = RequestMethod.GET)
    public void print144DayBookSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print144DayBookSlip(response, objectMapper.convertValue(query, Rpt144DayBookSlipRequest.class));
    }

    /**
     * Template: 002-ContractorWagesSlip.rpt
     * Procedure: Sp_InvContractorWagesBillHeader_SlipandRegister
     * Desktop: InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister
     */
    @RequestMapping(value = "/reports/print/002-contractor-wages-slip", method = RequestMethod.POST)
    public void print002ContractorWagesSlip(HttpServletResponse response, @RequestBody(required = false) Rpt002ContractorWagesSlipRequest request) throws Exception {
        if (request == null) request = new Rpt002ContractorWagesSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "002-ContractorWagesSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/002-contractor-wages-slip", method = RequestMethod.GET)
    public void print002ContractorWagesSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print002ContractorWagesSlip(response, objectMapper.convertValue(query, Rpt002ContractorWagesSlipRequest.class));
    }

    /**
     * Template: 004-ContractorWagesBillManualSlip.rpt
     * Procedure: Sp_InvContractorWagesBillHeader_SlipandRegister
     * Desktop: InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister
     */
    @RequestMapping(value = "/reports/print/004-contractor-wages-bill-manual-slip", method = RequestMethod.POST)
    public void print004ContractorWagesBillManualSlip(HttpServletResponse response, @RequestBody(required = false) Rpt004ContractorWagesBillManualSlipRequest request) throws Exception {
        if (request == null) request = new Rpt004ContractorWagesBillManualSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "004-ContractorWagesBillManualSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/004-contractor-wages-bill-manual-slip", method = RequestMethod.GET)
    public void print004ContractorWagesBillManualSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print004ContractorWagesBillManualSlip(response, objectMapper.convertValue(query, Rpt004ContractorWagesBillManualSlipRequest.class));
    }

    /**
     * Template: 102B-SpVouchers_PartyPaymentAndReceiptSummary.rpt
     * Procedure: SpVouchers_PartyPaymentAndReceiptSummary_Rpt
     * Desktop: VoucherReports.SummaryVoucherReprot
     */
    @RequestMapping(value = "/reports/print/102b-sp-vouchers-party-payment-and-receipt-summary", method = RequestMethod.POST)
    public void print102BSpVouchersPartyPaymentAndReceiptSummary(HttpServletResponse response, @RequestBody(required = false) Rpt102BSpVouchersPartyPaymentAndReceiptSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt102BSpVouchersPartyPaymentAndReceiptSummaryRequest();
        printReport(response, "102B-SpVouchers_PartyPaymentAndReceiptSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/102b-sp-vouchers-party-payment-and-receipt-summary", method = RequestMethod.GET)
    public void print102BSpVouchersPartyPaymentAndReceiptSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print102BSpVouchersPartyPaymentAndReceiptSummary(response, objectMapper.convertValue(query, Rpt102BSpVouchersPartyPaymentAndReceiptSummaryRequest.class));
    }

    /**
     * Template: 115-AcRptVoucherValidation.rpt
     * Procedure: Sp_Accounts_VouchersValidation_Rpt
     * Desktop: VoucherReports.VoucherValidationReport
     */
    @RequestMapping(value = "/reports/print/115-voucher-validation", method = RequestMethod.POST)
    public void print115VoucherValidation(HttpServletResponse response, @RequestBody(required = false) Rpt115VoucherValidationRequest request) throws Exception {
        if (request == null) request = new Rpt115VoucherValidationRequest();
        printReport(response, "115-AcRptVoucherValidation.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/115-voucher-validation", method = RequestMethod.GET)
    public void print115VoucherValidationGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print115VoucherValidation(response, objectMapper.convertValue(query, Rpt115VoucherValidationRequest.class));
    }

    /**
     * Template: 119-AcRptVoucherValidation2.rpt
     * Procedure: Sp_Accounts_VouchersValidation_Rpt
     * Desktop: VoucherReports.VoucherValidationReport
     */
    @RequestMapping(value = "/reports/print/119-voucher-validation-2", method = RequestMethod.POST)
    public void print119VoucherValidation2(HttpServletResponse response, @RequestBody(required = false) Rpt119VoucherValidation2Request request) throws Exception {
        if (request == null) request = new Rpt119VoucherValidation2Request();
        printReport(response, "119-AcRptVoucherValidation2.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/119-voucher-validation-2", method = RequestMethod.GET)
    public void print119VoucherValidation2Get(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print119VoucherValidation2(response, objectMapper.convertValue(query, Rpt119VoucherValidation2Request.class));
    }

    /**
     * Template: 594-CashBalancesInOutFlow.rpt
     * Procedure: Sp_Accounts_CashAndBank_InflowOutFlow_Rpt
     * Desktop: VoucherReports.CashAndBank_InflowOutFlow_Rpt
     */
    @RequestMapping(value = "/reports/print/594-cash-balances-in-out-flow", method = RequestMethod.POST)
    public void print594CashBalancesInOutFlow(HttpServletResponse response, @RequestBody(required = false) Rpt594CashBalancesInOutFlowRequest request) throws Exception {
        if (request == null) request = new Rpt594CashBalancesInOutFlowRequest();
        printReport(response, "594-CashBalancesInOutFlow.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/594-cash-balances-in-out-flow", method = RequestMethod.GET)
    public void print594CashBalancesInOutFlowGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print594CashBalancesInOutFlow(response, objectMapper.convertValue(query, Rpt594CashBalancesInOutFlowRequest.class));
    }

    /**
     * Template: 595-Cash&BankPaymentReciept.rpt
     * Procedure: Sp_Accounts_DialyCashAndBankBalances_Rpt
     * Desktop: VoucherReports.DialyCashAndBankBalances_Rpt
     */
    @RequestMapping(value = "/reports/print/595-cash-bank-payment-reciept", method = RequestMethod.POST)
    public void print595CashBankPaymentReciept(HttpServletResponse response, @RequestBody(required = false) Rpt595CashBankPaymentRecieptRequest request) throws Exception {
        if (request == null) request = new Rpt595CashBankPaymentRecieptRequest();
        printReport(response, "595-Cash&BankPaymentReciept.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/595-cash-bank-payment-reciept", method = RequestMethod.GET)
    public void print595CashBankPaymentRecieptGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print595CashBankPaymentReciept(response, objectMapper.convertValue(query, Rpt595CashBankPaymentRecieptRequest.class));
    }

    /**
     * Template: 599_Cash&BankPaymentRecieptWithAllJv.rpt
     * Procedure: Sp_Accounts_DialyCashAndBankBalances_Rpt
     * Desktop: VoucherReports.DialyCashAndBankBalances_Rpt
     */
    @RequestMapping(value = "/reports/print/599-cash-bank-payment-reciept-with-all-jv", method = RequestMethod.POST)
    public void print599CashBankPaymentRecieptWithAllJv(HttpServletResponse response, @RequestBody(required = false) Rpt599CashBankPaymentRecieptWithAllJvRequest request) throws Exception {
        if (request == null) request = new Rpt599CashBankPaymentRecieptWithAllJvRequest();
        printReport(response, "599_Cash&BankPaymentRecieptWithAllJv.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/599-cash-bank-payment-reciept-with-all-jv", method = RequestMethod.GET)
    public void print599CashBankPaymentRecieptWithAllJvGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print599CashBankPaymentRecieptWithAllJv(response, objectMapper.convertValue(query, Rpt599CashBankPaymentRecieptWithAllJvRequest.class));
    }

    /**
     * Template: 241-FreightVoucherSlip.rpt
     * Procedure: SP_FreightVoucherSlipAndRegister
     * Desktop: InvFreightVoucher.FreightVoucherSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/241-freight-voucher-slip", method = RequestMethod.POST)
    public void print241FreightVoucherSlip(HttpServletResponse response, @RequestBody(required = false) Rpt241FreightVoucherSlipRequest request) throws Exception {
        if (request == null) request = new Rpt241FreightVoucherSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "241-FreightVoucherSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/241-freight-voucher-slip", method = RequestMethod.GET)
    public void print241FreightVoucherSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print241FreightVoucherSlip(response, objectMapper.convertValue(query, Rpt241FreightVoucherSlipRequest.class));
    }

    /**
     * Template: 102-ANewAcRptPaymentReceiptsVoucherSlip.rpt
     * Procedure: SpVouchers_PaymentReceiptVoucherSlipNew_Rpt
     * Desktop: VoucherReports.NewVoucherReprot
     */
    @RequestMapping(value = "/reports/print/102-a-new-ac-payment-receipts-voucher-slip", method = RequestMethod.POST)
    public void print102ANewAcPaymentReceiptsVoucherSlip(HttpServletResponse response, @RequestBody(required = false) Rpt102ANewAcPaymentReceiptsVoucherSlipRequest request) throws Exception {
        if (request == null) request = new Rpt102ANewAcPaymentReceiptsVoucherSlipRequest();
        printReport(response, "102-ANewAcRptPaymentReceiptsVoucherSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/102-a-new-ac-payment-receipts-voucher-slip", method = RequestMethod.GET)
    public void print102ANewAcPaymentReceiptsVoucherSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print102ANewAcPaymentReceiptsVoucherSlip(response, objectMapper.convertValue(query, Rpt102ANewAcPaymentReceiptsVoucherSlipRequest.class));
    }

    /**
     * Template: 122A-CityWise-AcRptAccounts_ReceivablesWithStatus.rpt
     * Procedure: Sp_Accounts_Receivables_Rpt
     * Desktop: VoucherReports.ReceivablesReport
     */
    @RequestMapping(value = "/reports/print/122a-city-wise-ac-accounts-receivables-with-status", method = RequestMethod.POST)
    public void print122ACityWiseAcAccountsReceivablesWithStatus(HttpServletResponse response, @RequestBody(required = false) Rpt122ACityWiseAcAccountsReceivablesWithStatusRequest request) throws Exception {
        if (request == null) request = new Rpt122ACityWiseAcAccountsReceivablesWithStatusRequest();
        printReport(response, "122A-CityWise-AcRptAccounts_ReceivablesWithStatus.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/122a-city-wise-ac-accounts-receivables-with-status", method = RequestMethod.GET)
    public void print122ACityWiseAcAccountsReceivablesWithStatusGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print122ACityWiseAcAccountsReceivablesWithStatus(response, objectMapper.convertValue(query, Rpt122ACityWiseAcAccountsReceivablesWithStatusRequest.class));
    }

    /**
     * Template: 126-PdcInventoryPendingSummery.rpt
     * Procedure: Sp_PdcInventory_ReceiptsSummery_Rpt
     * Desktop: GeneralReprots.PdcReceiptsRegisterSummery
     */
    @RequestMapping(value = "/reports/print/126-pdc-inventory-pending-summery", method = RequestMethod.POST)
    public void print126PdcInventoryPendingSummery(HttpServletResponse response, @RequestBody(required = false) Rpt126PdcInventoryPendingSummeryRequest request) throws Exception {
        if (request == null) request = new Rpt126PdcInventoryPendingSummeryRequest();
        printReport(response, "126-PdcInventoryPendingSummery.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/126-pdc-inventory-pending-summery", method = RequestMethod.GET)
    public void print126PdcInventoryPendingSummeryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print126PdcInventoryPendingSummery(response, objectMapper.convertValue(query, Rpt126PdcInventoryPendingSummeryRequest.class));
    }

    /**
     * Template: 125-PdcInventoryPending.rpt
     * Procedure: Sp_PdcInventory_SlipAndRegister_Rpt
     * Desktop: GeneralReprots.PdcReceiptsRegister
     */
    @RequestMapping(value = "/reports/print/125-pdc-inventory-pending", method = RequestMethod.POST)
    public void print125PdcInventoryPending(HttpServletResponse response, @RequestBody(required = false) Rpt125PdcInventoryPendingRequest request) throws Exception {
        if (request == null) request = new Rpt125PdcInventoryPendingRequest();
        printReport(response, "125-PdcInventoryPending.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/125-pdc-inventory-pending", method = RequestMethod.GET)
    public void print125PdcInventoryPendingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print125PdcInventoryPending(response, objectMapper.convertValue(query, Rpt125PdcInventoryPendingRequest.class));
    }

    /**
     * Template: 1870-WagesDetail.rpt
     * Procedure: [pcc].[USP_WagesReportWithActivities]
     * Desktop: ContractorWagesActivity.WagesActivity
     */
    @RequestMapping(value = "/reports/print/1870-wages-detail", method = RequestMethod.POST)
    public void print1870WagesDetail(HttpServletResponse response, @RequestBody(required = false) Rpt1870WagesDetailRequest request) throws Exception {
        if (request == null) request = new Rpt1870WagesDetailRequest();
        printReport(response, "1870-WagesDetail.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1870-wages-detail", method = RequestMethod.GET)
    public void print1870WagesDetailGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1870WagesDetail(response, objectMapper.convertValue(query, Rpt1870WagesDetailRequest.class));
    }

    /**
     * Template: 1881_WagesDetailWithoutItem.rpt
     * Procedure: [pcc].[USP_WagesReportWithActivities]
     * Desktop: ContractorWagesActivity.WagesActivity
     */
    @RequestMapping(value = "/reports/print/1881-wages-detail-without-item", method = RequestMethod.POST)
    public void print1881WagesDetailWithoutItem(HttpServletResponse response, @RequestBody(required = false) Rpt1881WagesDetailWithoutItemRequest request) throws Exception {
        if (request == null) request = new Rpt1881WagesDetailWithoutItemRequest();
        printReport(response, "1881_WagesDetailWithoutItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1881-wages-detail-without-item", method = RequestMethod.GET)
    public void print1881WagesDetailWithoutItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1881WagesDetailWithoutItem(response, objectMapper.convertValue(query, Rpt1881WagesDetailWithoutItemRequest.class));
    }

    /**
     * Template: 1880-dtSummaryByContractor.rpt
     * Procedure: [pcc].[USP_WagesReportWithActivities]
     * Desktop: ContractorWagesActivity.WagesActivity
     */
    @RequestMapping(value = "/reports/print/1880-dt-summary-by-contractor", method = RequestMethod.POST)
    public void print1880DtSummaryByContractor(HttpServletResponse response, @RequestBody(required = false) Rpt1880DtSummaryByContractorRequest request) throws Exception {
        if (request == null) request = new Rpt1880DtSummaryByContractorRequest();
        printReport(response, "1880-dtSummaryByContractor.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1880-dt-summary-by-contractor", method = RequestMethod.GET)
    public void print1880DtSummaryByContractorGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1880DtSummaryByContractor(response, objectMapper.convertValue(query, Rpt1880DtSummaryByContractorRequest.class));
    }

    /**
     * Template: 1871-SummaryByItem.rpt
     * Procedure: [pcc].[USP_WagesReportWithActivities]
     * Desktop: ContractorWagesActivity.WagesActivity
     */
    @RequestMapping(value = "/reports/print/1871-summary-by-item", method = RequestMethod.POST)
    public void print1871SummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt1871SummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt1871SummaryByItemRequest();
        printReport(response, "1871-SummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1871-summary-by-item", method = RequestMethod.GET)
    public void print1871SummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1871SummaryByItem(response, objectMapper.convertValue(query, Rpt1871SummaryByItemRequest.class));
    }

    /**
     * Template: 1872-SummaryByItemandContractor.rpt
     * Procedure: [pcc].[USP_WagesReportWithActivities]
     * Desktop: ContractorWagesActivity.WagesActivity
     */
    @RequestMapping(value = "/reports/print/1872-summary-by-itemand-contractor", method = RequestMethod.POST)
    public void print1872SummaryByItemandContractor(HttpServletResponse response, @RequestBody(required = false) Rpt1872SummaryByItemandContractorRequest request) throws Exception {
        if (request == null) request = new Rpt1872SummaryByItemandContractorRequest();
        printReport(response, "1872-SummaryByItemandContractor.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1872-summary-by-itemand-contractor", method = RequestMethod.GET)
    public void print1872SummaryByItemandContractorGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1872SummaryByItemandContractor(response, objectMapper.convertValue(query, Rpt1872SummaryByItemandContractorRequest.class));
    }

    /**
     * Template: 1873-SummaryByItemandServiceActivity.rpt
     * Procedure: [pcc].[USP_WagesReportWithActivities]
     * Desktop: ContractorWagesActivity.WagesActivity
     */
    @RequestMapping(value = "/reports/print/1873-summary-by-itemand-service-activity", method = RequestMethod.POST)
    public void print1873SummaryByItemandServiceActivity(HttpServletResponse response, @RequestBody(required = false) Rpt1873SummaryByItemandServiceActivityRequest request) throws Exception {
        if (request == null) request = new Rpt1873SummaryByItemandServiceActivityRequest();
        printReport(response, "1873-SummaryByItemandServiceActivity.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1873-summary-by-itemand-service-activity", method = RequestMethod.GET)
    public void print1873SummaryByItemandServiceActivityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1873SummaryByItemandServiceActivity(response, objectMapper.convertValue(query, Rpt1873SummaryByItemandServiceActivityRequest.class));
    }

    /**
     * Template: 1874-SummaryByContractorandServiceActivity.rpt
     * Procedure: [pcc].[USP_WagesReportWithActivities]
     * Desktop: ContractorWagesActivity.WagesActivity
     */
    @RequestMapping(value = "/reports/print/1874-summary-by-contractorand-service-activity", method = RequestMethod.POST)
    public void print1874SummaryByContractorandServiceActivity(HttpServletResponse response, @RequestBody(required = false) Rpt1874SummaryByContractorandServiceActivityRequest request) throws Exception {
        if (request == null) request = new Rpt1874SummaryByContractorandServiceActivityRequest();
        printReport(response, "1874-SummaryByContractorandServiceActivity.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1874-summary-by-contractorand-service-activity", method = RequestMethod.GET)
    public void print1874SummaryByContractorandServiceActivityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1874SummaryByContractorandServiceActivity(response, objectMapper.convertValue(query, Rpt1874SummaryByContractorandServiceActivityRequest.class));
    }

    /**
     * Template: 1875-SummaryByItemContractorandServiceActivity.rpt
     * Procedure: [pcc].[USP_WagesReportWithActivities]
     * Desktop: ContractorWagesActivity.WagesActivity
     */
    @RequestMapping(value = "/reports/print/1875-summary-by-item-contractorand-service-activity", method = RequestMethod.POST)
    public void print1875SummaryByItemContractorandServiceActivity(HttpServletResponse response, @RequestBody(required = false) Rpt1875SummaryByItemContractorandServiceActivityRequest request) throws Exception {
        if (request == null) request = new Rpt1875SummaryByItemContractorandServiceActivityRequest();
        printReport(response, "1875-SummaryByItemContractorandServiceActivity.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1875-summary-by-item-contractorand-service-activity", method = RequestMethod.GET)
    public void print1875SummaryByItemContractorandServiceActivityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1875SummaryByItemContractorandServiceActivity(response, objectMapper.convertValue(query, Rpt1875SummaryByItemContractorandServiceActivityRequest.class));
    }

    /**
     * Template: 1876-SummaryByWagesGroup.rpt
     * Procedure: [pcc].[USP_WagesReportWithActivities]
     * Desktop: ContractorWagesActivity.WagesActivity
     */
    @RequestMapping(value = "/reports/print/1876-summary-by-wages-group", method = RequestMethod.POST)
    public void print1876SummaryByWagesGroup(HttpServletResponse response, @RequestBody(required = false) Rpt1876SummaryByWagesGroupRequest request) throws Exception {
        if (request == null) request = new Rpt1876SummaryByWagesGroupRequest();
        printReport(response, "1876-SummaryByWagesGroup.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1876-summary-by-wages-group", method = RequestMethod.GET)
    public void print1876SummaryByWagesGroupGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1876SummaryByWagesGroup(response, objectMapper.convertValue(query, Rpt1876SummaryByWagesGroupRequest.class));
    }

    /**
     * Template: 1877-SummaryByWagesGroupandServiceActivity.rpt
     * Procedure: [pcc].[USP_WagesReportWithActivities]
     * Desktop: ContractorWagesActivity.WagesActivity
     */
    @RequestMapping(value = "/reports/print/1877-summary-by-wages-groupand-service-activity", method = RequestMethod.POST)
    public void print1877SummaryByWagesGroupandServiceActivity(HttpServletResponse response, @RequestBody(required = false) Rpt1877SummaryByWagesGroupandServiceActivityRequest request) throws Exception {
        if (request == null) request = new Rpt1877SummaryByWagesGroupandServiceActivityRequest();
        printReport(response, "1877-SummaryByWagesGroupandServiceActivity.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1877-summary-by-wages-groupand-service-activity", method = RequestMethod.GET)
    public void print1877SummaryByWagesGroupandServiceActivityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1877SummaryByWagesGroupandServiceActivity(response, objectMapper.convertValue(query, Rpt1877SummaryByWagesGroupandServiceActivityRequest.class));
    }

    /**
     * Template: 1878-SummaryByWagesGroupandContractor.rpt
     * Procedure: [pcc].[USP_WagesReportWithActivities]
     * Desktop: ContractorWagesActivity.WagesActivity
     */
    @RequestMapping(value = "/reports/print/1878-summary-by-wages-groupand-contractor", method = RequestMethod.POST)
    public void print1878SummaryByWagesGroupandContractor(HttpServletResponse response, @RequestBody(required = false) Rpt1878SummaryByWagesGroupandContractorRequest request) throws Exception {
        if (request == null) request = new Rpt1878SummaryByWagesGroupandContractorRequest();
        printReport(response, "1878-SummaryByWagesGroupandContractor.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1878-summary-by-wages-groupand-contractor", method = RequestMethod.GET)
    public void print1878SummaryByWagesGroupandContractorGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1878SummaryByWagesGroupandContractor(response, objectMapper.convertValue(query, Rpt1878SummaryByWagesGroupandContractorRequest.class));
    }

    /**
     * Template: 1879-SummaryByWagesGroupContractorandServiceActivity.rpt
     * Procedure: [pcc].[USP_WagesReportWithActivities]
     * Desktop: ContractorWagesActivity.WagesActivity
     */
    @RequestMapping(value = "/reports/print/1879-summary-by-wages-group-contractorand-service-activity", method = RequestMethod.POST)
    public void print1879SummaryByWagesGroupContractorandServiceActivity(HttpServletResponse response, @RequestBody(required = false) Rpt1879SummaryByWagesGroupContractorandServiceActivityRequest request) throws Exception {
        if (request == null) request = new Rpt1879SummaryByWagesGroupContractorandServiceActivityRequest();
        printReport(response, "1879-SummaryByWagesGroupContractorandServiceActivity.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1879-summary-by-wages-group-contractorand-service-activity", method = RequestMethod.GET)
    public void print1879SummaryByWagesGroupContractorandServiceActivityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1879SummaryByWagesGroupContractorandServiceActivity(response, objectMapper.convertValue(query, Rpt1879SummaryByWagesGroupContractorandServiceActivityRequest.class));
    }

    /**
     * Template: 102-CashPaymentVoucher.rpt
     * Procedure: SpVouchers_PaymentReceiptVoucherSlipNew_Rpt
     * Desktop: VoucherReports.NewVoucherReprot
     */
    @RequestMapping(value = "/reports/print/102-cash-payment-voucher", method = RequestMethod.POST)
    public void print102CashPaymentVoucher(HttpServletResponse response, @RequestBody(required = false) Rpt102CashPaymentVoucherRequest request) throws Exception {
        if (request == null) request = new Rpt102CashPaymentVoucherRequest();
        printReport(response, "102-CashPaymentVoucher.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/102-cash-payment-voucher", method = RequestMethod.GET)
    public void print102CashPaymentVoucherGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print102CashPaymentVoucher(response, objectMapper.convertValue(query, Rpt102CashPaymentVoucherRequest.class));
    }

    /**
     * Template: 132-PaymentByInvoiceSlipNew_Report.rpt
     * Procedure: SpVouchers_Payment&ReceipteByInvoiceVoucherSlipNew_Rpt
     * Desktop: VoucherReports.PaymentByInvoiceNewSlip
     */
    @RequestMapping(value = "/reports/print/132-payment-by-invoice-slip-new-report", method = RequestMethod.POST)
    public void print132PaymentByInvoiceSlipNewReport(HttpServletResponse response, @RequestBody(required = false) Rpt132PaymentByInvoiceSlipNewReportRequest request) throws Exception {
        if (request == null) request = new Rpt132PaymentByInvoiceSlipNewReportRequest();
        printReport(response, "132-PaymentByInvoiceSlipNew_Report.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/132-payment-by-invoice-slip-new-report", method = RequestMethod.GET)
    public void print132PaymentByInvoiceSlipNewReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print132PaymentByInvoiceSlipNewReport(response, objectMapper.convertValue(query, Rpt132PaymentByInvoiceSlipNewReportRequest.class));
    }

    /**
     * Template: 250_PdcPaymentTransactionsSlip.rpt
     * Procedure: [Bank].[USP_pdcTransaction_SlipAndRegister]
     * Desktop: pdcTransaction.pdcTransaction_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/250-pdc-payment-transactions-slip", method = RequestMethod.POST)
    public void print250PdcPaymentTransactionsSlip(HttpServletResponse response, @RequestBody(required = false) Rpt250PdcPaymentTransactionsSlipRequest request) throws Exception {
        if (request == null) request = new Rpt250PdcPaymentTransactionsSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "250_PdcPaymentTransactionsSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/250-pdc-payment-transactions-slip", method = RequestMethod.GET)
    public void print250PdcPaymentTransactionsSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print250PdcPaymentTransactionsSlip(response, objectMapper.convertValue(query, Rpt250PdcPaymentTransactionsSlipRequest.class));
    }

    /**
     * Template: 100-PartyWisePaymentReceiptsVoucherSlip.rpt
     * Procedure: Usp_VouchersPaymentReceiptSlip_Report
     * Desktop: VoucherReports.VoucherSlipPartyWise
     */
    @RequestMapping(value = "/reports/print/100-party-wise-payment-receipts-voucher-slip", method = RequestMethod.POST)
    public void print100PartyWisePaymentReceiptsVoucherSlip(HttpServletResponse response, @RequestBody(required = false) Rpt100PartyWisePaymentReceiptsVoucherSlipRequest request) throws Exception {
        if (request == null) request = new Rpt100PartyWisePaymentReceiptsVoucherSlipRequest();
        printReport(response, "100-PartyWisePaymentReceiptsVoucherSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/100-party-wise-payment-receipts-voucher-slip", method = RequestMethod.GET)
    public void print100PartyWisePaymentReceiptsVoucherSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print100PartyWisePaymentReceiptsVoucherSlip(response, objectMapper.convertValue(query, Rpt100PartyWisePaymentReceiptsVoucherSlipRequest.class));
    }

    /**
     * Template: 102-CashReceiptVoucher.rpt
     * Procedure: SpVouchers_PaymentReceiptVoucherSlipNew_Rpt
     * Desktop: VoucherReports.NewVoucherReprot
     */
    @RequestMapping(value = "/reports/print/102-cash-receipt-voucher", method = RequestMethod.POST)
    public void print102CashReceiptVoucher(HttpServletResponse response, @RequestBody(required = false) Rpt102CashReceiptVoucherRequest request) throws Exception {
        if (request == null) request = new Rpt102CashReceiptVoucherRequest();
        printReport(response, "102-CashReceiptVoucher.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/102-cash-receipt-voucher", method = RequestMethod.GET)
    public void print102CashReceiptVoucherGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print102CashReceiptVoucher(response, objectMapper.convertValue(query, Rpt102CashReceiptVoucherRequest.class));
    }

    /**
     * Template: 105_002-AcRptGeneralLedger.rpt
     * Procedure: usp_AccountsLedgerByJobOrder
     * Desktop: VoucherReports.AccountsLedgerByJobOrder
     */
    @RequestMapping(value = "/reports/print/105-00-general-ledger", method = RequestMethod.POST)
    public void print10500GeneralLedger(HttpServletResponse response, @RequestBody(required = false) Rpt10500GeneralLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt10500GeneralLedgerRequest();
        printReport(response, "105_002-AcRptGeneralLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/105-00-general-ledger", method = RequestMethod.GET)
    public void print10500GeneralLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print10500GeneralLedger(response, objectMapper.convertValue(query, Rpt10500GeneralLedgerRequest.class));
    }

    /**
     * Template: 106A_GeneralLedgerSummary2.rpt
     * Procedure: [dbo].[USP_InvFoodProduction_GetJobOrderIdByProduction]
     * Desktop: InvFoodProduction.InvFoodProduction_GetJobOrderIdByProduction
     */
    @RequestMapping(value = "/reports/print/106a-general-ledger-summary-2", method = RequestMethod.POST)
    public void print106AGeneralLedgerSummary2(HttpServletResponse response, @RequestBody(required = false) Rpt106AGeneralLedgerSummary2Request request) throws Exception {
        if (request == null) request = new Rpt106AGeneralLedgerSummary2Request();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "106A_GeneralLedgerSummary2.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/106a-general-ledger-summary-2", method = RequestMethod.GET)
    public void print106AGeneralLedgerSummary2Get(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print106AGeneralLedgerSummary2(response, objectMapper.convertValue(query, Rpt106AGeneralLedgerSummary2Request.class));
    }

    /**
     * Template: 107A_GeneralLedger.rpt
     * Procedure: [dbo].[USP_InvFoodProduction_GetJobOrderIdByProduction]
     * Desktop: InvFoodProduction.InvFoodProduction_GetJobOrderIdByProduction
     */
    @RequestMapping(value = "/reports/print/107a-general-ledger", method = RequestMethod.POST)
    public void print107AGeneralLedger(HttpServletResponse response, @RequestBody(required = false) Rpt107AGeneralLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt107AGeneralLedgerRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "107A_GeneralLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/107a-general-ledger", method = RequestMethod.GET)
    public void print107AGeneralLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print107AGeneralLedger(response, objectMapper.convertValue(query, Rpt107AGeneralLedgerRequest.class));
    }

    /**
     * Template: 108A_GeneralLedgerSummery.rpt
     * Procedure: [dbo].[USP_InvFoodProduction_GetJobOrderIdByProduction]
     * Desktop: InvFoodProduction.InvFoodProduction_GetJobOrderIdByProduction
     */
    @RequestMapping(value = "/reports/print/108a-general-ledger-summery", method = RequestMethod.POST)
    public void print108AGeneralLedgerSummery(HttpServletResponse response, @RequestBody(required = false) Rpt108AGeneralLedgerSummeryRequest request) throws Exception {
        if (request == null) request = new Rpt108AGeneralLedgerSummeryRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "108A_GeneralLedgerSummery.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/108a-general-ledger-summery", method = RequestMethod.GET)
    public void print108AGeneralLedgerSummeryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print108AGeneralLedgerSummery(response, objectMapper.convertValue(query, Rpt108AGeneralLedgerSummeryRequest.class));
    }

    /**
     * Template: 11-AccountsBudgetSlip.rpt
     * Procedure: [dbo].[USP_AccountsBudgetSlipAndRegister]
     * Desktop: AccountsBudgetHeader.AccountsBudgetSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/11-accounts-budget-slip", method = RequestMethod.POST)
    public void print11AccountsBudgetSlip(HttpServletResponse response, @RequestBody(required = false) Rpt11AccountsBudgetSlipRequest request) throws Exception {
        if (request == null) request = new Rpt11AccountsBudgetSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "11-AccountsBudgetSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/11-accounts-budget-slip", method = RequestMethod.GET)
    public void print11AccountsBudgetSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print11AccountsBudgetSlip(response, objectMapper.convertValue(query, Rpt11AccountsBudgetSlipRequest.class));
    }

    /**
     * Template: 111-AcRptReceivablesAging.rpt
     * Procedure: Sp_Accounts_ReceivablesAging_Rpt
     * Desktop: VoucherReports.ReceivableAging
     */
    @RequestMapping(value = "/reports/print/111-receivables-aging", method = RequestMethod.POST)
    public void print111ReceivablesAging(HttpServletResponse response, @RequestBody(required = false) Rpt111ReceivablesAgingRequest request) throws Exception {
        if (request == null) request = new Rpt111ReceivablesAgingRequest();
        printReport(response, "111-AcRptReceivablesAging.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/111-receivables-aging", method = RequestMethod.GET)
    public void print111ReceivablesAgingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print111ReceivablesAging(response, objectMapper.convertValue(query, Rpt111ReceivablesAgingRequest.class));
    }

    /**
     * Template: 114_01-AcRptChartOfAccounts.rpt
     * Procedure: USP_Organization_ChartOfAccountTemplate_GetAll
     * Desktop: Organization_ChartOfAccountTemplate.FormHistory
     */
    @RequestMapping(value = "/reports/print/114-01-chart-of-accounts", method = RequestMethod.POST)
    public void print11401ChartOfAccounts(HttpServletResponse response, @RequestBody(required = false) Rpt11401ChartOfAccountsRequest request) throws Exception {
        if (request == null) request = new Rpt11401ChartOfAccountsRequest();
        printReport(response, "114_01-AcRptChartOfAccounts.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/114-01-chart-of-accounts", method = RequestMethod.GET)
    public void print11401ChartOfAccountsGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print11401ChartOfAccounts(response, objectMapper.convertValue(query, Rpt11401ChartOfAccountsRequest.class));
    }

    /**
     * Template: 116-AcRptAccountsActivityDetail.rpt
     * Procedure: Sp_Accounts_ActivityDetail_Rpt
     * Desktop: VoucherReports.ActivitySummaryDetail
     */
    @RequestMapping(value = "/reports/print/116-accounts-activity-detail", method = RequestMethod.POST)
    public void print116AccountsActivityDetail(HttpServletResponse response, @RequestBody(required = false) Rpt116AccountsActivityDetailRequest request) throws Exception {
        if (request == null) request = new Rpt116AccountsActivityDetailRequest();
        printReport(response, "116-AcRptAccountsActivityDetail.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/116-accounts-activity-detail", method = RequestMethod.GET)
    public void print116AccountsActivityDetailGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print116AccountsActivityDetail(response, objectMapper.convertValue(query, Rpt116AccountsActivityDetailRequest.class));
    }

    /**
     * Template: 117-AcRptTrialBalanceSelectedCurrent.rpt
     * Procedure: Sp_PaymentByInvoiceSummery_Register
     * Desktop: VoucherReports.PaymentByInvoiceSummery_Detail
     */
    @RequestMapping(value = "/reports/print/117-trial-balance-selected-current", method = RequestMethod.POST)
    public void print117TrialBalanceSelectedCurrent(HttpServletResponse response, @RequestBody(required = false) Rpt117TrialBalanceSelectedCurrentRequest request) throws Exception {
        if (request == null) request = new Rpt117TrialBalanceSelectedCurrentRequest();
        printReport(response, "117-AcRptTrialBalanceSelectedCurrent.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/117-trial-balance-selected-current", method = RequestMethod.GET)
    public void print117TrialBalanceSelectedCurrentGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print117TrialBalanceSelectedCurrent(response, objectMapper.convertValue(query, Rpt117TrialBalanceSelectedCurrentRequest.class));
    }

    /**
     * Template: 120-AcRptPayablesAging.rpt
     * Procedure: SpAccounts_LedgerAging
     * Desktop: VoucherReports.LedgerAging
     */
    @RequestMapping(value = "/reports/print/120-payables-aging", method = RequestMethod.POST)
    public void print120PayablesAging(HttpServletResponse response, @RequestBody(required = false) Rpt120PayablesAgingRequest request) throws Exception {
        if (request == null) request = new Rpt120PayablesAgingRequest();
        printReport(response, "120-AcRptPayablesAging.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/120-payables-aging", method = RequestMethod.GET)
    public void print120PayablesAgingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print120PayablesAging(response, objectMapper.convertValue(query, Rpt120PayablesAgingRequest.class));
    }

    /**
     * Template: 120A-CapitalOwnerEquityReport.rpt
     * Procedure: SpAccounts_TrialBalanceSelectedNew_Report
     * Desktop: VoucherReports.SelectedTrialBalanceNew
     */
    @RequestMapping(value = "/reports/print/120a-capital-owner-equity-report", method = RequestMethod.POST)
    public void print120ACapitalOwnerEquityReport(HttpServletResponse response, @RequestBody(required = false) Rpt120ACapitalOwnerEquityReportRequest request) throws Exception {
        if (request == null) request = new Rpt120ACapitalOwnerEquityReportRequest();
        printReport(response, "120A-CapitalOwnerEquityReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/120a-capital-owner-equity-report", method = RequestMethod.GET)
    public void print120ACapitalOwnerEquityReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print120ACapitalOwnerEquityReport(response, objectMapper.convertValue(query, Rpt120ACapitalOwnerEquityReportRequest.class));
    }

    /**
     * Template: 121-Accounts_Payables_Rpt.rpt
     * Procedure: Sp_Accounts_Payables_Rpt
     * Desktop: VoucherReports.PayablesReport
     */
    @RequestMapping(value = "/reports/print/121-accounts-payables", method = RequestMethod.POST)
    public void print121AccountsPayables(HttpServletResponse response, @RequestBody(required = false) Rpt121AccountsPayablesRequest request) throws Exception {
        if (request == null) request = new Rpt121AccountsPayablesRequest();
        printReport(response, "121-Accounts_Payables_Rpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/121-accounts-payables", method = RequestMethod.GET)
    public void print121AccountsPayablesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print121AccountsPayables(response, objectMapper.convertValue(query, Rpt121AccountsPayablesRequest.class));
    }

    /**
     * Template: 121_01_PayablesAndPaymentSchedule.rpt
     * Procedure: usp_PayablesAndPaymentSchedule
     * Desktop: VoucherReports.PayablesAndPaymentSchedule
     */
    @RequestMapping(value = "/reports/print/121-01-payables-and-payment-schedule", method = RequestMethod.POST)
    public void print12101PayablesAndPaymentSchedule(HttpServletResponse response, @RequestBody(required = false) Rpt12101PayablesAndPaymentScheduleRequest request) throws Exception {
        if (request == null) request = new Rpt12101PayablesAndPaymentScheduleRequest();
        printReport(response, "121_01_PayablesAndPaymentSchedule.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/121-01-payables-and-payment-schedule", method = RequestMethod.GET)
    public void print12101PayablesAndPaymentScheduleGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print12101PayablesAndPaymentSchedule(response, objectMapper.convertValue(query, Rpt12101PayablesAndPaymentScheduleRequest.class));
    }

    /**
     * Template: 131-AccountsTrialBalance.rpt
     * Procedure: Sp_Accounts_TrialBalances_Rpt
     * Desktop: VoucherReports.TrialBalanceNewReport
     */
    @RequestMapping(value = "/reports/print/131-accounts-trial-balance", method = RequestMethod.POST)
    public void print131AccountsTrialBalance(HttpServletResponse response, @RequestBody(required = false) Rpt131AccountsTrialBalanceRequest request) throws Exception {
        if (request == null) request = new Rpt131AccountsTrialBalanceRequest();
        printReport(response, "131-AccountsTrialBalance.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/131-accounts-trial-balance", method = RequestMethod.GET)
    public void print131AccountsTrialBalanceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print131AccountsTrialBalance(response, objectMapper.convertValue(query, Rpt131AccountsTrialBalanceRequest.class));
    }

    /**
     * Template: 133-PaymentbyInvoice_Summary&Detail.rpt
     * Procedure: Sp_COAAllocation_GetAllMethod
     * Desktop: COAAllocation.GetSupplierGlAccountExistInPurchaseInvoice
     */
    @RequestMapping(value = "/reports/print/133-paymentby-invoice-summary-detail", method = RequestMethod.POST)
    public void print133PaymentbyInvoiceSummaryDetail(HttpServletResponse response, @RequestBody(required = false) Rpt133PaymentbyInvoiceSummaryDetailRequest request) throws Exception {
        if (request == null) request = new Rpt133PaymentbyInvoiceSummaryDetailRequest();
        printReport(response, "133-PaymentbyInvoice_Summary&Detail.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/133-paymentby-invoice-summary-detail", method = RequestMethod.GET)
    public void print133PaymentbyInvoiceSummaryDetailGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print133PaymentbyInvoiceSummaryDetail(response, objectMapper.convertValue(query, Rpt133PaymentbyInvoiceSummaryDetailRequest.class));
    }

    /**
     * Template: 134_PayablesAndReceivablesWithPaymentAndReceipts.rpt
     * Procedure: usp_getPayablesAndReceivablesWithPaymentAndReceipts
     * Desktop: GeneralReprots.PayablesAndReceivablesWithPaymentAndReceipts
     */
    @RequestMapping(value = "/reports/print/134-payables-and-receivables-with-payment-and-receipts", method = RequestMethod.POST)
    public void print134PayablesAndReceivablesWithPaymentAndReceipts(HttpServletResponse response, @RequestBody(required = false) Rpt134PayablesAndReceivablesWithPaymentAndReceiptsRequest request) throws Exception {
        if (request == null) request = new Rpt134PayablesAndReceivablesWithPaymentAndReceiptsRequest();
        printReport(response, "134_PayablesAndReceivablesWithPaymentAndReceipts.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/134-payables-and-receivables-with-payment-and-receipts", method = RequestMethod.GET)
    public void print134PayablesAndReceivablesWithPaymentAndReceiptsGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print134PayablesAndReceivablesWithPaymentAndReceipts(response, objectMapper.convertValue(query, Rpt134PayablesAndReceivablesWithPaymentAndReceiptsRequest.class));
    }

    /**
     * Template: 142-DueByDatePayablesAndReceivables.rpt
     * Procedure: USp_AccountsPayablesByDuedateBetweenPeriod
     * Desktop: VoucherReports.USp_AccountsPayablesByDuedateBetweenPeriod
     */
    @RequestMapping(value = "/reports/print/142-due-by-date-payables-and-receivables", method = RequestMethod.POST)
    public void print142DueByDatePayablesAndReceivables(HttpServletResponse response, @RequestBody(required = false) Rpt142DueByDatePayablesAndReceivablesRequest request) throws Exception {
        if (request == null) request = new Rpt142DueByDatePayablesAndReceivablesRequest();
        printReport(response, "142-DueByDatePayablesAndReceivables.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/142-due-by-date-payables-and-receivables", method = RequestMethod.GET)
    public void print142DueByDatePayablesAndReceivablesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print142DueByDatePayablesAndReceivables(response, objectMapper.convertValue(query, Rpt142DueByDatePayablesAndReceivablesRequest.class));
    }

    /**
     * Template: 143_02_ProfitLossBreakUp.rpt
     * Procedure: usp_Accounts_COGS_BreakUp
     * Desktop: VoucherReports.Accounts_COGS_BreakUp
     */
    @RequestMapping(value = "/reports/print/143-02-profit-loss-break-up", method = RequestMethod.POST)
    public void print14302ProfitLossBreakUp(HttpServletResponse response, @RequestBody(required = false) Rpt14302ProfitLossBreakUpRequest request) throws Exception {
        if (request == null) request = new Rpt14302ProfitLossBreakUpRequest();
        printReport(response, "143_02_ProfitLossBreakUp.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/143-02-profit-loss-break-up", method = RequestMethod.GET)
    public void print14302ProfitLossBreakUpGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print14302ProfitLossBreakUp(response, objectMapper.convertValue(query, Rpt14302ProfitLossBreakUpRequest.class));
    }

    /**
     * Template: 151-AcRptAccountsBalanceSheetStandard.rpt
     * Procedure: Sp_Accounts_BalanceSheetStandard_Rpt
     * Desktop: VoucherReports.AccountsBalanceSheetStandardRpt
     */
    @RequestMapping(value = "/reports/print/151-accounts-balance-sheet-standard", method = RequestMethod.POST)
    public void print151AccountsBalanceSheetStandard(HttpServletResponse response, @RequestBody(required = false) Rpt151AccountsBalanceSheetStandardRequest request) throws Exception {
        if (request == null) request = new Rpt151AccountsBalanceSheetStandardRequest();
        printReport(response, "151-AcRptAccountsBalanceSheetStandard.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/151-accounts-balance-sheet-standard", method = RequestMethod.GET)
    public void print151AccountsBalanceSheetStandardGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print151AccountsBalanceSheetStandard(response, objectMapper.convertValue(query, Rpt151AccountsBalanceSheetStandardRequest.class));
    }

    /**
     * Template: 152-AcRptAccountsProfitLoss.rpt
     * Procedure: Sp_Accounts_ProfitLoassStandard_Rpt
     * Desktop: VoucherReports.AccountsProfitLoassStandard
     */
    @RequestMapping(value = "/reports/print/152-accounts-profit-loss", method = RequestMethod.POST)
    public void print152AccountsProfitLoss(HttpServletResponse response, @RequestBody(required = false) Rpt152AccountsProfitLossRequest request) throws Exception {
        if (request == null) request = new Rpt152AccountsProfitLossRequest();
        printReport(response, "152-AcRptAccountsProfitLoss.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/152-accounts-profit-loss", method = RequestMethod.GET)
    public void print152AccountsProfitLossGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152AccountsProfitLoss(response, objectMapper.convertValue(query, Rpt152AccountsProfitLossRequest.class));
    }

    /**
     * Template: 154-Payables&Receiveableaging-Rpt.rpt
     * Procedure: SpAccounts_PayablesReceivablesAging_Rpt
     * Desktop: GeneralReprots.PayablesandReceivablesMenagment
     */
    @RequestMapping(value = "/reports/print/154-payables-receiveableaging", method = RequestMethod.POST)
    public void print154PayablesReceiveableaging(HttpServletResponse response, @RequestBody(required = false) Rpt154PayablesReceiveableagingRequest request) throws Exception {
        if (request == null) request = new Rpt154PayablesReceiveableagingRequest();
        printReport(response, "154-Payables&Receiveableaging-Rpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/154-payables-receiveableaging", method = RequestMethod.GET)
    public void print154PayablesReceiveableagingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print154PayablesReceiveableaging(response, objectMapper.convertValue(query, Rpt154PayablesReceiveableagingRequest.class));
    }

    /**
     * Template: 154-Profit&Loss.rpt
     * Procedure: SpAccounts_ProfitLoassFormatA_Report
     * Desktop: VoucherReports.ProfitandLoss
     */
    @RequestMapping(value = "/reports/print/154-profit-loss", method = RequestMethod.POST)
    public void print154ProfitLoss(HttpServletResponse response, @RequestBody(required = false) Rpt154ProfitLossRequest request) throws Exception {
        if (request == null) request = new Rpt154ProfitLossRequest();
        printReport(response, "154-Profit&Loss.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/154-profit-loss", method = RequestMethod.GET)
    public void print154ProfitLossGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print154ProfitLoss(response, objectMapper.convertValue(query, Rpt154ProfitLossRequest.class));
    }

    /**
     * Template: 159-PayablesAndReceivablebetweenPeriod.rpt
     * Procedure: USp_AccountsPayablesByDuedateBetweenPeriod
     * Desktop: VoucherReports.USp_AccountsPayablesByDuedateBetweenPeriod
     */
    @RequestMapping(value = "/reports/print/159-payables-and-receivablebetween-period", method = RequestMethod.POST)
    public void print159PayablesAndReceivablebetweenPeriod(HttpServletResponse response, @RequestBody(required = false) Rpt159PayablesAndReceivablebetweenPeriodRequest request) throws Exception {
        if (request == null) request = new Rpt159PayablesAndReceivablebetweenPeriodRequest();
        printReport(response, "159-PayablesAndReceivablebetweenPeriod.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/159-payables-and-receivablebetween-period", method = RequestMethod.GET)
    public void print159PayablesAndReceivablebetweenPeriodGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print159PayablesAndReceivablebetweenPeriod(response, objectMapper.convertValue(query, Rpt159PayablesAndReceivablebetweenPeriodRequest.class));
    }

    /**
     * Template: 164-AcRptQuantativeLedger.rpt
     * Procedure: Sp_VoucherHead_GLQuantitativeLedgerTradePro
     * Desktop: StocksReport.itemLedgerTradePro
     */
    @RequestMapping(value = "/reports/print/164-quantative-ledger", method = RequestMethod.POST)
    public void print164QuantativeLedger(HttpServletResponse response, @RequestBody(required = false) Rpt164QuantativeLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt164QuantativeLedgerRequest();
        printReport(response, "164-AcRptQuantativeLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/164-quantative-ledger", method = RequestMethod.GET)
    public void print164QuantativeLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print164QuantativeLedger(response, objectMapper.convertValue(query, Rpt164QuantativeLedgerRequest.class));
    }

    /**
     * Template: 166-ProfitLossNewReport.rpt
     * Procedure: SpAccounts_ProfitLoss_ForCrystal
     * Desktop: VoucherReports.Accounts_ProfitLoss_ForCrystal
     */
    @RequestMapping(value = "/reports/print/166-profit-loss-new-report", method = RequestMethod.POST)
    public void print166ProfitLossNewReport(HttpServletResponse response, @RequestBody(required = false) Rpt166ProfitLossNewReportRequest request) throws Exception {
        if (request == null) request = new Rpt166ProfitLossNewReportRequest();
        printReport(response, "166-ProfitLossNewReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/166-profit-loss-new-report", method = RequestMethod.GET)
    public void print166ProfitLossNewReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print166ProfitLossNewReport(response, objectMapper.convertValue(query, Rpt166ProfitLossNewReportRequest.class));
    }

    /**
     * Template: 168-CommisionTradeBillAndSlip.rpt
     * Procedure: Sp_Vouchers_GetMethods
     * Desktop: VoucherHead.Save
     */
    @RequestMapping(value = "/reports/print/168-commision-trade-bill-and-slip", method = RequestMethod.POST)
    public void print168CommisionTradeBillAndSlip(HttpServletResponse response, @RequestBody(required = false) Rpt168CommisionTradeBillAndSlipRequest request) throws Exception {
        if (request == null) request = new Rpt168CommisionTradeBillAndSlipRequest();
        printReport(response, "168-CommisionTradeBillAndSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/168-commision-trade-bill-and-slip", method = RequestMethod.GET)
    public void print168CommisionTradeBillAndSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print168CommisionTradeBillAndSlip(response, objectMapper.convertValue(query, Rpt168CommisionTradeBillAndSlipRequest.class));
    }

    /**
     * Template: 1865-SalesWages_Register.rpt
     * Procedure: UPS_ReceivablesAging_New
     * Desktop: VoucherReports.ReceivablesAging_New
     */
    @RequestMapping(value = "/reports/print/1865-sales-wages-register", method = RequestMethod.POST)
    public void print1865SalesWagesRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1865SalesWagesRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1865SalesWagesRegisterRequest();
        printReport(response, "1865-SalesWages_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1865-sales-wages-register", method = RequestMethod.GET)
    public void print1865SalesWagesRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1865SalesWagesRegister(response, objectMapper.convertValue(query, Rpt1865SalesWagesRegisterRequest.class));
    }

    /**
     * Template: 240-FreightVoucherRegister.rpt
     * Procedure: USP_FreightVoucher_PendingForApproval
     * Desktop: InvFreightVoucher.FreightVoucher_PendingForApproval
     */
    @RequestMapping(value = "/reports/print/240-freight-voucher-register", method = RequestMethod.POST)
    public void print240FreightVoucherRegister(HttpServletResponse response, @RequestBody(required = false) Rpt240FreightVoucherRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt240FreightVoucherRegisterRequest();
        printReport(response, "240-FreightVoucherRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/240-freight-voucher-register", method = RequestMethod.GET)
    public void print240FreightVoucherRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print240FreightVoucherRegister(response, objectMapper.convertValue(query, Rpt240FreightVoucherRegisterRequest.class));
    }

    /**
     * Template: 242-InLandFreightAgreementSlip.rpt
     * Procedure: SP_InLandFreightAgreementSlipAndRegister
     * Desktop: VoucherReports.InlandFreightAggreement
     */
    @RequestMapping(value = "/reports/print/242-in-land-freight-agreement-slip", method = RequestMethod.POST)
    public void print242InLandFreightAgreementSlip(HttpServletResponse response, @RequestBody(required = false) Rpt242InLandFreightAgreementSlipRequest request) throws Exception {
        if (request == null) request = new Rpt242InLandFreightAgreementSlipRequest();
        printReport(response, "242-InLandFreightAgreementSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/242-in-land-freight-agreement-slip", method = RequestMethod.GET)
    public void print242InLandFreightAgreementSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print242InLandFreightAgreementSlip(response, objectMapper.convertValue(query, Rpt242InLandFreightAgreementSlipRequest.class));
    }

    /**
     * Template: 243-InLandFreightAgreementRegister.rpt
     * Procedure: SP_InLandFreightAgreementSlipAndRegister
     * Desktop: VoucherReports.InlandFreightAggreement
     */
    @RequestMapping(value = "/reports/print/243-in-land-freight-agreement-register", method = RequestMethod.POST)
    public void print243InLandFreightAgreementRegister(HttpServletResponse response, @RequestBody(required = false) Rpt243InLandFreightAgreementRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt243InLandFreightAgreementRegisterRequest();
        printReport(response, "243-InLandFreightAgreementRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/243-in-land-freight-agreement-register", method = RequestMethod.GET)
    public void print243InLandFreightAgreementRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print243InLandFreightAgreementRegister(response, objectMapper.convertValue(query, Rpt243InLandFreightAgreementRegisterRequest.class));
    }

    /**
     * Template: 291-SaleInvoice_AutoRated.rpt
     * Procedure: Sp_Vouchers_GetMethods
     * Desktop: VoucherHead.Save
     */
    @RequestMapping(value = "/reports/print/291-sale-invoice-auto-rated", method = RequestMethod.POST)
    public void print291SaleInvoiceAutoRated(HttpServletResponse response, @RequestBody(required = false) Rpt291SaleInvoiceAutoRatedRequest request) throws Exception {
        if (request == null) request = new Rpt291SaleInvoiceAutoRatedRequest();
        printReport(response, "291-SaleInvoice_AutoRated.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/291-sale-invoice-auto-rated", method = RequestMethod.GET)
    public void print291SaleInvoiceAutoRatedGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print291SaleInvoiceAutoRated(response, objectMapper.convertValue(query, Rpt291SaleInvoiceAutoRatedRequest.class));
    }

    /**
     * Template: 380-DueByDateReceivables.rpt
     * Procedure: [dbo].[USP_DueByDateReceivables]
     * Desktop: VoucherReports.DueByDateReceivables
     */
    @RequestMapping(value = "/reports/print/380-due-by-date-receivables", method = RequestMethod.POST)
    public void print380DueByDateReceivables(HttpServletResponse response, @RequestBody(required = false) Rpt380DueByDateReceivablesRequest request) throws Exception {
        if (request == null) request = new Rpt380DueByDateReceivablesRequest();
        printReport(response, "380-DueByDateReceivables.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/380-due-by-date-receivables", method = RequestMethod.GET)
    public void print380DueByDateReceivablesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print380DueByDateReceivables(response, objectMapper.convertValue(query, Rpt380DueByDateReceivablesRequest.class));
    }

    /**
     * Template: 381-SaleInvoicewiseProfitablity.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/381-sale-invoicewise-profitablity", method = RequestMethod.POST)
    public void print381SaleInvoicewiseProfitablity(HttpServletResponse response, @RequestBody(required = false) Rpt381SaleInvoicewiseProfitablityRequest request) throws Exception {
        if (request == null) request = new Rpt381SaleInvoicewiseProfitablityRequest();
        printReport(response, "381-SaleInvoicewiseProfitablity.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/381-sale-invoicewise-profitablity", method = RequestMethod.GET)
    public void print381SaleInvoicewiseProfitablityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print381SaleInvoicewiseProfitablity(response, objectMapper.convertValue(query, Rpt381SaleInvoicewiseProfitablityRequest.class));
    }

    /**
     * Template: 405-RptEBGLBySupplier.rpt
     * Procedure: Sp_InventoryEBBalancesByPartyAndItem_Rpt
     * Desktop: StocksReport.InventoryEBTrialBalancesByPartyAndItem
     */
    @RequestMapping(value = "/reports/print/405-ebgl-by-supplier", method = RequestMethod.POST)
    public void print405EBGLBySupplier(HttpServletResponse response, @RequestBody(required = false) Rpt405EBGLBySupplierRequest request) throws Exception {
        if (request == null) request = new Rpt405EBGLBySupplierRequest();
        printReport(response, "405-RptEBGLBySupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/405-ebgl-by-supplier", method = RequestMethod.GET)
    public void print405EBGLBySupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print405EBGLBySupplier(response, objectMapper.convertValue(query, Rpt405EBGLBySupplierRequest.class));
    }

    /**
     * Template: 420_ProfitLossReport.rpt
     * Procedure: usp_ProfitAndLoss_New
     * Desktop: VoucherReports.ProfitAndLoss_New
     */
    @RequestMapping(value = "/reports/print/420-profit-loss-report", method = RequestMethod.POST)
    public void print420ProfitLossReport(HttpServletResponse response, @RequestBody(required = false) Rpt420ProfitLossReportRequest request) throws Exception {
        if (request == null) request = new Rpt420ProfitLossReportRequest();
        printReport(response, "420_ProfitLossReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/420-profit-loss-report", method = RequestMethod.GET)
    public void print420ProfitLossReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print420ProfitLossReport(response, objectMapper.convertValue(query, Rpt420ProfitLossReportRequest.class));
    }

    /**
     * Template: 901_01_VoucherInvoicesAdjustment_Slip.rpt
     * Procedure: [dbo].[USP_VoucherInvoicesAdjustment_ReadByVoucherHeadId]
     * Desktop: VoucherInvoicesAdjustment.GetByVoucherHeadId
     */
    @RequestMapping(value = "/reports/print/901-01-voucher-invoices-adjustment-slip", method = RequestMethod.POST)
    public void print90101VoucherInvoicesAdjustmentSlip(HttpServletResponse response, @RequestBody(required = false) Rpt90101VoucherInvoicesAdjustmentSlipRequest request) throws Exception {
        if (request == null) request = new Rpt90101VoucherInvoicesAdjustmentSlipRequest();
        printReport(response, "901_01_VoucherInvoicesAdjustment_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/901-01-voucher-invoices-adjustment-slip", method = RequestMethod.GET)
    public void print90101VoucherInvoicesAdjustmentSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print90101VoucherInvoicesAdjustmentSlip(response, objectMapper.convertValue(query, Rpt90101VoucherInvoicesAdjustmentSlipRequest.class));
    }

    /**
     * Template: 910A_StockEvaluationCgsReport.rpt
     * Procedure: [dbo].[USP_SaleReportWithGrossProfitsAndLossNew]
     * Desktop: InventoryStockEvalautionDetail.SaleReportWithGrossProfitsAndLossNew
     */
    @RequestMapping(value = "/reports/print/910a-stock-evaluation-cgs-report", method = RequestMethod.POST)
    public void print910AStockEvaluationCgsReport(HttpServletResponse response, @RequestBody(required = false) Rpt910AStockEvaluationCgsReportRequest request) throws Exception {
        if (request == null) request = new Rpt910AStockEvaluationCgsReportRequest();
        printReport(response, "910A_StockEvaluationCgsReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/910a-stock-evaluation-cgs-report", method = RequestMethod.GET)
    public void print910AStockEvaluationCgsReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print910AStockEvaluationCgsReport(response, objectMapper.convertValue(query, Rpt910AStockEvaluationCgsReportRequest.class));
    }

    /**
     * Template: ActivitySummeryCreditSubReportForB.rpt
     * Procedure: Sp_ActivityReportSummeryDocumentTypeWise_Rpt
     * Desktop: VoucherReports.ActivitySummaryDocumentTypeWise
     */
    @RequestMapping(value = "/reports/print/activity-summery-credit-sub-report-for-b", method = RequestMethod.POST)
    public void printActivitySummeryCreditSubReportForB(HttpServletResponse response, @RequestBody(required = false) RptActivitySummeryCreditSubReportForBRequest request) throws Exception {
        if (request == null) request = new RptActivitySummeryCreditSubReportForBRequest();
        printReport(response, "ActivitySummeryCreditSubReportForB.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/activity-summery-credit-sub-report-for-b", method = RequestMethod.GET)
    public void printActivitySummeryCreditSubReportForBGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printActivitySummeryCreditSubReportForB(response, objectMapper.convertValue(query, RptActivitySummeryCreditSubReportForBRequest.class));
    }

    /**
     * Template: ActivitySummeryDebitSubReportForB.rpt
     * Procedure: Sp_ActivityReportSummeryDocumentTypeWise_Rpt
     * Desktop: VoucherReports.ActivitySummaryDocumentTypeWise
     */
    @RequestMapping(value = "/reports/print/activity-summery-debit-sub-report-for-b", method = RequestMethod.POST)
    public void printActivitySummeryDebitSubReportForB(HttpServletResponse response, @RequestBody(required = false) RptActivitySummeryDebitSubReportForBRequest request) throws Exception {
        if (request == null) request = new RptActivitySummeryDebitSubReportForBRequest();
        printReport(response, "ActivitySummeryDebitSubReportForB.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/activity-summery-debit-sub-report-for-b", method = RequestMethod.GET)
    public void printActivitySummeryDebitSubReportForBGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printActivitySummeryDebitSubReportForB(response, objectMapper.convertValue(query, RptActivitySummeryDebitSubReportForBRequest.class));
    }

    /**
     * Template: DayBookCreditSubReport.rpt
     * Procedure: Sp_DayBookSlip
     * Desktop: GeneralReprots.DayBookSlip
     */
    @RequestMapping(value = "/reports/print/day-book-credit-sub-report", method = RequestMethod.POST)
    public void printDayBookCreditSubReport(HttpServletResponse response, @RequestBody(required = false) RptDayBookCreditSubReportRequest request) throws Exception {
        if (request == null) request = new RptDayBookCreditSubReportRequest();
        printReport(response, "DayBookCreditSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/day-book-credit-sub-report", method = RequestMethod.GET)
    public void printDayBookCreditSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printDayBookCreditSubReport(response, objectMapper.convertValue(query, RptDayBookCreditSubReportRequest.class));
    }

    /**
     * Template: DayBookDebitSubReport.rpt
     * Procedure: Sp_DayBookSlip
     * Desktop: GeneralReprots.DayBookSlip
     */
    @RequestMapping(value = "/reports/print/day-book-debit-sub-report", method = RequestMethod.POST)
    public void printDayBookDebitSubReport(HttpServletResponse response, @RequestBody(required = false) RptDayBookDebitSubReportRequest request) throws Exception {
        if (request == null) request = new RptDayBookDebitSubReportRequest();
        printReport(response, "DayBookDebitSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/day-book-debit-sub-report", method = RequestMethod.GET)
    public void printDayBookDebitSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printDayBookDebitSubReport(response, objectMapper.convertValue(query, RptDayBookDebitSubReportRequest.class));
    }

    /**
     * Template: RptCWBInward.rpt
     * Procedure: Sp_Vouchers_GetMethods
     * Desktop: VoucherHead.Save
     */
    @RequestMapping(value = "/reports/print/cwb-inward", method = RequestMethod.POST)
    public void printCWBInward(HttpServletResponse response, @RequestBody(required = false) RptCWBInwardRequest request) throws Exception {
        if (request == null) request = new RptCWBInwardRequest();
        printReport(response, "RptCWBInward.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/cwb-inward", method = RequestMethod.GET)
    public void printCWBInwardGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printCWBInward(response, objectMapper.convertValue(query, RptCWBInwardRequest.class));
    }

    /**
     * Template: rptGRN_Print.rpt
     * Procedure: Sp_Vouchers_GetMethods
     * Desktop: VoucherHead.Save
     */
    @RequestMapping(value = "/reports/print/grn-print", method = RequestMethod.POST)
    public void printGRNPrint(HttpServletResponse response, @RequestBody(required = false) RptGRNPrintRequest request) throws Exception {
        if (request == null) request = new RptGRNPrintRequest();
        printReport(response, "rptGRN_Print.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/grn-print", method = RequestMethod.GET)
    public void printGRNPrintGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printGRNPrint(response, objectMapper.convertValue(query, RptGRNPrintRequest.class));
    }

    /**
     * Template: RptInvBOMSlipA.rpt
     * Procedure: Sp_Vouchers_GetMethods
     * Desktop: VoucherHead.Save
     */
    @RequestMapping(value = "/reports/print/inv-bom-slip-a", method = RequestMethod.POST)
    public void printInvBOMSlipA(HttpServletResponse response, @RequestBody(required = false) RptInvBOMSlipARequest request) throws Exception {
        if (request == null) request = new RptInvBOMSlipARequest();
        printReport(response, "RptInvBOMSlipA.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/inv-bom-slip-a", method = RequestMethod.GET)
    public void printInvBOMSlipAGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printInvBOMSlipA(response, objectMapper.convertValue(query, RptInvBOMSlipARequest.class));
    }

    /**
     * Template: RptInvSupplySchedule.rpt
     * Procedure: Sp_Vouchers_GetMethods
     * Desktop: VoucherHead.Save
     */
    @RequestMapping(value = "/reports/print/inv-supply-schedule", method = RequestMethod.POST)
    public void printInvSupplySchedule(HttpServletResponse response, @RequestBody(required = false) RptInvSupplyScheduleRequest request) throws Exception {
        if (request == null) request = new RptInvSupplyScheduleRequest();
        printReport(response, "RptInvSupplySchedule.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/inv-supply-schedule", method = RequestMethod.GET)
    public void printInvSupplyScheduleGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printInvSupplySchedule(response, objectMapper.convertValue(query, RptInvSupplyScheduleRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
