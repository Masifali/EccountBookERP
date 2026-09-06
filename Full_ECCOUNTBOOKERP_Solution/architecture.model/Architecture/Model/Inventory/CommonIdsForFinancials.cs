namespace Architecture.Model.Inventory;

public class CommonIdsForFinancials
{
	public int SupplierCustomerId { get; set; }

	public int SupplierGlAccountId { get; set; }

	public int CommissionGlAccountId { get; set; }

	public int TransporterGlAccountId { get; set; }

	public int BrokerGlAccountId { get; set; }

	public int ItemStockAccountId { get; set; }

	public int OffsetAccountId { get; set; }

	public int SaleGLAccountId { get; set; }

	public int StockPartyGlAccountId { get; set; }

	public int CostCenterId { get; set; }

	public int CommissionCreditAccountAgainstSaleReturnId { get; set; }

	public string CompanyName { get; set; }

	public string BrokerAgentName { get; set; }

	public string ItemName { get; set; }

	public string Remarks { get; set; }

	public bool AutoRemarksStatus { get; set; }

	public double TotalQty { get; set; }

	public double TotalBillWeight { get; set; }

	public double TotalItemAmount { get; set; }

	public double TotalFreight { get; set; }

	public double TotalCommission { get; set; }
}
