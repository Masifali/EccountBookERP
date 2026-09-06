using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class GDBreakUpBankRequestHeader
{
	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int BankId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<GDBreakUpBankRequestDetail> GdBreakUpBankRequestDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
