using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Lab;

namespace Architecture.DAL.Lab;

public class InvLabAnalysisStandardDeductionPolicyHeader
{
	public int SetData(Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader obj, string ProcName)
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
			foreach (InvLabAnalysisStandardDeductionPolicyDetail item in obj.LabAnalysisStandardDetail)
			{
				item.InvLabAnalysisStandardDeductionPolicyHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_InvLabAnalysisStandardDeductionPolicyDetail_Insert"));
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

	public List<Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader> lst = new List<Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
					item.LabAnalysisStandardDetail = GenericProvider.GetAllDetail<InvLabAnalysisStandardDeductionPolicyDetail>(sqlcon, "Sp_InvLabAnalysisStandardDeductionPolicyHeader_GetAllMethod", lstPrm);
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
