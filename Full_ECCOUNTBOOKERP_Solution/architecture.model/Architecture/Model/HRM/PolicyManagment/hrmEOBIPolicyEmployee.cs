namespace Architecture.Model.HRM.PolicyManagment;

public class hrmEOBIPolicyEmployee
{
	public decimal CompanyShare { get; set; }

	public decimal EmployeeShare { get; set; }

	public long EmployeeId { get; set; }

	public long hrmEOBIPolicyEmployeeId { get; set; }

	public long hrmEOBIPolicyId { get; set; }

	public virtual string EmployeeName { get; set; }
}
