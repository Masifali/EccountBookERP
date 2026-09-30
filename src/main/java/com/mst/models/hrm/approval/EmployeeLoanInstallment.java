package com.mst.models.hrm.approval;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.ApprovalManagement.EmployeeLoanInstallment (model 0737) - every property as
 * SetProc sends it to hrm.Sp_EmployeeLoanInstallment_Insert (16 params, all declared).
 */
public class EmployeeLoanInstallment extends DesktopModel {
    public int ActionTypeId;          // C# byte
    public int InstallmentNo;
    public boolean IsPaid;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public BigDecimal Amount = BigDecimal.ZERO;
    public int CompanyId;
    public int EmployeeLoanId;
    public int OrganizationId;
    public int Year;
    public long AlteredById;
    public long CreatedById;
    public long EmployeeLoanInstallmentId;
    public long PayrollId;
    public long UserLogId;
    public String Month;
}
