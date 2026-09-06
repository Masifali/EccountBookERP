using System;

namespace Architecture.Model.Inventory;

public class ProcessStep
{
	public DateTime EntryDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int SortNo { get; set; }

	public string ProcessStepName { get; set; }
}
