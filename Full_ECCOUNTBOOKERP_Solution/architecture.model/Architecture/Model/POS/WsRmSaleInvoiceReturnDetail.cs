using Architecture.Model.Inventory;

namespace Architecture.Model.POS;

public class WsRmSaleInvoiceReturnDetail
{
	public double DiscountAmount { get; set; }

	public double GstPrcnt { get; set; }

	public double GstTotalAmount { get; set; }

	public double ItemAmount { get; set; }

	public double ItemCgsRate { get; set; }

	public double ItemQty { get; set; }

	public double ItemRate { get; set; }

	public double NetItemAmount { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int WarehouseId { get; set; }

	public int WsRmSaleInvoiceReturnId { get; set; }

	public int WsRmSalesOrderDetailId { get; set; }

	public int WsRmSalesOrderId { get; set; }

	public int LineId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual Item Item { get; set; }
}
