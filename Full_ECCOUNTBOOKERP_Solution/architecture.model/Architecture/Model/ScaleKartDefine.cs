using System;

namespace Architecture.Model;

public class ScaleKartDefine
{
	public DateTime EffectiveDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double RangeFrom { get; set; }

	public double RangeTo { get; set; }

	public double ScaleKart { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string Type { get; set; }
}
