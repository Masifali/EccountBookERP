package com.mst.models.hrm.leave;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.LeaveManagement.genEmployeeLeaveDetail (model 0701) - the 11 non-virtual
 * properties sent to Sp_genEmployeeLeaveDetail_Insert. The proc also declares @IsRejected and @Reason
 * (both "= null"); the desktop model has no such properties, so they are not sent (proc default NULL).
 * CPLDate / WeekDayName are virtual - not parameters.
 */
public class GenEmployeeLeaveDetail extends DesktopModel {
    public int ActionTypeId;
    public boolean IsApproved;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime LeaveDate;
    public long AlteredById;
    public long CreatedById;
    public long EmployeeLeaveDetailId;
    public long EmployeeLeaveId;
    public long UserLogId;
    public int CPLAttendanceId;
}
