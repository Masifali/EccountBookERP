using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.POS;

namespace Architecture.DAL.Pos;

public class WsRmIncentivePolicyHeader
{
	public int SetData(Architecture.Model.POS.WsRmIncentivePolicyHeader obj, string ProcName)
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
			foreach (WsRmIncentivePolicyDetail item in obj.WsRmIncentivePolicyDetailsList)
			{
				item.WsRmIncentivePolicyHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_WsRmIncentivePolicyDetail_Insert"));
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

	public List<Architecture.Model.POS.WsRmIncentivePolicyHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.POS.WsRmIncentivePolicyHeader> lst = new List<Architecture.Model.POS.WsRmIncentivePolicyHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.POS.WsRmIncentivePolicyHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.POS.WsRmIncentivePolicyHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.WsRmIncentivePolicyDetailsList = GenericProvider.GetAllDetail<WsRmIncentivePolicyDetail>(sqlcon, "Sp_WsRmIncentivePolicyHeader_GetAllMetod", lstPrm);
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
