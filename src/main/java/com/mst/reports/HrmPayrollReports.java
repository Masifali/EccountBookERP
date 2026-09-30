package com.mst.reports;

import org.springframework.stereotype.Component;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;
import static com.mst.reports.HrmReportSupport.sub;

import java.util.Arrays;

/**
 * Crystal prints of the HRM "Payroll" screens, traced from the desktop.
 *
 * hrm-670-102  frmPayrollPosting.DataGridHistory_ColumnButtonClick "Voucher" ->
 *          CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(VoucherHeadIdGet(PayrollId, 1000)):
 *          VoucherReports.NewVoucherReprot -> SpVouchers_PaymentReceiptVoucherSlipNew_Rpt @OrganizationId,
 *          @CompanyId, @UserId, @Id (when != 0); sub-report getLedgerforPrint.rpt from
 *          InvPurchaseInvoiceReports.GetDataForLedgerSubReport -> usp_getLedgerforPrint @OrganizationId,
 *          @CompanyId, @BranchesId (ReportsParameters default 0 - the caller never sets it; dates /
 *          supplier / account unset and so omitted); rpt parameters @CompanyAddress, @CompanyName, @PrintedBy.
 *
 * hrm-670-1100 / hrm-670-1115  frmPayrollPosting.tsDropDownPrint_DropDownItemClicked:
 *          PayRollReports.GetEmployeePostedSalary -> Sp_GetEmployeePostedSalary @PayrollId (RecId, when
 *          != 0), @Month (the month's text), @Year, @MonthValue, @OrganizationId, @CompanyId, shown with
 *          Reporting.ShowReportWithDataTable(dt, item + ".rpt"). The items are the files of the desktop's
 *          Reports\SalarySheet folder (CommonServices.DynamicReportsLoad), which the server cannot list;
 *          the two salary-sheet templates present on this installation are offered.
 */
@Component
public class HrmPayrollReports {

    public HrmPayrollReports(ReportRegistry registry) {
        registry.register(def("hrm-670-102", "102-ANewAcRptPaymentReceiptsVoucherSlip.rpt", "SpVouchers_PaymentReceiptVoucherSlipNew_Rpt",
                "frmPayrollPosting.DataGridHistory_ColumnButtonClick(Voucher) -> CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102",
                ps(P("@OrganizationId", "session:organizationId"),
                   P("@CompanyId", "session:companyId"),
                   P("@UserId", "session:userId"),
                   G("@Id", "arg:id"), G("@VoucherCodeF", "arg:docnofrom"), G("@VoucherCodeT", "arg:docnoto"),
                   G("@DocumentTypeIds", "arg:documentTypeIds"), G("@Ids", "arg:ids"),
                   P("rpt:@PrintedBy", "session:userName")),
                Arrays.asList(sub("getLedgerforPrint.rpt", "usp_getLedgerforPrint",
                        ps(P("@OrganizationId", "session:organizationId"),
                           P("@CompanyId", "session:companyId"),
                           P("@BranchesId", "const:0"))))));
        for (String rpt : new String[]{"1100", "1115"}) {
            registry.register(def("hrm-670-" + rpt, rpt + "-EmployeeSalarySheet.rpt", "Sp_GetEmployeePostedSalary",
                    "frmPayrollPosting.tsDropDownPrint_DropDownItemClicked -> PayRollReports.GetEmployeePostedSalary",
                    ps(G("@PayrollId", "arg:payrollId"),
                       P("@Month", "arg:month"),
                       P("@Year", "arg:year"),
                       P("@MonthValue", "arg:monthValue"),
                       P("@OrganizationId", "session:organizationId"),
                       P("@CompanyId", "session:companyId"))));
        }
    }
}
