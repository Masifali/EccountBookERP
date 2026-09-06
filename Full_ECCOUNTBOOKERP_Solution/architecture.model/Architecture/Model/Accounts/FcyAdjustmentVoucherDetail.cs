using System;

namespace Architecture.Model.Accounts;

public class FcyAdjustmentVoucherDetail
{
	public DateTime DetailEntryDate { get; set; }

	public double FcyAmount { get; set; }

	public double HcyAmount { get; set; }

	public int ActionId { get; set; }

	public int CreditAccountId { get; set; }

	public int DebitAccountId { get; set; }

	public int DetailEntryUserId { get; set; }

	public int FcyAdjustmentVoucherId { get; set; }

	public int Id { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int SortNo { get; set; }

	public int TransactionReasonId { get; set; }

	public string CreditRemarks { get; set; }

	public string DebitRemarks { get; set; }

	public virtual string CreditAccountTitle { get; set; }

	public virtual string DebitAccountTitle { get; set; }

	public virtual string RefDocumentType { get; set; }

	public virtual string RefDocNo { get; set; }

	public virtual string TransactionType { get; set; }
}
