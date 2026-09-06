using System;
using System.Collections.Generic;

namespace Architecture.Model.Lab;

public class InvLabAnalysisInProcessHeader
{
	public bool IsAccepted { get; set; }

	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime DocTime { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int DocumentTypeId { get; set; }

	public int InvLabAnalysisGroup { get; set; }

	public int ItemId { get; set; }

	public int LotNo { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int Plant { get; set; }

	public int ProductionNo { get; set; }

	public int ProjectsId { get; set; }

	public int SaleOrderId { get; set; }

	public string AnalysisPic { get; set; }

	public string AnalyzedBy { get; set; }

	public string CookingPic { get; set; }

	public string Crop { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<InvLabAnalysisInProcessDetail> LabSampleDetail { get; set; }

	public virtual List<DMSAttachments> dmsAttachment { get; set; }
}
