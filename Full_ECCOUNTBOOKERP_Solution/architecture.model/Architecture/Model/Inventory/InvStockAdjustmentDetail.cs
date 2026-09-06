using System;

namespace Architecture.Model.Inventory;

public class InvStockAdjustmentDetail
{
	public decimal ItemRate { get; set; }

	public decimal NetWeight { get; set; }

	public decimal Qty { get; set; }

	public double Amount { get; set; }

	public int CreditAccountId { get; set; }

	public int CropYearId { get; set; }

	public int DebitAccountId { get; set; }

	public int Id { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int InvStockAdjustmentId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int PackUomId { get; set; }

	public int RateUomId { get; set; }

	public int WarehouseId { get; set; }

	public int LineId { get; set; }

	public int ItemConditionId { get; set; }

	public int RackId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string JobLot { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string PackUom { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual double RateUomEquivalent { get; set; }

	public virtual string RefDocumentType { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual string rackName { get; set; }

	public virtual DateTime? RefDocDate { get; set; }

	public virtual int RefDocNo { get; set; }
}
