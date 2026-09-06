using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class FcyAdjustmentVoucher
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EnteyDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal ExchangeRate { get; set; }

	public double HcyTotalAmount { get; set; }

	public double FcyTotalAmount { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FcyId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<FcyAdjustmentVoucherDetail> FcyAdjustmentVoucherDetailslist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }
}
