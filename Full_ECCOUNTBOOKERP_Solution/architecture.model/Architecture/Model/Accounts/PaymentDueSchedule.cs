using System;

namespace Architecture.Model.Accounts;

public class PaymentDueSchedule
{
	public DateTime DueDate { get; set; }

	public double Amount { get; set; }

	public double DuePercentage { get; set; }

	public int DueDays { get; set; }

	public int RefdocNoId { get; set; }

	public int refDocumentTypeId { get; set; }

	public long PaymentDueScheduleId { get; set; }
}
