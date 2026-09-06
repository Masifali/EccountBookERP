using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.ChequePrinting;

namespace Architecture.DAL.ChequePrinting;

public class ChequePrintList
{
	public int SetData(Architecture.Model.ChequePrinting.ChequePrintList obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public void UpdateChequePrintStatus(string Ids, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = ProcName;
			sqlCommand.Parameters.AddWithValue("@Ids", Ids);
			sqlCommand.ExecuteScalar();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.ChequePrinting.ChequePrintList> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ChequePrinting.ChequePrintList> lst = new List<Architecture.Model.ChequePrinting.ChequePrintList>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ChequePrinting.ChequePrintList>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
