using System;

namespace Architecture.Model.Inventory;

public class DeliveryScheduleDetailDates
{
	public DateTime MonthDesc { get; set; }

	public DateTime SchDate { get; set; }

	public double SchQty { get; set; }

	public double TotalQty { get; set; }

	public int DeliveryScheduleId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int OrderId { get; set; }
}
