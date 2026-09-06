namespace Architecture.Model.Service;

public class ExImClearingAgentBillTaxDetail
{
	public double TaxAmount { get; set; }

	public double TaxRate { get; set; }

	public int BillHeaderId { get; set; }

	public int ChartOfAccountId { get; set; }

	public int Id { get; set; }

	public int TaxNameId { get; set; }

	public int TaxTypeId { get; set; }

	public string TaxName { get; set; }
}
