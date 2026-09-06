using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ItemMultiLingo
{
	public static int Save(Architecture.Model.Inventory.ItemMultiLingo obj)
	{
		try
		{
			return new Architecture.DAL.Inventory.ItemMultiLingo().SetData(obj, "Sp_ItemMultiLingo_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(int MultiLanguagesId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (MultiLanguagesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MultiLanguagesId",
					Value = MultiLanguagesId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemClass>("USP_ItemMultiLingo_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
