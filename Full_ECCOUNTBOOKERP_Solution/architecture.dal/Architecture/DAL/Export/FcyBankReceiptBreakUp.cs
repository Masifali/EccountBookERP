using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Export;

namespace Architecture.DAL.Export;

public class FcyBankReceiptBreakUp
{
	public int SetData(Architecture.Model.Export.FcyBankReceiptBreakUp obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "usp_FcyBankReceiptBreakUpDeleteByFcyReceiptsId";
			sqlCommand.Parameters.AddWithValue("@Id", obj.Id);
			sqlCommand.ExecuteNonQuery();
			foreach (Architecture.Model.Export.FcyBankReceiptBreakUp item in obj.FcyBankReceiptBreakUpList)
			{
				returnID = Conversion.ToInt(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.Export.FcyBankReceiptBreakUp> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.FcyBankReceiptBreakUp> lst = new List<Architecture.Model.Export.FcyBankReceiptBreakUp>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Export.FcyBankReceiptBreakUp>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
