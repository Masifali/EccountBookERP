package com.mst.reports;

import org.springframework.stereotype.Component;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;

/**
 * Crystal print contracts of the HRM report screens (Architecture.WinApp.HRM_Reports, 371-378).
 *
 * Most buttons print the DataTable of the form's last Show (Reporting.ShowReportWithDataTable(dtGrid, ...));
 * the contract re-runs the procedure of that Show with the arguments the page kept from it, exactly
 * as the BLL sends them (GUARDED = the BLL's "if (x != 0)" / "if (!CheckDateTimeNull(x))").
 * Only templates listed in rpt_list.txt are registered. Not registered (the page shows
 * "Report file <x>.rpt is not available."):
 *   1101_01-EmployeeListDepartmentWiseActiveInactive.rpt  EmployeeHistoryRpt.btnPrint1101_01_Click
 *   EmployeeAttendenceMonthly.rpt                         MonthlyAttendanceSummary.grd_LinkClicked (EmployeeName)
 *   1110-EmployeeSalarySlip.rpt                           PayRollSalarySheetRpt.btn1110SalaryList_Click
 *   Reports\SalarySheet\*.rpt                             PayRollSalarySheetRpt.tsDropDownPrint (DynamicReportsLoad folder)
 *
 * Page arguments: departmentId sectionId shiftId designationId employeeId partyLocationId active
 * date fromDate toDate month year debitAccountId.
 */
@Component
public class HrmReportsPrints {

    public HrmReportsPrints(ReportRegistry registry) {

        /* 371 EmployeeHistoryRpt.print_Click - dtGrid of GridHistory: HRM_Reports.EmployeeHistoryRpt.
           @DesignationId carries obj.PartyLocationId (= cmbDesignation); @Active only when a status row is active. */
        registry.register(def("hrm-1101", "1101-EmployeeListDepartmentWiseActiveInactive.rpt", "Sp_genEmployeeHistory_Rpt",
                "EmployeeHistoryRpt.print_Click (dtGrid of GridHistory)",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   G("@ShiftId", "arg:shiftId"), G("@SectionId", "arg:sectionId"), G("@DepartmentId", "arg:departmentId"),
                   G("@BranchId", "arg:branchesId"),
                   G("@DesignationId", "arg:designationId"), G("@EmployeeId", "arg:employeeId"), G("@Active", "arg:active"))));

