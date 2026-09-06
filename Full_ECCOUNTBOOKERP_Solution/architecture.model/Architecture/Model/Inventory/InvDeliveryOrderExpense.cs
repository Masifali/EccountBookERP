namespace Architecture.Model.Inventory;

public class InvDeliveryOrderExpense
{
	public int Id { get; set; }

	public int SaleOrderId { get; set; }

	public int SaleOrderCustomerExpId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int InvoiceOtherItemDetailId { get; set; }

	public int InvDeliveryOrderId { get; set; }

	public int ItemId { get; set; }

	public double Qty { get; set; }

	public double WeightPerQty { get; set; }

	public double NetWeight { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherItemName { get; set; }

	public virtual int SaleOrder { get; set; }

	public virtual int InvoiceNo { get; set; }
}
