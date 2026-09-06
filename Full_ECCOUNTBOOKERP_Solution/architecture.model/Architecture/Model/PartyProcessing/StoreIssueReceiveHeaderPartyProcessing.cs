using System;
using System.Collections.Generic;

namespace Architecture.Model.PartyProcessing;

public class StoreIssueReceiveHeaderPartyProcessing
{
	public bool IsApproved { get; set; }

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

	public int EntryUser { get; set; }

	public int FromStoreId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefEntryNoId { get; set; }

	public int StockPartyId { get; set; }

	public int ToStoreId { get; set; }

	public int TransactionTypeId { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<StoreIssueReceiveHeaderDetailPartyProcessing> StoreIssueReceivesList { get; set; }
}
