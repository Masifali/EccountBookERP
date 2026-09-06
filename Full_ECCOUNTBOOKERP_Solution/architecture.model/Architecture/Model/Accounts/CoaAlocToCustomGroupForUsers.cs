using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class CoaAlocToCustomGroupForUsers
{
	public int BranchId { get; set; }

	public int ChartOfAccountId { get; set; }

	public int CoaCustomGroupForUsersId { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public virtual List<CoaAlocToCustomGroupForUsers> CoaAlocToCustomGroupForUsersList { get; set; }
}
