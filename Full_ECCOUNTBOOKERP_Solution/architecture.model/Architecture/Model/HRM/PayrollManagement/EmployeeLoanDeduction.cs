using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.PayrollManagement;

public class EmployeeLoanDeduction
{
	public byte ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public decimal InstallmentAmount { get; set; }

	public decimal TotalLoanAmount { get; set; }

	public int NoOfInstallment { get; set; }

	public int PayrollMonth { get; set; }

	public int PayrollYear { get; set; }

	public long AlteredById { get; set; }

	public long CompanyId { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeId { get; set; }

	public long EmployeeLoanDeductionId { get; set; }

	public long LocationId { get; set; }

	public long OrganizationId { get; set; }

	public long UserLogId { get; set; }

	public virtual string EmployeeName { get; set; }

	public virtual int RemaningNoOfInstallment { get; set; }

	public virtual List<EmployeeLoanDeduction> LoanDeductionsList { get; set; }
}
