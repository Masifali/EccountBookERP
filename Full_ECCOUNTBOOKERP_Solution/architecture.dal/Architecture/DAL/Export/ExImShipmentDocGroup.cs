using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Export;

namespace Architecture.DAL.Export;

public class ExImShipmentDocGroup : DALBase
{
	public int SetData(Architecture.Model.Export.ExImShipmentDocGroup obj, string ProcName)
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

	public List<Architecture.Model.Export.ExImShipmentDocGroup> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImShipmentDocGroup> lst = new List<Architecture.Model.Export.ExImShipmentDocGroup>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Export.ExImShipmentDocGroup>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
