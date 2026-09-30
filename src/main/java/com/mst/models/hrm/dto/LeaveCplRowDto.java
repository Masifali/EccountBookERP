package com.mst.models.hrm.dto;

import java.math.BigDecimal;

/**
 * A CPLAttendance grid row of 662/463 (frmEmployeeCPLLeaveOpening: Id, EmployeeId, CPLDate, InTime,
 * OutTime, CPLQuota) or 461 (frmEmployeeCPLAttendance: CPLAttendanceId, EmployeeAttendanceId, EmployeeId,
 * DutyDate, InDateTime, OutDateTime, CPLQuota). Dates yyyy-MM-dd, times HH:mm or a full ISO date-time.
 */
public class LeaveCplRowDto {
    public long id;
    public long employeeId;
    public long employeeAttendanceId;
    public String cplDate;
    public String inTime;
    public String outTime;
    public BigDecimal cplQuota;
}
