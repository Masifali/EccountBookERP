using System.Collections.Generic;

namespace Architecture.Model.MultiLanguages;

public class WarehouseMultilingo
{
	public int Id { get; set; }

	public int MultiLanguagesId { get; set; }

	public int WarehouseId { get; set; }

	public string WarehouesName { get; set; }

	public virtual List<WarehouseMultilingo> WarehouseMultilingoList { get; set; }
}
