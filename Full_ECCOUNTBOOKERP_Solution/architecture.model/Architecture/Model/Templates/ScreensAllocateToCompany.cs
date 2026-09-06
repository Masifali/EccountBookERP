using System;
using System.Collections.Generic;

namespace Architecture.Model.Templates;

public class ScreensAllocateToCompany
{
	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public bool IsActive { get; set; }

	public string ScreenName { get; set; }

	public virtual List<ScreensAllocateToCompany> ScreensAllocateToCompanyList { get; set; }
}
