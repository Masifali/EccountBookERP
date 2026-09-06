using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Export;

namespace Architecture.DAL.Export;

public class ThirdPartyType : DALBase
{
	public int SetData(Architecture.Model.Export.ThirdPartyType obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.ThirdPartyTypeID = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.ThirdPartyTypeID;
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.Export.ThirdPartyType> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ThirdPartyType> lst = new List<Architecture.Model.Export.ThirdPartyType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Export.ThirdPartyType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
