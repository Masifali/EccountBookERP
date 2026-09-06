using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ItemTaxSchedule
{
	public bool IsActive { get; set; }

	public DateTime EffectedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int TaxTypeId { get; set; }

	public virtual List<ItemTaxSchedule> ItemTaxScheduleList { get; set; }
}
