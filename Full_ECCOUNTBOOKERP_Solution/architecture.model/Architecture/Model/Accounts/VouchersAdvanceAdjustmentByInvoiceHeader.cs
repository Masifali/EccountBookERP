using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class VouchersAdvanceAdjustmentByInvoiceHeader
{
	public bool PostState { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public double AdjustedAmount { get; set; }

	public double TaxAmount { get; set; }

	public double TaxPrcnt { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUser { get; set; }

	public int GlAccountId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int SupplierCustomerId { get; set; }

	public int WhtTaxAccountId { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<VouchersAdvanceAdjustedInvoicesDetail> AdjustedInvoicesDetails { get; set; }

	public virtual List<VouchersAdvanceAdjustedAccountsDetail> AdjustedAccountsDetails { get; set; }
}
