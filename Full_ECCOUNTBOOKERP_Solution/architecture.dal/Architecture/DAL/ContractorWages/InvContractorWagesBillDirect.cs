using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.ContractorWages;

namespace Architecture.DAL.ContractorWages;

public class InvContractorWagesBillDirect
{
	public int SetData(Architecture.Model.ContractorWages.InvContractorWagesBillDirect obj, string ProcName)
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
			foreach (InvContractorWagesBillDirectDetail item in obj.InvContractorWagesBillDirectDetailslist)
			{
				item.InvContractorWagesBillDirectId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_InvContractorWagesBillDirectDetail_Insert"));
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

	public List<Architecture.Model.ContractorWages.InvContractorWagesBillDirect> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ContractorWages.InvContractorWagesBillDirect> lst = new List<Architecture.Model.ContractorWages.InvContractorWagesBillDirect>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.ContractorWages.InvContractorWagesBillDirect>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.ContractorWages.InvContractorWagesBillDirect item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
					item.InvContractorWagesBillDirectDetailslist = GenericProvider.Get<InvContractorWagesBillDirectDetail>(sqlcon, "Sp_InvContractorWagesBillDirect_GetAllMethod", lstPrm);
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
