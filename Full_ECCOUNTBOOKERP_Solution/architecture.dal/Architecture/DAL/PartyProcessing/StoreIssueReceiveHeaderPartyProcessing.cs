using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.PartyProcessing;

namespace Architecture.DAL.PartyProcessing;

public class StoreIssueReceiveHeaderPartyProcessing
{
	public int SetData(Architecture.Model.PartyProcessing.StoreIssueReceiveHeaderPartyProcessing obj, string ProcName)
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
			foreach (StoreIssueReceiveHeaderDetailPartyProcessing item in obj.StoreIssueReceivesList)
			{
				if (item.Id > 0)
				{
					item.StoreIssueReceiveHeaderPartyProcessingId = obj.Id;
					item.RefEntryNoId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_StoreIssueReceiveHeaderDetailPartyProcessing_Update"));
				}
				else
				{
					item.StoreIssueReceiveHeaderPartyProcessingId = obj.Id;
					item.RefEntryNoId = obj.Id;
					int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_StoreIssueReceiveHeaderDetailPartyProcessing_Insert"));
					item.RefEntryNoDetailId = code;
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

	public List<Architecture.Model.PartyProcessing.StoreIssueReceiveHeaderPartyProcessing> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.StoreIssueReceiveHeaderPartyProcessing> lst = new List<Architecture.Model.PartyProcessing.StoreIssueReceiveHeaderPartyProcessing>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.PartyProcessing.StoreIssueReceiveHeaderPartyProcessing>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.PartyProcessing.StoreIssueReceiveHeaderPartyProcessing item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.StoreIssueReceivesList = GenericProvider.GetAllDetail<StoreIssueReceiveHeaderDetailPartyProcessing>(sqlcon, "Sp_StoreIssueReceiveHeaderPartyProcessing_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
