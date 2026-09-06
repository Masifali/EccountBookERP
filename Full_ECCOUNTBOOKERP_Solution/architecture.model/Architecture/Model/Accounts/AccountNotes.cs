using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class AccountNotes
{
	public bool Status { get; set; }

	public int Id { get; set; }

	public string NoteRole { get; set; }

	public string NoteTitle { get; set; }

	public virtual List<AccountNotes> AccountNoteslist { get; set; }
}
