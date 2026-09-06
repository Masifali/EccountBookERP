using System;
using System.Collections.Generic;

namespace Architecture.Model.Import;

public class ImProformaInvoice
{
	public bool IsApproved { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime LastShipmentDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double FcAmountTotal { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int MultiCurrencyId { get; set; }

	public int NoOfContainer { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int SeaPortsIdDestination { get; set; }

	public int SeaPortsIdLoading { get; set; }

	public int SupplierCustomerId { get; set; }

	public string CustomerRefNo { get; set; }

	public string InquieryReference { get; set; }

	public string OtherTermsConditions { get; set; }

	public string PaymentDetail { get; set; }

	public string QuotReference { get; set; }

	public string RemarksHeader { get; set; }

	public string ShipmentDetail { get; set; }

	public string Status { get; set; }

	public virtual List<ImProformaInvoiceDetail> ProformaInvoiceDetail { get; set; }
}
