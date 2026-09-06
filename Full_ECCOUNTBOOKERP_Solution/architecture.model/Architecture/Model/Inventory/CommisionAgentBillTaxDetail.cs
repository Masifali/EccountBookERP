namespace Architecture.Model.Inventory;

public class CommisionAgentBillTaxDetail
{
	public int Id { get; set; }

	public int InvCommAgentBillTradeId { get; set; }

	public int EntrySideId { get; set; }

	public int TaxAccountId { get; set; }

	public int TaxTypeId { get; set; }

	public decimal TaxPercantage { get; set; }

	public decimal TaxAmount { get; set; }

	public int sortNo { get; set; }

	public string Remarks { get; set; }

	public virtual string EntrySide { get; set; }

	public virtual string TaxAccountTitle { get; set; }

	public virtual string TaxTypeName { get; set; }
}
