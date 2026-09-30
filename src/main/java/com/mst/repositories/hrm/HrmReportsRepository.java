package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * DAL of the HRM report screens (Architecture.WinApp.HRM_Reports, screens 371-378). Every call is the
 * desktop BLL's own procedure with exactly the parameters it sends; a guarded parameter
 * ("if (obj.X != 0)", "if (!CheckDateTimeNull(...))") is passed as null here, and HrmProcRepository
 * leaves a null out of the EXEC as ADO.NET does. No SQL of our own.
 *
 *  Combos
 *   genDepartment.Getall            Sp_genDepartment_GetAllMethod        @OrganizationId @CompanyId @Activity 'ReadAll'
 *   genSection.Getall               Sp_genSection_GetAllMethod           idem
 *   genShift.Getall                 Sp_genShift_GetAllMethod             idem
 *   genDesignation.Getall           Sp_genDesignation_GetAllMethod       idem
 *   genLocation.Getall              Sp_genLocation_GetAllMethod          idem
 *   genPartyLocation.Getall         Sp_genPartyLocation_GetAllMethod     idem
 *   genEmployee.Getall(RP)          Sp_genEmployee_GetAllMethod          @OrganizationId @CompanyId [@DepartmentId] @Activity 'ReadAll'
 *   genEmployeeHistory.GetAllEmployeesActive  Sp_genEmployeeHistory_GetAllMethod  'ReadAllActiveEmployee'
 *                                   [@DepartmentId] [@SectionId] [@LocationId] [@ShiftId]
 *   genEmployeeHistory.GetAccountsFromEmployee  USP_GetAccountsFromEmployee  @OrganizationId @CompanyId @Activity 'Expense'
 *   clsGlobalVariables.ActiveYr     Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId
 *  Reports (BLL HRM_Reports / PayRollReports)
 *   EmployeeHistoryRpt              Sp_genEmployeeHistory_Rpt
 *   EmployeeMonthlyAttendenceRpt    Sp_genEmployeeMonthlyAttendence_rpt
 *   MonthlyAttendanceRegisterRpt    Sp_AttendanceRegister_Rpt
 *   genEmployeeAttendence           Sp_genEmployeeAttendence_rpt
 *   DailyAttendanceRpt              Sp_genDailyAttandance_rpt
 *   DailyLateandEarlyDeparture      Sp_hrmDailyLED_rpt
 *   genDutyRosterEmployeeWise       Sp_genDutyRosterEmployeeWise_rpt
 *   MonthlyAttendanceSummary        Sp_genEmployeeAttendanceSummery_rpt
 *   MonthlyAttendanceRegisterNew    Sp_MonthlyAttendanceRegister_Rpt
 *   GetEmployeePayrollSalary        Sp_GetEmployeePayrollSalary
 *   GetEmployeePostedSalary         Sp_GetEmployeePostedSalary
 *   EmployeeSalarySlip_Rpt          Sp_EmployeeSalarySlip_Rpt
 */
@Repository
public class HrmReportsRepository {

    private final HrmProcRepository db;

    public HrmReportsRepository(HrmProcRepository db) { this.db = db; }

