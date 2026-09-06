using System.Collections.Generic;

namespace Architecture.Model;

public class ScreenRights
{
	public int Id { get; set; }

	public int ScreenID { get; set; }

	public string RightName { get; set; }

	public virtual List<ScreenRights> DetailScreenRights { get; set; }
}
