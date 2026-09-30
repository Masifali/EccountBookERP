package com.mst.models.hrm.payroll;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.PayrollManagement.EmployeeShortAdjustment (model 0689) - the 23 non-virtual
 * properties hrm.Sp_EmployeeShortAdjustment_Insert / _Update declare (checked). EmployeeName is virtual.
 */
public class EmployeeShortAdjustment extends DesktopModel {
    public boolean IsDeduction;
    public boolean IsLeaveAdjust;
    public LocalDateTime AlteredOn;
    public LocalDateTime ApprovedOn;
    public LocalDateTime CreatedOn;
    public BigDecimal DeductionRate = BigDecimal.ZERO;
    public BigDecimal ShortDeductionAmount = BigDecimal.ZERO;
    public int ActionTypeId;
    public int DeductedShortHours;
    public int EmployeeLeaveId;
    public int SAMonth;
    public int SAYear;
    public int TotalShortHours;
    public int TotalShortMinutes;
    public long AlteredById;
    public long ApprovedById;
    public long CompanyId;
    public long CreatedById;
    public long EmployeeId;
    public long EmployeeShortAdjustmentId;
    public long LocationId;
    public long OrganizationId;
    public long UserLogId;
}