    private static Map<String, Object> p(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    /** "if (obj.X != 0)": the value, or null (not sent). */
    private static Integer g(int v) { return v != 0 ? v : null; }

    private static Map<String, Object> tenancy(UserAccount u) {
        return p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    // ================================================================== combos

    public List<Map<String, Object>> departments(UserAccount u) {
        return db.rows("Sp_genDepartment_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    public List<Map<String, Object>> sections(UserAccount u) {
        return db.rows("Sp_genSection_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    public List<Map<String, Object>> shifts(UserAccount u) {
        return db.rows("Sp_genShift_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genDesignation.Getall: @Activity first, then the tenancy pair (order is the BLL's; names decide). */
    public List<Map<String, Object>> designations(UserAccount u) {
        return db.rows("Sp_genDesignation_GetAllMethod", "Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    public List<Map<String, Object>> locations(UserAccount u) {
        return db.rows("Sp_genLocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    public List<Map<String, Object>> partyLocations(UserAccount u) {
        return db.rows("Sp_genPartyLocation_GetAllMethod", "Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** genEmployee.Getall(ReportsParameters): the forms never set DepartmentId on it, so it is not sent. */
    public List<Map<String, Object>> employees(UserAccount u) {
        return db.rows("Sp_genEmployee_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genEmployeeHistory.GetAllEmployeesActive - @LocationId carries obj.PartyLocationId (BLL). */
    public List<Map<String, Object>> activeEmployees(UserAccount u, int departmentId, int sectionId, int partyLocationId, int shiftId) {
        Map<String, Object> m = tenancy(u);
        m.put("DepartmentId", g(departmentId));
        m.put("SectionId", g(sectionId));
        m.put("LocationId", g(partyLocationId));
        m.put("ShiftId", g(shiftId));
        m.put("Activity", "ReadAllActiveEmployee");
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", m);
    }

    public List<Map<String, Object>> debitAccounts(UserAccount u) {
        return db.rows("USP_GetAccountsFromEmployee", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "Expense");
    }

    public List<Map<String, Object>> financialYears(UserAccount u) {
        return db.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    // ================================================================== reports

    /**
     * HRM_Reports.EmployeeHistoryRpt: @OrganizationId @CompanyId [@ShiftId] [@SectionId] [@DepartmentId]
     * [@DesignationId = obj.PartyLocationId] [@EmployeeId] [@Active when ApprovedFilter != "All"].
     * (@BranchId: obj.BranchesId is never set by the form, so never sent.)
     */
    public List<Map<String, Object>> employeeHistory(UserAccount u, int shiftId, int sectionId, int departmentId, int designationId,
                                                     int employeeId, Boolean active) {
        Map<String, Object> m = tenancy(u);
        m.put("ShiftId", g(shiftId));
        m.put("SectionId", g(sectionId));
        m.put("DepartmentId", g(departmentId));
        m.put("DesignationId", g(designationId));
        m.put("EmployeeId", g(employeeId));
        m.put("Active", active);
        return db.rows("Sp_genEmployeeHistory_Rpt", m);
    }

    /** HRM_Reports.EmployeeMonthlyAttendenceRpt: @Org @Comp @Month @Year [@EmployeeId] [@DepartmentId] [@SectionId] [@PartyLocationId] [@ShiftId]. */
    public List<Map<String, Object>> employeeMonthlyAttendence(UserAccount u, int month, int year, int employeeId, int departmentId,
                                                               int sectionId, int partyLocationId, int shiftId) {
        Map<String, Object> m = tenancy(u);
        m.put("Month", month);
        m.put("Year", year);
        m.put("EmployeeId", g(employeeId));
        m.put("DepartmentId", g(departmentId));
        m.put("SectionId", g(sectionId));
        m.put("PartyLocationId", g(partyLocationId));
        m.put("ShiftId", g(shiftId));
        return db.rows("Sp_genEmployeeMonthlyAttendence_rpt", m);
    }

    /** HRM_Reports.MonthlyAttendanceRegisterRpt (1002): @Org @Comp @Month @Year [@DepartmentId] [@EmployeeId] [@ShiftId] [@SectionId] [@PartyLocationId]. */
    public List<Map<String, Object>> attendanceRegister(UserAccount u, int month, int year, int departmentId, int employeeId,
                                                        int shiftId, int sectionId, int partyLocationId) {
        Map<String, Object> m = tenancy(u);
        m.put("Month", month);
        m.put("Year", year);
        m.put("DepartmentId", g(departmentId));
        m.put("EmployeeId", g(employeeId));
        m.put("ShiftId", g(shiftId));
        m.put("SectionId", g(sectionId));
        m.put("PartyLocationId", g(partyLocationId));
        return db.rows("Sp_AttendanceRegister_Rpt", m);
    }

    /** HRM_Reports.genEmployeeAttendence: @Org @Comp [@EmployeeId] [@DepartmentId] [@FromDate] [@ToDate]. */
    public List<Map<String, Object>> employeeAttendence(UserAccount u, int employeeId, int departmentId, Timestamp from, Timestamp to) {
        Map<String, Object> m = tenancy(u);
        m.put("EmployeeId", g(employeeId));
        m.put("DepartmentId", g(departmentId));
        m.put("FromDate", from);
        m.put("ToDate", to);
        return db.rows("Sp_genEmployeeAttendence_rpt", m);
    }

    /**
     * HRM_Reports.DailyAttendanceRpt with ApprovedFilter "All" (so @IsMissing is not sent):
     * @Org @Comp [@DepartmentId] [@Date] [@EmployeeId]; Designation/Shift/Location/Category/Section are never set by the form.
     */
    public List<Map<String, Object>> dailyAttendance(UserAccount u, int departmentId, Timestamp date, int employeeId) {
        Map<String, Object> m = tenancy(u);
        m.put("DepartmentId", g(departmentId));
        m.put("Date", date);
        m.put("EmployeeId", g(employeeId));
        return db.rows("Sp_genDailyAttandance_rpt", m);
    }

    /** HRM_Reports.DailyLateandEarlyDeparture: @CompanyId [@PartyLocationId] [@Date] (the BLL sends no @OrganizationId). */
    public List<Map<String, Object>> dailyLateAndEarly(UserAccount u, int partyLocationId, Timestamp date) {
        return db.rows("Sp_hrmDailyLED_rpt", p("CompanyId", u.getCompanyId(), "PartyLocationId", g(partyLocationId), "Date", date));
    }

    /**
     * HRM_Reports.genDutyRosterEmployeeWise: [@EmployeeId] [@Date]. The BLL drops obj.CompanyId, but the
     * procedure INNER JOINs Company ON Id = @CompanyId, so without it the report is always empty; the
     * session's company is sent (newer-resolved-source rule, see HrmReportsService D2).
     */
    public List<Map<String, Object>> dutyRoster(UserAccount u, int employeeId, Timestamp date) {
        return db.rows("Sp_genDutyRosterEmployeeWise_rpt", p("CompanyId", u.getCompanyId(), "EmployeeId", g(employeeId), "Date", date));
    }

    /** HRM_Reports.MonthlyAttendanceSummary: @Org @Comp @Month @Year [@DepartmentId] [@EmployeeId] [@SectionId]. */
    public List<Map<String, Object>> monthlyAttendanceSummary(UserAccount u, int month, int year, int departmentId, int employeeId, int sectionId) {
        Map<String, Object> m = tenancy(u);
        m.put("Month", month);
        m.put("Year", year);
        m.put("DepartmentId", g(departmentId));
        m.put("EmployeeId", g(employeeId));
        m.put("SectionId", g(sectionId));
        return db.rows("Sp_genEmployeeAttendanceSummery_rpt", m);
    }

    /** HRM_Reports.MonthlyAttendanceRegisterNew (1005): @Org @Comp @Month @Year [@EmployeeId] [@DepartmentId]. */
    public List<Map<String, Object>> monthlyAttendanceRegisterNew(UserAccount u, int month, int year, int employeeId, int departmentId) {
        Map<String, Object> m = tenancy(u);
        m.put("Month", month);
        m.put("Year", year);
        m.put("EmployeeId", g(employeeId));
        m.put("DepartmentId", g(departmentId));
        return db.rows("Sp_MonthlyAttendanceRegister_Rpt", m);
    }

    /**
     * PayRollReports.GetEmployeePayrollSalary / GetEmployeePostedSalary: [@EmployeeId] [@DepartmentId]
     * [@SectionId] [@DebitAccountId] @Month (cmbMonth.Text) @Year @MonthValue @Org @Comp
     * (EmployeeCategoryId and the posted variant's @PayrollId (obj.Id) are never set by the form).
     */
    public List<Map<String, Object>> salary(UserAccount u, boolean posted, int employeeId, int departmentId, int sectionId,
                                            int debitAccountId, String monthName, int year, int monthValue) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("EmployeeId", g(employeeId));
        m.put("DepartmentId", g(departmentId));
        m.put("SectionId", g(sectionId));
        m.put("DebitAccountId", g(debitAccountId));
        m.put("Month", monthName);
        m.put("Year", year);
        m.put("MonthValue", monthValue);
        m.put("OrganizationId", u.getOrganizationId());
        m.put("CompanyId", u.getCompanyId());
        return db.rows(posted ? "Sp_GetEmployeePostedSalary" : "Sp_GetEmployeePayrollSalary", m);
    }

    /** PayRollReports.EmployeeSalarySlip_Rpt: @Org @Comp @Month (MonthValue) @Year [@EmployeeId] [@DepartmentId] [@DebitAccountId]. */
    public List<Map<String, Object>> salarySlip(UserAccount u, int monthValue, int year, int employeeId, int departmentId, int debitAccountId) {
        Map<String, Object> m = tenancy(u);
        m.put("Month", monthValue);
        m.put("Year", year);
        m.put("EmployeeId", g(employeeId));
        m.put("DepartmentId", g(departmentId));
        m.put("DebitAccountId", g(debitAccountId));
        return db.rows("Sp_EmployeeSalarySlip_Rpt", m);
    }
}
