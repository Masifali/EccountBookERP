using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Import;
using Architecture.Model.VPS;

namespace Architecture.DAL.Import;

public class invoiceMasterProvider
{
	public static List<invoiceMaster> GetAll(string Activity, List<ParameterList> p = null)
	{
		List<invoiceMaster> lst = new List<invoiceMaster>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetAll<invoiceMaster>(sqlConnection, Activity, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (invoiceMaster item in lst)
					{
						item.invoiceDetailList = GenericProvider.GetAll<invoiceDetail>(sqlConnection2, "SEARCH", p);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (invoiceMaster item2 in lst)
					{
						item2.invoicePaymentTermList = GenericProvider.GetAll<invoicePaymentTerm>(sqlConnection3, "SEARCH", p);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (invoiceMaster item3 in lst)
				{
					item3.invoiceOtherItemList = GenericProvider.GetAll<invoiceOtherItem>(sqlcon, "SEARCH", p);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public static object Set(invoiceMaster obj, string Activity)
	{
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.invoiceDetailList == null || obj.invoiceDetailList.Count == 0)
			{
				throw new Exception("Detail List not found");
			}
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
			object returnID = GenericProvider.Set(sqlTrn, Activity, obj);
			obj.invoiceMasterId = Convert.ToInt32(returnID);
			foreach (invoiceDetail item5 in obj.invoiceDetailList)
			{
				item5.invoiceMasterId = obj.invoiceMasterId;
				string ReturnActivity = CommonProvider.GetActivity(item5.appActionId, Activity);
				GenericProvider.Set(sqlTrn, ReturnActivity, item5);
			}
			foreach (invoicePaymentTerm item4 in obj.invoicePaymentTermList)
			{
				item4.invoiceMasterId = obj.invoiceMasterId;
				string ReturnActivity = CommonProvider.GetActivity(item4.appActionId, Activity);
				GenericProvider.Set(sqlTrn, ReturnActivity, item4);
			}
			foreach (invoiceOtherItem item3 in obj.invoiceOtherItemList)
			{
				item3.invoiceMasterId = obj.invoiceMasterId;
				string ReturnActivity = CommonProvider.GetActivity(item3.appActionId, Activity);
				GenericProvider.Set(sqlTrn, ReturnActivity, item3);
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
					item2.RefAccountId = Conversion.ToInt(obj.exporterId);
					item2.RefDocumentTypeId = obj.documentTypeId;
					item2.RefDocumentNo = Conversion.ToInt(returnID);
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					item2.EntryUser = Conversion.ToInt(obj.createdUserId);
					item2.ModifyUser = Conversion.ToInt(obj.lastModifiedUserId);
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

	public invoiceMaster Get(string Activity, List<ParameterList> p = null)
	{
		invoiceMaster lst = new invoiceMaster();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.Get<invoiceMaster>(sqlcon, Activity, p);
		}
		catch
		{
			throw;
		}
	}

	public static void SetAll(List<invoiceMaster> lst, ParameterObject p = null)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (invoiceMaster item in lst)
			{
				string ReturnActivity = CommonProvider.GetActivity(item.AppActionId);
				GenericProvider.Set(sqlTrn, ReturnActivity, item);
			}
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public void SetXML(object Id, string objXML)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			GenericProvider.SetXML<invoiceMaster>(sqlTrn, Id, objXML);
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public void SetJSON(object Id, string objJSON)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			GenericProvider.SetJSON<invoiceMaster>(sqlTrn, Id, objJSON);
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
