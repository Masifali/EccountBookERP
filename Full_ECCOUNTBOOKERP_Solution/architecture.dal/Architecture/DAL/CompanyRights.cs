using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class CompanyRights : DALBase
{
	public int SetData(Architecture.Model.CompanyRights obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.CompanyRights item in obj.CompanyRightslst)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.CompanyRights> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CompanyRights> lst = new List<Architecture.Model.CompanyRights>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.CompanyRights>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public void DeleteByCompanyId(Architecture.Model.CompanyRights obj)
	{
		try
		{
			foreach (Architecture.Model.CompanyRights item in obj.CompanyRightslst)
			{
				DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_CompanyRights_GetAllMethod");
				sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.BigInt, item.CompanyId);
				sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.BigInt, item.Id);
				sqlDataBase.AddInParameter(cmd, "Activity", SqlDbType.NVarChar, "DeleteFromCompanyRightByCompanyIdandScreenId");
				Conversion.ToInt(sqlDataBase.ExecuteNonQuery(cmd));
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void DeleteByUserId(CompanyRightsInsertAndDelete obj)
	{
		try
		{
			foreach (CompanyRightsInsertAndDelete item in obj.CompanyRightsInsertAndDeletelist)
			{
				DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_CompanyRights_GetAllMethod");
				sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.BigInt, item.OrganizationId);
				sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.BigInt, item.CompanyId);
				sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.BigInt, item.ScreenId);
				sqlDataBase.AddInParameter(cmd, "UserId", SqlDbType.BigInt, item.UserId);
				sqlDataBase.AddInParameter(cmd, "EntryUserId", SqlDbType.BigInt, item.EntryUserId);
				sqlDataBase.AddInParameter(cmd, "Activity", SqlDbType.NVarChar, "DeleteByCompanyIdAndUserId");
				Conversion.ToInt(sqlDataBase.ExecuteNonQuery(cmd));
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
