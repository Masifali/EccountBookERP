namespace Architecture.Model.FeedMill.Production;

public class ProductionOutPutDetail
{
	public decimal Amount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ConsumptionQty { get; set; }

	public decimal NetWeight { get; set; }

	public decimal Rate { get; set; }

	public int ActionTypeId { get; set; }

	public int BOMHeaderId { get; set; }

	public int Id { get; set; }

	public int ProductionHeaderId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int RateUomId { get; set; }

	public int SaleOrderId { get; set; }

	public int EntryTypeId { get; set; }

	public int WareHouseId { get; set; }

	public int WorkOrderId { get; set; }

	public int LineId { get; set; }

	public int JobLotId { get; set; }

	public int PriceTypeId { get; set; }

	public string Remarks { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual string EntryType { get; set; }

	public virtual string BOMHeaderNo { get; set; }

	public virtual string WorkOrderNo { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemUomCode { get; set; }

	public virtual double ItemUomEquivalent { get; set; }

	public virtual string RateUomCode { get; set; }

	public virtual string JobLot { get; set; }

	public virtual string PriceType { get; set; }

	public virtual double RateUomEquivalent { get; set; }

	public virtual double BomQty { get; set; }
}
