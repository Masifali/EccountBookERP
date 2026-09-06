using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.AttendanceManagement;

public class genHoliday
{
	public byte ActionTypeId { get; set; }

	public bool IsGazetted { get; set; }

	public bool IsOffDay { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime HolidayDate { get; set; }

	public int CompanyId { get; set; }

	public int HolidayId { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long OrginizationId { get; set; }

	public long UserLogId { get; set; }

	public string HolidayDetail { get; set; }

	public string Name { get; set; }

	public virtual List<genHoliday> genHolidaysList { get; set; }
}
