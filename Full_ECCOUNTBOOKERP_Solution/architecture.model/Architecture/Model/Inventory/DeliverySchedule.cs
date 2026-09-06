using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class DeliverySchedule
{
	public bool PostState { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime FromDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? PostDate { get; set; }

	public DateTime ToDate { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int ProjectId { get; set; }

	public int SupCustId { get; set; }

	public int ThisDocumentTypeId { get; set; }

	public string CustomerSchCode { get; set; }

	public string PaymentSchCode { get; set; }

	public string ProductionSchCode { get; set; }

	public string Remarks { get; set; }

	public string SupplierSchCode { get; set; }

	public virtual List<DeliveryScheduleDetail> DeliveryScheduleDetail { get; set; }

	public virtual List<DeliveryScheduleDetailDates> DeliveryScheduleDetaildate { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
