using System.Collections.Generic;

namespace Architecture.Model.MultiLanguages;

public class InvConractorWagesAccountsMultilingo
{
	public int Id { get; set; }

	public int MultiLanguagesId { get; set; }

	public int WagesAccountId { get; set; }

	public string WagesAccountName { get; set; }

	public virtual List<InvConractorWagesAccountsMultilingo> InvConractorWagesAccountsMultilingoslist { get; set; }
}
