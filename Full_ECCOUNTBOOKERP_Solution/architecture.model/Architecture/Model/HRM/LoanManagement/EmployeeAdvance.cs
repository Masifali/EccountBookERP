using System;

namespace Architecture.Model.HRM.LoanManagement;

public class EmployeeAdvance
{
	public int ActionTypeId { get; set; }

	public bool IsApproved { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime AppliedOn { get; set; }

	public DateTime ApprovedOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime DocDate { get; set; }

	public decimal AdvanceAmount { get; set; }

	public decimal ApprovedAmount { get; set; }

	public decimal PreviousApprovedAmount { get; set; }

	public decimal PreviousAdvanceAmount { get; set; }

	public int DocNo { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EmployeeAdvanceId { get; set; }

	public int OrganizationId { get; set; }

	public int PayrollMonth { get; set; }

	public int PayrollYear { get; set; }

	public int ProjectId { get; set; }

	public long AlteredById { get; set; }

	public long ApprovedById { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeId { get; set; }

	public long LocationId { get; set; }

	public long UserLogId { get; set; }

	public string Reason { get; set; }
}
