using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class CheqBookHeader
{
	public bool PostState { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public int BankId { get; set; }

	public int ChartOfAccountId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public string CbPrefix { get; set; }

	public string CbSrFrom { get; set; }

	public string CbSrTo { get; set; }

	public string Remarks { get; set; }

	public virtual List<CheqBookDetail> Cheqbookdetaillist { get; set; }
}
