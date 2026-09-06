using System;
using System.Collections.Generic;

namespace Architecture.Model.Import.Transaction;

public class ShipmentBooking
{
	public bool isApproved { get; set; }

	public DateTime approvedOn { get; set; }

	public DateTime bookingDate { get; set; }

	public DateTime createdOn { get; set; }

	public DateTime DeletedOn { get; set; }

	public DateTime docDate { get; set; }

	public DateTime etaDestinationPort { get; set; }

	public DateTime etaLoadingport { get; set; }

	public DateTime etdLoadingPort { get; set; }

	public DateTime lastModifiedOn { get; set; }

	public decimal exchangeRate { get; set; }

	public int appActionId { get; set; }

	public int approvedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int containerTypeId { get; set; }

	public int createdUserId { get; set; }

	public int currencyId { get; set; }

	public int dischargePortId { get; set; }

	public int docNo { get; set; }

	public int FinancialYearId { get; set; }

	public int freeDays { get; set; }

	public int loadingPortId { get; set; }

	public int locationBranchId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public long ShipmentBookingId { get; set; }

	public int shippingLIneId { get; set; }

	public int totalContainer { get; set; }

	public int transitDays { get; set; }

	public long DeletedUserId { get; set; }

	public long invoiceMasterId { get; set; }

	public long lastModifiedUserId { get; set; }

	public long LocOrderMasterId { get; set; }

	public string address { get; set; }

	public string AttachmentsValues { get; set; }

	public string bookingNo { get; set; }

	public string containerCollectionLocation { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string vessel { get; set; }

	public string voyage { get; set; }

	public long RowVersionLong { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
