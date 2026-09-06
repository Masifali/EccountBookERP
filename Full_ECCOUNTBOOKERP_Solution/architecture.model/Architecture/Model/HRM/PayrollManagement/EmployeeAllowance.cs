using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.PayrollManagement;

public class EmployeeAllowance
{
	public byte ActionTypeId { get; set; }

	public bool IsApproved { get; set; }

	public bool IsForPayroll { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime AppliedOn { get; set; }

	public DateTime ApprovedOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public decimal TotalAmount { get; set; }

	public long AlteredById { get; set; }

	public long ApprovedById { get; set; }

	public long CompanyId { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeAllowanceId { get; set; }

	public long EmployeeBenefitId { get; set; }

	public long EmployeeId { get; set; }

	public long InwardId { get; set; }

	public long LocationId { get; set; }

	public long OrganizationId { get; set; }

	public long UserLogId { get; set; }

	public string Remarks { get; set; }

	public virtual string EmployeeName { get; set; }

	public virtual string EmployeeBenefit { get; set; }

	public virtual List<EmployeeAllowance> EmployeeAllowanceList { get; set; }
}
