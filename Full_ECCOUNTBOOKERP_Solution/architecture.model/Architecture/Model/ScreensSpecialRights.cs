using System.Collections.Generic;

namespace Architecture.Model;

public class ScreensSpecialRights
{
	public int Id { get; set; }

	public int RightId { get; set; }

	public int ScreenId { get; set; }

	public virtual List<ScreensSpecialRights> ScreensSpecialRightsList { get; set; }
}
