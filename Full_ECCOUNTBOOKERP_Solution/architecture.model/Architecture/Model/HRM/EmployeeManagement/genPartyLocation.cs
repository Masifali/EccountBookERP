using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class genPartyLocation
{
	public bool IsActive { get; set; }

	public bool IsAllowShare { get; set; }

	public bool IsCashInvoice { get; set; }

	public bool IsCreditInvoice { get; set; }

	public bool NeedLogin { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public decimal FlatSharePercent { get; set; }

	public int ActionTypeId { get; set; }

	public int CompanyId { get; set; }

	public int OrganizationId { get; set; }

	public int PartyLocationTypeProfileId { get; set; }

	public int PartyLocationZoneProfileId { get; set; }

	public int AddressId { get; set; }

	public int AlteredById { get; set; }

	public int CreatedById { get; set; }

	public int PartyId { get; set; }

	public int PartyLocationId { get; set; }

	public int UserLogId { get; set; }

	public string Mobile1 { get; set; }

	public string Mobile2 { get; set; }

	public string PartyLocationCode { get; set; }

	public string PartyLocationName { get; set; }

	public string PartyLocationPrefix { get; set; }
}
