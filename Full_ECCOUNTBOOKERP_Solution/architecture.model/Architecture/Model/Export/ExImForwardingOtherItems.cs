namespace Architecture.Model.Export;

public class ExImForwardingOtherItems
{
	public decimal oItemAmount { get; set; }

	public decimal oItemQty { get; set; }

	public decimal oItemWeightKgs { get; set; }

	public double oItemRate { get; set; }

	public int ExImForwardingId { get; set; }

	public int Id { get; set; }

	public int otherItemId { get; set; }

	public int InvoiceOtherItemsDetailId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int WareHouseFromId { get; set; }

	public int WarehouseToId { get; set; }

	public string ContainerNo { get; set; }

	public string OtherItemRemarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string WareHouseFrom { get; set; }

	public virtual string WareHouseTo { get; set; }
}
