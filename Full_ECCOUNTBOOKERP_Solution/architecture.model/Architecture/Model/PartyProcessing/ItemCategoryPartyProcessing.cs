using System;

namespace Architecture.Model.PartyProcessing;

public class ItemCategoryPartyProcessing
{
	public bool CategoryStatus { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ParentCategoriesId { get; set; }

	public int SerialFrom { get; set; }

	public int SerialTo { get; set; }

	public string CategoryCode { get; set; }

	public string CategoryDescription { get; set; }
}
