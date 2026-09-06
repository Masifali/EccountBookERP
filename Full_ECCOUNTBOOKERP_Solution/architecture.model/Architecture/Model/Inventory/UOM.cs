using System;

namespace Architecture.Model.Inventory;

public class UOM
{
	public bool UOMStatus { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ParentUOMId { get; set; }

	public string UOMCode { get; set; }

	public string UOMDescription { get; set; }

	public string UOMSymbol { get; set; }

	public double Equivalent { get; set; }

	public double QtyEquivalent { get; set; }

	public virtual string ParentUOMCode { get; set; }
}
