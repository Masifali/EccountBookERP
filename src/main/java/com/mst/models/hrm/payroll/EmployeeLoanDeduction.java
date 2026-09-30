package com.mst.models.hrm.payroll;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.PayrollManagement.EmployeeLoanDeduction (model 0688) - the 16 non-virtual
 * properties hrm.Sp_EmployeeLoanDeduction_Insert declares (checked). EmployeeName,
 * RemaningNoOfInstallment and LoanDeductionsList are virtual.
 */
public class EmployeeLoanDeduction extends DesktopModel {
    public int ActionTypeId;               // byte
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public BigDecimal InstallmentAmount = BigDecimal.ZERO;
    public BigDecimal TotalLoanAmount = BigDecimal.ZERO;
    public int NoOfInstallment;
    public int PayrollMonth;
    public int PayrollYear;
    public long AlteredById;
    public long CompanyId;
    public long CreatedById;
    public long EmployeeId;
    public long EmployeeLoanDeductionId;
    public long LocationId;
    public long OrganizationId;
    public long UserLogId;
}
