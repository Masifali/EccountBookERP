using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;

namespace Architecture.DAL.pcc;

public class InvGrn
{
	public int SetData(Architecture.Model.pcc.InvGrn obj, string ProcName)
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
			int LineIdForPDI = 0;
			decimal TotalGrossWeight = default(decimal);
			foreach (Architecture.Model.pcc.InvGrnDetail item2 in obj.InvGrnDetailList)
			{
				LineIdForPDI = (item2.LineId = LineIdForPDI + 1);
				item2.InvGrnId = obj.Id;
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[pcc].[USP_InvGrnDetail_Insert]"));
				item2.Id = Code;
				TotalGrossWeight += item2.ItemNetWeight;
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
					item.EntryUser = obj.EntryUserId;
					item.ModifyUser = obj.ModifyUserId;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
				}
			}
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = obj.Id;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.pcc.InvGrn> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.pcc.InvGrn> lst = new List<Architecture.Model.pcc.InvGrn>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.Get<Architecture.Model.pcc.InvGrn>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.pcc.InvGrn item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "InvGrnDetail_ReadByInvGrnId"));
					item.InvGrnDetailList = GenericProvider.GetAllDetail<Architecture.Model.pcc.InvGrnDetail>(sqlcon, "[pcc].[USP_InvGrn_GetAllMethod]", lstPrm);
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
