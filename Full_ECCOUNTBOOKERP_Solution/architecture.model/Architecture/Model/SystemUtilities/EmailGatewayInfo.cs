using System;

namespace Architecture.Model.SystemUtilities;

public class EmailGatewayInfo
{
	public int Id { get; set; }

	public string Gateway { get; set; }

	public string OutgoingSMTP { get; set; }

	public string EmailAddress { get; set; }

	public int ServerPort { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public int EntryUser { get; set; }

	public int ModifyUser { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime? ModifyDate { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }
}
