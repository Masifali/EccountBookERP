using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Export;

namespace Architecture.DAL.Export;

public class ExImScheduleDetailWithPm
{
	public int SetDateForPmDetail(Architecture.Model.Export.ExImScheduleDetailWithPm obj, ExImScheduleOtherItemsDetail objOther)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (objOther.ExImScheduleOtherItemsDetailList != null && objOther.ExImScheduleOtherItemsDetailList.Count > 0)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "[dbo].[USP_ExImScheduleOtherItemsDetail_DeletePreviousData]";
				sqlCommand.Parameters.AddWithValue("@ContractScheduleId", objOther.ExImScheduleOtherItemsDetailList[0].ContractScheduleId);
				sqlCommand.ExecuteNonQuery();
				foreach (ExImScheduleOtherItemsDetail item2 in objOther.ExImScheduleOtherItemsDetailList)
				{
					item2.Id = 0;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "USP_ExImScheduleOtherItemsDetail_Insert"));
				}
			}
			int returnID = obj.ContractScheduleId;
			if (obj.ExImScheduleDetailWithPmList != null)
			{
				foreach (Architecture.Model.Export.ExImScheduleDetailWithPm item in obj.ExImScheduleDetailWithPmList)
				{
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "USP_ExImScheduleDetailWithPm_InsertAndUpdate"));
				}
			}
			SqlCommand sqlCommand2 = new SqlCommand();
			sqlCommand2.Connection = sqlcon;
			sqlCommand2.Transaction = sqlTrn;
			sqlCommand2.CommandType = CommandType.StoredProcedure;
			sqlCommand2.CommandText = "[dbo].[USP_ExImScheduleDetailWithPm_UpdateStatus]";
			sqlCommand2.Parameters.AddWithValue("@ContractScheduleId", obj.ExImScheduleDetailWithPmList[0].ContractScheduleId);
			sqlCommand2.Parameters.AddWithValue("@ContractId", obj.ExImScheduleDetailWithPmList[0].ContractId);
			sqlCommand2.ExecuteNonQuery();
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Export.ExImScheduleDetailWithPm> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImScheduleDetailWithPm> lst = new List<Architecture.Model.Export.ExImScheduleDetailWithPm>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Export.ExImScheduleDetailWithPm>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
