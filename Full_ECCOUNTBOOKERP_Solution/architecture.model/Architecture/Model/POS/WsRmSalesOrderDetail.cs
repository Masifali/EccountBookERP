using System.Collections.Generic;

namespace Architecture.Model.POS;

public class WsRmSalesOrderDetail
{
	public double GstPrcnt { get; set; }

	public double ItemAmount { get; set; }

	public double ItemQty { get; set; }

	public double ItemRate { get; set; }

	public string RemarksDetail { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int WsRmSalesOrderId { get; set; }

	public double TotalDiscount { get; set; }

	public double DiscountPerItem { get; set; }

	public double ItemNetAmount { get; set; }

	public int PricingScheduleId { get; set; }

	public int DiscountingScheduleId { get; set; }

	public int CustomerDiscountingScheduleId { get; set; }

	public int CashDiscountingScheduleId { get; set; }

	public double ItemLineAmount { get; set; }

	public double DiscountOnCashPerItem { get; set; }

	public double CustomerDiscPerItem { get; set; }

	public double ItemDiscount { get; set; }

	public virtual string UOMDescription { get; set; }

	public virtual string ItemName { get; set; }

	public virtual List<WsRmSalesOrderDetailDiscount> WsRmSalesOrderDetailDiscountslist { get; set; }
}
