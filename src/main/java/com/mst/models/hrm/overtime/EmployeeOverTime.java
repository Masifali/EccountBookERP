package com.mst.models.hrm.overtime;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.OverTimeManagement.EmployeeOverTime (model 0692) - the 25 non-virtual
 * properties SetProc sends to [hrm].[Sp_EmployeeOverTime_Insert] (25 parameters, checked:
 * employeeovertimeid locationid overtimemonth overtimeyear employeeid overtimerequestdetailid
 * employeeattendanceid othours shorthours netothours otrate isapproved approvedhours approvedamount
 * approvedbyid approvedon createdbyid createdon alteredbyid alteredon actiontypeid userlogid
 * organizationid companyid addlessothours). The virtual EmployeeOverTimeList is not a parameter.
 */
public class EmployeeOverTime extends DesktopModel {
    public int ActionTypeId;              // byte
    public boolean IsApproved;
    public LocalDateTime AlteredOn;
    public LocalDateTime ApprovedOn;
    public LocalDateTime CreatedOn;
    public BigDecimal ApprovedAmount = BigDecimal.ZERO;   // never set by the form -> 0
    public BigDecimal ApprovedHours = BigDecimal.ZERO;
    public BigDecimal NetOTHours = BigDecimal.ZERO;
    public BigDecimal OTHours = BigDecimal.ZERO;
    public BigDecimal OTRate = BigDecimal.ZERO;
    public BigDecimal ShortHours = BigDecimal.ZERO;
    public BigDecimal AddLessOTHours = BigDecimal.ZERO;
    public int OverTimeMonth;
    public int OverTimeYear;
    public long AlteredById;
    public long ApprovedById;
    public long CompanyId;
    public long CreatedById;
    public long EmployeeAttendanceId;
    public long EmployeeId;
    public long EmployeeOverTimeId;
    public long LocationId;               // never set by the form -> 0
    public long OrganizationId;
    public long OverTimeRequestDetailId;
    public long UserLogId;                // never set by the form -> 0
}
