using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.ApprovalManagement;

public class EmployeeLoanInstallment
{
	public byte ActionTypeId { get; set; }

	public int InstallmentNo { get; set; }

	public bool IsPaid { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public decimal Amount { get; set; }

	public int CompanyId { get; set; }

	public int EmployeeLoanId { get; set; }

	public int OrganizationId { get; set; }

	public int Year { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeLoanInstallmentId { get; set; }

	public long PayrollId { get; set; }

	public long UserLogId { get; set; }

	public string Month { get; set; }

	public virtual List<EmployeeLoanInstallment> EmployeeLoanInstallmentslist { get; set; }
}
