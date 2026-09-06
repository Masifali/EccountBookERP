using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Mfg;
using Architecture.Model.VPS;

namespace Architecture.DAL.Mfg;

public class ProductionHeader
{
	public int SetData(Architecture.Model.Mfg.ProductionHeader obj, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
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
				obj.AttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.Attachment));
				obj.CustomAttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.UploadedFileCustomName));
			}
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.ProductionHeaderId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.ProductionHeaderId;
			}
			int LineId = 1;
			foreach (ProductionOutPutDetail item5 in obj.ProductionOutPutDetailslist)
			{
				item5.ProductionHeaderId = obj.ProductionHeaderId;
				item5.LineId = LineId;
				int code3 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Mfg.USP_ProductionOutPutDetail_Insert"));
				item5.ProductionOutPutDetailId = code3;
				LineId++;
			}
			LineId = 1;
			foreach (ProductionInputDetail item4 in obj.ProductionInputDetailslist)
			{
				item4.ProductionHeaderId = obj.ProductionHeaderId;
				item4.SortNo = LineId;
				int code2 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[Mfg].[USP_productionInputDetail_Insert]"));
				item4.productionInputDetailId = code2;
				LineId++;
			}
			foreach (ProductionOverHeadsDetail item3 in obj.ProductionOverHeadsDetailList)
			{
				item3.ProductionHeaderId = obj.ProductionHeaderId;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[Mfg].[USP_ProductionOverHeadsDetail_Insert]"));
				item3.ProductionOverHeadsDetailId = code;
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
				foreach (DMSAttachments item2 in obj.AttachmentsList.ToList())
				{
					item2.RefAccountId = obj.EntryUserId;
					item2.RefDocumentTypeId = obj.DocumentTypeId;
					item2.RefDocumentNo = returnID;
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					item2.EntryUser = obj.EntryUserId;
					item2.ModifyUser = obj.ModifyUserId;
					item2.OrganizationId = obj.OrganizationId;
					item2.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
			}
			if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
			{
				CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
			}
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
			inventoryTransactions.OrganizationId = obj.OrganizationId;
			inventoryTransactions.CompanyId = obj.CompanyId;
			inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
			inventoryTransactions.RefDocIdNo = obj.ProductionHeaderId;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			if (obj.IsSettled == 1)
			{
				DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.ProductionHeaderId);
				if (dtvh.Rows.Count > 0)
				{
					VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
					obj.VoucherHeadInvoices.Id = VoucherHeadIdGet;
				}
				if (VoucherHeadIdGet == 0)
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.ProductionHeaderId;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.ProductionHeaderId;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Update"));
				}
				if (VoucherHeadId > 0)
				{
					obj.VoucherHeadInvoices.Id = VoucherHeadId;
				}
				else
				{
					VoucherHeadId = obj.VoucherHeadInvoices.Id;
				}
				if (obj.VoucherHeadInvoices.voucherDetailList.Count > 0 && obj.VoucherHeadInvoices.voucherDetailList != null)
				{
					foreach (VoucherDetail item6 in obj.VoucherHeadInvoices.voucherDetailList)
					{
						ProductionOutPutDetail Op = obj.ProductionOutPutDetailslist.Find((ProductionOutPutDetail x) => x.LineId == item6.LineId && x.LineId > 0 && item6.LineId > 0 && item6.DebitAmount > 0.0);
						if (Op != null)
						{
							item6.RefDocSubIdNo = Op.ProductionOutPutDetailId;
						}
						ProductionInputDetail InPut = obj.ProductionInputDetailslist.Find((ProductionInputDetail x) => x.LineId == item6.LineId && x.LineId > 0 && item6.LineId > 0 && item6.CreditAmount > 0.0);
						if (InPut != null)
						{
							item6.RefDocSubIdNo = InPut.productionInputDetailId;
						}
						item6.VoucherHeadId = obj.VoucherHeadInvoices.Id;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "Sp_VoucherDetail_Insert"));
					}
					if (obj.VoucherHeadInvoices.Id > 0)
					{
						SqlCommand sqlCommand = new SqlCommand();
						sqlCommand.Connection = sqlcon;
						sqlCommand.Transaction = sqlTrn;
						sqlCommand.CommandType = CommandType.StoredProcedure;
						sqlCommand.CommandText = "USP_VoucherBalanceCheck";
						sqlCommand.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
						sqlCommand.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
						sqlCommand.Parameters.AddWithValue("@Id", obj.VoucherHeadInvoices.Id);
						sqlCommand.ExecuteScalar();
					}
				}
			}
			else
			{
				DataTable dtvh2 = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.ProductionHeaderId);
				if (dtvh2 != null && dtvh2.Rows.Count > 0)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "usp_VoucherDeleteByVoucherId";
					sqlCommand2.Parameters.AddWithValue("@Id", Conversion.ToInt(dtvh2.Rows[0]["Id"]));
					sqlCommand2.ExecuteNonQuery();
				}
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

	public List<Architecture.Model.Mfg.ProductionHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Mfg.ProductionHeader> lst = new List<Architecture.Model.Mfg.ProductionHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Mfg.ProductionHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Mfg.ProductionHeader item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.ProductionHeaderId));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadDetailByHeaderId"));
						item3.ProductionOutPutDetailslist = GenericProvider.GetAllDetail<ProductionOutPutDetail>(sqlConnection2, "Mfg.USP_ProductionHeader_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Mfg.ProductionHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.ProductionHeaderId));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadInputDetailByHeaderId"));
						item2.ProductionInputDetailslist = GenericProvider.GetAllDetail<ProductionInputDetail>(sqlConnection3, "Mfg.USP_ProductionHeader_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Mfg.ProductionHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.ProductionHeaderId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadOverHeadsDetailByHeaderId"));
					item.ProductionOverHeadsDetailList = GenericProvider.GetAllDetail<ProductionOverHeadsDetail>(sqlcon, "Mfg.USP_ProductionHeader_GetAllMethod", lstPrm);
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
