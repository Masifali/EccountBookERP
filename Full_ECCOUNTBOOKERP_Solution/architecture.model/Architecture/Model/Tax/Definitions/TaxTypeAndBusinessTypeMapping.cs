using System;
using System.Collections.Generic;

namespace Architecture.Model.Tax.Definitions;

public class TaxTypeAndBusinessTypeMapping
{
	public int OrganizationId { get; set; }

	public long Id { get; set; }

	public int BusinessTypeId { get; set; }

	public int CompanyId { get; set; }

	public int ModifyUserId { get; set; }

	public DateTime EntryDate { get; set; }

	public int TaxTypeId { get; set; }

	public int EntryUserId { get; set; }

	public bool IsActive { get; set; }

	public DateTime ModifyDate { get; set; }

	public virtual List<TaxTypeAndBusinessTypeMapping> TaxTypeAndBusinessTypeMappingList { get; set; }
}
