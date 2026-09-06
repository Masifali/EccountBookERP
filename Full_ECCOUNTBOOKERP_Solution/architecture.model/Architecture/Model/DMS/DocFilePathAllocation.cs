using System;
using System.Collections.Generic;

namespace Architecture.Model.DMS;

public class DocFilePathAllocation
{
	public DateTime EnteryDate { get; set; }

	public int CompanyId { get; set; }

	public long DocFilePathId { get; set; }

	public int EnteryUserId { get; set; }

	public long Id { get; set; }

	public int OrganizationId { get; set; }

	public int SubUserId { get; set; }

	public virtual List<DocFilePathAllocation> DocFilePathAllocationslist { get; set; }
}
