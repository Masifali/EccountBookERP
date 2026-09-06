using System;
using System.Data;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ItemPriceTypes
{
	public static DataTable GetAll()
	{
		try
		{
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemPriceTypes>("Sp_ItemPriceTypes_GetAllMethod");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
