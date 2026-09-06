using System;

namespace Architecture.Model.Accounts;

public class PaymentByInvoiceOtherDetail
{
	public DateTime CheqDate { get; set; }

	public double Amount { get; set; }

	public int CheqId { get; set; }

	public int GLAccountId { get; set; }

	public int Id { get; set; }

	public int PaymentByInvoiceHeaderId { get; set; }

	public string CheqNo { get; set; }

	public string PayTitle { get; set; }

	public string Remarks { get; set; }

	public string TransType { get; set; }

	public virtual string AccountTitle { get; set; }
}
