using System.Collections.Generic;

namespace Architecture.Model.MultiLanguages;

public class ItemMultiLingo
{
	public int Id { get; set; }

	public int ItemId { get; set; }

	public int MultiLanguagesId { get; set; }

	public string ItemName { get; set; }

	public virtual List<ItemMultiLingo> ItemMultiLingoList { get; set; }
}
