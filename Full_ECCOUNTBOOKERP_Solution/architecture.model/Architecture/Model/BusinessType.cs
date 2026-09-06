using System;

namespace Architecture.Model;

public class BusinessType
{
	public int organizationId { get; set; }

	public string BusinessTypeName { get; set; }

	public int BusinessTypeID { get; set; }

	public int companyId { get; set; }

	public int modifyUserId { get; set; }

	public DateTime entryDate { get; set; }

	public int entryUserId { get; set; }

	public bool IsActive { get; set; }

	public string Description { get; set; }

	public DateTime modifyDate { get; set; }
}
