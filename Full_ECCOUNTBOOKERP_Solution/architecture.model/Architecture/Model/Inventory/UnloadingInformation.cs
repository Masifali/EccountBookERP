using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class UnloadingInformation
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime? GpDate { get; set; }

	public DateTime? ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int GatePassId { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int LabId { get; set; }

	public int WbId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public string AttachmentsValues { get; set; }

	public string BiltyNo { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string RemarksHeader { get; set; }

	public string VehicleNo { get; set; }

	public virtual List<UnloadingInformationDetail> UnloadingInformationDetailList { get; set; }

	public virtual List<UnloadingInformationEmptyBagDetail> UnloadingInformationEmptyBagDetailList { get; set; }

	public virtual List<UnloadingInformationWagesDetail> UnloadingInformationWagesDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public UnloadingInformation()
	{
		UnloadingInformationDetailList = new List<UnloadingInformationDetail>();
		UnloadingInformationWagesDetailList = new List<UnloadingInformationWagesDetail>();
		UnloadingInformationEmptyBagDetailList = new List<UnloadingInformationEmptyBagDetail>();
	}
}
