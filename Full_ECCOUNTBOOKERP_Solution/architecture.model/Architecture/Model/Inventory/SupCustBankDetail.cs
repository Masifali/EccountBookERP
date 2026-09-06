using System;

namespace Architecture.Model.Inventory;

public class SupCustBankDetail
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int BankId { get; set; }

	public int BranchesId { get; set; }

	public int CityId { get; set; }

	public int CompanyId { get; set; }

	public int CountryId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int SupCustId { get; set; }

	public string AcNumber { get; set; }

	public string AcTitle { get; set; }

	public string BranchCode { get; set; }
}
