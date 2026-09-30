package com.mst.models.hrm.payroll;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.PayrollManagement.EmployeeLateAdjustment (model 0687) - the 24 non-virtual
 * properties hrm.Sp_EmployeeLateAdjustment_Insert / _Update declare (checked). EmployeeName /
 * LocationName are virtual.
 */
public class EmployeeLateAdjustment extends DesktopModel {
    public long ActionTypeId;
    public boolean IsDeduction;
    public boolean IsLeaveAdjust;
    public long TotalLates;
    public LocalDateTime AlteredOn;
    public LocalDateTime ApprovedOn;
    public LocalDateTime CreatedOn;
    public BigDecimal DeductedDays = BigDecimal.ZERO;
    public BigDecimal DeductionRate = BigDecimal.ZERO;
    public BigDecimal LateDeductionAmount = BigDecimal.ZERO;
    public BigDecimal TotalLateDays = BigDecimal.ZERO;
    public int DocMovementId;
    public int LAMonth;
    public int LAYear;
    public long AlteredById;
    public long ApprovedById;
    public long CompanyId;
    public long CreatedById;
    public long EmployeeId;
    public long EmployeeLateAdjustmentId;
    public long EmployeeLeaveId;
    public long LocationId;
    public long OrganizationId;
    public long UserLogId;
}