        /* 372 HRM_Reports.btnPrintRegister_Click (1001-Print) - dtGrid of btnShow_Click: EmployeeMonthlyAttendenceRpt. */
        registry.register(def("hrm-1001", "1001-EmployeeAttendenceMonthly.rpt", "Sp_genEmployeeMonthlyAttendence_rpt",
                "HRM_Reports.btnPrintRegister_Click (dtGrid of btnShow_Click)",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@Month", "arg:month"), P("@Year", "arg:year"),
                   G("@EmployeeId", "arg:employeeId"), G("@DepartmentId", "arg:departmentId"), G("@SectionId", "arg:sectionId"),
                   G("@PartyLocationId", "arg:partyLocationId"), G("@ShiftId", "arg:shiftId"))));

        /* 372 HRM_Reports.btn1002Print_Click - HRM_Reports.MonthlyAttendanceRegisterRpt with the current filters. */
        registry.register(def("hrm-1002", "1002-AttendanceRegister.rpt", "Sp_AttendanceRegister_Rpt",
                "HRM_Reports.btn1002Print_Click",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@Month", "arg:month"), P("@Year", "arg:year"),
                   G("@DepartmentId", "arg:departmentId"), G("@EmployeeId", "arg:employeeId"), G("@ShiftId", "arg:shiftId"),
                   G("@SectionId", "arg:sectionId"), G("@PartyLocationId", "arg:partyLocationId"))));

        /* 372 HRM_Reports.print_Click (1004-Print, Short Register tab) and 376 genEmployeeAttendence.print_Click -
           dtGrid of GridHistory: HRM_Reports.genEmployeeAttendence. */
        registry.register(def("hrm-1004", "1004-EmployeeAttendance.rpt", "Sp_genEmployeeAttendence_rpt",
                "genEmployeeAttendence.print_Click / HRM_Reports.print_Click (dtGrid of GridHistory)",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   G("@EmployeeId", "arg:employeeId"), G("@DepartmentId", "arg:departmentId"),
                   G("@FromDate", "arg:fromDate"), G("@ToDate", "arg:toDate"))));

        /* 373 DailyAttendanceRpt.print_Click (1003) / btnprint_Click (1003_01) - dtGrid of GridHistory:
           HRM_Reports.DailyAttendanceRpt with ApprovedFilter "All" (no @IsMissing). */
        for (String[] r : new String[][]{
                {"hrm-1003", "1003-DailyAttendance.rpt", "DailyAttendanceRpt.print_Click (dtGrid of GridHistory)"},
                {"hrm-1003-01", "1003_01-DailyAttendance.rpt", "DailyAttendanceRpt.btnprint_Click (dtGrid of GridHistory)"}}) {
            registry.register(def(r[0], r[1], "Sp_genDailyAttandance_rpt", r[2],
                    ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                       G("@DepartmentId", "arg:departmentId"), G("@Date", "arg:date"), G("@EmployeeId", "arg:employeeId"),
                       G("@DesignationId", "arg:designationId"), G("@ShiftId", "arg:shiftId"), G("@LocationId", "arg:locationId"),
                       G("@EmployeeCategoryId", "arg:employeeCategoryId"), G("@SectionId", "arg:sectionId"),
                       G("@IsMissing", "arg:isMissing"))));
        }

        /* 374 DailyLateandEarlyDeparture.print_Click - dtGrid of GridHistory: HRM_Reports.DailyLateandEarlyDeparture. */
        registry.register(def("hrm-1113", "1113-DailyLateandEarlyDeparture.rpt", "Sp_hrmDailyLED_rpt",
                "DailyLateandEarlyDeparture.print_Click (dtGrid of GridHistory)",
                ps(P("@CompanyId", "session:companyId"), G("@PartyLocationId", "arg:partyLocationId"), G("@Date", "arg:date"))));

        /* 375 genDutyRosterEmployeeWise.print_Click - dtGrid of GridHistory: HRM_Reports.genDutyRosterEmployeeWise.
           @CompanyId added: the procedure INNER JOINs Company on it (the BLL drops it; see HrmReportsService D2). */
        registry.register(def("hrm-1114", "1114-DutyRosterEmployeeWise.rpt", "Sp_genDutyRosterEmployeeWise_rpt",
                "genDutyRosterEmployeeWise.print_Click (dtGrid of GridHistory)",
                ps(P("@CompanyId", "session:companyId"), G("@EmployeeId", "arg:employeeId"), G("@Date", "arg:date"))));

        /* 377 MonthlyAttendanceSummary.btnMonthlyRegister_Click - HRM_Reports.MonthlyAttendanceRegisterNew. */
        registry.register(def("hrm-1005", "1005-MonthlyAttendanceRegister_New.rpt", "Sp_MonthlyAttendanceRegister_Rpt",
                "MonthlyAttendanceSummary.btnMonthlyRegister_Click",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@Month", "arg:month"), P("@Year", "arg:year"),
                   G("@EmployeeId", "arg:employeeId"), G("@DepartmentId", "arg:departmentId"))));

        /* 377 MonthlyAttendanceSummary.print_Click - dtGrid of GridHistory: HRM_Reports.MonthlyAttendanceSummary. */
        registry.register(def("hrm-1006", "1006-AttendanceSummery.rpt", "Sp_genEmployeeAttendanceSummery_rpt",
                "MonthlyAttendanceSummary.print_Click (dtGrid of GridHistory)",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@Month", "arg:month"), P("@Year", "arg:year"),
                   G("@DepartmentId", "arg:departmentId"), G("@EmployeeId", "arg:employeeId"), G("@SectionId", "arg:sectionId"))));

        /* 378 PayRollSalarySheetRpt.SalarySlipRpt (1110B-SalarySlip button with cmbEmployee / cmbDepartment,
           and the EmployeeNo link with that row's EmployeeId) - PayRollReports.EmployeeSalarySlip_Rpt. */
        registry.register(def("hrm-1110B", "1110B_EmployeeSalarySlip.rpt", "Sp_EmployeeSalarySlip_Rpt",
                "PayRollSalarySheetRpt.SalarySlipRpt",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@Month", "arg:month"), P("@Year", "arg:year"),
                   G("@EmployeeId", "arg:employeeId"), G("@DepartmentId", "arg:departmentId"),
                   G("@DebitAccountId", "arg:debitAccountId"))));
    }
}
