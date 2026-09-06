using System;

namespace Architecture.Model.lgstcm;

public class FreightVoucherOutwardPaymentDetail
{
	public DateTime CheqDate { get; set; }

	public decimal CreditAmount { get; set; }

	public decimal DebitAmount { get; set; }

	public int AccountId { get; set; }

	public int ActionTypeId { get; set; }

	public int CheqId { get; set; }

	public int FreightVoucherOutwardId { get; set; }

	public int FreightVoucherOutwardPaymentDetailId { get; set; }

	public int InstrumentTypeId { get; set; }

	public int TransTypeId { get; set; }

	public int LineId { get; set; }

	public string CheqNo { get; set; }

	public string PayeeTitle { get; set; }

	public string Remarks { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string InstrumentType { get; set; }

	public virtual string TransactionType { get; set; }
}
