using System;

namespace Architecture.Model;

public class DocumentType
{
	public bool PostState { get; set; }

	public bool Status { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public int CompanyId { get; set; }

	public int ControlAccountId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int SortNo { get; set; }

	public string DocumentTypeCode { get; set; }

	public string DocumentTypeDescription { get; set; }

	public string DocumentTypeDescriptionOtherLing { get; set; }

	public string Remarks { get; set; }

	public string ScreenName { get; set; }

	public string TargetUrl { get; set; }
}
