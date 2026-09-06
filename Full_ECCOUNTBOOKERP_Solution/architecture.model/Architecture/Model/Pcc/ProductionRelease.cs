using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.pcc;

public class ProductionRelease
{
	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int RefDocId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public string Remarks { get; set; }

	public virtual List<ProductionRelease> ProductionReleaseslist { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }
}
