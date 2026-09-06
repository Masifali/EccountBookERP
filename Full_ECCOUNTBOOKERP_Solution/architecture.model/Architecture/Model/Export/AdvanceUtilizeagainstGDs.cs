using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class AdvanceUtilizeagainstGDs
{
	public int Id { get; set; }

	public int GDId { get; set; }

	public int EximInvoiceId { get; set; }

	public int FIId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int DocumentTypeId { get; set; }

	public int PaymentTermId { get; set; }

	public int InvoiceDocTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public string FINo { get; set; }

	public decimal UtilizeAmount { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public virtual string GDNO { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual string Ids { get; set; }

	public virtual List<AdvanceUtilizeagainstGDs> AdvanceUtilizeagainstGDslist { get; set; }
}
