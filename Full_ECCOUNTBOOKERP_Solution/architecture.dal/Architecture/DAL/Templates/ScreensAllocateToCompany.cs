using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Templates;

namespace Architecture.DAL.Templates;

public class ScreensAllocateToCompany : DALBase
{
	public int SetData(Architecture.Model.Templates.ScreensAllocateToCompany obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Templates.ScreensAllocateToCompany item in obj.ScreensAllocateToCompanyList)
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

	public List<Architecture.Model.Templates.ScreensAllocateToCompany> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Templates.ScreensAllocateToCompany> lst = new List<Architecture.Model.Templates.ScreensAllocateToCompany>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Templates.ScreensAllocateToCompany>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
