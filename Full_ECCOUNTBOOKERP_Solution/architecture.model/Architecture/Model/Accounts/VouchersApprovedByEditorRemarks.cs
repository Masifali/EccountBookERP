using System;

namespace Architecture.Model.Accounts;

public class VouchersApprovedByEditorRemarks
{
	public int Id { get; set; }

	public int RefDocTypeId { get; set; }

	public int RefDocMasterRecordId { get; set; }

	public string Comments { get; set; }

	public int CommentsUserId { get; set; }

	public DateTime CommentsEntryDate { get; set; }

	public int SortNo { get; set; }
}
