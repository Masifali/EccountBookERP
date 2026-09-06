using System;

namespace Architecture.Model.POS;

public class WsRmCreditNoteDetail
{
	public DateTime TargetFrom { get; set; }

	public DateTime TargetTo { get; set; }

	public double IncentiveAmount { get; set; }

	public double InCentiveRate { get; set; }

	public double TargetAcheived { get; set; }

	public int CalcOn { get; set; }

	public int CreditAccountId { get; set; }

	public int IncentiveTypeId { get; set; }

	public int TargetTypeId { get; set; }

	public int PolicyId { get; set; }

	public long Id { get; set; }

	public long WsRmCreditNoteHeaderId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string TargetType { get; set; }

	public virtual string IncentiveType { get; set; }

	public virtual string CompanyName { get; set; }

	public virtual string IncentiveCalculationOn { get; set; }
}
