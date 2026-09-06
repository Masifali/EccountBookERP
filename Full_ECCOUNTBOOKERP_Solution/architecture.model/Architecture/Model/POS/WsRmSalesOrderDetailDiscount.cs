namespace Architecture.Model.POS;

public class WsRmSalesOrderDetailDiscount
{
	public double ItemDiscountAmount { get; set; }

	public long Id { get; set; }

	public long ItemDiscountScheduleId { get; set; }

	public long LineId { get; set; }

	public long WsRmSaleOrderDetailId { get; set; }

	public long WsRmSaleOrderId { get; set; }
}
