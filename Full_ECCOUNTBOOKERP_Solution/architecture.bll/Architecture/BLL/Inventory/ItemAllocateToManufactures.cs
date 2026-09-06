using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ItemAllocateToManufactures
{
	public static int Save(Architecture.Model.Inventory.ItemAllocateToManufactures obj)
	{
		try
		{
			return Convert.ToInt32(GenericProvider.SetDataNew(obj, ""));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			p.Add(new SqlParameter("@Activity", "ReadAll"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemAllocateToManufactures>("Sp_ItemAllocateToManufactures_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
