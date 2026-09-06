namespace Architecture.Model.HRM.PolicyManagment;

public class hrmPFPolicyEmployee
{
	public double PFCompanyAmount { get; set; }

	public double PFEmployeeAmount { get; set; }

	public double PFTotalAmount { get; set; }

	public int EmployeeId { get; set; }

	public int PFPolicyEmployeeId { get; set; }

	public int PFPolicyId { get; set; }

	public int PFPolicySlabId { get; set; }

	public int PFPolicySlabLineId { get; set; }

	public virtual string EmployeeName { get; set; }

	public virtual decimal TotalSalary { get; set; }
}
