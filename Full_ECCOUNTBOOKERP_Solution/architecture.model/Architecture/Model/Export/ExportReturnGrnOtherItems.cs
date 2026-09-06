namespace Architecture.Model.Export;

public class ExportReturnGrnOtherItems
{
	public decimal Amount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public decimal WeightKgs { get; set; }

	public int ExportReturnGrnId { get; set; }

	public int Id { get; set; }

	public int OtherItemId { get; set; }

	public string ContainerNo { get; set; }

	public string OtherItemRemarks { get; set; }

	public virtual string OtherItemName { get; set; }

	public virtual string OtherItemCode { get; set; }
}
