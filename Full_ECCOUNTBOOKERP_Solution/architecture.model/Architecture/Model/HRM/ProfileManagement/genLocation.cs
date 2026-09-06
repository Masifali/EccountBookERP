using System;

namespace Architecture.Model.HRM.ProfileManagement;

public class genLocation
{
	public bool IsDefaultLocation { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int ActionTypeId { get; set; }

	public int AddressId { get; set; }

	public int AlteredById { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int LocationId { get; set; }

	public int LocationSettingId { get; set; }

	public int LocationTypeProfileId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int UserLogId { get; set; }

	public string Email { get; set; }

	public string FooterDetail { get; set; }

	public string HeaderDetail { get; set; }

	public string LicenseNo { get; set; }

	public string LocationName { get; set; }

	public string LocationShortName { get; set; }

	public string Mobile1 { get; set; }

	public string Mobile2 { get; set; }

	public string PortalURL { get; set; }

	public string SubLocationName { get; set; }
}
