using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.AttendanceManagement;

public class genDutyRoaster
{
	public int ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime FromDate { get; set; }

	public DateTime ToDate { get; set; }

	public int DepartmentId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int EmployeeCategoryId { get; set; }

	public int SectionId { get; set; }

	public int ShiftId { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long DutyRoasterId { get; set; }

	public long LocationId { get; set; }

	public long PartyLocationId { get; set; }

	public long UserLogId { get; set; }

	public string Description { get; set; }

	public virtual List<genDutyRoasterDetail> GenDutyRoasterDetailsList { get; set; }

	public virtual List<DutyRoasterGroups> DutyRoasterGroupsList { get; set; }
}
