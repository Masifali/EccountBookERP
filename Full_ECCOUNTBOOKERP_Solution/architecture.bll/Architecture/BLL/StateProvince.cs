using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class StateProvince
{
	public static int Save(Architecture.Model.StateProvince obj)
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
				return new Architecture.DAL.StateProvince().SetData(obj, "Sp_StateProvince_Insert");
			}
			return new Architecture.DAL.StateProvince().SetData(obj, "Sp_StateProvince_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.StateProvince obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = clsGlobalVariables.UserAccount.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = clsGlobalVariables.UserAccount.CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.StateProvince>("Proc_StateProvince_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.StateProvince GetByID(int Id)
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
			new List<Architecture.Model.StateProvince>();
			return new Architecture.DAL.StateProvince().GetAll("Proc_StateProvince_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
