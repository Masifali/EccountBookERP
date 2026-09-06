using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ItemsCustomizedGroups
{
	public static int Save(Architecture.Model.Inventory.ItemsCustomizedGroups obj)
	{
		try
		{
			if (obj.CustomGroupId == 0)
			{
				return new Architecture.DAL.Inventory.ItemsCustomizedGroups().SetData(obj, "Sp_ItemsCustomizedGroups_Insert");
			}
			return new Architecture.DAL.Inventory.ItemsCustomizedGroups().SetData(obj, "Sp_ItemsCustomizedGroups_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.ItemsCustomizedGroups GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CustomGroupId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.Inventory.ItemsCustomizedGroups>();
			return new Architecture.DAL.Inventory.ItemsCustomizedGroups().GetAll("Sp_ItemsCustomizedGroups_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemsCustomizedGroups>("Sp_ItemsCustomizedGroups_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
