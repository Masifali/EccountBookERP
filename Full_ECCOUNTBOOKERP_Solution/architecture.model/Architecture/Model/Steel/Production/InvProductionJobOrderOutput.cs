namespace Architecture.Model.Steel.Production;

public class InvProductionJobOrderOutput
{
	public int Id { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int EntryTypeId { get; set; }

	public int ItemId { get; set; }

	public int InvPackingTypeId { get; set; }

	public decimal Qty { get; set; }

	public int PackUomId { get; set; }

	public decimal NetWeight { get; set; }

	public string RemarksDetail { get; set; }

	public int? JobLotId { get; set; }

	public int? WarehouseId { get; set; }

	public double? ItemRate { get; set; }

	public double? ItemAmount { get; set; }

	public int? RateUomId { get; set; }

	public decimal? OHExpense { get; set; }

	public decimal? PMExpense { get; set; }

	public decimal? OHGeneralExpense { get; set; }

	public decimal? PMGeneralExpense { get; set; }

	public decimal? NetRate { get; set; }

	public decimal? NetAmount { get; set; }

	public int ActionTypeId { get; set; }

	public virtual string PackTypeCode { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string PackUomCode { get; set; }

	public virtual string PackUOMDescription { get; set; }

	public virtual double PackUomCodeEquivalent { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string RateUom { get; set; }

	public virtual string EntryType { get; set; }

	public virtual double Equivalent { get; set; }
}
