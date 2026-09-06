using System;

namespace Architecture.Model.TaxProject.Accounts;

public class PdcInventory
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime CheqDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double CheqAmount { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BankId { get; set; }

	public int PdcInventoryHeaderId { get; set; }

	public int BranchId { get; set; }

	public int CheqId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int GlCreditAcId { get; set; }

	public int GlDebitAcId { get; set; }

	public int GlVoucherRefId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public string BankBranch { get; set; }

	public string BranchCode { get; set; }

	public string cheqAttachment { get; set; }

	public string CheqNo { get; set; }

	public string CheqStatus { get; set; }

	public string CheqType { get; set; }

	public string pdcType { get; set; }

	public string Remarks { get; set; }

	public virtual string BankName { get; set; }
}
