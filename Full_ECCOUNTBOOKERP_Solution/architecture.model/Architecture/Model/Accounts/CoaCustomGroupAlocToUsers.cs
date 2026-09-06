using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class CoaCustomGroupAlocToUsers
{
	public int CoaAlocToCustomGroupForUsersId { get; set; }

	public int Id { get; set; }

	public int UsersId { get; set; }

	public virtual int OrganizationId { get; set; }

	public virtual int CompanyId { get; set; }

	public virtual List<CoaCustomGroupAlocToUsers> CoaCustomGroupAlocToUsersList { get; set; }
}
