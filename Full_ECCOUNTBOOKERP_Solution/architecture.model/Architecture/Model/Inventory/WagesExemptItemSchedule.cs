using System;

namespace Architecture.Model.Inventory;

public class WagesExemptItemSchedule
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EffectedFrom { get; set; }

	public DateTime EffectedTo { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int WagesAccountId { get; set; }
}
