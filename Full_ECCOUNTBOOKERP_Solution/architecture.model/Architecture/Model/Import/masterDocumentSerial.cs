using System;

namespace Architecture.Model.Import;

public class masterDocumentSerial
{
	public bool isApproved { get; set; }

	public DateTime approvedOn { get; set; }

	public DateTime createdOn { get; set; }

	public DateTime invoiceDate { get; set; }

	public DateTime lastModifiedOn { get; set; }

	public DateTime locOrderDate { get; set; }

	public DateTime proformaDate { get; set; }

	public int AppActionId { get; set; }

	public int approvedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int createdUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int locationBranchId { get; set; }

	public int OrganizationId { get; set; }

	public int refDocumentTypeId { get; set; }

	public long invoiceMasterId { get; set; }

	public long lastModifiedUserId { get; set; }

	public long LocOrderMasterId { get; set; }

	public long masterDocumentSerialId { get; set; }

	public long ProformaMasterId { get; set; }

	public long ShipmentBookingId { get; set; }

	public string invoiceNo { get; set; }

	public string locOrderNo { get; set; }

	public string proformaNo { get; set; }

	public virtual string RowVersionLong { get; set; }
}
