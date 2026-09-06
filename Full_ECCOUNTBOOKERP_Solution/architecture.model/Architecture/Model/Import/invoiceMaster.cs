using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.Import;

public class invoiceMaster
{
	public bool isApproved { get; set; }

	public DateTime approvedOn { get; set; }

	public DateTime BillOfLadingDate { get; set; }

	public DateTime createdOn { get; set; }

	public DateTime docDate { get; set; }

	public DateTime GoodsDeclarationDate { get; set; }

	public DateTime lastModifiedOn { get; set; }

	public DateTime lcOrderMasterDate { get; set; }

	public DateTime proformaMasterDate { get; set; }

	public decimal exchangeRate { get; set; }

	public double fcyAmountTotal { get; set; }

	public double GoodsDeclarationValue { get; set; }

	public double grossWeightTotal { get; set; }

	public double netWeightTotal { get; set; }

	public double NoOfPackages { get; set; }

	public int AppActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int bankIdExporter { get; set; }

	public int bankIdImporter { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int currencyId { get; set; }

	public int customGroupIdDocument { get; set; }

	public int DabitAccountId { get; set; }

	public int destinationPortId { get; set; }

	public int docNo { get; set; }

	public int documentTypeId { get; set; }

	public int fclTotal { get; set; }

	public int FinancialYearId { get; set; }

	public int incoTermId { get; set; }

	public int legalEntityId { get; set; }

	public int loadingPortId { get; set; }

	public int OrganizationId { get; set; }

	public int payementTermId { get; set; }

	public int ProjectsId { get; set; }

	public int RevisionNo { get; set; }

	public int salePersonId { get; set; }

	public long appUserLogId { get; set; }

	public long ConsigneeId { get; set; }

	public long createdUserId { get; set; }

	public long exporterId { get; set; }

	public long importerId { get; set; }

	public long invoiceMasterId { get; set; }

	public long lastModifiedUserId { get; set; }

	public long lcOrderMasterId { get; set; }

	public long masterDocumentSerialId { get; set; }

	public long proformaMasterId { get; set; }

	public long RowVersionLong { get; set; }

	public string AttachmentsValues { get; set; }

	public string BillOfLadingNo { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string docStatus { get; set; }

	public string exporterRefNo { get; set; }

	public string GoodsDeclarationNo { get; set; }

	public string importerRefNo { get; set; }

	public string invoiceMasterNo { get; set; }

	public string remarksHeader { get; set; }

	public virtual List<invoiceDetail> invoiceDetailList { get; set; }

	public virtual List<invoiceOtherItem> invoiceOtherItemList { get; set; }

	public virtual List<invoicePaymentTerm> invoicePaymentTermList { get; set; }

	public virtual VoucherHead VoucherHeadExImInvoices { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
