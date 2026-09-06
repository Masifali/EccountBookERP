using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ItemWiseCommissionSchedule
{
	public bool OnPurchase { get; set; }

	public bool OnSale { get; set; }

	public DateTime EffectedFromDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrgnizationId { get; set; }

	public virtual List<ItemWiseCommissionSchedule> ItemWiseCommissionSchedulelist { get; set; }
}
