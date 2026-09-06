using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.BankSchema;

namespace Architecture.DAL.BankSchema;

public class PayeeTitle
{
	public long SetData(Architecture.Model.BankSchema.PayeeTitle obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.payeeTitleId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.payeeTitleId;
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

	public List<Architecture.Model.BankSchema.PayeeTitle> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.BankSchema.PayeeTitle> lst = new List<Architecture.Model.BankSchema.PayeeTitle>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.BankSchema.PayeeTitle>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
