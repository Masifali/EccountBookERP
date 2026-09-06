using System;

namespace Architecture.Model.Production;

public class ProductionManualCostingInvoiceDetail
{
	public decimal AmountAfterComm { get; set; }

	public decimal CommissionAmount { get; set; }

	public decimal ContributionPerMTon { get; set; }

	public decimal CostAfterProcessing { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal FinancialCharges { get; set; }

	public decimal FOBCharges { get; set; }

	public decimal Freight { get; set; }

	public decimal ItemSaleAmount { get; set; }

	public decimal PackingCharges { get; set; }

	public decimal Tax { get; set; }

	public decimal TotalCost { get; set; }

	public int ActionTypeId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int FcyCodeId { get; set; }

	public int Id { get; set; }

	public int ProductionManualCostingId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual DateTime? InvoiceDate { get; set; }

	public virtual string FcyCode { get; set; }
}
