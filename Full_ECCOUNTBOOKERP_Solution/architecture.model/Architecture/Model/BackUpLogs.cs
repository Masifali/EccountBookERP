using System;

namespace Architecture.Model;

public class BackUpLogs
{
	public bool BackupStatus { get; set; }

	public DateTime BackupEndTime { get; set; }

	public DateTime BackupStartTime { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ZipAndEncryptEndTime { get; set; }

	public DateTime ZipAndEncryptStartTime { get; set; }

	public int BackupTypeId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int OldDeletedFileCount { get; set; }

	public int OrganizationId { get; set; }

	public string BackupLocation { get; set; }

	public string BackupType { get; set; }
}
