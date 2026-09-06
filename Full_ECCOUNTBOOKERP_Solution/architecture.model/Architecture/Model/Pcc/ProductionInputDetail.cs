namespace Architecture.Model.pcc;

public class ProductionInputDetail
{
	public decimal Amount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal MinQty { get; set; }

	public decimal MaxQty { get; set; }

	public decimal NetWeight { get; set; }

	public decimal Rate { get; set; }

	public int ActionTypeId { get; set; }

	public int BomInputId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ProductionHeaderId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int RateUomId { get; set; }

	public int WareHouseId { get; set; }

	public int LineId { get; set; }

	public string Remarks { get; set; }

	public int BomHeaderId { get; set; }

	public virtual int BomMainItemId { get; set; }

	public virtual string BomHeaderNo { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemUomCode { get; set; }

	public virtual double ItemUomEquivalent { get; set; }

	public virtual decimal BomQty { get; set; }

	public virtual decimal BomMinQty { get; set; }

	public virtual decimal BomMaxQty { get; set; }

	public virtual decimal BomRate { get; set; }

	public virtual string RateUomCode { get; set; }

	public virtual double RateUomEquivalent { get; set; }
}
