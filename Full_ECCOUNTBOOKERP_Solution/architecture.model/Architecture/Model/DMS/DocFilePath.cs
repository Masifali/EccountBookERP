using System;
using System.Collections.Generic;

namespace Architecture.Model.DMS;

public class DocFilePath
{
	public bool IsApproved { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public long CreatedById { get; set; }

	public long DocFilePathId { get; set; }

	public long FileOwnerId { get; set; }

	public long FolderHierarchyId { get; set; }

	public long ModifyById { get; set; }

	public string FileExtension { get; set; }

	public string FileName { get; set; }

	public string FilePath { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public virtual List<DocFilePath> docFilePathsList { get; set; }
}
