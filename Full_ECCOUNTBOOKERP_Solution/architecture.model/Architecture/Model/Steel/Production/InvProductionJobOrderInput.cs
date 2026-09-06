namespace Architecture.Model.Steel.Production;

public class InvProductionJobOrderInput
{
	public int Id { get; set; }

	public int? InvProductionJobOrderId { get; set; }

	public int? JobLotId { get; set; }

	public int ItemId { get; set; }

	public int? PackUomId { get; set; }

	public decimal? Qty { get; set; }

	public decimal? NetWeight { get; set; }

	public int? WarehouseId { get; set; }

	public string RemarksDetail { get; set; }

	public int? PackingTypeId { get; set; }

	public decimal? ItemRate { get; set; }

	public decimal? ItemAmount { get; set; }

	public int? RateUomId { get; set; }

	public int? RefDocumentTypeId { get; set; }

	public int? RefDocIdNo { get; set; }

	public int? RefDocSubIdNo { get; set; }

	public int ActionTypeId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string JobLotCode { get; set; }

	public virtual string PackUom { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual double RateUomEquivalent { get; set; }
}
