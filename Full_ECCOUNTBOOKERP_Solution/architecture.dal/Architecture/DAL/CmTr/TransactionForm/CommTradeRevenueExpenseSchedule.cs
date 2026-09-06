using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.CmTr.TransactionForm;

namespace Architecture.DAL.CmTr.TransactionForm;

public class CommTradeRevenueExpenseSchedule
{
	public int SetData(Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule obj, string ProcName)
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
			foreach (CommTradeRevenueExpenseScheduleDetail item2 in obj.CommTradeRevenueExpenseScheduleList)
			{
				item2.CommTradeRevenueExpenseScheduleId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[CmTr].[USP_CommTradeRevenueExpenseScheduleDetail_Insert]"));
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.AttachmentsList[0].ScreenName
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = returnID
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteById"
				});
				new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = obj.SupplierCustomerId;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = clsGlobalVariables.UserAccount.ID;
					item.ModifyUser = clsGlobalVariables.UserAccount.ID;
					item.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
					item.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
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

	public List<Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule> lst = new List<Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.CmTr.TransactionForm.CommTradeRevenueExpenseSchedule item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByCommTradeRevenueExpenseScheduleId"));
					item.CommTradeRevenueExpenseScheduleList = GenericProvider.GetAllDetail<CommTradeRevenueExpenseScheduleDetail>(sqlcon, "[CmTr].[USP_CommTradeRevenueExpenseSchedule_GetAllMethod]", lstPrm);
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
