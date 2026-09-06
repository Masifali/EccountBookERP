using System;
using System.Collections.Generic;

namespace Architecture.Model.Lab;

public class LabInProcessAnalysisRawHeader
{
	public bool IsApproved { get; set; }

	public DateTime AnalysisTime { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUser { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int AnalysisGroupId { get; set; }

	public int CropYearId { get; set; }

	public int DocNo { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int FinancialYearId { get; set; }

	public int ItemId { get; set; }

	public int JobOrderId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PlantId { get; set; }

	public string AnalysisPic { get; set; }

	public string AnalystPerson { get; set; }

	public string CookingPic { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<InvLabAnalysisInProcessDetail> AnalysisInProcessDetail { get; set; }

	public virtual List<LabInProcessAnalysisRawDetail> LabInProcessAnalysisRawDetail { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<(string, int)> ImagesList { get; set; }

	public virtual List<string> ImagesDeleteList { get; set; }
}
