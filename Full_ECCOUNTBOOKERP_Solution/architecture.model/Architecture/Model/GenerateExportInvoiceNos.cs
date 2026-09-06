using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class GenerateExportInvoiceNos
{
	public int MainId { get; set; }

	public int Id { get; set; }

	public string PreFix { get; set; }

	public int? SerialFrom { get; set; }

	public int? SerialTo { get; set; }

	public string FinalInvoiceNo { get; set; }

	public bool? IsActive { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int PrefixTypeId { get; set; }

	public int? EntryUserId { get; set; }

	public DateTime? EntryDate { get; set; }

	public int? ModifyUserId { get; set; }

	public DateTime? ModifyDate { get; set; }

	public virtual List<GenerateExportInvoiceNos> GenerateExportInvoiceNosList { get; set; }
}
