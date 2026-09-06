using System;

namespace Architecture.Model.SMS;

public class SmsHistory
{
	public bool SendStatus { get; set; }

	public DateTime? EntryDate { get; set; }

	public DateTime? SmsDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int ReceiverId { get; set; }

	public int SmsSourceId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int ToUserId { get; set; }

	public string AutoManual { get; set; }

	public string ScreenName { get; set; }

	public string SmsBody { get; set; }

	public string SmsCellNo { get; set; }

	public string SmsSourceDoc { get; set; }
}
