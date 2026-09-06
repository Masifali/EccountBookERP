using System;
using System.Collections.Generic;

namespace Architecture.Model.Lab;

public class InvLabSampleAnalysisHeader
{
	public bool IsAccepted { get; set; }

	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int DocumentTypeId { get; set; }

	public int AnalysisTypeId { get; set; }

	public int InvLabAnalysisGroupId { get; set; }

	public int InvLabSampleLogRegisterId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int CommissionAgentId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int OrderId { get; set; }

	public double NoOfBagsInspected { get; set; }

	public string AnalysisPic { get; set; }

	public string Attachments { get; set; }

	public string CookingPic { get; set; }

	public string Crop { get; set; }

	public string InvSampleLogNo { get; set; }

	public string Remarks { get; set; }

	public string ReportNo { get; set; }

	public string PartyLotRefNo { get; set; }

	public string SupplierCode { get; set; }

	public virtual List<InvLabSampleAnalysisDetail> LabSampleDetail { get; set; }

	public virtual List<DMSAttachments> dmsAttachment { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string AnalysisGroupDescription { get; set; }
}
