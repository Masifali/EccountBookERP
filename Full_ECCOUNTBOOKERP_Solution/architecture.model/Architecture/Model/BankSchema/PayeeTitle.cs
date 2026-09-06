using System;

namespace Architecture.Model.BankSchema;

public class PayeeTitle
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int refDocumentTypeId { get; set; }

	public int refSupplierCustomerId { get; set; }

	public bool isActive { get; set; }

	public long payeeTitleId { get; set; }

	public string payeeTitle { get; set; }
}
