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
using Architecture.Model.Main;
using Architecture.Model.VPS;

namespace Architecture.DAL.Inventory;

public class InvFreightVoucher
{
	public int SetData(Architecture.Model.Inventory.InvFreightVoucher obj, string ProcName)
	{
		int returnID = 0;
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		string AttachmentFolderPath = "";
		List<string> copiedFiles = new List<string>();
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
			if (obj.driverBiodataId == 0)
			{
				driverBiodata driverInfo = new driverBiodata();
				driverInfo.driverName = obj.DriverName;
				driverInfo.cnicNo = obj.CnicNo;
				driverInfo.cellNo = obj.DriverCellNo;
				driverInfo.whatsappNo = obj.whatsappNo;
				driverInfo.alternateCellNo = obj.AlternateCellNo;
				driverInfo.fatherName = obj.FatherName;
				driverInfo.fatherCnicNo = obj.fatherCnicNo;
				driverInfo.OrganizationId = obj.OrganizationId;
				driverInfo.CompanyId = obj.CompanyId;
				driverInfo.EntryUserId = obj.EntryUserId;
				driverInfo.EntryDate = obj.EntryDate;
				driverInfo.BranchId = obj.BranchId;
				driverInfo.ModifyDate = obj.ModifyDate;
				driverInfo.ApprovedDate = obj.ApprovedDate;
				obj.driverBiodataId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, driverInfo, "[dbo].[USP_DriverBiodata_InsertIfNotExists]"));
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
			foreach (InvFreightVoucherDetail item5 in obj.InvFreightVoucherDetailslist)
			{
				item5.FreightVoucherId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_InvFreightVoucherDetail_Insert"));
			}
			foreach (InvFreightVoucherPaymentBreakup item4 in obj?.InvFreightVoucherPaymentBreakupList)
			{
				item4.InvFreightVoucherId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_InvFreightVoucherPaymentBreakup_Insert"));
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
				foreach (DMSAttachments item3 in obj.AttachmentsList.ToList())
				{
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
			if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
			{
				CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
			}
			if ((obj.InvFreightVoucherDetailslist?.Sum((InvFreightVoucherDetail x) => x.TotalPayableAmount) ?? 0m) > 0m)
			{
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
				if (obj.VoucherHeadInvoices.voucherDetailList == null || obj.VoucherHeadInvoices.voucherDetailList.Count <= 0)
				{
					throw new Exception("VoucherDetail List Not Found");
				}
				foreach (VoucherDetail item2 in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item2.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
				}
				int Code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj.VoucherHeadInvoices, "Sp_VoucherHead_H_Insert"));
				foreach (VoucherDetail item in obj.VoucherHeadInvoices.voucherDetailList)
				{
					item.VoucherHeadId = obj.VoucherHeadInvoices.Id;
					item.DocumentTypeIdRef = Code;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_H_Insert"));
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

	public List<Architecture.Model.Inventory.InvFreightVoucher> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.InvFreightVoucher> lst = new List<Architecture.Model.Inventory.InvFreightVoucher>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.Get<Architecture.Model.Inventory.InvFreightVoucher>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.InvFreightVoucher item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
						item2.InvFreightVoucherDetailslist = GenericProvider.GetAllDetail<InvFreightVoucherDetail>(sqlConnection2, "Sp_InvFreightVoucher_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.InvFreightVoucher item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "PaymentBreakup_ReadByHeaderId"));
					item.InvFreightVoucherPaymentBreakupList = GenericProvider.GetAllDetail<InvFreightVoucherPaymentBreakup>(sqlcon, "Sp_InvFreightVoucher_GetAllMethod", lstPrm);
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
