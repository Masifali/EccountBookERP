using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class CheqBookHeader
{
	public int SetDate(Architecture.Model.CheqBookHeader obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			obj.Id = Convert.ToInt32(returnID);
			foreach (CheqBookDetail item in obj.Cheqbookdetaillist)
			{
				item.CheqBookHeaderId = obj.Id;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "SP_CheqBookDetail_Insert"));
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

	public List<Architecture.Model.CheqBookHeader> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CheqBookHeader> lst = new List<Architecture.Model.CheqBookHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.CheqBookHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.CheqBookHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@DemoId", item.Id));
					item.Cheqbookdetaillist = GenericProvider.Get<CheqBookDetail>(sqlcon, "Proc_tbl_DemoDetail_ReadAll", lstPrm);
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
