using System;

namespace Architecture.Model.Export;

public class ExImLcContractScheduleDepartment
{
	public DateTime InspectionSchedule { get; set; }

	public DateTime InspectionStart { get; set; }

	public DateTime PackMaterialSchedule { get; set; }

	public DateTime PackMaterialStart { get; set; }

	public DateTime ProductionSchedule { get; set; }

	public DateTime ProductionStart { get; set; }

	public decimal Weight { get; set; }

	public int ActionTypeId { get; set; }

	public int ContractScheduleDetailItemId { get; set; }

	public int ContractScheduleId { get; set; }

	public int Id { get; set; }

	public int Plant { get; set; }

	public int ScheduleLockId { get; set; }
}
