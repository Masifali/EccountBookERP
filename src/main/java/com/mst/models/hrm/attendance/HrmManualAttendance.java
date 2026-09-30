package com.mst.models.hrm.attendance;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.AttendanceManagement.hrmManualAttendance (model 0735) - every non-virtual
 * property, as GenericProvider.SetProc sends it to Sp_hrmManualAttendance_Insert. Checked against
 * the procedure's 21 parameters (proc.py): all 21 properties are declared there, nothing is extra.
 *
 * IsPresent is a string the forms never set, so it stays null and is omitted (ADO.NET AddWithValue(null)).
 * listHrmManualAttendances is the C# virtual child list - transient, never a parameter.
 */
public class HrmManualAttendance extends DesktopModel {
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime DutyDate;
    public LocalDateTime InDateTime;
    public LocalDateTime OutDateTime;
    public BigDecimal ManualCPLQuota = BigDecimal.ZERO;
    public int ActionTypeId;
    public int AlteredById;
    public int BranchId;
    public int CompanyId;
    public int CreatedById;
    public int DepartmentId;
    public int EmployeeId;
    public int LocationId;
    public int ManualAttendanceId;
    public int OrganizationId;
    public int ShiftId;
    public int UserLogId;
    public String AttendanceStatus;
    public String Description;
    public String IsPresent;
}
