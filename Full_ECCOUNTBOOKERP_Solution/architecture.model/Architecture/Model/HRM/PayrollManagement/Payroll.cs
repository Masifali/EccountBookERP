using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.HRM.PayrollManagement;

public class Payroll
{
	public bool IsApproved { get; set; }

	public byte ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime PayrollDate { get; set; }

	public int DocMovementId { get; set; }

	public int PayrollMonth { get; set; }

	public int PayrollYear { get; set; }

	public int DocNo { get; set; }

	public int FinancialYearId { get; set; }

	public int BranchesId { get; set; }

	public int ProjectsId { get; set; }

	public string Remarks { get; set; }

	public long AlteredById { get; set; }

	public long CompanyId { get; set; }

	public long CreatedById { get; set; }

	public long LocationId { get; set; }

	public long OrganizationId { get; set; }

	public long PayrollId { get; set; }

	public long PostedById { get; set; }

	public long UserLogId { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual List<PayrollDetail> PayrollDetailList { get; set; }
}
