using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class ApplicationsAllocateToUser
{
	public long ApplicationsAllocateToUserId { get; set; }

	public int UserId { get; set; }

	public int CompanyId { get; set; }

	public int AppId { get; set; }

	public bool IsActive { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public virtual List<ApplicationsAllocateToUser> ApplicationsAllocateToUserList { get; set; }
}
