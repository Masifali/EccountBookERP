using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Lab;
using Architecture.Model.Lab;

namespace Architecture.BLL.Lab;

public class InvLabAnalysisItems
{
	public static int Save(Architecture.Model.Lab.InvLabAnalysisItems obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Lab.InvLabAnalysisItems().SetDate(obj, "Sp_InvLabAnalysisItems_Insert");
			}
			return new Architecture.DAL.Lab.InvLabAnalysisItems().SetDate(obj, "Sp_InvLabAnalysisItems_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllOrById(Architecture.Model.Lab.InvLabAnalysisItems obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisItems>("Sp_InvLabAnalysisItems_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getLabMasterParms()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisItems>("usp_getLabMasterParms", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
