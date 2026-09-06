namespace Architecture.Model.POS;

public class WsRmSalesInvoiceDetailDiscount
{
	public double ItemDiscountAmount { get; set; }

	public long Id { get; set; }

	public long ItemDiscountScheduleId { get; set; }

	public long LineId { get; set; }

	public long WsRmSaleInvoiceDetailId { get; set; }

	public long WsRmSaleInvoiceId { get; set; }
}
