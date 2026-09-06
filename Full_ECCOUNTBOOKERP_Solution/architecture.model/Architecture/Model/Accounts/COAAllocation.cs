using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class COAAllocation
{
	public bool IsActive { get; set; }

	public int ChartofAccountId { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int BranchId { get; set; }

	public string GLPageNo { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual int OrganizationId { get; set; }

	public virtual int DocumentTypeId { get; set; }

	public virtual int AccountTypeId { get; set; }

	public virtual int EntryUserId { get; set; }

	public virtual int FinancialYearId { get; set; }

	public virtual List<COAAllocation> COAAllocationlist { get; set; }
}
