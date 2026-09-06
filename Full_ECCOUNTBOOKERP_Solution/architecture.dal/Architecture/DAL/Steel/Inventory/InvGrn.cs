using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Steel.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.Steel.Inventory;

public class InvGrn : DALBase
{
	public int InsertInventoryTransaction(int OrganizationId, int CompanyId, int RefDocIdNo, int RefDocumentTypeId)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_InventoryTransactions_GetALLMethod");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, CompanyId);
			sqlDataBase.AddInParameter(cmd, "RefDocIdNo", SqlDbType.Int, RefDocIdNo);
			sqlDataBase.AddInParameter(cmd, "RefDocumentTypeId", SqlDbType.Int, RefDocumentTypeId);
			return Conversion.ToInt(sqlDataBase.ExecuteNonQuery(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int InsertInventoryTransactionEvaluation(int OrganizationId, int CompanyId, int RefDocIdNo, int RefDocumentTypeId)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_InventoryStockEvalautionDetail_Update");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, CompanyId);
			sqlDataBase.AddInParameter(cmd, "hId", SqlDbType.Int, RefDocIdNo);
			sqlDataBase.AddInParameter(cmd, "RefDocumentTypeId", SqlDbType.Int, RefDocumentTypeId);
			return Conversion.ToInt(sqlDataBase.ExecuteNonQuery(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int SetData(Architecture.Model.Steel.Inventory.InvGrn obj, string ProcName)
	{
		int returnID = 0;
		_ = DateTime.Now;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		if (obj.ActionId == 2)
		{
			GetRecordsById(obj.Id);
		}
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			else
			{
				AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
				if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0 && !Directory.Exists(AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				obj.AttachmentsList = CommonServices.CommonMethodForAttachmentsToPhysicallySave(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.AttachmentsList, copiedFiles);
			}
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (Architecture.Model.Steel.Inventory.InvGrnDetail item2 in obj.invGrnDetail)
			{
				item2.InvGrnId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[ST].[USP_InvGrnDetail_Insert]"));
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
					Value = obj.Id
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
					item.RefDocumentNo = obj.Id;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					item.EntryUser = obj.EntryUserId;
					item.ModifyUser = obj.EntryUserId;
					item.EntryDate = obj.EntryDate;
					item.ModifyDate = obj.ModifyDate;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
				}
			}
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
			{
				CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex;
		}
	}

	public List<Architecture.Model.Steel.Inventory.InvGrn> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Steel.Inventory.InvGrn> lst = new List<Architecture.Model.Steel.Inventory.InvGrn>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.Steel.Inventory.InvGrn>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Steel.Inventory.InvGrn item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
					item.invGrnDetail = GenericProvider.GetAllDetail<Architecture.Model.Steel.Inventory.InvGrnDetail>(sqlcon, "[ST].[USP_InvGrn_GetAllMethod]", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public static DateTime GetRecordsById(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetRecordId"
			});
			return Conversion.ToDateTime(GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("[ST].[USP_InvGrn_GetAllMethod]", p).Rows[0][0]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
