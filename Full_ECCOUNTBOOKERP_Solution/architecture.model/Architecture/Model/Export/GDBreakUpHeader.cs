using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class GDBreakUpHeader
{
	public DateTime BankInvoiceDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime GDDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal CommAmount { get; set; }

	public decimal CommPercent { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FobValue { get; set; }

	public decimal Freight { get; set; }

	public decimal GDValue { get; set; }

	public decimal GDWeight { get; set; }

	public decimal NetToBeRealized { get; set; }

	public int ActionId { get; set; }

	public int BankId { get; set; }

	public int DocumentTypeId { get; set; }

	public int CompanyId { get; set; }

	public int DueDays { get; set; }

	public int EntryUserId { get; set; }

	public int GDNoofContrainers { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string BankInvoiceNo { get; set; }

	public string GDNO { get; set; }

	public string Remarks { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public virtual List<GDBreakUpDetail> GDBreakUpDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
