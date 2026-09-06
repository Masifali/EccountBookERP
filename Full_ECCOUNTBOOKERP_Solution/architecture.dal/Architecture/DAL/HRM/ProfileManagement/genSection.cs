using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.HRM.ProfileManagement;

namespace Architecture.DAL.HRM.ProfileManagement;

public class genSection
{
	public int SetData(Architecture.Model.HRM.ProfileManagement.genSection obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.SectionId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.SectionId;
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

	public List<Architecture.Model.HRM.ProfileManagement.genSection> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.ProfileManagement.genSection> lst = new List<Architecture.Model.HRM.ProfileManagement.genSection>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.HRM.ProfileManagement.genSection>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
