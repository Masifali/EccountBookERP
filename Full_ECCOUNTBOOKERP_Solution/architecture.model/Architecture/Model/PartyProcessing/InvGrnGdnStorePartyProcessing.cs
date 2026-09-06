using System;
using System.Collections.Generic;

namespace Architecture.Model.PartyProcessing;

public class InvGrnGdnStorePartyProcessing
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int FinancialYearId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDoNo { get; set; }

	public int RefEntryIdNo { get; set; }

	public int ReferencePartyId { get; set; }

	public int StockPartyId { get; set; }

	public string BiltyNo { get; set; }

	public string RemarksHeader { get; set; }

	public string VehicleNo { get; set; }

	public string ScreenName { get; set; }

	public virtual string DocumentTypeDescription { get; set; }

	public virtual List<InvGrnGdnStoreDetailPartyProcessing> InvGrnGdnStoreDetails { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
