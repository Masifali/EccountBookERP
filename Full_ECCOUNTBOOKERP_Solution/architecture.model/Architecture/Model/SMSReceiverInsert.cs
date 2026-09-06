using System;

namespace Architecture.Model;

public class SMSReceiverInsert
{
	public int Id { get; set; }

	public int Screen_Id { get; set; }

	public int ToCompanyId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public DateTime Date { get; set; }
}
