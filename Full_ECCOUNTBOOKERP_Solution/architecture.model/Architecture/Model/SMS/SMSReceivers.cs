using System;

namespace Architecture.Model.SMS;

public class SMSReceivers
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string MobileNo { get; set; }

	public string ReferenceDocNo { get; set; }

	public int ReferenceDocTypeId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public bool Added { get; set; }

	public DateTime Date { get; set; }
}
