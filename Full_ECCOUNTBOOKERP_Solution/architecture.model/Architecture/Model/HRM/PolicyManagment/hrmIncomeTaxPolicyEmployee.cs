namespace Architecture.Model.HRM.PolicyManagment;

public class hrmIncomeTaxPolicyEmployee
{
	public decimal Amount { get; set; }

	public decimal Percentage { get; set; }

	public long EmployeeId { get; set; }

	public long hrmIncomeTaxPolicyEmployeeId { get; set; }

	public long hrmIncomeTaxPolicyId { get; set; }

	public long hrmIncomeTaxPolicySlabId { get; set; }

	public virtual string EmployeeName { get; set; }
}
