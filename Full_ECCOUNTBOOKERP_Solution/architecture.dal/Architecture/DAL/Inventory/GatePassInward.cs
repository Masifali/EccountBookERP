using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.VPS;

namespace Architecture.DAL.Inventory;

public class GatePassInward : DALBase
{
	public int SetData(Architecture.Model.Inventory.GatePassInward obj, string ProcName)
	{
		int returnID = 0;
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
			if (obj.driverBioDataId == 0)
			{
				driverBiodata driverInfo = new driverBiodata();
				driverInfo.driverName = obj.DriverName;
				driverInfo.cnicNo = obj.DriverCNICNO;
				driverInfo.cellNo = obj.DriverMobileNo;
				driverInfo.whatsappNo = obj.whatsappNo;
				driverInfo.alternateCellNo = obj.AlternateCellNo;
				driverInfo.fatherName = obj.FatherName;
				driverInfo.fatherCnicNo = obj.fatherCnicNo;
				driverInfo.OrganizationId = obj.OrganizationId;
				driverInfo.CompanyId = obj.CompanyId;
				driverInfo.EntryUserId = obj.EntryUser;
				driverInfo.EntryDate = Conversion.ToDateTime(obj.EntryDate);
				driverInfo.BranchId = obj.BranchesId;
				driverInfo.ModifyDate = Conversion.ToDateTime(obj.ModifyDate);
				driverInfo.ApprovedDate = Conversion.ToDateTime(obj.PostDate);
				obj.driverBioDataId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, driverInfo, "[dbo].[USP_DriverBiodata_InsertIfNotExists]"));
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
			if (obj.gatePassInwardDetails != null && obj.gatePassInwardDetails.Count > 0)
			{
				foreach (GatePassInwardDetail item3 in obj.gatePassInwardDetails)
				{
					item3.GatePassInwardId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_GatePassInwardDetail_Insert"));
				}
			}
			if (obj.GatePassInwardPurchaseBreakUpList != null && obj.GatePassInwardPurchaseBreakUpList.Count > 0)
			{
				foreach (GatePassInwardPurchaseBreakUp item2 in obj.GatePassInwardPurchaseBreakUpList)
				{
					item2.InwardGatePassId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[dbo].[USP_GatePassInwardPurchaseBreakUp_Insert]"));
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
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = obj.SupplierCustomerId;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = returnID;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = obj.EntryUser;
					item.ModifyUser = obj.ModifyUser;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
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

	public List<Architecture.Model.Inventory.GatePassInward> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.GatePassInward> lst = new List<Architecture.Model.Inventory.GatePassInward>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAllDetail<Architecture.Model.Inventory.GatePassInward>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.Inventory.GatePassInward item2 in lst)
					{
						if (item2.DocumentTypeId == 53)
						{
							List<SqlParameter> lstPrm2 = new List<SqlParameter>();
							lstPrm2.Add(new SqlParameter("@Id", item2.Id));
							lstPrm2.Add(new SqlParameter("@Activity", "ReadDetailByGatePassInward"));
							item2.gatePassInwardDetails = GenericProvider.Get<GatePassInwardDetail>(sqlConnection2, "Sp_GatePassInward_GetAllMethod", lstPrm2);
						}
						else
						{
							List<SqlParameter> lstPrm3 = new List<SqlParameter>();
							lstPrm3.Add(new SqlParameter("@Id", item2.Id));
							lstPrm3.Add(new SqlParameter("@Activity", "ReadById"));
							item2.gatePassInwardDetails = GenericProvider.Get<GatePassInwardDetail>(sqlConnection2, "Sp_GatePassInwardDetail_GetAllMethod", lstPrm3);
						}
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null && lst.Count > 0)
			{
				foreach (Architecture.Model.Inventory.GatePassInward item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_PurchaseBrekup"));
					item.GatePassInwardPurchaseBreakUpList = GenericProvider.Get<GatePassInwardPurchaseBreakUp>(sqlcon, "Sp_GatePassInward_GetAllMethod", lstPrm);
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
