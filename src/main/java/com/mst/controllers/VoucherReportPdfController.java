package com.mst.controllers;

import com.mst.security.CurrentUserContext;
import net.sf.jasperreports.engine.*;
import net.sf.jasperreports.engine.data.JRMapCollectionDataSource;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletResponse;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.*;

/**
 * Controller for PDF report generation using JasperReports for:
 * - 103-AcRptPurchaseSalesVoucherSlip.rpt
 * - 240-FreightVoucherRegister.rpt
 * - 241-FreightVoucherSlip.rpt
 * Reuses identical Desktop DAL stored procedures (Sp_Vouchers_GetMethods, Sp_InvFreightVoucher_GetAllMethod).
 */
@Controller
@RequestMapping("/accounts/reports")
public class VoucherReportPdfController {

    private static final Logger log = LoggerFactory.getLogger(VoucherReportPdfController.class);

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    /**
     * 103-AcRptPurchaseSalesVoucherSlip.rpt (Voucher Slip)
     */
    @GetMapping({"/voucher-slip/pdf", "/voucher-slip/pdf/{id}"})
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

        return renderPdf("/jasper/purchaseVoucher.jrxml", rows, parameters, "VoucherSlip-" + targetId + ".pdf");
    }

    /**
     * 240-FreightVoucherRegister.rpt (Freight Voucher Register)
     */
    @GetMapping("/freight-voucher-register/pdf")
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

        return renderPdf("/jasper/105-GeneralLedgerReport.jrxml", rows, parameters, "FreightVoucherRegister.pdf");
    }

    /**
     * 241-FreightVoucherSlip.rpt (Freight Voucher Slip)
     */
    @GetMapping({"/freight-voucher-slip/pdf", "/freight-voucher-slip/pdf/{id}"})
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

        return renderPdf("/jasper/purchaseVoucher.jrxml", rows, parameters, "FreightVoucherSlip-" + targetId + ".pdf");
    }

    private ResponseEntity<byte[]> renderPdf(String jrxmlPath, List<Map<String, Object>> rows, Map<String, Object> parameters, String filename) {
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
}
