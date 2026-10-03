package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.HrmPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Hrm print actions. Generated from the verified seeder contracts. */
@Controller
public class HrmPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 1003-DailyAttendance.rpt
     * Procedure: Sp_genDailyAttandance_rpt
     * Desktop: HRM_Reports.DailyAttendanceRpt
     */
    @RequestMapping(value = "/reports/print/1003-daily-attendance", method = RequestMethod.POST)
    public void print1003DailyAttendance(HttpServletResponse response, @RequestBody(required = false) Rpt1003DailyAttendanceRequest request) throws Exception {
        if (request == null) request = new Rpt1003DailyAttendanceRequest();
        printReport(response, "1003-DailyAttendance.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1003-daily-attendance", method = RequestMethod.GET)
    public void print1003DailyAttendanceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1003DailyAttendance(response, objectMapper.convertValue(query, Rpt1003DailyAttendanceRequest.class));
    }

    /**
     * Template: 1112-Employee_Registration_Slip.rpt
     * Procedure: Sp_genEmployee_SlipandRegister
     * Desktop: HRM_Reports.EmployeeRegistrationSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1112-employee-registration-slip", method = RequestMethod.POST)
    public void print1112EmployeeRegistrationSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1112EmployeeRegistrationSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1112EmployeeRegistrationSlipRequest();
        printReport(response, "1112-Employee_Registration_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1112-employee-registration-slip", method = RequestMethod.GET)
    public void print1112EmployeeRegistrationSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1112EmployeeRegistrationSlip(response, objectMapper.convertValue(query, Rpt1112EmployeeRegistrationSlipRequest.class));
    }

    /**
     * Template: 1008-EmployeeAdvanceSlip.rpt
     * Procedure: dbo.USP_EmployeeAdvanceSlip
     * Desktop: EmployeeAdvance.EmployeeAdvanceSlip
     */
    @RequestMapping(value = "/reports/print/1008-employee-advance-slip", method = RequestMethod.POST)
    public void print1008EmployeeAdvanceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1008EmployeeAdvanceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1008EmployeeAdvanceSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1008-EmployeeAdvanceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1008-employee-advance-slip", method = RequestMethod.GET)
    public void print1008EmployeeAdvanceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1008EmployeeAdvanceSlip(response, objectMapper.convertValue(query, Rpt1008EmployeeAdvanceSlipRequest.class));
    }

    /**
     * Template: 1010-OverTime_Slip.rpt
     * Procedure: [hrm].[USP_OverTimeRequest_SlipAndRegister]
     * Desktop: OverTimeRequest.OTSlipandRegister
     */
    @RequestMapping(value = "/reports/print/1010-over-time-slip", method = RequestMethod.POST)
    public void print1010OverTimeSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1010OverTimeSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1010OverTimeSlipRequest();
        printReport(response, "1010-OverTime_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1010-over-time-slip", method = RequestMethod.GET)
    public void print1010OverTimeSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1010OverTimeSlip(response, objectMapper.convertValue(query, Rpt1010OverTimeSlipRequest.class));
    }

    /**
     * Template: 1113-DailyLateandEarlyDeparture.rpt
     * Procedure: Sp_hrmDailyLED_rpt
     * Desktop: HRM_Reports.DailyLateandEarlyDeparture
     */
    @RequestMapping(value = "/reports/print/1113-daily-lateand-early-departure", method = RequestMethod.POST)
    public void print1113DailyLateandEarlyDeparture(HttpServletResponse response, @RequestBody(required = false) Rpt1113DailyLateandEarlyDepartureRequest request) throws Exception {
        if (request == null) request = new Rpt1113DailyLateandEarlyDepartureRequest();
        printReport(response, "1113-DailyLateandEarlyDeparture.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1113-daily-lateand-early-departure", method = RequestMethod.GET)
    public void print1113DailyLateandEarlyDepartureGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1113DailyLateandEarlyDeparture(response, objectMapper.convertValue(query, Rpt1113DailyLateandEarlyDepartureRequest.class));
    }

    /**
     * Template: 1114-DutyRosterEmployeeWise.rpt
     * Procedure: Sp_genDutyRosterEmployeeWise_rpt
     * Desktop: HRM_Reports.genDutyRosterEmployeeWise
     */
    @RequestMapping(value = "/reports/print/1114-duty-roster-employee-wise", method = RequestMethod.POST)
    public void print1114DutyRosterEmployeeWise(HttpServletResponse response, @RequestBody(required = false) Rpt1114DutyRosterEmployeeWiseRequest request) throws Exception {
        if (request == null) request = new Rpt1114DutyRosterEmployeeWiseRequest();
        printReport(response, "1114-DutyRosterEmployeeWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1114-duty-roster-employee-wise", method = RequestMethod.GET)
    public void print1114DutyRosterEmployeeWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1114DutyRosterEmployeeWise(response, objectMapper.convertValue(query, Rpt1114DutyRosterEmployeeWiseRequest.class));
    }

    /**
     * Template: 1004-EmployeeAttendance.rpt
     * Procedure: Sp_genEmployeeAttendence_rpt
     * Desktop: HRM_Reports.genEmployeeAttendence
     */
    @RequestMapping(value = "/reports/print/1004-employee-attendance", method = RequestMethod.POST)
    public void print1004EmployeeAttendance(HttpServletResponse response, @RequestBody(required = false) Rpt1004EmployeeAttendanceRequest request) throws Exception {
        if (request == null) request = new Rpt1004EmployeeAttendanceRequest();
        printReport(response, "1004-EmployeeAttendance.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1004-employee-attendance", method = RequestMethod.GET)
    public void print1004EmployeeAttendanceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1004EmployeeAttendance(response, objectMapper.convertValue(query, Rpt1004EmployeeAttendanceRequest.class));
    }

    /**
     * Template: 1005-MonthlyAttendanceRegister_New.rpt
     * Procedure: Sp_MonthlyAttendanceRegister_Rpt
     * Desktop: HRM_Reports.MonthlyAttendanceRegisterNew
     */
    @RequestMapping(value = "/reports/print/1005-monthly-attendance-register-new", method = RequestMethod.POST)
    public void print1005MonthlyAttendanceRegisterNew(HttpServletResponse response, @RequestBody(required = false) Rpt1005MonthlyAttendanceRegisterNewRequest request) throws Exception {
        if (request == null) request = new Rpt1005MonthlyAttendanceRegisterNewRequest();
        printReport(response, "1005-MonthlyAttendanceRegister_New.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1005-monthly-attendance-register-new", method = RequestMethod.GET)
    public void print1005MonthlyAttendanceRegisterNewGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1005MonthlyAttendanceRegisterNew(response, objectMapper.convertValue(query, Rpt1005MonthlyAttendanceRegisterNewRequest.class));
    }

    /**
     * Template: 1001-EmployeeAttendenceMonthly.rpt
     * Procedure: Sp_genEmployeeMonthlyAttendence_rpt
     * Desktop: HRM_Reports.EmployeeMonthlyAttendenceRpt
     */
    @RequestMapping(value = "/reports/print/1001-employee-attendence-monthly", method = RequestMethod.POST)
    public void print1001EmployeeAttendenceMonthly(HttpServletResponse response, @RequestBody(required = false) Rpt1001EmployeeAttendenceMonthlyRequest request) throws Exception {
        if (request == null) request = new Rpt1001EmployeeAttendenceMonthlyRequest();
        printReport(response, "1001-EmployeeAttendenceMonthly.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1001-employee-attendence-monthly", method = RequestMethod.GET)
    public void print1001EmployeeAttendenceMonthlyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1001EmployeeAttendenceMonthly(response, objectMapper.convertValue(query, Rpt1001EmployeeAttendenceMonthlyRequest.class));
    }

    /**
     * Template: 1002-AttendanceRegister.rpt
     * Procedure: Sp_AttendanceRegister_Rpt
     * Desktop: HRM_Reports.MonthlyAttendanceRegisterRpt
     */
    @RequestMapping(value = "/reports/print/1002-attendance-register", method = RequestMethod.POST)
    public void print1002AttendanceRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1002AttendanceRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1002AttendanceRegisterRequest();
        printReport(response, "1002-AttendanceRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1002-attendance-register", method = RequestMethod.GET)
    public void print1002AttendanceRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1002AttendanceRegister(response, objectMapper.convertValue(query, Rpt1002AttendanceRegisterRequest.class));
    }

    /**
     * Template: 1101-EmployeeListDepartmentWiseActiveInactive.rpt
     * Procedure: Sp_genEmployeeHistory_Rpt
     * Desktop: HRM_Reports.EmployeeHistoryRpt
     */
    @RequestMapping(value = "/reports/print/1101-employee-list-department-wise-active-inactive", method = RequestMethod.POST)
    public void print1101EmployeeListDepartmentWiseActiveInactive(HttpServletResponse response, @RequestBody(required = false) Rpt1101EmployeeListDepartmentWiseActiveInactiveRequest request) throws Exception {
        if (request == null) request = new Rpt1101EmployeeListDepartmentWiseActiveInactiveRequest();
        printReport(response, "1101-EmployeeListDepartmentWiseActiveInactive.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1101-employee-list-department-wise-active-inactive", method = RequestMethod.GET)
    public void print1101EmployeeListDepartmentWiseActiveInactiveGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1101EmployeeListDepartmentWiseActiveInactive(response, objectMapper.convertValue(query, Rpt1101EmployeeListDepartmentWiseActiveInactiveRequest.class));
    }

    /**
     * Template: 1101_01-EmployeeListDepartmentWiseActiveInactive.rpt
     * Procedure: Sp_genEmployeeHistory_Rpt
     * Desktop: HRM_Reports.EmployeeHistoryRpt
     */
    @RequestMapping(value = "/reports/print/1101-01-employee-list-department-wise-active-inactive", method = RequestMethod.POST)
    public void print110101EmployeeListDepartmentWiseActiveInactive(HttpServletResponse response, @RequestBody(required = false) Rpt110101EmployeeListDepartmentWiseActiveInactiveRequest request) throws Exception {
        if (request == null) request = new Rpt110101EmployeeListDepartmentWiseActiveInactiveRequest();
        printReport(response, "1101_01-EmployeeListDepartmentWiseActiveInactive.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1101-01-employee-list-department-wise-active-inactive", method = RequestMethod.GET)
    public void print110101EmployeeListDepartmentWiseActiveInactiveGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print110101EmployeeListDepartmentWiseActiveInactive(response, objectMapper.convertValue(query, Rpt110101EmployeeListDepartmentWiseActiveInactiveRequest.class));
    }

    /**
     * Template: 1011-EmployeeOverTimeSlip.rpt
     * Procedure: [hrm].[USP_EmployeeOverTime_SlipAndRegister]
     * Desktop: EmployeeOverTime.EmployeeOverTime_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1011-employee-over-time-slip", method = RequestMethod.POST)
    public void print1011EmployeeOverTimeSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1011EmployeeOverTimeSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1011EmployeeOverTimeSlipRequest();
        printReport(response, "1011-EmployeeOverTimeSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1011-employee-over-time-slip", method = RequestMethod.GET)
    public void print1011EmployeeOverTimeSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1011EmployeeOverTimeSlip(response, objectMapper.convertValue(query, Rpt1011EmployeeOverTimeSlipRequest.class));
    }

    /**
     * Template: 1003_01-DailyAttendance.rpt
     * Procedure: Sp_genDailyAttandance_rpt
     * Desktop: HRM_Reports.DailyAttendanceRpt
     */
    @RequestMapping(value = "/reports/print/1003-01-daily-attendance", method = RequestMethod.POST)
    public void print100301DailyAttendance(HttpServletResponse response, @RequestBody(required = false) Rpt100301DailyAttendanceRequest request) throws Exception {
        if (request == null) request = new Rpt100301DailyAttendanceRequest();
        printReport(response, "1003_01-DailyAttendance.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1003-01-daily-attendance", method = RequestMethod.GET)
    public void print100301DailyAttendanceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print100301DailyAttendance(response, objectMapper.convertValue(query, Rpt100301DailyAttendanceRequest.class));
    }

    /**
     * Template: 1006-AttendanceSummery.rpt
     * Procedure: Sp_genEmployeeAttendanceSummery_rpt
     * Desktop: HRM_Reports.MonthlyAttendanceSummary
     */
    @RequestMapping(value = "/reports/print/1006-attendance-summery", method = RequestMethod.POST)
    public void print1006AttendanceSummery(HttpServletResponse response, @RequestBody(required = false) Rpt1006AttendanceSummeryRequest request) throws Exception {
        if (request == null) request = new Rpt1006AttendanceSummeryRequest();
        printReport(response, "1006-AttendanceSummery.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1006-attendance-summery", method = RequestMethod.GET)
    public void print1006AttendanceSummeryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1006AttendanceSummery(response, objectMapper.convertValue(query, Rpt1006AttendanceSummeryRequest.class));
    }

    /**
     * Template: 1007-DailyShiftStrengthAttendenceDpt.rpt
     * Procedure: Sp_genDSStrengthAttendence_rpt
     * Desktop: HRM_Reports.genDSStrengthAttendence
     */
    @RequestMapping(value = "/reports/print/1007-daily-shift-strength-attendence-dpt", method = RequestMethod.POST)
    public void print1007DailyShiftStrengthAttendenceDpt(HttpServletResponse response, @RequestBody(required = false) Rpt1007DailyShiftStrengthAttendenceDptRequest request) throws Exception {
        if (request == null) request = new Rpt1007DailyShiftStrengthAttendenceDptRequest();
        printReport(response, "1007-DailyShiftStrengthAttendenceDpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1007-daily-shift-strength-attendence-dpt", method = RequestMethod.GET)
    public void print1007DailyShiftStrengthAttendenceDptGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1007DailyShiftStrengthAttendenceDpt(response, objectMapper.convertValue(query, Rpt1007DailyShiftStrengthAttendenceDptRequest.class));
    }

    /**
     * Template: 1009-EmployeeOverTimeRegister.rpt
     * Procedure: [dbo].[usp_ActualOverTimeLoaderForRequest]
     * Desktop: EmployeeOverTime.ActualOverTimeLoaderForRequest
     */
    @RequestMapping(value = "/reports/print/1009-employee-over-time-register", method = RequestMethod.POST)
    public void print1009EmployeeOverTimeRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1009EmployeeOverTimeRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1009EmployeeOverTimeRegisterRequest();
        printReport(response, "1009-EmployeeOverTimeRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1009-employee-over-time-register", method = RequestMethod.GET)
    public void print1009EmployeeOverTimeRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1009EmployeeOverTimeRegister(response, objectMapper.convertValue(query, Rpt1009EmployeeOverTimeRegisterRequest.class));
    }

    /**
     * Template: 1100-EmployeeSalarySheet.rpt
     * Procedure: Sp_GetEmployeePostedSalary
     * Desktop: PayRollReports.GetEmployeePostedSalary
     */
    @RequestMapping(value = "/reports/print/1100-employee-salary-sheet", method = RequestMethod.POST)
    public void print1100EmployeeSalarySheet(HttpServletResponse response, @RequestBody(required = false) Rpt1100EmployeeSalarySheetRequest request) throws Exception {
        if (request == null) request = new Rpt1100EmployeeSalarySheetRequest();
        printReport(response, "1100-EmployeeSalarySheet.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1100-employee-salary-sheet", method = RequestMethod.GET)
    public void print1100EmployeeSalarySheetGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1100EmployeeSalarySheet(response, objectMapper.convertValue(query, Rpt1100EmployeeSalarySheetRequest.class));
    }

    /**
     * Template: 1110-EmployeeSalarySlip.rpt
     * Procedure: Sp_EmployeeSalarySlip_Rpt
     * Desktop: PayRollReports.EmployeeSalarySlip_Rpt
     */
    @RequestMapping(value = "/reports/print/1110-employee-salary-slip", method = RequestMethod.POST)
    public void print1110EmployeeSalarySlip(HttpServletResponse response, @RequestBody(required = false) Rpt1110EmployeeSalarySlipRequest request) throws Exception {
        if (request == null) request = new Rpt1110EmployeeSalarySlipRequest();
        printReport(response, "1110-EmployeeSalarySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1110-employee-salary-slip", method = RequestMethod.GET)
    public void print1110EmployeeSalarySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1110EmployeeSalarySlip(response, objectMapper.convertValue(query, Rpt1110EmployeeSalarySlipRequest.class));
    }

    /**
     * Template: 1110B_EmployeeSalarySlip.rpt
     * Procedure: Sp_EmployeeSalarySlip_Rpt
     * Desktop: PayRollReports.EmployeeSalarySlip_Rpt
     */
    @RequestMapping(value = "/reports/print/1110b-employee-salary-slip", method = RequestMethod.POST)
    public void print1110BEmployeeSalarySlip(HttpServletResponse response, @RequestBody(required = false) Rpt1110BEmployeeSalarySlipRequest request) throws Exception {
        if (request == null) request = new Rpt1110BEmployeeSalarySlipRequest();
        printReport(response, "1110B_EmployeeSalarySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1110b-employee-salary-slip", method = RequestMethod.GET)
    public void print1110BEmployeeSalarySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1110BEmployeeSalarySlip(response, objectMapper.convertValue(query, Rpt1110BEmployeeSalarySlipRequest.class));
    }

    /**
     * Template: 1111-Employee_Registration_Register.rpt
     * Procedure: Sp_genEmployee_SlipandRegister
     * Desktop: HRM_Reports.EmployeeRegistrationSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1111-employee-registration-register", method = RequestMethod.POST)
    public void print1111EmployeeRegistrationRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1111EmployeeRegistrationRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1111EmployeeRegistrationRegisterRequest();
        printReport(response, "1111-Employee_Registration_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1111-employee-registration-register", method = RequestMethod.GET)
    public void print1111EmployeeRegistrationRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1111EmployeeRegistrationRegister(response, objectMapper.convertValue(query, Rpt1111EmployeeRegistrationRegisterRequest.class));
    }

    /**
     * Template: 1115-EmployeeSalarySheet.rpt
     * Procedure: Sp_GetEmployeePostedSalary
     * Desktop: PayRollReports.GetEmployeePostedSalary
     */
    @RequestMapping(value = "/reports/print/1115-employee-salary-sheet", method = RequestMethod.POST)
    public void print1115EmployeeSalarySheet(HttpServletResponse response, @RequestBody(required = false) Rpt1115EmployeeSalarySheetRequest request) throws Exception {
        if (request == null) request = new Rpt1115EmployeeSalarySheetRequest();
        printReport(response, "1115-EmployeeSalarySheet.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1115-employee-salary-sheet", method = RequestMethod.GET)
    public void print1115EmployeeSalarySheetGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1115EmployeeSalarySheet(response, objectMapper.convertValue(query, Rpt1115EmployeeSalarySheetRequest.class));
    }

    /**
     * Template: 1116_PayrollPostingForMultiApprovalReport.rpt
     * Procedure: [dbo].[USP_PayrollPostingForMultiApproval]
     * Desktop: Payroll.PayrollPostingForMultiApproval
     */
    @RequestMapping(value = "/reports/print/1116-payroll-posting-for-multi-approval-report", method = RequestMethod.POST)
    public void print1116PayrollPostingForMultiApprovalReport(HttpServletResponse response, @RequestBody(required = false) Rpt1116PayrollPostingForMultiApprovalReportRequest request) throws Exception {
        if (request == null) request = new Rpt1116PayrollPostingForMultiApprovalReportRequest();
        printReport(response, "1116_PayrollPostingForMultiApprovalReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1116-payroll-posting-for-multi-approval-report", method = RequestMethod.GET)
    public void print1116PayrollPostingForMultiApprovalReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1116PayrollPostingForMultiApprovalReport(response, objectMapper.convertValue(query, Rpt1116PayrollPostingForMultiApprovalReportRequest.class));
    }

    /**
     * Template: EmployeeAttendenceMonthly.rpt
     * Procedure: Sp_genEmployeeMonthlyAttendence_rpt
     * Desktop: HRM_Reports.EmployeeMonthlyAttendenceRpt
     */
    @RequestMapping(value = "/reports/print/employee-attendence-monthly", method = RequestMethod.POST)
    public void printEmployeeAttendenceMonthly(HttpServletResponse response, @RequestBody(required = false) RptEmployeeAttendenceMonthlyRequest request) throws Exception {
        if (request == null) request = new RptEmployeeAttendenceMonthlyRequest();
        printReport(response, "EmployeeAttendenceMonthly.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/employee-attendence-monthly", method = RequestMethod.GET)
    public void printEmployeeAttendenceMonthlyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printEmployeeAttendenceMonthly(response, objectMapper.convertValue(query, RptEmployeeAttendenceMonthlyRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
