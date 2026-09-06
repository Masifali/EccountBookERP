using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.PurchaseTrading;

public class InvStockOpeningBalanceHeader
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double ItemAmount { get; set; }

	public double ItemRate { get; set; }

	public double Qty { get; set; }

	public double WeightKgs { get; set; }

	public int PackingTypeId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomSch { get; set; }

	public int JobLotId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int FinancialYearId { get; set; }

	public int ProjectsId { get; set; }

	public int RateUomSch { get; set; }

	public int StockCrGLAcId { get; set; }

	public int RackId { get; set; }

	public int WarehouseId { get; set; }

	public int ItemVariantId { get; set; }

	public int ProductionStageId { get; set; }

	public int CastingTypeId { get; set; }

	public int BaseDocumentTypeId { get; set; }

	public int ItemConditionId { get; set; }

	public string BatchNo { get; set; }

	public string CropYear { get; set; }

	public string Remarks { get; set; }

	public string TransactionType { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string AttachmentsValues { get; set; }

	public int SecondaryUomId { get; set; }

	public decimal SecondaryUomQty { get; set; }

	public decimal SecondaryUomItemRate { get; set; }

	public virtual DateTime? PreviousDate { get; set; }

	public virtual VoucherHead VoucherHeadOpeningBalance { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
