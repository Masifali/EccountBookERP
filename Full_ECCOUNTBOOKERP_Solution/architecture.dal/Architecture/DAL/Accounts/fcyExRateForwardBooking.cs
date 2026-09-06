using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class fcyExRateForwardBooking : DALBase
{
	public int SetData(Architecture.Model.Accounts.fcyExRateForwardBooking obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			obj.Id = Convert.ToInt32(returnID);
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = obj.BankId;
					item.RefDocumentTypeId = 0;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = clsGlobalVariables.UserAccount.ID;
					item.ModifyUser = clsGlobalVariables.UserAccount.ID;
					item.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
					item.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
					item.ScreenName = obj.ScreenName;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
				}
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

	public List<Architecture.Model.Accounts.fcyExRateForwardBooking> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.fcyExRateForwardBooking> lst = new List<Architecture.Model.Accounts.fcyExRateForwardBooking>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Accounts.fcyExRateForwardBooking>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
