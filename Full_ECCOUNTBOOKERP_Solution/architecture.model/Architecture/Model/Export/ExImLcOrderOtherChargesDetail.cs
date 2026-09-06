namespace Architecture.Model.Export;

public class ExImLcOrderOtherChargesDetail
{
	public double AddAmount { get; set; }

	public double LessAmount { get; set; }

	public int ChargesItemId { get; set; }

	public int ExImLcOrderId { get; set; }

	public int Id { get; set; }

	public string Remarks { get; set; }

	public virtual int ChargesAccountId { get; set; }

	public virtual string ChargesAccount { get; set; }

	public virtual string ChargesItemName { get; set; }
}
