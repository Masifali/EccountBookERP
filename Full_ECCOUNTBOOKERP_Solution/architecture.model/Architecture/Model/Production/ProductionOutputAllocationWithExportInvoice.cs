using System;
using System.Collections.Generic;

namespace Architecture.Model.Production;

public class ProductionOutputAllocationWithExportInvoice
{
	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int FinancialYearId { get; set; }

	public int BranchesId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int ActionId { get; set; }

	public int Id { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int ItemId { get; set; }

	public double Qty { get; set; }

	public double NetWeight { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual double OutPutQty { get; set; }

	public virtual double OutPutWeight { get; set; }

	public virtual List<ProductionOutputAllocationWithExportInvoice> ProductionOutputAllocationWithExportInvoicesList { get; set; }

	public ProductionOutputAllocationWithExportInvoice()
	{
		ProductionOutputAllocationWithExportInvoicesList = new List<ProductionOutputAllocationWithExportInvoice>();
	}
}
