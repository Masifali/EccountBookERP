using System;

namespace Architecture.Model.Fcm;

public class CreditFacilityType
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int CompanyId { get; set; }

	public int CreditFacilityTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string creditFacilityCode { get; set; }

	public string CreditFacilityName { get; set; }

	public string creditFacilityNotes { get; set; }
}
