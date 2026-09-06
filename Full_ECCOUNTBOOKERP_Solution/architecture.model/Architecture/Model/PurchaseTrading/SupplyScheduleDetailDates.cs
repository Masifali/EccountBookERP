using System;

namespace Architecture.Model.PurchaseTrading;

public class SupplyScheduleDetailDates
{
	public DateTime MonthDesc { get; set; }

	public DateTime SchDate { get; set; }

	public double SchQty { get; set; }

	public double TotalQty { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int OrderId { get; set; }

	public int SupplyScheduleId { get; set; }
}
