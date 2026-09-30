package com.mst.reports;

import org.springframework.stereotype.Component;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;

/**
 * Crystal prints of screen 648 Employee Registration (frmEmployeeRegistration.cs):
 *
 *   hrm-1112  1112-Employee_Registration_Slip.rpt   btnPrint_Click ("1112-Print Slip", RecId) and the History grid's
 *             "Print" column (that row's EmployeeId): HRM_Reports.EmployeeRegistrationSlipAndRegister ->
 *             Sp_genEmployee_SlipandRegister @OrganizationId, @CompanyId, @EmployeeId (only when != 0),
 *             Reporting.RptPerameter("@CompanyName", UserAccount.CompName) -> ShowReportWithDataTable.
 *
 * btnPrintRegister ("1111-Print Register", 1111-Employee_Registration_Register.rpt, the same procedure without
 * @EmployeeId) is NOT registered: that .rpt is not in rpt_list.txt, so the page shows
 * "Report file 1111-Employee_Registration_Register.rpt is not available."
 */
@Component
public class HrmEmployeeReports {
    public HrmEmployeeReports(ReportRegistry registry) {
        registry.register(def("hrm-1112", "1112-Employee_Registration_Slip.rpt", "Sp_genEmployee_SlipandRegister",
                "frmEmployeeRegistration.btnPrint_Click / grdEmployeeRegistration_ColumnButtonClick(Print)",
                ps(P("@OrganizationId", "session:organizationId"),
                   P("@CompanyId", "session:companyId"),
                   G("@EmployeeId", "arg:employeeId"))));
    }
}
