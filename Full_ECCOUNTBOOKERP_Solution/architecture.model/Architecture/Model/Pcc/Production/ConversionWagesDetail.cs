namespace Architecture.Model.pcc.Production;

public class ConversionWagesDetail
{
	public decimal Amount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int ContractorId { get; set; }

	public int ContractorWagesRateScheduleId { get; set; }

	public int ConversionHeaderId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int OutPutItemId { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string OutPutItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ContractorName { get; set; }

	public virtual string ServiceActivity { get; set; }
}
