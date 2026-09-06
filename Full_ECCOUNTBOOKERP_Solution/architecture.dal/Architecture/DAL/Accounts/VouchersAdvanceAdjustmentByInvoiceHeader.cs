using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class VouchersAdvanceAdjustmentByInvoiceHeader
{
	public int SetDate(Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader obj, string ProcName)
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
			foreach (VouchersAdvanceAdjustedAccountsDetail item2 in obj.AdjustedAccountsDetails)
			{
				item2.VouchersAdvanceAdjustmentByInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VouchersAdvanceAdjustedAccountsDetail_Insert"));
			}
			foreach (VouchersAdvanceAdjustedInvoicesDetail item in obj.AdjustedInvoicesDetails)
			{
				item.VouchersAdvanceAdjustmentByInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VouchersAdvanceAdjustedInvoicesDetail_Insert"));
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

	public List<Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader> lst = new List<Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@VouchersAdvanceAdjustmentByInvoiceId", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item2.AdjustedInvoicesDetails = GenericProvider.Get<VouchersAdvanceAdjustedInvoicesDetail>(sqlConnection2, "Sp_VouchersAdvanceAdjustedInvoicesDetail_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Accounts.VouchersAdvanceAdjustmentByInvoiceHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@VouchersAdvanceAdjustmentByInvoiceId", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
					item.AdjustedAccountsDetails = GenericProvider.Get<VouchersAdvanceAdjustedAccountsDetail>(sqlcon, "Sp_VouchersAdvanceAdjustedAccountsDetail_GetAllMethod", lstPrm);
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
