namespace Architecture.Model.pcc;

public class ProductionOutPutDetail
{
	public decimal Amount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal VarientQty { get; set; }

	public decimal NetWeight { get; set; }

	public decimal Rate { get; set; }

	public decimal VarientEquivalent { get; set; }

	public int ActionTypeId { get; set; }

	public int BOMHeaderId { get; set; }

	public int Id { get; set; }

	public int ProductionHeaderId { get; set; }

	public int ItemAttributeVarientId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int JobLotId { get; set; }

	public int RateUomId { get; set; }

	public int SaleOrderId { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int EntryTypeId { get; set; }

	public int WareHouseId { get; set; }

	public int CustomerId { get; set; }

	public int CustomerTypeId { get; set; }

	public int WorkOrderId { get; set; }

	public int WorkOrderDetailId { get; set; }

	public int LineId { get; set; }

	public string Remarks { get; set; }

	public string CustomerName { get; set; }

	public string ReferenceNo { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual string EntryType { get; set; }

	public virtual string CustomerType { get; set; }

	public virtual string BOMHeaderNo { get; set; }

	public virtual string WorkOrderNo { get; set; }

	public virtual string ItemAttribute { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemUomCode { get; set; }

	public virtual double ItemUomEquivalent { get; set; }

	public virtual string RateUomCode { get; set; }

	public virtual double RateUomEquivalent { get; set; }
}
