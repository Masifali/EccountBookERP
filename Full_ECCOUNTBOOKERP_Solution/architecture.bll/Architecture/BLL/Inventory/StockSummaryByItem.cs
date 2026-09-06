using System;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class StockSummaryByItem
{
	public static int Save(Architecture.Model.Inventory.StockSummaryByItem obj)
	{
		try
		{
			return new Architecture.DAL.Inventory.StockSummaryByItem().SetData(obj, "USP_StockSummaryByItem_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
