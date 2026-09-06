using System.Collections.Generic;

namespace Architecture.Model;

public class ApplicationsAllocateToCompany
{
	public int Id { get; set; }

	public int CompanyId { get; set; }

	public int AppId { get; set; }

	public bool IsActive { get; set; }

	public virtual string AppName { get; set; }

	public virtual string CompanyName { get; set; }

	public virtual string CompanyCode { get; set; }

	public virtual List<ApplicationsAllocateToCompany> ApplicationsAllocateToCompanyList { get; set; }
}
