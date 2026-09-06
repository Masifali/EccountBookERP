using System;
using Architecture.Model.Accounts;

namespace Architecture.Model.POS;

public class WsRmShiftClsoing
{
	public bool IsApprvoed { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime ClosingDate { get; set; }

	public DateTime ClosingDatetime { get; set; }

	public DateTime ClosingTime { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double CashSubmitted { get; set; }

	public double ClosingCash { get; set; }

	public double PhysicalCash { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int ClosingNo { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int JobLotId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int ShiftId { get; set; }

	public string ClosingRemarks { get; set; }

	public string MachinePosId { get; set; }

	public string ShiftDayStatus { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }
}
