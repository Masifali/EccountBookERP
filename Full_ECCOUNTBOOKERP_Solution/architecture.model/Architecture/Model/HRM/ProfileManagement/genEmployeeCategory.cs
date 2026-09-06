using System;

namespace Architecture.Model.HRM.ProfileManagement;

public class genEmployeeCategory
{
	public byte ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int EmployeeCategoryId { get; set; }

	public int CompanyId { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long OrginizationId { get; set; }

	public long UserLogId { get; set; }

	public string EmployeeCategoryName { get; set; }

	public string EmployeeCategoryPrefix { get; set; }
}
