using System;

namespace Architecture.Model.FeedMill.Sales;

public class InvSaleInvoiceSlip
{
	public DateTime FromDate { get; set; }

	public DateTime ToDate { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int Id { get; set; }
}
