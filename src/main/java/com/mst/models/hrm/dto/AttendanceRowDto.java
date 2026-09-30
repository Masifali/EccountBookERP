package com.mst.models.hrm.dto;

/**
 * One grid row (GridEXRow) of the attendance screens as the page sends it back for Save / Update:
 * the cells btnsave_Click / btnupdate_Click read (ManualAttendanceId, EmployeeId, LocationId,
 * DepartmentId, ShiftId, DutyDate, InTime, OutTime, InEntryMode, OutEntryMode, Status, Description,
 * CPLQuota, IsAbsent). inTime / outTime are "HH:mm" (blank = empty cell); dutyDate is the
 * procedure's "dd-MMM-yyyy" text.
 */
public class AttendanceRowDto {
    public int manualAttendanceId;
    public int employeeId;
    public int locationId;
    public int departmentId;
    public int shiftId;
    public String dutyDate;
    public String inTime;
    public String outTime;
    public String inEntryMode;
    public String outEntryMode;
    public String status;
    public String description;
    public String cplQuota;
    public boolean isAbsent;
}
