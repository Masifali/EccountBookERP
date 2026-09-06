using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.FinanceManagment;

namespace Architecture.DAL.FinanceManagment;

public class FmLoanRegistrationHeader
{
	public int SetData(Architecture.Model.FinanceManagment.FmLoanRegistrationHeader obj, string ProcName)
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
			foreach (FmLoanRegistrationDetail item in obj.FmLoanRegistrationDetails)
			{
				item.FmLoanRegistrationHeaderId = obj.Id;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_FmLoanRegistrationDetail_Insert"));
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

	public List<Architecture.Model.FinanceManagment.FmLoanRegistrationHeader> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FinanceManagment.FmLoanRegistrationHeader> lst = new List<Architecture.Model.FinanceManagment.FmLoanRegistrationHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.FinanceManagment.FmLoanRegistrationHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.FinanceManagment.FmLoanRegistrationHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@FmLoanAgreementHeaderId", item.Id));
					item.FmLoanRegistrationDetails = GenericProvider.Get<FmLoanRegistrationDetail>(sqlcon, "Sp_FmLoanRegistrationDetail_GetByFmLoanAgreementHeaderId", lstPrm);
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
