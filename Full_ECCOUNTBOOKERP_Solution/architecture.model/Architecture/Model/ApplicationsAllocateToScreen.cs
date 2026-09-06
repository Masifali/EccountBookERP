namespace Architecture.Model;

public class ApplicationsAllocateToScreen
{
	public int Id { get; set; }

	public int ScreenId { get; set; }

	public int AppId { get; set; }

	public bool IsActive { get; set; }

	public string ScreenName { get; set; }
}
