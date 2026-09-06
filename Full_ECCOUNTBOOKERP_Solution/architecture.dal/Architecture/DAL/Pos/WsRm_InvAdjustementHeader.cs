using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.POS;

namespace Architecture.DAL.Pos;

public class WsRm_InvAdjustementHeader
{
	public long SetData(Architecture.Model.POS.WsRm_InvAdjustementHeader obj, string ProcName)
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
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (WsRmInvAdjustementDetail item in obj.WsRmInvAdjustementDetailList)
			{
				item.WsRm_InvAdjustementHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_WsRmInvAdjustementDetail_Insert"));
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

	public List<Architecture.Model.POS.WsRm_InvAdjustementHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.POS.WsRm_InvAdjustementHeader> lst = new List<Architecture.Model.POS.WsRm_InvAdjustementHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.POS.WsRm_InvAdjustementHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.POS.WsRm_InvAdjustementHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.WsRmInvAdjustementDetailList = GenericProvider.GetAllDetail<WsRmInvAdjustementDetail>(sqlcon, "Sp_WsRm_InvAdjustementHeader_GetAllMethod", lstPrm);
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
