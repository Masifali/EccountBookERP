using System.Collections.Generic;

namespace Architecture.Model;

public class CompanyRightsInsertAndDelete
{
	public int CompanyId { get; set; }

	public int OrganizationId { get; set; }

	public int UserId { get; set; }

	public int AppId { get; set; }

	public int ScreenId { get; set; }

	public int EntryUserId { get; set; }

	public virtual string SelectedScreedIds { get; set; }

	public virtual List<CompanyRightsInsertAndDelete> CompanyRightsInsertAndDeletelist { get; set; }
}
