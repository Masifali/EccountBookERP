using System;
using System.Collections.Generic;

namespace Architecture.Model.TaxProject.Accounts;

public class VoucherHead
{
	public virtual VoucherType Type { get; set; }

	public bool IncludeWHT { get; set; }

	public bool IsApproved { get; set; }

	public bool PostState { get; set; }

	public DateTime? ChequeDate { get; set; }

	public DateTime? DueDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? PostDate { get; set; }

	public DateTime VoucherDate { get; set; }

	public double BillAmount { get; set; }

	public double ExchangeCurrencyRate { get; set; }

	public double FcAmount { get; set; }

	public double VoucherAmount { get; set; }

	public double CostCenterAmount { get; set; }

	public int AgainstAccountId { get; set; }

	public int BranchId { get; set; }

	public int CheqId { get; set; }

	public int ChequePrintId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int DocumentTypeSrNo { get; set; }

	public int DueDays { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int MultiCurrencyId { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int ProjectId { get; set; }

	public int RefAccountId { get; set; }

	public int RefDocNoId { get; set; }

	public int VoucherCode { get; set; }

	public int ActionId { get; set; }

	public string BankBranch { get; set; }

	public string ChequeNo { get; set; }

	public string ConversionFormula { get; set; }

	public string DrCrNoteType { get; set; }

	public string ManualBillNo { get; set; }

	public string PayTitle { get; set; }

	public string Remarks { get; set; }

	public string RemarksOtherLingo { get; set; }

	public string Source { get; set; }

	public string AttachmentsValues { get; set; }

	public int RefDocumentTypeId { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public bool CustomAccounts { get; set; }

	public int BaseDocumentTypeId { get; set; }

	public int AdvanceTaxAccountId { get; set; }

	public double AdvanceTaxAmount { get; set; }

	public int OtherChargesAccountId { get; set; }

	public double OtherChargesAmount { get; set; }

	public virtual List<VoucherDetail> voucherDetailList { get; set; }

	public virtual List<PaymentDueSchedule> PaymentDuesSchedules { get; set; }

	public virtual List<voucherCostCenterDetail> costCenterDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DetailAttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public VoucherHead()
	{
		voucherDetailList = new List<VoucherDetail>();
		PaymentDuesSchedules = new List<PaymentDueSchedule>();
		costCenterDetailList = new List<voucherCostCenterDetail>();
	}
}
