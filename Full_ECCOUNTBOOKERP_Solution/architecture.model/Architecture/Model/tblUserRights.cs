using System.Collections.Generic;

namespace Architecture.Model;

public class tblUserRights
{
	public bool Value { get; set; }

	public int Id { get; set; }

	public int RightsID { get; set; }

	public int ScreenID { get; set; }

	public int UserId { get; set; }

	public int CompanyId { get; set; }

	public int AppId { get; set; }

	public virtual int ModuleID { get; set; }

	public virtual string RightName { get; set; }

	public virtual string ModuleDescription { get; set; }

	public virtual string ScreenName { get; set; }

	public virtual string ScreenAlias { get; set; }

	public virtual string MenuControllName { get; set; }

	public virtual string ModelMenuControllName { get; set; }

	public virtual string TargetUrl { get; set; }

	public virtual bool IsFavorite { get; set; }

	public virtual string IconUrl { get; set; }

	public virtual int ModuleTypeId { get; set; }

	public virtual string App { get; set; }

	public virtual List<tblUserRights> TblUserRightslst { get; set; }
}
