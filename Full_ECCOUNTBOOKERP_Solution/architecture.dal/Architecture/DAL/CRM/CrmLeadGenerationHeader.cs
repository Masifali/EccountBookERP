using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.CRM;

namespace Architecture.DAL.CRM;

public class CrmLeadGenerationHeader : DALBase
{
	public int SetData(Architecture.Model.CRM.CrmLeadGenerationHeader obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.LeadGenHeaderId = Convert.ToInt64(returnID);
			}
			foreach (CrmLeadGenerationDetail item in obj.crmLeadGenerationDetails)
			{
				item.CrmLeadGenerationHeaderId = obj.LeadGenHeaderId;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_CrmLeadGenerationDetail_Insert"));
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

	public List<Architecture.Model.CRM.CrmLeadGenerationHeader> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CRM.CrmLeadGenerationHeader> lst = new List<Architecture.Model.CRM.CrmLeadGenerationHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.CRM.CrmLeadGenerationHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.CRM.CrmLeadGenerationHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@LeadGenHeaderId", item.LeadGenHeaderId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.crmLeadGenerationDetails = GenericProvider.Get<CrmLeadGenerationDetail>(sqlcon, "Sp_CrmLeadGenerationHeader_GetAllMethod", lstPrm);
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
