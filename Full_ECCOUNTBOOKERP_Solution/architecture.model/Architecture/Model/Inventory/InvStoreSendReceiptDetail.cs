namespace Architecture.Model.Inventory;

public class InvStoreSendReceiptDetail
{
	public decimal QtyIn { get; set; }

	public decimal QtyOut { get; set; }

	public decimal ItemRate { get; set; }

	public decimal ItemAmount { get; set; }

	public int InvPackingTypeId { get; set; }

	public int InvStoreSendReceiptDetailId { get; set; }

	public int InvStoreSendReceiptId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int RefDocDetailId { get; set; }

	public int SortNo { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int ItemConditionId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string ItemCondition { get; set; }
}
