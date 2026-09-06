using System.Collections.Generic;

namespace Architecture.Model;

public class AppModules
{
	public int AppId { get; set; }

	public int Id { get; set; }

	public int ModuleTypeId { get; set; }

	public int SortNo { get; set; }

	public string IconUrl { get; set; }

	public string ModelMenuControllName { get; set; }

	public string ModuleDescription { get; set; }

	public virtual List<AppModules> AppModuleList { get; set; }
}
