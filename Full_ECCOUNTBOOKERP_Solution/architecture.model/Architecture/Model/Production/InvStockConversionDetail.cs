namespace Architecture.Model.Production;

public class InvStockConversionDetail
{
	public double Amount { get; set; }

	public double ExpenseAmount { get; set; }

	public double PackingMaterialAmount { get; set; }

	public double ItemPmCost { get; set; }

	public double ItemOhCost { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public double Weight { get; set; }

	public double Moisture { get; set; }

	public double WagesAmount { get; set; }

	public int Id { get; set; }

	public int InvStockConversionId { get; set; }

	public int MoistureSlabId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocSubId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int PackingtypeId { get; set; }

	public int PackUnit { get; set; }

	public int ProjectId { get; set; }

	public int RateUOMId { get; set; }

	public int LineId { get; set; }

	public int VoucherHeadId { get; set; }

	public int WarehouseId { get; set; }

	public int ItemConditionId { get; set; }

	public int RackId { get; set; }

	public int SortNo { get; set; }

	public int labIPmActivityLogId { get; set; }

	public bool IsOnHold { get; set; }

	public string CropBatch { get; set; }

	public string EntryType { get; set; }

	public string Remarks { get; set; }

	public virtual string rackName { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string UomCode { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double UOMDescription { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string MoistureSlabDescription { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual decimal IssueWeight { get; set; }
}
