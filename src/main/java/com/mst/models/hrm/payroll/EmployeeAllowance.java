package com.mst.models.hrm.payroll;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.PayrollManagement.EmployeeAllowance (model 0685) - the 20 non-virtual
 * properties hrm.Sp_EmployeeAllowance_Insert declares (checked). EmployeeName / EmployeeBenefit /
 * EmployeeAllowanceList are virtual and not sent.
 */
public class EmployeeAllowance extends DesktopModel {
    public int ActionTypeId;               // byte
    public boolean IsApproved;
    public boolean IsForPayroll;
    public LocalDateTime AlteredOn;
    public LocalDateTime AppliedOn;
    public LocalDateTime ApprovedOn;
    public LocalDateTime CreatedOn;
    public BigDecimal TotalAmount = BigDecimal.ZERO;
    public long AlteredById;
    public long ApprovedById;
    public long CompanyId;
    public long CreatedById;
    public long EmployeeAllowanceId;
    public long EmployeeBenefitId;
    public long EmployeeId;
    public long InwardId;
    public long LocationId;
    public long OrganizationId;
    public long UserLogId;
    public String Remarks;
}
