using System;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class PLBSSetting
{
	public int SetData(Architecture.Model.Accounts.PLBSSetting obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Accounts.PLBSSetting item in obj.PLBSSettingslist)
			{
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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
}
