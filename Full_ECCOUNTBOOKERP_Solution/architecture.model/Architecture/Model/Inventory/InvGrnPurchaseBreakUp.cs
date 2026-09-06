namespace Architecture.Model.Inventory;

public class InvGrnPurchaseBreakUp
{
	public decimal EBTotal { get; set; }

	public decimal NetPackSize { get; set; }

	public decimal SupplierShortWeight { get; set; }

	public decimal SupplierRcvdWeight { get; set; }

	public decimal ScaleShortage { get; set; }

	public decimal BillWeight { get; set; }

	public decimal EbWeight { get; set; }

	public decimal GrossWeight { get; set; }

	public decimal NetWeight { get; set; }

	public decimal Qty { get; set; }

	public decimal UOM { get; set; }

	public int Id { get; set; }

	public int InvGrnId { get; set; }

	public int InwardBreakupId { get; set; }

	public int InwardGatePassId { get; set; }
}
