namespace Architecture.Model.Import;

public class ImLcOrderPaymnetTerm
{
	public decimal FcyAmount { get; set; }

	public decimal PrcntOfTotal { get; set; }

	public int DueDays { get; set; }

	public int DueTypeId { get; set; }

	public int Id { get; set; }

	public int ImLcOrderId { get; set; }

	public int PaymentTermId { get; set; }

	public virtual string PaymentTerm { get; set; }
}
