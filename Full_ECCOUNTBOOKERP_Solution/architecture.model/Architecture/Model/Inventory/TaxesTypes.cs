using System;

namespace Architecture.Model.Inventory;

public class TaxesTypes
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int TaxGLAccountId { get; set; }

	public int Type { get; set; }

	public string TaxDescription { get; set; }

	public string TaxName { get; set; }
}
