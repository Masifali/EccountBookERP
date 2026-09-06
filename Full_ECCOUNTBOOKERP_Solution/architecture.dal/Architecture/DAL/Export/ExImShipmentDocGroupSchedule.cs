using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Export;

namespace Architecture.DAL.Export;

public class ExImShipmentDocGroupSchedule : DALBase
{
	public int SetData(Architecture.Model.Export.ExImShipmentDocGroupSchedule obj, string ProcName)
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
			throw ex;
		}
	}

	public List<Architecture.Model.Export.ExImShipmentDocGroupSchedule> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImShipmentDocGroupSchedule> lst = new List<Architecture.Model.Export.ExImShipmentDocGroupSchedule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Export.ExImShipmentDocGroupSchedule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
