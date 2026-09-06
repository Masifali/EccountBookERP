namespace Architecture.Model.HRM.PolicyManagment;

public class hrmIncomeTaxPolicySlab
{
	public double FlateTaxAmount { get; set; }

	public double FromSalary { get; set; }

	public double TaxPercent { get; set; }

	public double ToSalary { get; set; }

	public int IncomeTaxPolicyId { get; set; }

	public int IncomeTaxPolicySlabId { get; set; }

	public int IncomeTaxPolicySlabLineId { get; set; }
}
