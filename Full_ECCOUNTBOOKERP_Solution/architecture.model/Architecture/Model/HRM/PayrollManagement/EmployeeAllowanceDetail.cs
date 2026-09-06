using System;

namespace Architecture.Model.HRM.PayrollManagement;

public class EmployeeAllowanceDetail
{
	public byte ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime BillDate { get; set; }

	public DateTime CreatedOn { get; set; }

	public decimal Amount { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeAllowanceDetailId { get; set; }

	public long EmployeeAllowanceId { get; set; }

	public long UserLogId { get; set; }

	public string BillPartyName { get; set; }

	public string BillRefNo { get; set; }

	public string Remarks { get; set; }
}
