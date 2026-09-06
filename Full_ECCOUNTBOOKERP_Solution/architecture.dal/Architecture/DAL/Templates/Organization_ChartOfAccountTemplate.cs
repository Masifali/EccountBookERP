using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Templates;

namespace Architecture.DAL.Templates;

public class Organization_ChartOfAccountTemplate : DALBase
{
	public int SetData(Architecture.Model.Templates.Organization_ChartOfAccountTemplate obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Templates.Organization_ChartOfAccountTemplate> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Templates.Organization_ChartOfAccountTemplate> lst = new List<Architecture.Model.Templates.Organization_ChartOfAccountTemplate>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Templates.Organization_ChartOfAccountTemplate>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public int DELETEANDUPDATEBYACCOUNTID(ReportsParameters obj)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("SP_ChartOfAccount_DeleteAndUpdatebyAccountId");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, obj.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, obj.CompanyId);
			sqlDataBase.AddInParameter(cmd, "ReqType", SqlDbType.NVarChar, obj.ReqType);
			sqlDataBase.AddInParameter(cmd, "ChartOfAccountId", SqlDbType.Int, obj.ChartOfAccountId);
			sqlDataBase.AddInParameter(cmd, "AccountTitle", SqlDbType.NVarChar, obj.AccountTitle);
			sqlDataBase.AddInParameter(cmd, "OtherErpCode", SqlDbType.NVarChar, obj.OtherCode);
			sqlDataBase.AddInParameter(cmd, "IsActive", SqlDbType.Bit, obj.IsApproved);
			return sqlDataBase.ExecuteNonQuery(cmd);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
