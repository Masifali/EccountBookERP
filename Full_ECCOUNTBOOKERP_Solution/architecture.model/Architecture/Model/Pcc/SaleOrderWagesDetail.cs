namespace Architecture.Model.pcc;

public class SaleOrderWagesDetail
{
	public decimal Amount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int ContractorId { get; set; }

	public int ContractorWagesRateScheduleId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int SaleOrderId { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ContractorName { get; set; }

	public virtual string ServiceActivity { get; set; }
}
