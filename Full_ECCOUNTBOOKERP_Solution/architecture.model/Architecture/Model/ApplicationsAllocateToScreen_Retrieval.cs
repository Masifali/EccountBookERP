namespace Architecture.Model;

public class ApplicationsAllocateToScreen_Retrieval
{
	public int Id { get; set; }

	public int ScreenId { get; set; }

	public int AppId { get; set; }

	public string AppName { get; set; }

	public string MasterApp { get; set; }

	public int MasterAppId { get; set; }

	public bool IsActive { get; set; }
}
