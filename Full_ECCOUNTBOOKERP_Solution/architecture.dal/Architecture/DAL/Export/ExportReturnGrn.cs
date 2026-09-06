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
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.VPS;

namespace Architecture.DAL.Export;

public class ExportReturnGrn
{
	public int SetData(Architecture.Model.Export.ExportReturnGrn obj, string ProcName)
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
			int LineIdForPDI = 1;
			bool ReturnInvoiceExists = obj.ExportReturnGrnDetailList.Any((ExportReturnGrnDetail x) => x.ReturnInvoiceId > 0);
			foreach (ExportReturnGrnDetail item4 in obj.ExportReturnGrnDetailList)
			{
				item4.LineId += LineIdForPDI;
				item4.ExportReturnGrnId = obj.Id;
				int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[dbo].[USP_ExportReturnGrnDetail_Insert]"));
				item4.Id = code;
			}
			foreach (ExportReturnGrnOtherItems item3 in obj.ExportReturnGrnOtherItemsList)
			{
				item3.ExportReturnGrnId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[dbo].[USP_ExportReturnGrnOtherItems_Insert]"));
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item2 in obj.AttachmentsList.ToList())
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
					item2.RefAccountId = obj.SupplierCustomerId;
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
			InventoryTransactions transactions = new InventoryTransactions();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactions_GetALLMethod"));
			if (ReturnInvoiceExists)
			{
				InventoryStockEvalautionDetail inventoryTransactions = new InventoryStockEvalautionDetail();
				inventoryTransactions.OrganizationId = obj.OrganizationId;
				inventoryTransactions.CompanyId = obj.CompanyId;
				inventoryTransactions.RefDocumentTypeId = obj.DocumentTypeId;
				inventoryTransactions.RefDocIdNo = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, inventoryTransactions, "Sp_InventoryStockEvalautionDetail_Update"));
			}
			if (obj.ExportReturnGrnDetailList.Any((ExportReturnGrnDetail x) => x.ReturnInvoiceId > 0))
			{
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
						obj.VoucherHeadExportReturnGrn.Id = VoucherHeadIdGet;
					}
				}
				if (VoucherHeadIdGet == 0)
				{
					obj.VoucherHeadExportReturnGrn.DocumentTypeSrNo = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadExportReturnGrn, "Sp_VoucherHead_Insert"));
				}
				else
				{
					obj.VoucherHeadExportReturnGrn.DocumentTypeSrNo = obj.Id;
					VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadExportReturnGrn, "Sp_VoucherHead_Update"));
				}
				if (VoucherHeadId > 0)
				{
					obj.VoucherHeadExportReturnGrn.Id = VoucherHeadId;
				}
				else
				{
					VoucherHeadId = obj.VoucherHeadExportReturnGrn.Id;
				}
				if (obj.VoucherHeadExportReturnGrn.voucherDetailList.Count > 0 && obj.VoucherHeadExportReturnGrn.voucherDetailList != null)
				{
					foreach (VoucherDetail item5 in obj.VoucherHeadExportReturnGrn.voucherDetailList)
					{
						ExportReturnGrnDetail sid = obj.ExportReturnGrnDetailList.Find((ExportReturnGrnDetail x) => x.LineId == item5.LineId && x.LineId > 0 && item5.LineId > 0);
						if (sid != null)
						{
							item5.RefDocSubIdNo = sid.Id;
						}
						item5.VoucherHeadId = obj.VoucherHeadExportReturnGrn.Id;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_VoucherDetail_Insert"));
					}
				}
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadExportReturnGrn, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in obj.VoucherHeadExportReturnGrn.voucherDetailList)
				{
					item.VoucherHeadId = obj.VoucherHeadExportReturnGrn.Id;
					item.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
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

	public List<Architecture.Model.Export.ExportReturnGrn> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExportReturnGrn> lst = new List<Architecture.Model.Export.ExportReturnGrn>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.Export.ExportReturnGrn>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExportReturnGrn item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId_ExportReturnGrnDetail"));
						item2.ExportReturnGrnDetailList = GenericProvider.Get<ExportReturnGrnDetail>(sqlConnection2, "[dbo].[USP_ExportReturnGrn_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Export.ExportReturnGrn item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_ExportReturnGrnOtherItems"));
					item.ExportReturnGrnOtherItemsList = GenericProvider.Get<ExportReturnGrnOtherItems>(sqlcon, "[dbo].[USP_ExportReturnGrn_GetAllMethod]", lstPrm);
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
