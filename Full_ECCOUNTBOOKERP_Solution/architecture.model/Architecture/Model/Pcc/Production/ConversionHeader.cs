using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.Model.pcc.Production;

public class ConversionHeader
{
	public bool IsApproved { get; set; }

	public int Id { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int ConversionTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PlantId { get; set; }

	public int ProjectsId { get; set; }

	public string ConversionNo { get; set; }

	public string RemarksHeader { get; set; }

	public virtual string WorkOrderNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemUomCode { get; set; }

	public virtual string ItemUomEquivalent { get; set; }

	public virtual List<ConversionOutPutDetail> ConversionOutPutDetailList { get; set; }

	public virtual List<ConversionOverHeadsDetail> ConversionOverHeadsDetailList { get; set; }

	public virtual List<ConversionInputDetail> ConversionInputDetailList { get; set; }

	public virtual List<ConversionWagesDetail> ConversionWagesDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }
}
