using System;

namespace Architecture.Model.ModernHistoryModels;

public class VouchersHistory_Header
{
	public int VoucherHeadId { get; set; }

	public int DocumentTypeId { get; set; }

	public string DocumentTypeDescription { get; set; }

	public string DocumentType { get; set; }

	public int VoucherCode { get; set; }

	public DateTime VoucherDate { get; set; }

	public int RefDocNoId { get; set; }

	public int RefAccountId { get; set; }

	public string HeaderAccountTitle { get; set; }

	public string PayTitle { get; set; }

	public string ChequeNo { get; set; }

	public DateTime ChequeDate { get; set; }

	public string Remarks { get; set; }

	public int AttachmentsCount { get; set; }

	public int NoOfAttachments { get; set; }

	public string EntryUserName { get; set; }

	public int EntryUser { get; set; }

	public DateTime EntryDate { get; set; }

	public string ModifyUserName { get; set; }

	public int ModifyUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public string ApprovalUserName { get; set; }

	public int PostUser { get; set; }

	public DateTime PostDate { get; set; }

	public string EntryUserProfileImageUrl { get; set; }

	public string ModifyUserProfileImageUrl { get; set; }

	public string ApprovalUserProfileImageUrl { get; set; }

	public string Wht_AgainstAccount { get; set; }

	public string Wht_Account { get; set; }

	public bool IsApproved { get; set; }

	public bool IncludeWHT { get; set; }

	public string TaxType { get; set; }

	public double TaxPercent { get; set; }

	public double TaxAmount { get; set; }

	public double VoucherAmount { get; set; }

	public bool ActionTypeId { get; set; }

	public string NotesByApprovalPersonPresent { get; set; }
}
