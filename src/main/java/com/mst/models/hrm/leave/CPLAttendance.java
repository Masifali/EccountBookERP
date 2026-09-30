package com.mst.models.hrm.leave;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.AttendanceManagement.CPLAttendance (model 0731) - saved by the two CPL forms of
 * the leave group (frmEmployeeCPLLeaveOpening ReqTypeId 1, frmEmployeeCPLAttendance ReqTypeId 2).
 * The 15 non-virtual properties are exactly the 15 parameters of Sp_CPLAttendance_Insert.
 * EmployeeNo / EmployeeName / CPLAttendancesList are virtual - not parameters.
 */
public class CPLAttendance extends DesktopModel {
    public int ActionTypeId;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime CPLDate;
    public LocalDateTime InTime;
    public LocalDateTime OutTime;
    public int AlteredById;
    public int CompanyId;
    public int CreatedById;
    public int OrganizationId;
    public long CPLAttendanceId;
    public long EmployeeId;
    public int ReqTypeId;
    public long EmployeeAttendanceId;
    public BigDecimal CPLQuota = BigDecimal.ZERO;
}
