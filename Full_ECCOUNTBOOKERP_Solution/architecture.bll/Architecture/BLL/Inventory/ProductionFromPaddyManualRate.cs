using System;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ProductionFromPaddyManualRate
{
	public static int Save(Architecture.Model.Inventory.ProductionFromPaddyManualRate obj)
	{
		try
		{
			if (obj.ProductionFromPaddyManualRatesList == null || obj.ProductionFromPaddyManualRatesList.Count == 0)
			{
				throw new Exception("List not found. Please Check!");
			}
			return new Architecture.DAL.Inventory.ProductionFromPaddyManualRate().SetData(obj, "[dbo].[USP_ProductionFromPaddyManualRate_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
