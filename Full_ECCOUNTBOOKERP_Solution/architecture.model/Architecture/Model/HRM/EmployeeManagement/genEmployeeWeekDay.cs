using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class genEmployeeWeekDay
{
	public byte ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int LineId { get; set; }

	public int WeekDayProfileId { get; set; }

	public long AlteredById { get; set; }

	public long CompanyId { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeId { get; set; }

	public long EmployeeWeekDayId { get; set; }

	public long OrganizationId { get; set; }

	public long UserLogId { get; set; }

	public string WeekDayName { get; set; }

	public virtual string EmployeeName { get; set; }

	public virtual string WeekDayProfileName { get; set; }
}
