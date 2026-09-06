using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class EEStatement
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int FcyReceiptId { get; set; }

	public int FcyReceiptsDetailId { get; set; }

	public int FEReturnMonthId { get; set; }

	public int FEReturnYearId { get; set; }

	public int FEReturnPageNo { get; set; }

	public int Id { get; set; }

	public int ITRSNo { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string FEReturnMonth { get; set; }

	public string Remarks { get; set; }

	public string SalesTerm { get; set; }

	public string Schedule { get; set; }

	public virtual List<EEStatement> EEStatementList { get; set; }
}
