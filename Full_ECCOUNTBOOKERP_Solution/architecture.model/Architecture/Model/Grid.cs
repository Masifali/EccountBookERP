using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class Grid
{
	public int GridId { get; set; }

	public string GridName { get; set; }

	public string GridTitle { get; set; }

	public int InitializeRows { get; set; }

	public bool IsLookUpGrid { get; set; }

	public bool IsReadOnly { get; set; }

	public bool IsAllowUserToAddRow { get; set; }

	public bool IsAllowUserToDeleteRow { get; set; }

	public DateTime CreatedOn { get; set; }

	public long? CreatedUserId { get; set; }

	public DateTime LastModifiedOn { get; set; }

	public long? LastModifiedUserId { get; set; }

	public long? AppUserLogId { get; set; }

	public byte[] RowVersion { get; set; }

	public int AppActionId { get; set; }

	public virtual List<GridDetail> GridDetails { get; set; }

	public Grid()
	{
		GridDetails = new List<GridDetail>();
	}
}
