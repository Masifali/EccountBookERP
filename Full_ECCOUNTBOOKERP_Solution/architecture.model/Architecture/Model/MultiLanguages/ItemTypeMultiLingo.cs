using System.Collections.Generic;

namespace Architecture.Model.MultiLanguages;

public class ItemTypeMultiLingo
{
	public int Id { get; set; }

	public int ItemTypeId { get; set; }

	public int MultiLanguagesId { get; set; }

	public string TypeDescription { get; set; }

	public virtual List<ItemTypeMultiLingo> ItemTypeMultiLingoList { get; set; }
}
