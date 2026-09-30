package com.mst.models.hrm.dto;

/**
 * The filter box of the attendance screens (658 frmDailyAttendance, 659 hrmManualAttendance) - the
 * ReportsParameters the forms fill for genEmployeeHistory.GetAllEmployeesActive,
 * hrmManualAttendance.genEmployeeAttendanceLoad and HRM_Reports.DailyAttendanceRpt.
 * 0 / blank = the combo has no value (Conversion.ToInt(cmb.Value) = 0), which the BLL then omits.
 *
 *   date      txtDate (658)                  month / year  cmbMonth / cmbYear (659)
 *   isRestDay ChkRestDay (658) / chkIsRestDay (659)
 */
public class AttendanceFilterDto {
    public String date;
    public int month;
    public int year;
    public int shiftId;
    public int sectionId;
    public int departmentId;
    public int designationId;
    public int locationId;
    public int employeeGroupId;
    public int employeeCategoryId;
    public int employeeId;
    public boolean isRestDay;
}
