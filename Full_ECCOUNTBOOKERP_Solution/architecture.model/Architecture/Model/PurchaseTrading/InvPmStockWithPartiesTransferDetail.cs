namespace Architecture.Model.PurchaseTrading;

public class InvPmStockWithPartiesTransferDetail
{
	public decimal ItemQty { get; set; }

	public decimal ItemRate { get; set; }

	public double ItemAmount { get; set; }

	public int Id { get; set; }

	public int InvPmStockWithPartiesTransferHeaderId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int SortNo { get; set; }

	public int SupplierIdFrom { get; set; }

	public int SupplierIdTo { get; set; }

	public int ItemConditionId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string SupplierFrom { get; set; }

	public virtual string SupplierTo { get; set; }

	public virtual string ItemCondition { get; set; }
}
