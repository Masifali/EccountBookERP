using System;

namespace Architecture.Model;

public class Projects
{
	public bool IsActive { get; set; }

	public bool IsApproved { get; set; }

	public bool ProjectStatus { get; set; }

	public DateTime ApproveDate { get; set; }

	public DateTime EndDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime StartDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CityId { get; set; }

	public int CompanyId { get; set; }

	public int CustomerId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrderNo { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectGroupID { get; set; }

	public string AddressDescription { get; set; }

	public string CustomerClient { get; set; }

	public string InternalExternal { get; set; }

	public string ProjectCode { get; set; }

	public string ProjectDescription { get; set; }

	public string ProjectManger { get; set; }

	public string ProjectName { get; set; }

	public string ProjectType { get; set; }

	public string SiteLocationName { get; set; }
}
