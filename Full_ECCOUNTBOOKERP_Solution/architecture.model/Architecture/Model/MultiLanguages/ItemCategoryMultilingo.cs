using System.Collections.Generic;

namespace Architecture.Model.MultiLanguages;

public class ItemCategoryMultilingo
{
	public int Id { get; set; }

	public int ItemCategoryId { get; set; }

	public int MultiLanguagesId { get; set; }

	public string CategoryDescription { get; set; }

	public virtual List<ItemCategoryMultilingo> ItemCategoryMultilingoList { get; set; }
}
