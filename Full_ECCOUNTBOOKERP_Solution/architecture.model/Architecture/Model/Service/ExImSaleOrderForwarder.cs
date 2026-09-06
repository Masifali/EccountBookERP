using System;
using System.Collections.Generic;

namespace Architecture.Model.Service;

public class ExImSaleOrderForwarder
{
	public bool IsApproved { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double NetBillWeight { get; set; }

	public int Branch { get; set; }

	public int CompanyId { get; set; }

	public int DestinationPortId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int InvoiceId { get; set; }

	public int LcContractId { get; set; }

	public int LoadingPortId { get; set; }

	public int ModifyUser { get; set; }

	public int NoofContainers { get; set; }

	public int OrganizationId { get; set; }

	public int ShippingLineId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string InvoiceNo { get; set; }

	public string ReamarksHeader { get; set; }

	public virtual List<ExImSaleOrderForwarderDetail> ExImSaleOrderForwarderDetail { get; set; }
}
