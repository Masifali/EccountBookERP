namespace Architecture.Model.Mfg;

public class WorkOrderCustomer
{
	public decimal Qty { get; set; }

	public int ActionTypeId { get; set; }

	public int CustomerId { get; set; }

	public int WorkOrderCustomerId { get; set; }

	public int SaleOrderDetailId { get; set; }

	public int SaleOrderId { get; set; }

	public int WorkOrderId { get; set; }

	public string CustomerRemarks { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual int SaleOrderNo { get; set; }
}
