namespace Architecture.Model.POS;

public class PosSaleInvoiceDetailDiscount
{
	public double ItemDiscountAmount { get; set; }

	public long Id { get; set; }

	public long ItemDiscountScheduleId { get; set; }

	public long LineId { get; set; }

	public long PosSaleInvoiceDetailId { get; set; }

	public long PosSaleInvoiceId { get; set; }
}
