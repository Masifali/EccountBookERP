using System.Collections.Generic;

namespace Architecture.Model.Main;

public class UserDevices
{
	public int Id { get; set; }

	public int UserId { get; set; }

	public string DeviceId { get; set; }

	public bool IsActive { get; set; }

	public int EntryUserId { get; set; }

	public int? ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public virtual List<UserDevices> UserDevicesList { get; set; }
}
