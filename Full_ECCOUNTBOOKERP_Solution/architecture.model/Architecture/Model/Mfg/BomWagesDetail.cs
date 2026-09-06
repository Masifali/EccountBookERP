namespace Architecture.Model.Mfg;

public class BomWagesDetail
{
	public decimal Amount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal Rate { get; set; }

	public int BomHeaderId { get; set; }

	public int ContractorWagesRateScheduleId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ServiceActivity { get; set; }
}
