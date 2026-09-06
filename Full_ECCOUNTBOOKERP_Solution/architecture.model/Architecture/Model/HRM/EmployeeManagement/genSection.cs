using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class genSection
{
	public int SectionId { get; set; }

	public int DepartmentId { get; set; }

	public string ShortName { get; set; }

	public string SectionName { get; set; }

	public string SectionCode { get; set; }

	public int SectionSeqNo { get; set; }

	public int CreatedById { get; set; }

	public DateTime? CreatedOn { get; set; }

	public int AlteredById { get; set; }

	public DateTime? AlteredOn { get; set; }

	public int ActionTypeId { get; set; }

	public int UserLogId { get; set; }
}
