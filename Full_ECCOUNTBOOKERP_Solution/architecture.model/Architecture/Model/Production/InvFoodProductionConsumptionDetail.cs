using System;

namespace Architecture.Model.Production;

public class InvFoodProductionConsumptionDetail
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public double Weight { get; set; }

	public int Id { get; set; }

	public int InvFoodProductionId { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int InvProductionJobOrderNo { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int PackingtypeId { get; set; }

	public int RateUOMId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int WarehouseId { get; set; }

	public int LineId { get; set; }

	public string CropBatch { get; set; }

	public string EntryType { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double RateEquivalent { get; set; }

	public virtual string RateUOM { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeCode { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string JobLotCode { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual DateTime RefDocDate { get; set; }

	public virtual int RefDocNo { get; set; }
}
