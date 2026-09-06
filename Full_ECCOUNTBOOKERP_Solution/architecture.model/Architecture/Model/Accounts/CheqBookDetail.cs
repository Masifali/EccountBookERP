using System;

namespace Architecture.Model.Accounts;

public class CheqBookDetail
{
	public int CheqBookHeaderId { get; set; }

	public int Id { get; set; }

	public string CheqNo { get; set; }

	public string CheqStatus { get; set; }

	public string OtherRemarks { get; set; }

	public bool CheqCancelStatus { get; set; }

	public DateTime? ChequeStatusDate { get; set; }

	public int StatusChangeUserId { get; set; }

	public virtual int BankId { get; set; }

	public virtual string BankName { get; set; }
}
