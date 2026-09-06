namespace Architecture.Model.Authentication;

public class RoleRights
{
	public bool IsActive { get; set; }

	public string CreatedDate { get; set; }

	public string LastChangedDate { get; set; }

	public int ID { get; set; }

	public int IsLocked { get; set; }

	public int ModuleID { get; set; }

	public int RightID { get; set; }

	public int RoleID { get; set; }

	public string CreatedBy { get; set; }

	public string LastChangedBy { get; set; }
}
