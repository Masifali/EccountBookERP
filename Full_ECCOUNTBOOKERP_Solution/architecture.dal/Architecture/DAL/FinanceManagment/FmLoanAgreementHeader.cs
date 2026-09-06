using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.FinanceManagment;

namespace Architecture.DAL.FinanceManagment;

public class FmLoanAgreementHeader
{
	public int SetData(Architecture.Model.FinanceManagment.FmLoanAgreementHeader obj, string ProcName)
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
			foreach (Architecture.Model.FinanceManagment.FmLoanAgreementDetail item in obj.FmLoanAgreementDetails)
			{
				item.FmLoanAgreementHeaderId = obj.Id;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_FmLoanAgreementDetail_Insert"));
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

	public List<Architecture.Model.FinanceManagment.FmLoanAgreementHeader> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FinanceManagment.FmLoanAgreementHeader> lst = new List<Architecture.Model.FinanceManagment.FmLoanAgreementHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.FinanceManagment.FmLoanAgreementHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.FinanceManagment.FmLoanAgreementHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@FmLoanAgreementHeaderId", item.Id));
					item.FmLoanAgreementDetails = GenericProvider.Get<Architecture.Model.FinanceManagment.FmLoanAgreementDetail>(sqlcon, "Sp_FmLoanAgreementDetail_ReadByFmLoanAgreementHeaderId", lstPrm);
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
