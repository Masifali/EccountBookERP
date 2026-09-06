using System;

namespace Architecture.Model.Inventory;

public class GatepassType
{
	public DateTime EntryDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public string GpTypeDescription { get; set; }
}
