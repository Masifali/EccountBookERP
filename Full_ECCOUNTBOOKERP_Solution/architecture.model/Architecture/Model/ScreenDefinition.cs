using System.Collections.Generic;

namespace Architecture.Model;

public class ScreenDefinition
{
	public bool IsCreateSMSTemplate { get; set; }

	public int Id { get; set; }

	public int ModuleId { get; set; }

	public string ScreenAlias { get; set; }

	public string ScreenName { get; set; }

	public string TargetUrl { get; set; }

	public bool IsActive { get; set; }

	public int SortNo { get; set; }

	public int AppId { get; set; }

	public string CompanyIds { get; set; }

	public virtual List<ScreenDefinition> ScreenDefinitionslist { get; set; }

	public virtual List<ScreenRights> DetailScreenRights { get; set; }

	public virtual List<ApplicationsAllocateToScreen> ApplicationsAllocateToScreenslist { get; set; }
}
