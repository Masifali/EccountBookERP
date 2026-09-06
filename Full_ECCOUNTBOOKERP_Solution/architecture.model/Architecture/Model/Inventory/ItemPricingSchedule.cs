using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ItemPricingSchedule
{
	public int Id { get; set; }

	public string ScheduleDesc { get; set; }

	public int ItemId { get; set; }

	public DateTime EffectedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public int EntryUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public double ItemFactoryPrice { get; set; }

	public double ItemCostPrice { get; set; }

	public double ItemPurchasePrice { get; set; }

	public double ItemWholeSalePrice { get; set; }

	public double ItemRetailPrice { get; set; }

	public virtual string ItemIds { get; set; }

	public virtual string ItemName { get; set; }

	public virtual List<ItemPricingSchedule> ItemPricingScheduleslist { get; set; }
}
