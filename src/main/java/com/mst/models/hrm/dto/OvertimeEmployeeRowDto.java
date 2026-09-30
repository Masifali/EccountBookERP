package com.mst.models.hrm.dto;

/**
 * One entry of frmEmployeeOverTime.detailList (addDataToDetailList): the checked grdEmployeeDetail
 * row's keys and the AddLess cell the user typed. The hours / rate columns are read back from
 * [hrm].[usp_GetEmployeeAttendanceMonthWise] on the server, so only the editable cell comes from the page.
 */
public class OvertimeEmployeeRowDto {
    public long overTimeRequestDetailId;   // OvertimeDetailId
    public long employeeAttendanceId;      // EmpAttendanceId
    public long employeeId;
    public int month;                      // OvertimeMonth
    public int year;                       // OvertimeYear
    public double addLess;                 // AddLess
}
