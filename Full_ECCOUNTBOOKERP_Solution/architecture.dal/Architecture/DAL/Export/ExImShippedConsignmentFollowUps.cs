using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Export;

namespace Architecture.DAL.Export;

public class ExImShippedConsignmentFollowUps : DALBase
{
	public int SetData(Architecture.Model.Export.ExImShippedConsignmentFollowUps obj, string ProcName)
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
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public int SetDataMultiple(Architecture.Model.Export.ExImShippedConsignmentFollowUps obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Export.ExImShippedConsignmentFollowUps item in obj.ShippedConsignmentFollowUpList)
			{
				if (item.Id > 0)
				{
					ProcName = "sp_ExImShippedConsignmentFollowUps_Update";
				}
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
				if (returnID > 0)
				{
					obj.Id = Convert.ToInt32(returnID);
				}
				else
				{
					returnID = obj.Id;
				}
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

	public List<Architecture.Model.Export.ExImShippedConsignmentFollowUps> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImShippedConsignmentFollowUps> lst = new List<Architecture.Model.Export.ExImShippedConsignmentFollowUps>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Export.ExImShippedConsignmentFollowUps>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
