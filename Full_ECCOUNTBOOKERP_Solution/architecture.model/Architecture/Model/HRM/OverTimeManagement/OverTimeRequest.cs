using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.OverTimeManagement;

public class OverTimeRequest
{
	public byte ActionTypeId { get; set; }

	public bool IsApproved { get; set; }

	public bool IsOffDuty { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime ApprovedOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime OverTimeDate { get; set; }

	public DateTime RequestDate { get; set; }

	public int DepartmentId { get; set; }

	public int SectionId { get; set; }

	public long AlteredById { get; set; }

	public long ApprovedById { get; set; }

	public long CompanyId { get; set; }

	public long CreatedById { get; set; }

	public long LocationId { get; set; }

	public long OrganizationId { get; set; }

	public long OverTimeRequestId { get; set; }

	public long RequestById { get; set; }

	public long UserLogId { get; set; }

	public string Reason { get; set; }

	public virtual List<OverTimeRequestDetail> OverTimeRequestDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
