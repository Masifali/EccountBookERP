using System;

namespace Architecture.Model;

public class ExImVCIParameter
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int ExImVCICartegoryId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SortNo { get; set; }

	public string VciParameterDescription { get; set; }
}
