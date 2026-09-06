using System;
using System.Collections.Generic;

namespace Architecture.Model.ContractorWages;

public class WagesAccountAllocationToDocumentType
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int SortNo { get; set; }

	public int WagesAccountId { get; set; }

	public virtual List<WagesAccountAllocationToDocumentType> WagesAccountAllocationToDocumentTypesList { get; set; }
}
