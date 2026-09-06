namespace Architecture.Model.POS;

public class PosSaleInvoiceDetailXGetYFree
{
	public double SalesXItemQty { get; set; }

	public double XGroupTargetQty { get; set; }

	public double XItemMinQty { get; set; }

	public double YItemFactorQty { get; set; }

	public double YItemQty { get; set; }

	public int Id { get; set; }

	public int PolicyId { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public int XGroupId { get; set; }

	public int XItemId { get; set; }

	public int YGroupId { get; set; }

	public int YItemId { get; set; }

	public int WarehouseId { get; set; }

	public int? LineId { get; set; }

	public virtual string ItemName { get; set; }
}
