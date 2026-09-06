using System;

namespace Architecture.Model.DMS;

public class FolderHierarchy
{
	public DateTime CreatedDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int FolderLevel { get; set; }

	public int OrganizationId { get; set; }

	public long CreatedById { get; set; }

	public string DriveName { get; set; }

	public long FolderHierarchyId { get; set; }

	public long FolderOwnerId { get; set; }

	public long ModifyById { get; set; }

	public long ParentFolderHierarchyId { get; set; }

	public string FolderName { get; set; }

	public string FolderPath { get; set; }
}
