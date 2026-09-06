using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.BLL.Accounts;

public class PLBSSetting
{
	public static int Save(Architecture.Model.Accounts.PLBSSetting obj)
	{
		try
		{
			return new Architecture.DAL.Accounts.PLBSSetting().SetData(obj, "Sp_PLBSSetting_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Accounts.PLBSSetting obj)
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
				Value = "GetAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.PLBSSetting>("Sp_PLBSSetting_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllForBsPlSetting(ReportsParameters obj)
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
				ParameterName = "@Note",
				Value = obj.ReqType
			});
			if (obj.AccouuntClassId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountClassId",
					Value = obj.AccouuntClassId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAllForBsPlSetting"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.PLBSSetting>("Sp_PLBSSetting_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
