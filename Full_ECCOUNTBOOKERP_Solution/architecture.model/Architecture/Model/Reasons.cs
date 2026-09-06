using System;

namespace Architecture.Model;

public class Reasons
{
	public int ReasonId { get; set; }

	public string Reason { get; set; }

	public int RefDocumentTypeId { get; set; }

	public DateTime EntryDate { get; set; }

	public int EntryUserId { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ModifyUserId { get; set; }
}
