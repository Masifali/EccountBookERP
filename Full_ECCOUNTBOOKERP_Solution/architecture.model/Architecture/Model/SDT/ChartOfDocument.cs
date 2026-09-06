using System;

namespace Architecture.Model.SDT;

public class ChartOfDocument
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int chartOfDocumentId { get; set; }

	public int CompanyId { get; set; }

	public int leadTime { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int recSortNo { get; set; }

	public int requiredLevelId { get; set; }

	public int DocumentProviderId { get; set; }

	public int seqNo { get; set; }

	public string documentCode { get; set; }

	public string documentName { get; set; }
}
