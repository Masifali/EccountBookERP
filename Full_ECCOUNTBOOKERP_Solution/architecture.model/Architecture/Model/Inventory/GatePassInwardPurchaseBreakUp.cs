namespace Architecture.Model.Inventory;

public class GatePassInwardPurchaseBreakUp
{
	public decimal EBTotal { get; set; }

	public decimal EbWeight { get; set; }

	public decimal GrossWeight { get; set; }

	public decimal NetWeight { get; set; }

	public decimal Qty { get; set; }

	public decimal UOM { get; set; }

	public int Id { get; set; }

	public int InwardGatePassId { get; set; }
}
