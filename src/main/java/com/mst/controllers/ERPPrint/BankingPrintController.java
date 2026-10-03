package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.BankingPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import javax.servlet.http.HttpServletResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestBody;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.LinkedHashMap;
import com.mst.services.banking.ChequePrintingDesktopService;
import org.springframework.http.*;

/** Banking print actions. Existing print URLs are preserved. */
@Controller
public class BankingPrintController extends ReportPrintSupport {
    @Autowired private ChequePrintingDesktopService chequePrintingService;

    private static int chequeInt(Object value) {
        if (value == null) return 0;
        if (value instanceof Number) return ((Number) value).intValue();
        try { return Integer.parseInt(value.toString().trim()); }
        catch (NumberFormatException exception) { return 0; }
    }

    @PostMapping("/accounts/api/banking/cheque-printing/print")
    public ResponseEntity<?> printCheques(@RequestBody Map<String, Object> body) {
        try {
            int bankId = chequeInt(body == null ? null : body.get("bankId"));
            List<Integer> ids = new ArrayList<>();
            Object raw = body == null ? null : body.get("ids");
            if (raw instanceof List) for (Object o : (List<?>) raw) ids.add(chequeInt(o));
            ChequePrintingDesktopService.PrintResult r = chequePrintingService.print(bankId, ids);
            if (r.pdf == null) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("message", r.message);
                return ResponseEntity.ok().contentType(MediaType.APPLICATION_JSON).body(m);
            }
            return ResponseEntity.ok()
                    .contentType(MediaType.APPLICATION_PDF)
                    .header(HttpHeaders.CONTENT_DISPOSITION, "inline; filename=\"Cheques.pdf\"")
                    .body(r.pdf);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().contentType(MediaType.TEXT_PLAIN).body(e.getMessage());
        } catch (Exception e) {
            return ResponseEntity.status(500).contentType(MediaType.TEXT_PLAIN).body(e.getMessage());
        }
    }


    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 01-FcyExRateForwwardBookingSlip.rpt
     * Procedure: [dbo].[USP_FcyExRateForwardBooking_SlipAndRegister]
     * Desktop: fcyExRateForwardBooking.ExRateForwardBookingSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/01-fcy-ex-rate-forwward-booking-slip", method = RequestMethod.POST)
    public void print01FcyExRateForwwardBookingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt01FcyExRateForwwardBookingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt01FcyExRateForwwardBookingSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "01-FcyExRateForwwardBookingSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/01-fcy-ex-rate-forwward-booking-slip", method = RequestMethod.GET)
    public void print01FcyExRateForwwardBookingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print01FcyExRateForwwardBookingSlip(response, objectMapper.convertValue(query, Rpt01FcyExRateForwwardBookingSlipRequest.class));
    }

    /**
     * Template: 102-AcRptPaymentReceiptsVoucherSlip.rpt
     * Procedure: Sp_Vouchers_PaymentReceiptVoucherSlip_Rpt
     * Desktop: VoucherReports.VoucherReport
     */
    @RequestMapping(value = "/reports/print/102-payment-receipts-voucher-slip", method = RequestMethod.POST)
    public void print102PaymentReceiptsVoucherSlip(HttpServletResponse response, @RequestBody(required = false) Rpt102PaymentReceiptsVoucherSlipRequest request) throws Exception {
        if (request == null) request = new Rpt102PaymentReceiptsVoucherSlipRequest();
        printReport(response, "102-AcRptPaymentReceiptsVoucherSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/102-payment-receipts-voucher-slip", method = RequestMethod.GET)
    public void print102PaymentReceiptsVoucherSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print102PaymentReceiptsVoucherSlip(response, objectMapper.convertValue(query, Rpt102PaymentReceiptsVoucherSlipRequest.class));
    }

    /**
     * Template: 102-BankReceiptVoucher.rpt
     * Procedure: SpVouchers_PaymentReceiptVoucherSlipNew_Rpt
     * Desktop: VoucherReports.NewVoucherReprot
     */
    @RequestMapping(value = "/reports/print/102-bank-receipt-voucher", method = RequestMethod.POST)
    public void print102BankReceiptVoucher(HttpServletResponse response, @RequestBody(required = false) Rpt102BankReceiptVoucherRequest request) throws Exception {
        if (request == null) request = new Rpt102BankReceiptVoucherRequest();
        printReport(response, "102-BankReceiptVoucher.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/102-bank-receipt-voucher", method = RequestMethod.GET)
    public void print102BankReceiptVoucherGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print102BankReceiptVoucher(response, objectMapper.convertValue(query, Rpt102BankReceiptVoucherRequest.class));
    }

    /**
     * Template: 105_01_BankSummaryLedger.rpt
     * Procedure: USP_BankSummaryWithAgainstAccounts
     * Desktop: VoucherReports.BankSummaryLedgersWithAgainstsAccounts
     */
    @RequestMapping(value = "/reports/print/105-01-bank-summary-ledger", method = RequestMethod.POST)
    public void print10501BankSummaryLedger(HttpServletResponse response, @RequestBody(required = false) Rpt10501BankSummaryLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt10501BankSummaryLedgerRequest();
        printReport(response, "105_01_BankSummaryLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/105-01-bank-summary-ledger", method = RequestMethod.GET)
    public void print10501BankSummaryLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print10501BankSummaryLedger(response, objectMapper.convertValue(query, Rpt10501BankSummaryLedgerRequest.class));
    }

    /**
     * Template: 105_02_BankSummaryLedger.rpt
     * Procedure: USP_BankSummaryWithAgainstAccounts
     * Desktop: VoucherReports.BankSummaryLedgersWithAgainstsAccounts
     */
    @RequestMapping(value = "/reports/print/105-02-bank-summary-ledger", method = RequestMethod.POST)
    public void print10502BankSummaryLedger(HttpServletResponse response, @RequestBody(required = false) Rpt10502BankSummaryLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt10502BankSummaryLedgerRequest();
        printReport(response, "105_02_BankSummaryLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/105-02-bank-summary-ledger", method = RequestMethod.GET)
    public void print10502BankSummaryLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print10502BankSummaryLedger(response, objectMapper.convertValue(query, Rpt10502BankSummaryLedgerRequest.class));
    }

    /**
     * Template: 105C-AcRptFcyGeneralLedger.rpt
     * Procedure: Usp_Accounts_FcyGeneralLedger_Rpt
     * Desktop: VoucherReports.Accounts_FcyGeneralLedger_Rpt
     */
    @RequestMapping(value = "/reports/print/105c-fcy-general-ledger", method = RequestMethod.POST)
    public void print105CFcyGeneralLedger(HttpServletResponse response, @RequestBody(required = false) Rpt105CFcyGeneralLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt105CFcyGeneralLedgerRequest();
        printReport(response, "105C-AcRptFcyGeneralLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/105c-fcy-general-ledger", method = RequestMethod.GET)
    public void print105CFcyGeneralLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print105CFcyGeneralLedger(response, objectMapper.convertValue(query, Rpt105CFcyGeneralLedgerRequest.class));
    }

    /**
     * Template: 105D-AcRptFcyGeneralLedger.rpt
     * Procedure: Usp_Accounts_FcyGeneralLedger_Rpt
     * Desktop: VoucherReports.Accounts_FcyGeneralLedger_Rpt
     */
    @RequestMapping(value = "/reports/print/105d-fcy-general-ledger", method = RequestMethod.POST)
    public void print105DFcyGeneralLedger(HttpServletResponse response, @RequestBody(required = false) Rpt105DFcyGeneralLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt105DFcyGeneralLedgerRequest();
        printReport(response, "105D-AcRptFcyGeneralLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/105d-fcy-general-ledger", method = RequestMethod.GET)
    public void print105DFcyGeneralLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print105DFcyGeneralLedger(response, objectMapper.convertValue(query, Rpt105DFcyGeneralLedgerRequest.class));
    }

    /**
     * Template: 123-AcRptPdcInventorySlip.rpt
     * Procedure: Sp_PdcInventoryHeader_rpt
     * Desktop: GeneralReprots.PdcInventorySlip
     */
    @RequestMapping(value = "/reports/print/123-pdc-inventory-slip", method = RequestMethod.POST)
    public void print123PdcInventorySlip(HttpServletResponse response, @RequestBody(required = false) Rpt123PdcInventorySlipRequest request) throws Exception {
        if (request == null) request = new Rpt123PdcInventorySlipRequest();
        printReport(response, "123-AcRptPdcInventorySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/123-pdc-inventory-slip", method = RequestMethod.GET)
    public void print123PdcInventorySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print123PdcInventorySlip(response, objectMapper.convertValue(query, Rpt123PdcInventorySlipRequest.class));
    }

    /**
     * Template: 127-PdcInventoryPendingGuriWise.rpt
     * Procedure: Sp_PdcInventory_SlipAndRegister_Rpt
     * Desktop: GeneralReprots.PdcReceiptsRegister
     */
    @RequestMapping(value = "/reports/print/127-pdc-inventory-pending-guri-wise", method = RequestMethod.POST)
    public void print127PdcInventoryPendingGuriWise(HttpServletResponse response, @RequestBody(required = false) Rpt127PdcInventoryPendingGuriWiseRequest request) throws Exception {
        if (request == null) request = new Rpt127PdcInventoryPendingGuriWiseRequest();
        printReport(response, "127-PdcInventoryPendingGuriWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/127-pdc-inventory-pending-guri-wise", method = RequestMethod.GET)
    public void print127PdcInventoryPendingGuriWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print127PdcInventoryPendingGuriWise(response, objectMapper.convertValue(query, Rpt127PdcInventoryPendingGuriWiseRequest.class));
    }

    /**
     * Template: 153-FCYPayablesAndReceivablesRpt.rpt
     * Procedure: SPU_Accounts_FCYPayablesAndReceivables_Rpt
     * Desktop: VoucherReports.FCYPayablesAndReceivables_Rpt
     */
    @RequestMapping(value = "/reports/print/153-fcy-payables-and-receivables", method = RequestMethod.POST)
    public void print153FCYPayablesAndReceivables(HttpServletResponse response, @RequestBody(required = false) Rpt153FCYPayablesAndReceivablesRequest request) throws Exception {
        if (request == null) request = new Rpt153FCYPayablesAndReceivablesRequest();
        printReport(response, "153-FCYPayablesAndReceivablesRpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/153-fcy-payables-and-receivables", method = RequestMethod.GET)
    public void print153FCYPayablesAndReceivablesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print153FCYPayablesAndReceivables(response, objectMapper.convertValue(query, Rpt153FCYPayablesAndReceivablesRequest.class));
    }

    /**
     * Template: 156-FcyGeneralLedger.rpt
     * Procedure: SPU_Accounts_FCYCustomerLedger_Rpt
     * Desktop: VoucherReports.FCY_LedgerAgainstAccountId
     */
    @RequestMapping(value = "/reports/print/156-fcy-general-ledger", method = RequestMethod.POST)
    public void print156FcyGeneralLedger(HttpServletResponse response, @RequestBody(required = false) Rpt156FcyGeneralLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt156FcyGeneralLedgerRequest();
        printReport(response, "156-FcyGeneralLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/156-fcy-general-ledger", method = RequestMethod.GET)
    public void print156FcyGeneralLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print156FcyGeneralLedger(response, objectMapper.convertValue(query, Rpt156FcyGeneralLedgerRequest.class));
    }

    /**
     * Template: 176-FcyAdjustmentVoucherSlip.rpt
     * Procedure: USp_FcyAdjustmentVouchers_Rpt
     * Desktop: FcyAdjustmentVoucher.FcyAdjustmentSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/176-fcy-adjustment-voucher-slip", method = RequestMethod.POST)
    public void print176FcyAdjustmentVoucherSlip(HttpServletResponse response, @RequestBody(required = false) Rpt176FcyAdjustmentVoucherSlipRequest request) throws Exception {
        if (request == null) request = new Rpt176FcyAdjustmentVoucherSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "176-FcyAdjustmentVoucherSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/176-fcy-adjustment-voucher-slip", method = RequestMethod.GET)
    public void print176FcyAdjustmentVoucherSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print176FcyAdjustmentVoucherSlip(response, objectMapper.convertValue(query, Rpt176FcyAdjustmentVoucherSlipRequest.class));
    }

    /**
     * Template: 1801_LoanRegistration_Slip.rpt
     * Procedure: [Fcm].[USP_LoanAgreementSlipAndRegister]
     * Desktop: LoanRegistrationHeader.LoanAgreementSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1801-loan-registration-slip", method = RequestMethod.POST)
    public void print1801LoanRegistrationSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1801LoanRegistrationSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1801LoanRegistrationSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1801_LoanRegistration_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1801-loan-registration-slip", method = RequestMethod.GET)
    public void print1801LoanRegistrationSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1801LoanRegistrationSlip(response, objectMapper.convertValue(query, Rpt1801LoanRegistrationSlipRequest.class));
    }

    /**
     * Template: 350_FcyJournalVoucherSlip.rpt
     * Procedure: Sp_Vouchers_PaymentReceiptVoucherSlip_Rpt
     * Desktop: VoucherReports.VoucherReport
     */
    @RequestMapping(value = "/reports/print/350-fcy-journal-voucher-slip", method = RequestMethod.POST)
    public void print350FcyJournalVoucherSlip(HttpServletResponse response, @RequestBody(required = false) Rpt350FcyJournalVoucherSlipRequest request) throws Exception {
        if (request == null) request = new Rpt350FcyJournalVoucherSlipRequest();
        printReport(response, "350_FcyJournalVoucherSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/350-fcy-journal-voucher-slip", method = RequestMethod.GET)
    public void print350FcyJournalVoucherSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print350FcyJournalVoucherSlip(response, objectMapper.convertValue(query, Rpt350FcyJournalVoucherSlipRequest.class));
    }

    /**
     * Template: 481-StockReconcilationRegister.rpt
     * Procedure: USP_StockReconcilation
     * Desktop: StocksReport.StockReconcilationReport
     */
    @RequestMapping(value = "/reports/print/481-stock-reconcilation-register", method = RequestMethod.POST)
    public void print481StockReconcilationRegister(HttpServletResponse response, @RequestBody(required = false) Rpt481StockReconcilationRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt481StockReconcilationRegisterRequest();
        printReport(response, "481-StockReconcilationRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/481-stock-reconcilation-register", method = RequestMethod.GET)
    public void print481StockReconcilationRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print481StockReconcilationRegister(response, objectMapper.convertValue(query, Rpt481StockReconcilationRegisterRequest.class));
    }

    /**
     * Template: 598-PaymentAndReceipts_CustomersAndSupplirWise.rpt
     * Procedure: USP_PaymentAndReceipts_CustomersAndSupplirWise
     * Desktop: VoucherReports.PaymentAndReceipts_CustomersAndSupplirWise
     */
    @RequestMapping(value = "/reports/print/598-payment-and-receipts-customers-and-supplir-wise", method = RequestMethod.POST)
    public void print598PaymentAndReceiptsCustomersAndSupplirWise(HttpServletResponse response, @RequestBody(required = false) Rpt598PaymentAndReceiptsCustomersAndSupplirWiseRequest request) throws Exception {
        if (request == null) request = new Rpt598PaymentAndReceiptsCustomersAndSupplirWiseRequest();
        printReport(response, "598-PaymentAndReceipts_CustomersAndSupplirWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/598-payment-and-receipts-customers-and-supplir-wise", method = RequestMethod.GET)
    public void print598PaymentAndReceiptsCustomersAndSupplirWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print598PaymentAndReceiptsCustomersAndSupplirWise(response, objectMapper.convertValue(query, Rpt598PaymentAndReceiptsCustomersAndSupplirWiseRequest.class));
    }

    /**
     * Template: DocumentTypeId-Loan Facility Agreement_Slip.rpt
     * Procedure: [Fcm].[USP_LoanAgreementSlipAndRegister]
     * Desktop: LoanAgreementHeader.LoanAgreementSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/document-type-id-loan-facility-agreement-slip", method = RequestMethod.POST)
    public void printDocumentTypeIdLoanFacilityAgreementSlip(HttpServletResponse response, @RequestBody(required = false) RptDocumentTypeIdLoanFacilityAgreementSlipRequest request) throws Exception {
        if (request == null) request = new RptDocumentTypeIdLoanFacilityAgreementSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "DocumentTypeId-Loan Facility Agreement_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/document-type-id-loan-facility-agreement-slip", method = RequestMethod.GET)
    public void printDocumentTypeIdLoanFacilityAgreementSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printDocumentTypeIdLoanFacilityAgreementSlip(response, objectMapper.convertValue(query, RptDocumentTypeIdLoanFacilityAgreementSlipRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
