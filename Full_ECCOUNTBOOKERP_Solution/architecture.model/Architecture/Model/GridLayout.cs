namespace Architecture.Model;

public class GridLayout
{
	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public string Layout { get; set; }

	public string ScreenName { get; set; }

	public string LayoutFileXml { get; set; }

	public byte[] LayoutFile { get; set; }
}
