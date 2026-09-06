using System;

namespace Architecture.Model.lgstcm;

public class LogiticOtherChargesItems
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public bool Active { get; set; }

	public int EntryUserId { get; set; }

	public int LogiticOtherChargesItemsId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SortNo { get; set; }

	public string Description { get; set; }

	public string Code { get; set; }
}
