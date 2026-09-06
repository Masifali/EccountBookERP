using System.Collections.Generic;

namespace Architecture.Model;

public class AppMenu
{
	public bool IsActive { get; set; }

	public bool IsCloud { get; set; }

	public bool IsCompany { get; set; }

	public bool IsMobile { get; set; }

	public int AppId { get; set; }

	public int Id { get; set; }

	public int ModuleId { get; set; }

	public int SortNo { get; set; }

	public string RouteUrl { get; set; }

	public string ScreenAlias { get; set; }

	public string ScreenName { get; set; }

	public string TargetUrl { get; set; }

	public virtual List<AppMenu> AppMenuList { get; set; }

	public virtual List<ScreenRights> DetailScreenRights { get; set; }
}
