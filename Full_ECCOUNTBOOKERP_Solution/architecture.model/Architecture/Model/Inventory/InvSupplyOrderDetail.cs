namespace Architecture.Model.Inventory;

public class InvSupplyOrderDetail
{
	public double EbWtTotal { get; set; }

	public double EbWtUnit { get; set; }

	public double GrossWeight { get; set; }

	public double Moisture { get; set; }

	public double NetWeight { get; set; }

	public double Qty { get; set; }

	public int Id { get; set; }

	public int InvSupplyOrderId { get; set; }

	public int ItemId { get; set; }

	public int OrderNo { get; set; }

	public int PackSizeId { get; set; }

	public int PoDetailId { get; set; }

	public int PoHeaderId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string CropYear { get; set; }

	public string RemarksDetail { get; set; }

	public virtual double PackSize { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string ItemName { get; set; }
}
