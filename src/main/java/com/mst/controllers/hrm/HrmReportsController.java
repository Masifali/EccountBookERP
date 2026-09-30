package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.HrmReportsFilterDto;
import com.mst.services.hrm.HrmReportsService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import static com.mst.services.hrm.HrmReportsService.*;

/**
 * HRM reports (Architecture.WinApp.HRM_Reports, AppModules 29) pages and API.
 *
 *   371 /hrm/reports/employee-register                EmployeeHistoryRpt.cs
 *   372 /hrm/reports/employee-monthly-register        HRM_Reports.cs
 *   373 /hrm/reports/daily-attendance                 DailyAttendanceRpt.cs
 *   374 /hrm/reports/daily-late-and-early-departure   DailyLateandEarlyDeparture.cs
 *   375 /hrm/reports/duty-roster-employee-wise        genDutyRosterEmployeeWise.cs
 *   376 /hrm/reports/employee-attendence              genEmployeeAttendence.cs
 *   377 /hrm/reports/monthly-attendance-summary       MonthlyAttendanceSummary.cs
 *   378 /hrm/reports/salary-sheet                     PayRollSalarySheetRpt.cs
 *
 * API under /api/hrm/reports/<page>/... (setup | show | employees | sections | department | print checks).
 * Prints go to /api/reports/{key}/print.pdf with the hrm-* contracts of com.mst.reports.HrmReportsPrints.
 */
@Controller
public class HrmReportsController {

    private static final String API = "/api/hrm/reports";

    @Autowired private HrmReportsService service;

    private static String page(Model model, String view) { model.addAttribute("activeMenu", "apps"); return "hrm/reports/" + view; }

    // ------------------------------------------------------------------ pages

    @GetMapping("/hrm/reports/employee-register")
    public String employeeRegisterPage(Model model) { return page(model, "employee-register"); }

    @GetMapping("/hrm/reports/employee-monthly-register")
    public String monthlyRegisterPage(Model model) { return page(model, "employee-monthly-register"); }

    @GetMapping("/hrm/reports/daily-attendance")
    public String dailyAttendancePage(Model model) { return page(model, "daily-attendance"); }

    @GetMapping("/hrm/reports/daily-late-and-early-departure")
    public String dailyLateEarlyPage(Model model) { return page(model, "daily-late-and-early-departure"); }

    @GetMapping("/hrm/reports/duty-roster-employee-wise")
    public String dutyRosterPage(Model model) { return page(model, "duty-roster-employee-wise"); }

    @GetMapping("/hrm/reports/employee-attendence")
    public String employeeAttendencePage(Model model) { return page(model, "employee-attendence"); }

    @GetMapping("/hrm/reports/monthly-attendance-summary")
    public String monthlySummaryPage(Model model) { return page(model, "monthly-attendance-summary"); }

    @GetMapping("/hrm/reports/salary-sheet")
    public String salarySheetPage(Model model) { return page(model, "salary-sheet"); }

    // ------------------------------------------------------------------ 371

    @GetMapping(API + "/employee-register/setup") @ResponseBody
    public ResponseEntity<?> employeeRegisterSetup() { return HrmApi.run(() -> service.employeeRegisterSetup()); }

