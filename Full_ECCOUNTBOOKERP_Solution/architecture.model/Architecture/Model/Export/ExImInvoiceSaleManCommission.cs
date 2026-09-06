namespace Architecture.Model.Export;

public class ExImInvoiceSaleManCommission
{
	public int exImInvoiceSaleManCommissionId { get; set; }

	public int exImInvoiceId { get; set; }

	public int SalesPersonId { get; set; }

	public int commissionTypeId { get; set; }

	public int ActionTypeId { get; set; }

	public double commissionRate { get; set; }

	public string rateUom { get; set; }

	public double fcyAmount { get; set; }

	public double exchangeRate { get; set; }

	public double lcyAmount { get; set; }

	public string Remarks { get; set; }

	public virtual string SalesPerson { get; set; }

	public virtual string CommissionType { get; set; }
}
