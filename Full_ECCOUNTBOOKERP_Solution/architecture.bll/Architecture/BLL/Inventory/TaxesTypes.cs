using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class TaxesTypes
{
	public static int Save(Architecture.Model.Inventory.TaxesTypes obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.EntryUser = clsGlobalVariables.UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ModifyUser = clsGlobalVariables.UserAccount.ID;
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.TaxesTypes().SetData(obj, "Sp_TaxesTypes_Insert");
			}
			return new Architecture.DAL.Inventory.TaxesTypes().SetData(obj, "Sp_TaxesTypes_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.TaxesTypes GetByID(int Id)
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
			new List<Architecture.Model.Inventory.TaxesTypes>();
			return new Architecture.DAL.Inventory.TaxesTypes().GetData("Sp_TaxesTypes_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.Inventory.TaxesTypes obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.TaxesTypes>("Sp_TaxesTypes_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetForComboBind(Architecture.Model.Inventory.TaxesTypes obj)
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
			if (obj.Type != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Type",
					Value = obj.Type
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByCombo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.TaxesTypes>("Sp_TaxesTypes_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
