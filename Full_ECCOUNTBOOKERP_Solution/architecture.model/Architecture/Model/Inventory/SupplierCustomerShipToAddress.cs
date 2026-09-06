using System;

namespace Architecture.Model.Inventory;

public class SupplierCustomerShipToAddress
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CityId { get; set; }

	public int CompanyId { get; set; }

	public int CountryId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string AddressLine1 { get; set; }

	public string AddressLine2 { get; set; }

	public string AddressLine3 { get; set; }

	public string AddressLine4 { get; set; }

	public string MobileNo { get; set; }

	public string PhoneNo { get; set; }

	public string WhatsAppNo { get; set; }

	public string AddressTitle { get; set; }

	public string ContactPerson { get; set; }
}
