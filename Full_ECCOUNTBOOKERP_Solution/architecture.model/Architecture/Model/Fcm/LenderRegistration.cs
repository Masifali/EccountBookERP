using System;

namespace Architecture.Model.Fcm;

public class LenderRegistration
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int LenderRegistrationId { get; set; }

	public int LenderTypeId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SortNo { get; set; }

	public long BranchId { get; set; }

	public string LenderCode { get; set; }

	public string LenderName { get; set; }
}