    @PostMapping(API + "/employee-register/show") @ResponseBody
    public ResponseEntity<?> employeeRegisterShow(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.employeeRegisterShow(f)); }

    // ------------------------------------------------------------------ 372

    @GetMapping(API + "/employee-monthly-register/setup") @ResponseBody
    public ResponseEntity<?> monthlyRegisterSetup() { return HrmApi.run(() -> service.monthlyRegisterSetup()); }

    @PostMapping(API + "/employee-monthly-register/employees") @ResponseBody
    public ResponseEntity<?> monthlyRegisterEmployees(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.monthlyRegisterEmployees(f)); }

    @GetMapping(API + "/employee-monthly-register/sections") @ResponseBody
    public ResponseEntity<?> monthlyRegisterSections(@RequestParam(value = "departmentId", defaultValue = "0") int departmentId) {
        return HrmApi.run(() -> service.monthlyRegisterSections(departmentId));
    }

    @PostMapping(API + "/employee-monthly-register/show") @ResponseBody
    public ResponseEntity<?> monthlyRegisterShow(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.monthlyRegisterShow(f)); }

    @PostMapping(API + "/employee-monthly-register/show-register") @ResponseBody
    public ResponseEntity<?> monthlyRegisterShowRegister(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.monthlyRegisterShowRegister(f)); }

    @PostMapping(API + "/employee-monthly-register/print-1002-check") @ResponseBody
    public ResponseEntity<?> monthlyRegisterPrint1002(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.monthlyRegisterPrint1002Check(f)); }

    // ------------------------------------------------------------------ 373

    @GetMapping(API + "/daily-attendance/setup") @ResponseBody
    public ResponseEntity<?> dailyAttendanceSetup() { return HrmApi.run(() -> service.dailyAttendanceSetup()); }

    @GetMapping(API + "/daily-attendance/employees") @ResponseBody
    public ResponseEntity<?> dailyAttendanceEmployees(@RequestParam(value = "departmentId", defaultValue = "0") int departmentId) {
        return HrmApi.run(() -> service.employeesOfDepartment(SCREEN_DAILY_ATTENDANCE, departmentId));
    }

    @PostMapping(API + "/daily-attendance/show") @ResponseBody
    public ResponseEntity<?> dailyAttendanceShow(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.dailyAttendanceShow(f)); }

    // ------------------------------------------------------------------ 374

    @GetMapping(API + "/daily-late-and-early-departure/setup") @ResponseBody
    public ResponseEntity<?> dailyLateEarlySetup() { return HrmApi.run(() -> service.dailyLateEarlySetup()); }

    @PostMapping(API + "/daily-late-and-early-departure/show") @ResponseBody
    public ResponseEntity<?> dailyLateEarlyShow(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.dailyLateEarlyShow(f)); }

    // ------------------------------------------------------------------ 375

    @GetMapping(API + "/duty-roster-employee-wise/setup") @ResponseBody
    public ResponseEntity<?> dutyRosterSetup() { return HrmApi.run(() -> service.dutyRosterSetup()); }

    @PostMapping(API + "/duty-roster-employee-wise/show") @ResponseBody
    public ResponseEntity<?> dutyRosterShow(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.dutyRosterShow(f)); }

    // ------------------------------------------------------------------ 376

    @GetMapping(API + "/employee-attendence/setup") @ResponseBody
    public ResponseEntity<?> employeeAttendenceSetup() { return HrmApi.run(() -> service.employeeAttendenceSetup()); }

    @GetMapping(API + "/employee-attendence/employees") @ResponseBody
    public ResponseEntity<?> employeeAttendenceEmployees(@RequestParam(value = "departmentId", defaultValue = "0") int departmentId) {
        return HrmApi.run(() -> service.employeesOfDepartment(SCREEN_EMPLOYEE_ATTENDENCE, departmentId));
    }

    @PostMapping(API + "/employee-attendence/show") @ResponseBody
    public ResponseEntity<?> employeeAttendenceShow(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.employeeAttendenceShow(f)); }

    // ------------------------------------------------------------------ 377

    @GetMapping(API + "/monthly-attendance-summary/setup") @ResponseBody
    public ResponseEntity<?> monthlySummarySetup() { return HrmApi.run(() -> service.monthlySummarySetup()); }

    @GetMapping(API + "/monthly-attendance-summary/employees") @ResponseBody
    public ResponseEntity<?> monthlySummaryEmployees(@RequestParam(value = "departmentId", defaultValue = "0") int departmentId) {
        return HrmApi.run(() -> service.employeesOfDepartment(SCREEN_MONTHLY_SUMMARY, departmentId));
    }

    @PostMapping(API + "/monthly-attendance-summary/show") @ResponseBody
    public ResponseEntity<?> monthlySummaryShow(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.monthlySummaryShow(f)); }

    @PostMapping(API + "/monthly-attendance-summary/print-1005-check") @ResponseBody
    public ResponseEntity<?> monthlySummaryPrint1005(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.monthlySummaryPrint1005Check(f)); }

    // ------------------------------------------------------------------ 378

    @GetMapping(API + "/salary-sheet/setup") @ResponseBody
    public ResponseEntity<?> salarySheetSetup() { return HrmApi.run(() -> service.salarySheetSetup()); }

    @GetMapping(API + "/salary-sheet/department") @ResponseBody
    public ResponseEntity<?> salarySheetDepartment(@RequestParam(value = "departmentId", defaultValue = "0") int departmentId) {
        return HrmApi.run(() -> service.salarySheetDepartment(departmentId));
    }

    @PostMapping(API + "/salary-sheet/show") @ResponseBody
    public ResponseEntity<?> salarySheetShow(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.salarySheetShow(f)); }

    @PostMapping(API + "/salary-sheet/slip-check") @ResponseBody
    public ResponseEntity<?> salarySheetSlipCheck(@RequestBody HrmReportsFilterDto f) { return HrmApi.run(() -> service.salarySlipCheck(f)); }
}
