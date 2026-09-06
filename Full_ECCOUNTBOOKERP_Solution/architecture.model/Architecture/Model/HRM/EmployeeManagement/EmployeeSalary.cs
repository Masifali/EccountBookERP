using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class EmployeeSalary
{
	public byte ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public decimal Amount { get; set; }

	public decimal SalaryTypePercent { get; set; }

	public int SalaryTypeProfileId { get; set; }

	public long AlteredById { get; set; }

	public long CompanyId { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeHistoryId { get; set; }

	public long EmployeeSalaryId { get; set; }

	public long OrganizationId { get; set; }

	public long UserLogId { get; set; }

	public long EmployeeId { get; set; }

	public virtual string EmployeeName { get; set; }

	public virtual string SalaryType { get; set; }
}
