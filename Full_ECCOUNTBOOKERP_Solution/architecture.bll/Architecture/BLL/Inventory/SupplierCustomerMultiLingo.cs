using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class SupplierCustomerMultiLingo
{
	public static int Save(Architecture.Model.Inventory.SupplierCustomerMultiLingo obj)
	{
		try
		{
			return new Architecture.DAL.Inventory.SupplierCustomerMultiLingo().SetData(obj, "Sp_SupplierCustomerMultiLingo_Insert");
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomerMultiLingo>("USP_SupplierCustomerMultiLingo_ReadAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
