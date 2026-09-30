package com.mst.models.hrm.loan;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.LoanManagement.EmployeeLoan (model 0696) - every non-virtual property, as
 * GenericProvider.SetProc sends it to hrm.Sp_EmployeeLoan_Insert / hrm.Sp_EmployeeLoan_Update
 * (23 params; every field below is declared by both procedures).
 * The virtual EmployeeLoanInstallmentslist / VoucherHeadInvoices are not parameters (transient).
 */
public class EmployeeLoan extends DesktopModel {
    public boolean IsApproved;
    public LocalDateTime AlteredOn;
    public LocalDateTime AppliedOn;
    public LocalDateTime ApprovedOn;
    public LocalDateTime CreatedOn;
    public BigDecimal ApprovedAmount = BigDecimal.ZERO;
    public BigDecimal LoanAmount = BigDecimal.ZERO;
    public int DocumentTypeId;
    public int ActionTypeId;
    public int BranchId;
    public int CompanyId;
    public int FinancialYearId;
    public int EmployeeLoanId;
    public int NoOfInstallment;
    public int OrganizationId;
    public int ProjectId;
    public long AlteredById;
    public long ApprovedById;
    public long CreatedById;
    public long EmployeeId;
    public long LocationId;
    public long UserLogId;
    public String Reason;

    /** virtual List&lt;EmployeeLoanInstallment&gt; EmployeeLoanInstallmentslist (saved by the DAL after the header). */
    public transient java.util.List<com.mst.models.hrm.approval.EmployeeLoanInstallment> EmployeeLoanInstallmentslist = new java.util.ArrayList<>();
    /** virtual VoucherHead VoucherHeadInvoices (BLL EmployeeLoan.MakeVoucher, only when IsApproved on an update). */
    public transient com.mst.models.hrm.approval.VoucherHead VoucherHeadInvoices;
}
