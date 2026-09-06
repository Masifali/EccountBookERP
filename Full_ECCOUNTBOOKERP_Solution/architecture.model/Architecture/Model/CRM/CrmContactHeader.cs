using System;
using System.Collections.Generic;

namespace Architecture.Model.CRM;

public class CrmContactHeader
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int ContactCityId { get; set; }

	public int ContactCountryId { get; set; }

	public int ContactTitle { get; set; }

	public int crmContactGroupId { get; set; }

	public int EntryUserId { get; set; }

	public int ErpSupplierCustomerId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public long CrmContactCode { get; set; }

	public string CompanyType { get; set; }

	public string ContactCompanyName { get; set; }

	public string ContactFatherName { get; set; }

	public string ContactFirstName { get; set; }

	public string ContactLastName { get; set; }

	public string ContactMiddleName { get; set; }

	public string ContactName { get; set; }

	public virtual List<CrmContactDetail> crmContactDetails { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
