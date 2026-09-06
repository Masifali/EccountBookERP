using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ConsumptionItems
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string Remarks { get; set; }

	public virtual List<ConsumptionItems> ConsumptionItemslist { get; set; }
}
