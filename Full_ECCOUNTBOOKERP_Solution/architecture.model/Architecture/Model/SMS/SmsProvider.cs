using System;

namespace Architecture.Model.SMS;

public class SmsProvider
{
	public int Id { get; set; }

	public string ProviderName { get; set; }

	public string ProviderApi { get; set; }

	public string ProviderApiToken { get; set; }

	public string ProviderApiSecret { get; set; }

	public string ProviderApiMasking { get; set; }

	public string ProviderApiAcceptType { get; set; }

	public string ProviderApiContentType { get; set; }

	public string ProviderApiMethodType { get; set; }

	public int EntryUserId { get; set; }

	public DateTime EntryDate { get; set; }

	public int ModifyUserId { get; set; }

	public DateTime ModifyDate { get; set; }

	public int PostUserId { get; set; }

	public DateTime PostDate { get; set; }

	public int BranchedId { get; set; }

	public int CompanyId { get; set; }

	public int OrganizationId { get; set; }
}
