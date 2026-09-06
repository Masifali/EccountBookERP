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
using Architecture.Model.PurchaseTrading;
using Architecture.Model.VPS;

namespace Architecture.DAL.PurchaseTrading;

public class InvStockOpeningBalanceHeader
{
	public int SetData(Architecture.Model.PurchaseTrading.InvStockOpeningBalanceHeader obj, string ProcName)
	{
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
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
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
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
				foreach (DMSAttachments item4 in obj.AttachmentsList.ToList())
				{
					item4.RefAccountId = obj.ItemId;
					item4.RefDocumentTypeId = obj.DocumentTypeId;
					item4.RefDocumentNo = returnID;
					item4.EntryDate = DateTime.Now;
					item4.ModifyDate = DateTime.Now;
					item4.EntryUser = obj.EntryUserId;
					item4.ModifyUser = obj.ModifyUserId;
					item4.OrganizationId = obj.OrganizationId;
					item4.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Proc_DMSAttachments_Insert"));
				}
			}
			if (obj.ModifyUserId > 0)
			{
				List<SqlParameter> s = new List<SqlParameter>();
				s.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId"
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = obj.OrganizationId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.CompanyId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeSrNo",
					Value = obj.Id
				});
				DataTable dtvh = GenericProvider.GetDataTableProc<VoucherHead>("Sp_Vouchers_GetMethods", s);
				if (dtvh.Rows.Count > 0)
				{
					VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
					obj.VoucherHeadOpeningBalance.Id = VoucherHeadIdGet;
				}
			}
			if (VoucherHeadIdGet == 0)
			{
				obj.VoucherHeadOpeningBalance.DocumentTypeSrNo = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadOpeningBalance, "Sp_VoucherHead_Insert"));
			}
			else
			{
				obj.VoucherHeadOpeningBalance.DocumentTypeSrNo = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadOpeningBalance, "Sp_VoucherHead_Update"));
			}
			if (VoucherHeadId > 0)
			{
				obj.VoucherHeadOpeningBalance.Id = VoucherHeadId;
			}
			else
			{
				VoucherHeadId = obj.VoucherHeadOpeningBalance.Id;
			}
			if (obj.VoucherHeadOpeningBalance.voucherDetailList.Count > 0 && obj.VoucherHeadOpeningBalance.voucherDetailList != null)
			{
				foreach (VoucherDetail item3 in obj.VoucherHeadOpeningBalance.voucherDetailList)
				{
					item3.VoucherHeadId = obj.VoucherHeadOpeningBalance.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_VoucherDetail_Insert"));
				}
			}
			int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadOpeningBalance, "Sp_VoucherHead_H_Insert"));
			foreach (VoucherDetail item2 in obj.VoucherHeadOpeningBalance.voucherDetailList)
			{
				item2.VoucherHeadId = obj.VoucherHeadOpeningBalance.Id;
				item2.DocumentTypeIdRef = Code;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_H_Insert"));
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
			inventoryTransactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
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
}
