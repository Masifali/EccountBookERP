using System;
using System.Collections.Generic;

namespace Architecture.Model.Production;

public class ProductionManualCosting
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime CostingDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime? ModifyDate { get; set; }

	public decimal ByProductCost { get; set; }

	public decimal FinishedOutputPercent { get; set; }

	public decimal InputAvgRate { get; set; }

	public decimal InputToBeProcessed { get; set; }

	public decimal ProcessingCharges { get; set; }

	public decimal RateInPMT { get; set; }

	public decimal TotalCost { get; set; }

	public decimal TotalCostInPMT { get; set; }

	public int ActionId { get; set; }

	public int ApprovalUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProductionJobOrderId { get; set; }

	public string RemarksHeader { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public virtual List<ProductionManualCostingByProductDetail> ProductionManualCostingByProductDetailList { get; set; }

	public virtual List<ProductionManualCostingInputDetail> ProductionManualCostingInputDetailList { get; set; }

	public virtual List<ProductionManualCostingInvoiceDetail> ProductionManualCostingInvoiceDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
