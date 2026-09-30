package com.mst.models.hrm.leave;

import com.mst.models.hrm.DesktopModel;

/**
 * Architecture.Model.HRM.LeaveManagement.CPLEmployeeLeaveDetail (model 0697) - the 4 non-virtual
 * properties Sp_CPLEmployeeLeaveDetail_Insert declares (checked: same 4). CPLDate / WeekDayName /
 * CPLQuota / Active are virtual - not parameters.
 */
public class CPLEmployeeLeaveDetail extends DesktopModel {
    public long CPLEmployeeLeaveId;
    public long CPLAttendanceId;
    public long EmployeeLeaveId;
    public int ActionTypeId;
}
