namespace Architecture.Model.Salt;

public class ProductionOutPutDetail
{
	public decimal Amount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ItemRate { get; set; }

	public decimal NetWeight { get; set; }

	public int Id { get; set; }

	public int ProductionHeaderId { get; set; }

	public int SaleOrderId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int PackingTypeId { get; set; }

	public int RateUomId { get; set; }

	public int WareHouseId { get; set; }

	public int ActionTypeId { get; set; }

	public int LineId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackEquivalent { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double RateEquivalent { get; set; }

	public virtual int SaleOrder { get; set; }
}
