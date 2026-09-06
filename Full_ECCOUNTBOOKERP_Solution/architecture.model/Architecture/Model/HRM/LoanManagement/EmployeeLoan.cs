using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.HRM.ApprovalManagement;

namespace Architecture.Model.HRM.LoanManagement;

public class EmployeeLoan
{
	public bool IsApproved { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime AppliedOn { get; set; }

	public DateTime ApprovedOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public decimal ApprovedAmount { get; set; }

	public decimal LoanAmount { get; set; }

	public int DocumentTypeId { get; set; }

	public int ActionTypeId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int FinancialYearId { get; set; }

	public int EmployeeLoanId { get; set; }

	public int NoOfInstallment { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public long AlteredById { get; set; }

	public long ApprovedById { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeId { get; set; }

	public long LocationId { get; set; }

	public long UserLogId { get; set; }

	public string Reason { get; set; }

	public virtual List<EmployeeLoanInstallment> EmployeeLoanInstallmentslist { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }
}
