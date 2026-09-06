using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class genShiftLocation
{
	public bool IsActive { get; set; }

	public DateTime? AlteredOn { get; set; }

	public DateTime? CreatedOn { get; set; }

	public int ActionTypeId { get; set; }

	public int AlteredById { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int LocationId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int ShiftId { get; set; }

	public int ShiftLocationId { get; set; }

	public int UserLogId { get; set; }

	public virtual string LocationName { get; set; }

	public virtual string ShiftName { get; set; }
}
