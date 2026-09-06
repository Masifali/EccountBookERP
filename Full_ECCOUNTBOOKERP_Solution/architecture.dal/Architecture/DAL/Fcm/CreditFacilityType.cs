using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Fcm;

namespace Architecture.DAL.Fcm;

public class CreditFacilityType
{
	public int SetData(Architecture.Model.Fcm.CreditFacilityType obj, string ProcName)
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

	public List<Architecture.Model.Fcm.CreditFacilityType> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Fcm.CreditFacilityType> lst = new List<Architecture.Model.Fcm.CreditFacilityType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.Fcm.CreditFacilityType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
