using System.Collections.Generic;

namespace Architecture.Model.POS;

public class PosSaleInvoiceDetail
{
	public double Discount { get; set; }

	public double DiscountAmount { get; set; }

	public double ItemAmount { get; set; }

	public double ItemQty { get; set; }

	public double ItemRate { get; set; }

	public double NetItemAmount { get; set; }

	public double ItemCgsRate { get; set; }

	public int DiscountScheduleId { get; set; }

	public int Id { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public int ItemId { get; set; }

	public int WarehouseId { get; set; }

	public string RemarksDetail { get; set; }

	public int ItemPricingScheduleId { get; set; }

	public int LineId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual List<PosSaleInvoiceDetailDiscount> PosalesInvoiceDetailDiscountsList { get; set; }
}
