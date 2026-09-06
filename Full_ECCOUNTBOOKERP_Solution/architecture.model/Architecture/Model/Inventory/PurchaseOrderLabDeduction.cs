namespace Architecture.Model.Inventory;

public class PurchaseOrderLabDeduction
{
	public decimal DeductionValue { get; set; }

	public decimal RangeFrom { get; set; }

	public decimal RangeTo { get; set; }

	public decimal StandardValue { get; set; }

	public decimal WeightKgs { get; set; }

	public int AnalysisParameterId { get; set; }

	public int Id { get; set; }

	public int InvLabAnalysisStandardDeductionPolicyHeaderId { get; set; }

	public int ItemId { get; set; }

	public int PurchaseOrderDetailId { get; set; }

	public int PurchaseOrderId { get; set; }

	public string DeductFrom { get; set; }

	public virtual string AnalysisItem { get; set; }

	public virtual string ItemName { get; set; }
}
