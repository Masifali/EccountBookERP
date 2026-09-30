package com.mst.reports;

import org.springframework.stereotype.Component;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;

/**
 * Crystal prints of the HRM "Attendance Management" screens.
 *
 *   hrm-1003-658  frmDailyAttendance.btnPrint_Click ("1003-&Print")
 *                 HRM_Reports.DailyAttendanceRpt(ReportsParameters { OrganizationId, CompanyId, EmployeeCategoryId,
 *                 ShiftId, DepartmentId, DesignationId, EmployeeId, DocDate = txtDate, PartyLocationId, SectionId,
 *                 ApprovedFilter = "All" }) -> Sp_genDailyAttandance_rpt, table 0 -> 1003-DailyAttendance.rpt.
 *                 Guards as the BLL: every id "!= 0", @Date "!CheckDateTimeNull"; @IsMissing is not sent
 *                 (ApprovedFilter == "All"). The key carries the screen id so it cannot collide with the
 *                 HRM_Reports.DailyAttendanceRpt screen (373), which prints the same template from its own filters.
 *
 * hrmManualAttendance (659) has a Print button that is Visible = false with no Click handler - nothing to register.
 */
@Component
public class HrmAttendanceReports {

    public HrmAttendanceReports(ReportRegistry registry) {
        registry.register(def("hrm-1003-658", "1003-DailyAttendance.rpt", "Sp_genDailyAttandance_rpt",
                "frmDailyAttendance.btnPrint_Click",
                ps(P("@OrganizationId", "session:organizationId"),
                   P("@CompanyId", "session:companyId"),
                   G("@DepartmentId", "arg:departmentId"),
                   G("@Date", "arg:date"),
                   G("@EmployeeId", "arg:employeeId"),
                   G("@DesignationId", "arg:designationId"),
                   G("@ShiftId", "arg:shiftId"),
                   G("@LocationId", "arg:locationId"),
                   G("@EmployeeCategoryId", "arg:employeeCategoryId"),
                   G("@SectionId", "arg:sectionId"),
                   G("@IsMissing", "arg:isMissing"))));
    }
}
