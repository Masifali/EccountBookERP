using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.TaxProject.Accounts;
using Architecture.Model.TaxProject.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.TaxProject.Inventory;

public class InvGsStoreIssuanceHeader : DALBase
{
	public int SetData(Architecture.Model.TaxProject.Inventory.InvGsStoreIssuanceHeader obj, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		new List<SupplierCustomer>();
		using SqlConnection sqlcon = ConnectionObject.SQLForServer();
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
			if (obj.DocumentTypeId == 1616 && obj.ActionForVoucherId != 1)
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
				if (obj.gsStoreIssuanceDetails != null)
				{
					foreach (InvGsStoreIssuanceDetail item6 in obj.gsStoreIssuanceDetails)
					{
						item6.InvGsStoreIssuanceHeaderId = obj.Id;
						int code3 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "Sp_InvGsStoreIssuanceDetail_Insert"));
						item6.Id = code3;
					}
				}
				InventoryTransactions transactions3 = new InventoryTransactions();
				transactions3.OrganizationId = obj.OrganizationId;
				transactions3.CompanyId = obj.CompanyId;
				transactions3.RefDocumentTypeId = obj.DocumentTypeId;
				transactions3.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions3, "Sp_InventoryTransactions_GetALLMethod"));
				InventoryStockEvalautionDetail inventoryTransactions3 = new InventoryStockEvalautionDetail();
				inventoryTransactions3.OrganizationId = obj.OrganizationId;
				inventoryTransactions3.CompanyId = obj.CompanyId;
				inventoryTransactions3.RefDocumentTypeId = obj.DocumentTypeId;
				inventoryTransactions3.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions3, "Sp_InventoryStockEvalautionDetail_Update"));
			}
			if (obj.DocumentTypeId == 451 && obj.ActionForVoucherId != 1)
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
				if (obj.gsStoreIssuanceDetails != null)
				{
					foreach (InvGsStoreIssuanceDetail item5 in obj.gsStoreIssuanceDetails)
					{
						item5.InvGsStoreIssuanceHeaderId = obj.Id;
						int code2 = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_InvGsStoreIssuanceDetail_Insert"));
						item5.Id = code2;
					}
				}
				InventoryTransactions transactions2 = new InventoryTransactions();
				transactions2.OrganizationId = obj.OrganizationId;
				transactions2.CompanyId = obj.CompanyId;
				transactions2.RefDocumentTypeId = obj.DocumentTypeId;
				transactions2.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions2, "Sp_InventoryTransactions_GetALLMethod"));
				InventoryStockEvalautionDetail inventoryTransactions2 = new InventoryStockEvalautionDetail();
				inventoryTransactions2.OrganizationId = obj.OrganizationId;
				inventoryTransactions2.CompanyId = obj.CompanyId;
				inventoryTransactions2.RefDocumentTypeId = obj.DocumentTypeId;
				inventoryTransactions2.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions2, "Sp_InventoryStockEvalautionDetail_Update"));
			}
			if (obj.DocumentTypeId == 452 || obj.DocumentTypeId == 213)
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
				if (obj.gsStoreIssuanceDetails != null)
				{
					foreach (InvGsStoreIssuanceDetail item4 in obj.gsStoreIssuanceDetails)
					{
						item4.InvGsStoreIssuanceHeaderId = obj.Id;
						int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_InvGsStoreIssuanceDetail_Insert"));
						item4.Id = code;
					}
				}
				InventoryTransactions transactions = new InventoryTransactions();
				transactions.OrganizationId = obj.OrganizationId;
				transactions.CompanyId = obj.CompanyId;
				transactions.RefDocumentTypeId = obj.DocumentTypeId;
				transactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
				InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
				inventoryTransactions.OrganizationId = obj.OrganizationId;
				inventoryTransactions.CompanyId = obj.CompanyId;
				inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
				inventoryTransactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			}
			if (obj.ActionForVoucherId == 1 && obj.VoucherHead != null)
			{
				DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
				if (dtvh.Rows.Count > 0)
				{
					VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
					obj.VoucherHead.Id = VoucherHeadIdGet;
				}
				if (VoucherHeadIdGet == 0)
				{
					obj.VoucherHead.DocumentTypeSrNo = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHead, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHead.DocumentTypeSrNo = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHead, "Sp_VoucherHead_Update"));
				}
				if (VoucherHeadId > 0)
				{
					obj.VoucherHead.Id = VoucherHeadId;
				}
				else
				{
					VoucherHeadId = obj.VoucherHead.Id;
				}
				if (obj.VoucherHead.voucherDetailList.Count <= 0 || obj.VoucherHead.voucherDetailList == null)
				{
					throw new Exception("VoucherDetail List Not Found");
				}
				foreach (VoucherDetail item7 in obj.VoucherHead.voucherDetailList)
				{
					InvGsStoreIssuanceDetail sid = obj.gsStoreIssuanceDetails.Find((InvGsStoreIssuanceDetail x) => x.LineId == item7.LineId);
					if (sid != null)
					{
						item7.RefDocSubIdNo = sid.Id;
					}
					item7.VoucherHeadId = obj.VoucherHead.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "Sp_VoucherDetail_Insert"));
				}
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHead, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item3 in obj.VoucherHead.voucherDetailList)
				{
					item3.VoucherHeadId = obj.VoucherHead.Id;
					item3.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_VoucherDetail_H_Insert"));
				}
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
					item2.RefDocumentTypeId = obj.DocumentTypeId;
					item2.RefDocumentNo = returnID;
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					item2.EntryUser = obj.EntryUser;
					item2.ModifyUser = obj.EntryUser;
					item2.OrganizationId = obj.OrganizationId;
					item2.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
			}
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

	public List<Architecture.Model.TaxProject.Inventory.InvGsStoreIssuanceHeader> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.TaxProject.Inventory.InvGsStoreIssuanceHeader> lst = new List<Architecture.Model.TaxProject.Inventory.InvGsStoreIssuanceHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.TaxProject.Inventory.InvGsStoreIssuanceHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.TaxProject.Inventory.InvGsStoreIssuanceHeader item in lst)
				{
					if (item.RefDocumentTypeId == 450 || item.DocumentTypeId == 1616)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "InvGsStoreIssuanceDetailReadById"));
						item.gsStoreIssuanceDetails = GenericProvider.Get<InvGsStoreIssuanceDetail>(sqlcon, "Sp_InvGsStoreIssuanceHeader_GetAllMethod", lstPrm3);
					}
					if (item.RefDocumentTypeId == 85)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "InvGsStoreIssuanceDetailReadByIdForDelivery"));
						item.gsStoreIssuanceDetails = GenericProvider.Get<InvGsStoreIssuanceDetail>(sqlcon, "Sp_InvGsStoreIssuanceHeader_GetAllMethod", lstPrm2);
					}
					if (item.DocumentTypeId == 452 || item.DocumentTypeId == 213)
					{
						List<SqlParameter> lstPrm = new List<SqlParameter>();
						lstPrm.Add(new SqlParameter("@Id", item.Id));
						lstPrm.Add(new SqlParameter("@Activity", "InvGsStoreIssuanceDetailReadByIdForIssuanceDirect"));
						item.gsStoreIssuanceDetails = GenericProvider.Get<InvGsStoreIssuanceDetail>(sqlcon, "Sp_InvGsStoreIssuanceHeader_GetAllMethod", lstPrm);
					}
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
