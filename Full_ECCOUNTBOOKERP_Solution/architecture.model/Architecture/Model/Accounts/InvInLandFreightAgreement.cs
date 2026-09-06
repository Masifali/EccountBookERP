using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class InvInLandFreightAgreement
{
	public bool IsApproved { get; set; }

	public DateTime AgreementDate { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUser { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int DocNo { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransporterId { get; set; }

	public string AgreementCode { get; set; }

	public virtual List<InvInLandFreightAgreementDetail> InvInLandFreightAgreementDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
