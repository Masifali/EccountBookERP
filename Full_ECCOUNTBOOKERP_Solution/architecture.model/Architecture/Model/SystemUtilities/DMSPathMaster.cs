using System;
using System.Collections.Generic;

namespace Architecture.Model.SystemUtilities;

public class DMSPathMaster
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime UploadedDateTime { get; set; }

	public int ApprovedUserId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string DocumentName { get; set; }

	public string DocumentPath { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<DMSPathAllocationToParty> DMSPathAllocationToPartieslist { get; set; }
}
