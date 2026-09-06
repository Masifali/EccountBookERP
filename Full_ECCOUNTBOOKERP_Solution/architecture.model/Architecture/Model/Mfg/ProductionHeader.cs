using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.Mfg;

public class ProductionHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int IsSettled { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int ProductionHeaderId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public string RemarksHeader { get; set; }

	public string ProductionNo { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public DateTime inTime { get; set; }

	public DateTime outTime { get; set; }

	public int heatCode { get; set; }

	public int BomHeaderId { get; set; }

	public int CastingTypeId { get; set; }

	public int JobLotId { get; set; }

	public string batchCode { get; set; }

	public virtual List<ProductionInputDetail> ProductionInputDetailslist { get; set; }

	public virtual List<ProductionOverHeadsDetail> ProductionOverHeadsDetailList { get; set; }

	public virtual List<ProductionOutPutDetail> ProductionOutPutDetailslist { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
