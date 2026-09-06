using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class hrmEmployeeBankAccount
{
	public bool IsPayrollAccount { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int ActionTypeId { get; set; }

	public int AlteredById { get; set; }

	public int BankAccountId { get; set; }

	public int BankProfileId { get; set; }

	public int BankRequisitionId { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int EmployeeBankAccountId { get; set; }

	public int EmployeeBankAccountLineId { get; set; }

	public int EmployeeId { get; set; }

	public int OrganizationId { get; set; }

	public int UserLogId { get; set; }

	public string AccountNo { get; set; }

	public string AccountTitle { get; set; }

	public virtual string BankName { get; set; }

	public virtual string EmployeeName { get; set; }
}
