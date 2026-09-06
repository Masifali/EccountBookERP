using System;
using System.Collections.Generic;

namespace Architecture.Model.Import;

public class ProformaMaster
{
	public int appActionId { get; set; }

	public bool isApproved { get; set; }

	public DateTime approvedOn { get; set; }

	public DateTime createdOn { get; set; }

	public DateTime dateOfIssue { get; set; }

	public DateTime docDate { get; set; }

	public DateTime lastModifiedOn { get; set; }

	public DateTime lastShipmentDate { get; set; }

	public DateTime validityUpto { get; set; }

	public decimal fclTotal { get; set; }

	public decimal fcyAmountTotal { get; set; }

	public decimal grossWeightTotal { get; set; }

	public decimal netWeightTotal { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int currencyId { get; set; }

	public int destinationPortId { get; set; }

	public int docNo { get; set; }

	public int documentTypeId { get; set; }

	public int FinancialYearId { get; set; }

	public int incoTermId { get; set; }

	public int loadingPortId { get; set; }

	public int OrganizationId { get; set; }

	public int placeOfIssue { get; set; }

	public int ProjectsId { get; set; }

	public int paymentTermId { get; set; }

	public int GoodsOriginId { get; set; }

	public long appUserLogId { get; set; }

	public long bankIdExporter { get; set; }

	public long bankIdImporter { get; set; }

	public long clientLocationId { get; set; }

	public long createdUserId { get; set; }

	public long exporterId { get; set; }

	public long importerId { get; set; }

	public long lastModifiedUserId { get; set; }

	public long masterDocumentSerialId { get; set; }

	public long proformaMasterId { get; set; }

	public long RowVersionLong { get; set; }

	public long salePersonId { get; set; }

	public long stationBranchId { get; set; }

	public string TermsConditions { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string docStatus { get; set; }

	public string exporterRefNo { get; set; }

	public string importerRefNo { get; set; }

	public string proformaNo { get; set; }

	public string remarksHeader { get; set; }

	public virtual List<ProformaDetail> ProformaDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
