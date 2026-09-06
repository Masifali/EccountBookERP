using System;

namespace Architecture.Model.Inventory;

public class InvFreightVoucherPaymentBreakup
{
	public decimal Amount { get; set; }

	public int AccountId { get; set; }

	public int CheqId { get; set; }

	public DateTime? CheqDate { get; set; }

	public int Id { get; set; }

	public int InstrumentTypeId { get; set; }

	public int InvFreightVoucherId { get; set; }

	public int TransTypeId { get; set; }

	public string CheqNo { get; set; }

	public string PayeeTitle { get; set; }

	public string Remarks { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string InstrumentType { get; set; }

	public virtual string TransactionType { get; set; }
}
