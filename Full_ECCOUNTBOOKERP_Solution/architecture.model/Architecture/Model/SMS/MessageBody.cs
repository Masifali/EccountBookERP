using System;

namespace Architecture.Model.SMS;

public class MessageBody
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int GroupId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public string SMSBody { get; set; }

	public string ScreenName { get; set; }

	public string SMSTemplateName { get; set; }
}
