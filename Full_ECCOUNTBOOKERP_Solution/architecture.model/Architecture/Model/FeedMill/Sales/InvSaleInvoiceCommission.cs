namespace Architecture.Model.FeedMill.Sales;

public class InvSaleInvoiceCommission
{
	public double CommAmount { get; set; }

	public double CommType { get; set; }

	public double Rate { get; set; }

	public double RateUom { get; set; }

	public int CommissionAgentId { get; set; }

	public int Id { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public int CommDebitSupCustId { get; set; }

	public int DebitAccountId { get; set; }

	public int SaleOrderId { get; set; }

	public string Remarks { get; set; }

	public virtual int OrderNo { get; set; }
}
