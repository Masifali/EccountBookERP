using System;

namespace Architecture.Model.SMS;

public class SMSContacts
{
	public DateTime Date { get; set; }

	public int CompanyId { get; set; }

	public int GroupId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int ReferenceDocTypeId { get; set; }

	public string MobileNo { get; set; }

	public string Name { get; set; }

	public string ReferenceDocNo { get; set; }
}
