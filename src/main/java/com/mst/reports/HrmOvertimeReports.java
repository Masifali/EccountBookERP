package com.mst.reports;

import org.springframework.stereotype.Component;

import static com.mst.reports.HrmReportSupport.G;
import static com.mst.reports.HrmReportSupport.P;
import static com.mst.reports.HrmReportSupport.def;
import static com.mst.reports.HrmReportSupport.ps;

/**
 * Crystal prints of the HRM "Over Time Management" screens. Each desktop button fills its report
 * with Reporting.ShowReportWithDataTable(dt, "<rpt>") where dt is the DataTable of the procedure
 * below, plus @CompanyName / @CompanyAddress (ReportDefinition adds those two).
 *
 *   hrm-1009  EmpOverTimeLoadForRequest.print_Click  dtGrid = usp_ActualOverTimeLoaderForRequest (the Show filters)
 *   hrm-1010  CommonServices.OverTimeSlip(PrintId)    OverTimeRequest.OTSlipandRegister @Id (OverTimeRequest 1010-Print,
 *             after-save print, History "Print" button)
 *   hrm-1011  frmEmployeeOverTime.btnPrint1011_Click  EmployeeOverTime_SlipAndRegister @Month, @Year, guarded @EmployeeId / @Id (the form's
 *             @EmployeeId is always 0 -> never sent; @Id (DutyRoasterId) is never set -> never sent)
 */
@Component
public class HrmOvertimeReports {

    public HrmOvertimeReports(ReportRegistry registry) {
        registry.register(def("hrm-1009", "1009-EmployeeOverTimeRegister.rpt", "[dbo].[usp_ActualOverTimeLoaderForRequest]",
                "EmpOverTimeLoadForRequest.print_Click",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@FromDate", "arg:fromDate"), P("@ToDate", "arg:toDate"),
                   G("@EmployeeId", "arg:employeeId"), G("@DepartmentId", "arg:departmentId"))));
        registry.register(def("hrm-1010", "1010-OverTime_Slip.rpt", "[hrm].[USP_OverTimeRequest_SlipAndRegister]",
                "CommonServices.OverTimeSlip",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   G("@Id", "arg:id"), G("@SectionId", "arg:sectionId"), G("@DepartmentId", "arg:departmentId"),
                   G("@RequestById", "arg:requestedById"), G("@RequestToDate", "arg:toDate"), G("@RequestFromDate", "arg:fromDate"),
                   G("@OverTimeToDate", "arg:gpDateT"), G("@OverTimeFromDate", "arg:gpDateF"),
                   G("@EntryFromDate", "arg:entryDateFrom"), G("@EntryToDate", "arg:entryDateTo"),
                   G("@ModifyFromDate", "arg:modifyDateFrom"), G("@ModifyToDate", "arg:modifyDateTo"),
                   G("@ApprovedFromDate", "arg:approvedDateFrom"), G("@ApprovedToDate", "arg:approvedDateTo"),
                   G("@ActionId", "arg:actionId"))));
        registry.register(def("hrm-1011", "1011-EmployeeOverTimeSlip.rpt", "[hrm].[USP_EmployeeOverTime_SlipAndRegister]",
                "frmEmployeeOverTime.btnPrint1011_Click",
                ps(P("@OrganizationId", "session:organizationId"), P("@CompanyId", "session:companyId"),
                   P("@Month", "arg:month"), P("@Year", "arg:year"),
                   G("@EmployeeId", "arg:employeeId"), G("@Id", "arg:dutyRoasterId"))));
    }
}
