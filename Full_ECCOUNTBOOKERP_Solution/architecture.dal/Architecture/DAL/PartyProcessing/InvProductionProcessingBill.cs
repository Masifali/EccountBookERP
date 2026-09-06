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
using Architecture.Model.PartyProcessing;
using Architecture.Model.VPS;

namespace Architecture.DAL.PartyProcessing;

public class InvProductionProcessingBill
{
	public int SetData(Architecture.Model.PartyProcessing.InvProductionProcessingBill obj, string ProcName)
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
			foreach (InvProductionProcessingBillOH item6 in obj.InvProductionProcessingBillOHslist)
			{
				item6.InvProductionProcessingBillId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "USP_InvProductionProcessingBillOH_Insert"));
			}
			foreach (InvProductionProcessingBillPM item5 in obj.InvProductionProcessingBillPMslist)
			{
				item5.InvProductionProcessingBillId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "USP_InvProductionProcessingBillPM_Insert"));
			}
			foreach (InvProductionProcessingBillOutPut item4 in obj.InvProductionProcessingBillOutPutslist)
			{
				item4.InvProductionProcessingBillId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_InvProductionProcessingBillOutPut_Insert"));
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
				foreach (DMSAttachments item3 in obj.AttachmentsList.ToList())
				{
					item3.RefAccountId = obj.StockPartyId;
					item3.RefDocumentTypeId = obj.DocumentTypeId;
					item3.RefDocumentNo = obj.Id;
					item3.OrganizationId = obj.OrganizationId;
					item3.CompanyId = obj.CompanyId;
					item3.EntryUser = obj.EntryUserId;
					item3.ModifyUser = obj.ModifyUserId;
					item3.EntryDate = obj.EntryDate;
					item3.ModifyDate = obj.ModifyDate;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Proc_DMSAttachments_Insert"));
				}
			}
			DataTable dtvh = CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
			if (dtvh.Rows.Count > 0)
			{
				VoucherHeadIdGet = Conversion.ToInt(dtvh.Rows[0]["Id"]);
				obj.VoucherHeadInvoices.Id = VoucherHeadIdGet;
			}
			if (VoucherHeadIdGet == 0)
			{
				obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
				obj.VoucherHeadInvoices.RefDocNoId = obj.Id;
				VoucherHeadId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_Insert"));
			}
			else
			{
				obj.VoucherHeadInvoices.DocumentTypeSrNo = obj.Id;
				obj.VoucherHeadInvoices.RefDocNoId = obj.Id;
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
			foreach (VoucherDetail item2 in obj.VoucherHeadInvoices.voucherDetailList)
			{
				item2.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
			}
			if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count == 0)
			{
				throw new Exception("Voucher Detail list Not Found");
			}
			int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_H_Insert"));
			foreach (VoucherDetail item in obj.VoucherHeadInvoices.voucherDetailList)
			{
				item.VoucherHeadId = obj.VoucherHeadInvoices.Id;
				item.DocumentTypeIdRef = Code;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
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

	public List<Architecture.Model.PartyProcessing.InvProductionProcessingBill> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.InvProductionProcessingBill> lst = new List<Architecture.Model.PartyProcessing.InvProductionProcessingBill>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.PartyProcessing.InvProductionProcessingBill>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.PartyProcessing.InvProductionProcessingBill item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadBillOhById"));
						item3.InvProductionProcessingBillOHslist = GenericProvider.Get<InvProductionProcessingBillOH>(sqlConnection2, "USP_InvProductionProcessingBill_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.PartyProcessing.InvProductionProcessingBill item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadBillPMById"));
						item2.InvProductionProcessingBillPMslist = GenericProvider.Get<InvProductionProcessingBillPM>(sqlConnection3, "USP_InvProductionProcessingBill_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.PartyProcessing.InvProductionProcessingBill item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadBillOutPutByHeaderId"));
					item.InvProductionProcessingBillOutPutslist = GenericProvider.Get<InvProductionProcessingBillOutPut>(sqlcon, "USP_InvProductionProcessingBill_GetAllMethod", lstPrm);
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
