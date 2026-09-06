namespace Architecture.Model.pcc;

public class WorkOrderCustomerDetail
{
	public decimal ItemQty { get; set; }

	public decimal NetWeight { get; set; }

	public decimal VarientEquivalent { get; set; }

	public decimal VarientQty { get; set; }

	public int ActionTypeId { get; set; }

	public int CustomerTypeId { get; set; }

	public int CustomerId { get; set; }

	public int ItemAttributeVarientId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int LineId { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int SaleOrderId { get; set; }

	public int WorkOrderCustomerDetailId { get; set; }

	public int WorkOrderHeaderId { get; set; }

	public string ReferenceNo { get; set; }

	public string Remarks { get; set; }

	public string CustomerName { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemUomCode { get; set; }

	public virtual double ItemUomEquivalent { get; set; }

	public virtual string ItemAttribute { get; set; }

	public virtual string CustomerType { get; set; }
}
