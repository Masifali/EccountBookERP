using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.DeviceManagement;

public class mmProductDeviceJob
{
	public int ActionTypeId { get; set; }

	public bool IsAllEmployee { get; set; }

	public bool JobPriority { get; set; }

	public DateTime? AlteredOn { get; set; }

	public DateTime? CreatedOn { get; set; }

	public DateTime? RequestFromDate { get; set; }

	public DateTime? RequestToDate { get; set; }

	public int BiometricTypeProfileId { get; set; }

	public int ProductDeviceId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long ProductDeviceJobId { get; set; }

	public long UserLogId { get; set; }

	public string EmployeeNo { get; set; }

	public string JobMessage { get; set; }

	public string JobStatus { get; set; }

	public string JobTask { get; set; }

	public virtual List<mmProductDeviceJob> MmProductDeviceJobsList { get; set; }
}
