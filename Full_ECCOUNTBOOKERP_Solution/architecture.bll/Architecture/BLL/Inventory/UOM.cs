using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class UOM
{
	public static int Save(Architecture.Model.Inventory.UOM obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.UOM().SetData(obj, "Sp_UOM_Insert");
			}
			return new Architecture.DAL.Inventory.UOM().SetData(obj, "Sp_UOM_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.UOM GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.Inventory.UOM>();
			return new Architecture.DAL.Inventory.UOM().GetData("Sp_UOM_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.Inventory.UOM obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.UOM>("Sp_UOM_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllParentUOM()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAllParentUOM"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.UOM>("Sp_UOM_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable UOMStaticAll()
	{
		try
		{
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.UOM>("[dbo].[USP_UOMStatic_GetAllMethod]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
