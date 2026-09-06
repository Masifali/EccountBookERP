using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Export;

namespace Architecture.DAL.Export;

public class ExImGdEeEfShipmentHistory : DALBase
{
	public int SetDate(Architecture.Model.Export.ExImGdEeEfShipmentHistory obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (ExImGdEeEfShipmentHistoryGpVehicleContainerDetail item in obj.eximGdEeEfShipmentHistoryDetail)
			{
				item.InvoiceNoIdd = obj.Id;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_ExImGdEeEfShipmentHistoryGpVehicleContainerDetail_Insert"));
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

	public List<Architecture.Model.Export.ExImGdEeEfShipmentHistory> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImGdEeEfShipmentHistory> lst = new List<Architecture.Model.Export.ExImGdEeEfShipmentHistory>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Export.ExImGdEeEfShipmentHistory>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Export.ExImGdEeEfShipmentHistory item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadById"));
					item.eximGdEeEfShipmentHistoryDetail = GenericProvider.Get<ExImGdEeEfShipmentHistoryGpVehicleContainerDetail>(sqlcon, "Sp_ExImGdEeEfShipmentHistoryGpVehicleContainerDetail_GetAllMethod", lstPrm);
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
