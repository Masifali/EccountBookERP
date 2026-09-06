using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImBillOfLading
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime? BLDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime ShipingCertDate { get; set; }

	public double FobValue { get; set; }

	public int ApprovedUser { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int ExImInvoiceId { get; set; }

	public int Id { get; set; }

	public int ImporterId { get; set; }

	public int ModifyUser { get; set; }

	public int NotifyPartyId { get; set; }

	public int OrderOfId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int ShipingCertNo { get; set; }

	public string BLNumber { get; set; }

	public DateTime BookingDate { get; set; }

	public string BookingNo { get; set; }

	public string CarierType { get; set; }

	public string FreightType { get; set; }

	public string NotifyPartyName { get; set; }

	public string Remarks { get; set; }

	public string ToTheOrderOfName { get; set; }

	public string VesselNo { get; set; }

	public string VoyageNo { get; set; }

	public string ImporterName { get; set; }

	public string ItemsGoodsDesc { get; set; }

	public virtual string ExImInvoiceNo { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
