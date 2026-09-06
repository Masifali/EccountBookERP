using System;

namespace Architecture.Model.Export;

public class ThirdPartyType
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public bool IsActive { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ThirdPartyTypeID { get; set; }

	public string PartyTypeCode { get; set; }

	public string PartyTypeName { get; set; }
}
