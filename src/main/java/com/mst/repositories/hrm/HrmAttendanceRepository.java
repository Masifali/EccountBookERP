package com.mst.repositories.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.attendance.HrmManualAttendance;
import com.mst.models.hrm.dto.AttendanceFilterDto;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * DAL of the HRM "Attendance Management" screens (AppModules 2022) - Architecture.DAL/BLL
 * HRM.AttendanceManagement.hrmManualAttendance and the combo BLLs the two forms call.
 * Every call is the desktop DAL's own procedure with the parameters its BLL sends; a BLL
 * "if (x != 0)" guard is reproduced by passing null (DesktopProc omits it).
 *
 *   658 Daily Attendance   frmDailyAttendance.cs
 *   659 Manual Attendance  hrmManualAttendance.cs
 */
@Repository
public class HrmAttendanceRepository {

    private final HrmProcRepository db;

    public HrmAttendanceRepository(HrmProcRepository db) { this.db = db; }

    private static Integer nz(int v) { return v == 0 ? null : v; }

    // ------------------------------------------------------------------ filter combos

    /** genShift.Getall: Sp_genShift_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> shifts(UserAccount u) {
        return db.rows("Sp_genShift_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genDepartment.Getall: Sp_genDepartment_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> departments(UserAccount u) {
        return db.rows("Sp_genDepartment_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genSection.Getall: Sp_genSection_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> sections(UserAccount u) {
        return db.rows("Sp_genSection_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genLocation.Getall: Sp_genLocation_GetAllMethod @OrganizationId, @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> locations(UserAccount u) {
        return db.rows("Sp_genLocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genDesignation.Getall: Sp_genDesignation_GetAllMethod @Activity 'ReadAll', @OrganizationId, @CompanyId. */
    public List<Map<String, Object>> designations(UserAccount u) {
        return db.rows("Sp_genDesignation_GetAllMethod", "Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** genEmployeeGroup.Getall: Sp_genEmployeeGroup_GetAllMethod @OrganizationId (OrginizationId), @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> employeeGroups(UserAccount u) {
        return db.rows("Sp_genEmployeeGroup_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** genEmployeeCategory.Getall: Sp_genEmployeeCategory_GetAllMethod @OrganizationId (OrginizationId), @CompanyId, @Activity 'ReadAll'. */
    public List<Map<String, Object>> employeeCategories(UserAccount u) {
        return db.rows("Sp_genEmployeeCategory_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /**
     * genEmployeeHistory.GetAllEmployeesActive (EmployeeName()): Sp_genEmployeeHistory_GetAllMethod
     * @OrganizationId, @CompanyId, [@DepartmentId], [@DesignationId], [@SectionId], [@EmployeeGroupId],
     * [@LocationId = PartyLocationId], [@ShiftId], @Activity 'ReadAllActiveEmployee'.
     * The forms also fill EmployeeCategoryId, but this BLL method never sends it (nor @StoreId /
     * @DepartmentIds, which the forms leave unset) - reproduced.
     */
    public List<Map<String, Object>> employeesActive(UserAccount u, AttendanceFilterDto f) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DepartmentId", nz(f.departmentId));
        p.put("DesignationId", nz(f.designationId));
        p.put("SectionId", nz(f.sectionId));
        p.put("EmployeeGroupId", nz(f.employeeGroupId));
        p.put("LocationId", nz(f.locationId));
        p.put("ShiftId", nz(f.shiftId));
        p.put("Activity", "ReadAllActiveEmployee");
        return db.rows("Sp_genEmployeeHistory_GetAllMethod", p);
    }

    /**
     * CommonServices.GetYears: ActiveYr.Start_Period.Year .. DateTime.Now.Year. The active year is read
     * with the procedure the rest of the Java app uses for clsGlobalVariables.ActiveYr
     * (Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId), the row whose Id is the session's year.
     */
    public List<Map<String, Object>> activeYears(UserAccount u) {
        return db.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    // ------------------------------------------------------------------ grid

    /**
     * hrmManualAttendance.genEmployeeAttendanceLoad: Sp_genEmployeeAttendanceWithActivities
     * @OrganizationId, @CompanyId, then only the set filters in the BLL's order - @ShiftId, @DutyDate,
     * @EmployeeId, @DepartmentId, @DesignationId, @SectionId, @LocationId (PartyLocationId),
     * @EmployeeCategoryId, @EmployeeGroupId, @Month (GetMonth), @Year (GetYear), and @IsRestDay only
     * when ApprovedFilter != "All" (isRestDay null = not sent).
     */
    public List<Map<String, Object>> attendanceLoad(UserAccount u, AttendanceFilterDto f, LocalDateTime dutyDate, Boolean isRestDay) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("ShiftId", nz(f.shiftId));
        p.put("DutyDate", dutyDate);
        p.put("EmployeeId", nz(f.employeeId));
        p.put("DepartmentId", nz(f.departmentId));
        p.put("DesignationId", nz(f.designationId));
        p.put("SectionId", nz(f.sectionId));
        p.put("LocationId", nz(f.locationId));
        p.put("EmployeeCategoryId", nz(f.employeeCategoryId));
        p.put("EmployeeGroupId", nz(f.employeeGroupId));
        p.put("Month", nz(f.month));
        p.put("Year", nz(f.year));
        p.put("IsRestDay", isRestDay);
        return db.rows("Sp_genEmployeeAttendanceWithActivities", p);
    }

    /**
     * DAL hrmManualAttendance.SetData(obj, "Sp_hrmManualAttendance_Insert"): one SqlTransaction, every
     * row of listHrmManualAttendances through SetProc (result = the last row's Convert.ToInt32(ExecuteScalar)),
     * Commit; THEN, outside the transaction, CommonProvider.ExecuteProcedure("[dbo].[Sp_SysUpdateAttendanceFromPull]")
     * and ("[dbo].[Sp_SysUpdateAttendanceStatus]") with @OrganizationId, @CompanyId - also when the list is empty.
     */
    public int saveManualAttendance(int organizationId, int companyId, List<HrmManualAttendance> rows) {
        int result = db.tx(() -> {
            int r = 0;
            for (HrmManualAttendance m : rows) r = db.set("Sp_hrmManualAttendance_Insert", m);
            return r;
        });
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", organizationId);
        p.put("CompanyId", companyId);
        db.scalar("[dbo].[Sp_SysUpdateAttendanceFromPull]", p);
        db.scalar("[dbo].[Sp_SysUpdateAttendanceStatus]", p);
        return result;
    }

    // ------------------------------------------------------------------ print

    /**
     * HRM_Reports.DailyAttendanceRpt as frmDailyAttendance.btnPrint_Click calls it (ApprovedFilter "All",
     * so @IsMissing is not sent): Sp_genDailyAttandance_rpt @OrganizationId, @CompanyId, [@DepartmentId],
     * [@Date], [@EmployeeId], [@DesignationId], [@ShiftId], [@LocationId], [@EmployeeCategoryId], [@SectionId].
     * Used only for the form's "Not Record Found For Display" check before the viewer opens.
     */
    public List<Map<String, Object>> dailyAttendanceRpt(UserAccount u, AttendanceFilterDto f, LocalDateTime date) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DepartmentId", nz(f.departmentId));
        p.put("Date", date);
        p.put("EmployeeId", nz(f.employeeId));
        p.put("DesignationId", nz(f.designationId));
        p.put("ShiftId", nz(f.shiftId));
        p.put("LocationId", nz(f.locationId));
        p.put("EmployeeCategoryId", nz(f.employeeCategoryId));
        p.put("SectionId", nz(f.sectionId));
        return db.rows("Sp_genDailyAttandance_rpt", p);
    }
}
