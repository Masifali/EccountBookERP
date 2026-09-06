using System.Collections.Generic;

namespace Architecture.Model;

public class CompanyRights
{
	public int CompanyId { get; set; }

	public int Id { get; set; }

	public bool IsActive { get; set; }

	public int ScreenId { get; set; }

	public virtual string SelectedScreedIds { get; set; }

	public virtual List<CompanyRights> CompanyRightslst { get; set; }
}
