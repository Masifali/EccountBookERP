namespace Architecture.Model.Production;

public class InvStockConversionAddExpense
{
	public double ExpAmount { get; set; }

	public int ChartOfAccountId { get; set; }

	public int Id { get; set; }

	public int BrandItemId { get; set; }

	public int BrandItemUomId { get; set; }

	public int InvStockConversionId { get; set; }

	public string LedgerRemarks { get; set; }

	public string ChargeTo { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string BrandName { get; set; }

	public virtual string BrandUom { get; set; }
}
