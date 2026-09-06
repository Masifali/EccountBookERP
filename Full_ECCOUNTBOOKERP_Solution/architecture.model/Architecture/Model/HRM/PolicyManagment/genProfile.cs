using System;

namespace Architecture.Model.HRM.PolicyManagment;

public class genProfile
{
	public int ActionTypeId { get; set; }

	public bool IsActive { get; set; }

	public int ProfileTypeId { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int ProfileId { get; set; }

	public int ProfileSeqNo { get; set; }

	public long EntryUser { get; set; }

	public long ModifyUser { get; set; }

	public long OrganizationId { get; set; }

	public long UserLogId { get; set; }

	public string ProfileCode { get; set; }

	public string ProfileName { get; set; }

	public string ProfilePrefix { get; set; }

	public string ProfileUrduName { get; set; }
}
