using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Service;

namespace Architecture.DAL.Service;

public class ExImSaleOrderForwarder : DALBase
{
	public int SetDate(Architecture.Model.Service.ExImSaleOrderForwarder obj, string ProcName)
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
			foreach (ExImSaleOrderForwarderDetail item in obj.ExImSaleOrderForwarderDetail)
			{
				item.HeaderId = obj.Id;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_ExImSaleOrderForwarderDetail_Insert"));
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

	public List<Architecture.Model.Service.ExImSaleOrderForwarder> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Service.ExImSaleOrderForwarder> lst = new List<Architecture.Model.Service.ExImSaleOrderForwarder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Service.ExImSaleOrderForwarder>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Service.ExImSaleOrderForwarder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
					item.ExImSaleOrderForwarderDetail = GenericProvider.Get<ExImSaleOrderForwarderDetail>(sqlcon, "Sp_ExImSaleOrderForwarder_GetAll", lstPrm);
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
