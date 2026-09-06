namespace Architecture.Model.Inventory;

public class SaleOrderExtraItemsDetail
{
	public decimal ItemQty { get; set; }

	public decimal NetWeight { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int MasterItemId { get; set; }

	public int MasterPackUomId { get; set; }

	public int PackingTypeId { get; set; }

	public int PackUomId { get; set; }

	public int SaleOrderId { get; set; }

	public string ItemDiscription { get; set; }

	public string Remarks { get; set; }

	public virtual string MasterItemName { get; set; }

	public virtual string MasterItemCode { get; set; }

	public virtual string MasterPackUomCode { get; set; }

	public virtual double MasterPackUomEquivalent { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string PackUomCode { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackingType { get; set; }
}
