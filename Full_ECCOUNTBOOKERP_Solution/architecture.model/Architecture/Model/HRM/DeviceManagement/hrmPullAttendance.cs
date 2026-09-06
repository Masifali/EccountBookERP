using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.DeviceManagement;

public class hrmPullAttendance
{
	public int ActionTypeId { get; set; }

	public bool IsTransfered { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CheckDateTime { get; set; }

	public DateTime CheckTime { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime MachineDateTime { get; set; }

	public int ProductDeviceId { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long LocationId { get; set; }

	public long PullAttendanceId { get; set; }

	public long UserLogId { get; set; }

	public string CheckFlag { get; set; }

	public string EmployeeNo { get; set; }

	public string VerifyMode { get; set; }

	public virtual List<hrmPullAttendance> HrmPullAttendancesList { get; set; }
}
