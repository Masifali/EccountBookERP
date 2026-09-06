namespace Architecture.Model.CommissionAgent;

public class saleOrderEmptyBagDetail
{
	public decimal Rate { get; set; }

	public decimal weightCutKg { get; set; }

	public int PackingTypeId { get; set; }

	public int purchaseOrderEmptyBagDetailId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int saleOrderEmptyBagDetailId { get; set; }

	public int saleOrderMasterId { get; set; }

	public int sortNo { get; set; }

	public string remarks { get; set; }

	public virtual string PackingType { get; set; }
}
