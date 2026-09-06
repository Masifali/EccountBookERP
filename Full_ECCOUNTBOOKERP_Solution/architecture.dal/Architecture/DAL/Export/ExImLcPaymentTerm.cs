using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Export;

namespace Architecture.DAL.Export;

public class ExImLcPaymentTerm : DALBase
{
	public int SetData(Architecture.Model.Export.ExImLcPaymentTerm obj, string ProcName)
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

	public List<Architecture.Model.Export.ExImLcPaymentTerm> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImLcPaymentTerm> lst = new List<Architecture.Model.Export.ExImLcPaymentTerm>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Export.ExImLcPaymentTerm>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
