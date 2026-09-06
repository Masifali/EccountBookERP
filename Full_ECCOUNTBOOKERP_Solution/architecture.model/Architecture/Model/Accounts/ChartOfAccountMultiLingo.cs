using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class ChartOfAccountMultiLingo
{
	public int Id { get; set; }

	public int ChartOfAccountId { get; set; }

	public int MultiLanguagesId { get; set; }

	public string AccountTitle { get; set; }

	public virtual List<ChartOfAccountMultiLingo> ChartOfAccountMultiLingoList { get; set; }
}
