namespace Architecture.Model;

public class FavoriteScreens
{
	public int Id { get; set; }

	public int UserId { get; set; }

	public string ScreenName { get; set; }

	public int ScreenId { get; set; }

	public string ScreenTitle { get; set; }

	public string ScreenRoute { get; set; }

	public string TargetUrl { get; set; }

	public int CompanyId { get; set; }

	public int OrganizationId { get; set; }
}
