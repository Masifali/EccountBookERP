using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class ItemType
{
	public static int Save(Architecture.Model.Inventory.ItemType obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.ItemType().SetData(obj, "Sp_ItemType_Insert");
			}
			return new Architecture.DAL.Inventory.ItemType().SetData(obj, "Sp_ItemType_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GenerateCode(Architecture.Model.Inventory.ItemType obj)
	{
		DataTable dt = null;
		string code = "0";
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
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemType>("[dbo].[Sp_ItemType_GetAllMethod]", p);
			if (dt.Rows.Count > 0)
			{
				code = Conversion.ToString(dt.Rows[0]["TypeCode"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.ItemType GetByID(int Id)
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
			new List<Architecture.Model.Inventory.ItemType>();
			return new Architecture.DAL.Inventory.ItemType().GetData("Sp_ItemType_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(ReportsParameters obj)
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
			if (obj.TypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Type",
					Value = obj.TypeId
				});
			}
			if (!string.IsNullOrEmpty(obj.ItemIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoryIds",
					Value = obj.ItemIds
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemType>("Sp_ItemType_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getMotherItems(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ItemType>("usp_getMotherItems", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
