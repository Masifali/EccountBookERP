using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.CRM;

namespace Architecture.DAL.CRM;

public class CrmContactHeader : DALBase
{
	public int SetData(Architecture.Model.CRM.CrmContactHeader obj, string ProcName)
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
			foreach (CrmContactDetail item in obj.crmContactDetails)
			{
				item.CrmContactHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_CrmContactDetail_Insert"));
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

	public List<Architecture.Model.CRM.CrmContactHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CRM.CrmContactHeader> lst = new List<Architecture.Model.CRM.CrmContactHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.CRM.CrmContactHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.CRM.CrmContactHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter
					{
						ParameterName = "@Activity",
						Value = "ReadDetailByHeaderId"
					});
					item.crmContactDetails = GenericProvider.Get<CrmContactDetail>(sqlcon, "Sp_CrmContactHeader_GetAllMethod", lstPrm);
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
