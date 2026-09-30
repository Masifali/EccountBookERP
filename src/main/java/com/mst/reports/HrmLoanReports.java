package com.mst.reports;

import org.springframework.stereotype.Component;

import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;
import static com.mst.reports.HrmReportSupport.sub;

/**
 * Crystal prints of the HRM "Loan Management" forms, traced from the desktop:
 *
 * hrm-102  frmEmployeeLoan.btnPrint_Click / grd_LinkClicked(DocNo) -> CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(
 *          VoucherHeadIdGet(RecId, 1001)) -> VoucherReports.NewVoucherReprot: SpVouchers_PaymentReceiptVoucherSlipNew_Rpt
 *          @OrganizationId, @CompanyId, @UserId, @Id (only when != 0; VoucherIds / doc-no range not set by this caller);
 *          sub-report getLedgerforPrint.rpt <- InvPurchaseInvoiceReports.GetDataForLedgerSubReport: usp_getLedgerforPrint
 *          @OrganizationId, @CompanyId, @BranchesId (ReportsParameters.BranchesId, never set here -> 0); dates, supplier and
 *          account are guarded and not set by this caller. Template parameters @CompanyAddress, @CompanyName, @PrintedBy.
 * hrm-1008 frmEmployeeAdvance.btnPrint_Click / grd_ColumnButtonClick(Print) -> GenerateSlip(PrintId) ->
 *          EmployeeAdvance.EmployeeAdvanceSlip: USP_EmployeeAdvanceSlip @Id; template parameters @CompanyAddress, @CompanyName.
 */
@Component
public class HrmLoanReports {

    public HrmLoanReports(ReportRegistry registry) {
        registry.register(def("hrm-102", "102-ANewAcRptPaymentReceiptsVoucherSlip.rpt", "SpVouchers_PaymentReceiptVoucherSlipNew_Rpt",
                "frmEmployeeLoan.btnPrint_Click -> CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102",
                ps(P("@OrganizationId", "session:organizationId"),
                   P("@CompanyId", "session:companyId"),
                   P("@UserId", "session:userId"),
                   G("@Id", "arg:id"), G("@VoucherCodeF", "arg:docnofrom"), G("@VoucherCodeT", "arg:docnoto"),
                   G("@DocumentTypeIds", "arg:documentTypeIds"), G("@Ids", "arg:ids"),
                   P("rpt:@PrintedBy", "session:userName")),
                java.util.Collections.singletonList(sub("getLedgerforPrint.rpt", "usp_getLedgerforPrint",
                        ps(P("@OrganizationId", "session:organizationId"),
                           P("@CompanyId", "session:companyId"),
                           P("@BranchesId", "const:0"))))));
        registry.register(def("hrm-1008", "1008-EmployeeAdvanceSlip.rpt", "USP_EmployeeAdvanceSlip",
                "frmEmployeeAdvance.GenerateSlip",
                ps(P("@Id", "arg:id"))));
    }
}
