using System;

namespace Architecture.Model.Inventory;

public class InvCropYear
{
	public bool DefaultKey { get; set; }

	public int Id { get; set; }

	public string CropYear { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public DateTime EntryDate { get; set; }

	public int ModifyUserId { get; set; }

	public DateTime ModifyDate { get; set; }
}
