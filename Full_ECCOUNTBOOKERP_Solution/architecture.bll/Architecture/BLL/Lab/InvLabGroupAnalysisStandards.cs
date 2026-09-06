using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Lab;
using Architecture.Model;
using Architecture.Model.Lab;

namespace Architecture.BLL.Lab;

public class InvLabGroupAnalysisStandards
{
	public static int Save(Architecture.Model.Lab.InvLabGroupAnalysisStandards obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Lab.InvLabGroupAnalysisStandards().SetDate(obj, "Sp_InvLabGroupAnalysisStandards_Insert");
			}
			return new Architecture.DAL.Lab.InvLabGroupAnalysisStandards().SetDate(obj, "Sp_InvLabGroupAnalysisStandards_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllOrById(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
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
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabGroupAnalysisStandards>("Sp_InvLabGroupAnalysisStandards_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemByGroup(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
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
				Value = "ReadAllForSearching"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisGroup>("Sp_InvLabAnalysisGroup_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetParametersFromGroupStandards(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
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
				Value = "GetParametersFromGroupStandards"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabGroupAnalysisStandards>("Sp_InvLabGroupAnalysisStandards_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
