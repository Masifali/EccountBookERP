using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Mfg;
using Architecture.Model.VPS;

namespace Architecture.DAL.Mfg.BomHeader;

public class BomHeader
{
	public int SetData(Architecture.Model.Mfg.BomHeader obj, string ProcName)
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
				obj.AttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.Attachment));
				obj.CustomAttachmentsValues = string.Join(",", obj.AttachmentsList.Select((DMSAttachments item) => item.UploadedFileCustomName));
			}
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.BomHeaderId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.BomHeaderId;
			}
			foreach (BomInputDetail item7 in obj.BomInputDetailList)
			{
				item7.BomHeaderId = obj.BomHeaderId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "[Mfg].[USP_BomInputDetail_Insert]"));
			}
			foreach (BomFohDetail item6 in obj.BomFohDetailList)
			{
				item6.BomHeaderId = obj.BomHeaderId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "[Mfg].[USP_BomFohDetail_Insert]"));
			}
			foreach (BomByProductDetail item5 in obj.BomByProductDetailList)
			{
				item5.BomHeaderId = obj.BomHeaderId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[Mfg].[USP_BomByProductDetail_Insert]"));
			}
			foreach (BomOperationDetail item4 in obj.BomOperationDetailList)
			{
				item4.bomHeaderId = obj.BomHeaderId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[Mfg].[USP_BomOperationDetail_Insert]"));
			}
			foreach (BomWagesDetail item3 in obj.BomWagesDetailList)
			{
				item3.BomHeaderId = obj.BomHeaderId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[Mfg].[USP_BomWagesDetail_Insert]"));
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

	public List<Architecture.Model.Mfg.BomHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Mfg.BomHeader> lst = new List<Architecture.Model.Mfg.BomHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Mfg.BomHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Mfg.BomHeader item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.BomHeaderId));
						lstPrm5.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomInputDetail"));
						item5.BomInputDetailList = GenericProvider.GetAllDetail<BomInputDetail>(sqlConnection2, "[Mfg].[USP_BomHeader_GetAllMethod]", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Mfg.BomHeader item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.BomHeaderId));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomByProductDetail"));
						item4.BomByProductDetailList = GenericProvider.GetAllDetail<BomByProductDetail>(sqlConnection3, "[Mfg].[USP_BomHeader_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Mfg.BomHeader item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.BomHeaderId));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomFohDetail"));
						item3.BomFohDetailList = GenericProvider.GetAllDetail<BomFohDetail>(sqlConnection4, "[Mfg].[USP_BomHeader_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Mfg.BomHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.BomHeaderId));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomOperationDetail"));
						item2.BomOperationDetailList = GenericProvider.GetAllDetail<BomOperationDetail>(sqlConnection5, "[Mfg].[USP_BomHeader_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Mfg.BomHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.BomHeaderId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomWagesDetail"));
					item.BomWagesDetailList = GenericProvider.GetAllDetail<BomWagesDetail>(sqlcon, "[Mfg].[USP_BomHeader_GetAllMethod]", lstPrm);
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
