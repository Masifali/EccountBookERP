using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class DepartmentRequest
{
	public bool IsApproved { get; set; }

	public bool PostState { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? PostDate { get; set; }

	public int CompanyId { get; set; }

	public int DocTypeId { get; set; }

	public int DocNo { get; set; }

	public int EntryUser { get; set; }

	public int FromDepartmentId { get; set; }

	public int Id { get; set; }

	public int JobLotId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int ProjectId { get; set; }

	public int StatusId { get; set; }

	public int ActionId { get; set; }

	public int BranchId { get; set; }

	public int ToDepartmentId { get; set; }

	public int FinancialYearId { get; set; }

	public string RemarksHeader { get; set; }

	public string ScreenName { get; set; }

	public virtual string FromDepartmentName { get; set; }

	public virtual string ToDepartmentName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual List<DepartmentRequestDetail> DepartmentRequestDetail { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
