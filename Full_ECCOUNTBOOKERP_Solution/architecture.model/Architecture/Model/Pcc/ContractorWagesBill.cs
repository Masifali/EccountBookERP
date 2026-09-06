using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.pcc;

public class ContractorWagesBill
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime RefDocDate { get; set; }

	public double TotalQty { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public int CurrencyId { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int AutoUpdate { get; set; }

	public int RefDocNo { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string RemarksHeader { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string AttachmentsValues { get; set; }

	public virtual List<ContractorWagesBillDetail> ContractorWagesBillDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadForContractorWages { get; set; }
}
