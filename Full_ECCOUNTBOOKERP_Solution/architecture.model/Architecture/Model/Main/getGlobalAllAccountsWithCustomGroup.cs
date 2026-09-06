namespace Architecture.Model.Main;

public class getGlobalAllAccountsWithCustomGroup
{
	public int ChartOfAccountId { get; set; }

	public string AccountTitle { get; set; }

	public string AccountCode { get; set; }

	public int CustomGroupId { get; set; }

	public string CustomGroup { get; set; }

	public int AccountTypeId { get; set; }

	public bool ExistsInCustomGroup { get; set; }

	public int PLNoteId { get; set; }

	public int BSNoteId { get; set; }

	public int AccountClass { get; set; }

	public int ParentAccountId { get; set; }

	public string ParentAccountTitle { get; set; }

	public string ParentAccountCode { get; set; }

	public string AccountType { get; set; }

	public string AccountClassName { get; set; }

	public int CurrencyId { get; set; }

	public string CurrencyCode { get; set; }
}
