using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Authentication;

namespace Architecture.DAL.Authentication;

public class ActivityLog : DALBase
{
	private const string PROC_INSERT = "Proc_ActivityLog_Insert";

	private const string PROC_GetLoginDate = "GetMaxDate";

	private const string ACTIVITY = "Activity";

	private const string COMPANYID = "CompanyId";

	private const string ID = "Id";

	private const string LOGINDATE = "LoginDate";

	private const string ORGANIZATIONID = "OrganizationId";

	private const string REFDOCNO = "RefDocNo";

	private const string SCREENNAME = "ScreenName";

	private const string USERID = "UserId";

	public int SetData(Architecture.Model.Authentication.ActivityLog obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Authentication.ActivityLog> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Authentication.ActivityLog> lst = new List<Architecture.Model.Authentication.ActivityLog>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Authentication.ActivityLog>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
