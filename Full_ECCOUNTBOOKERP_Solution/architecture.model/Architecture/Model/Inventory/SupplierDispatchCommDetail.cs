namespace Architecture.Model.Inventory;

public class SupplierDispatchCommDetail
{
	public double CommAmount { get; set; }

	public double CommRate { get; set; }

	public string CommType { get; set; }

	public double RateUom { get; set; }

	public int CommissionAgentGLId { get; set; }

	public int CommissionAgentId { get; set; }

	public int TypeId { get; set; }

	public int Id { get; set; }

	public int SupplierDispatchId { get; set; }

	public string Remarks { get; set; }

	public virtual string CommissionAgent { get; set; }

	public virtual string CommissionAgentGLAccount { get; set; }
}
