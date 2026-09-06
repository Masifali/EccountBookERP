namespace Architecture.Model.pcc;

public class InvStockTransferDetail
{
	public bool IsApproved { get; set; }

	public decimal ItemAmount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ItemRate { get; set; }

	public decimal ItemWeight { get; set; }

	public decimal NetWeight { get; set; }

	public decimal VareintEquivalent { get; set; }

	public int ActionTypeId { get; set; }

	public int AttributeVareintId { get; set; }

	public int Id { get; set; }

	public int InvStockTransferHeaderId { get; set; }

	public int ItemId { get; set; }

	public int JobLotFromId { get; set; }

	public int JobLotToId { get; set; }

	public int LineId { get; set; }

	public int WarehouseFromId { get; set; }

	public int WarehouseToId { get; set; }

	public int ItemUomId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string AttributeVareint { get; set; }

	public virtual string PackUom { get; set; }

	public virtual string WarehouseFrom { get; set; }

	public virtual string WarehouseTo { get; set; }

	public virtual string JobLotFrom { get; set; }

	public virtual string JobLotTo { get; set; }
}
