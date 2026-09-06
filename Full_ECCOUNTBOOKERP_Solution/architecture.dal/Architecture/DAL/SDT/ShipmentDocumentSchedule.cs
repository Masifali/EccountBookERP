using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.SDT;
using Architecture.Model.VPS;

namespace Architecture.DAL.SDT;

public class ShipmentDocumentSchedule
{
	public int SetData(Architecture.Model.SDT.ShipmentDocumentSchedule obj, string ProcName)
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
			if (obj != null && obj.ShipmentDocumentScheduleList.Count > 0)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "[sdt].[USP_ShipmentDocumentSchedule_UpdateRefDocument]";
				sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", obj.ShipmentDocumentScheduleList[0].referenceDocumentTypeId);
				sqlCommand.Parameters.AddWithValue("@RefDocId", obj.ShipmentDocumentScheduleList[0].refDocumentId);
				sqlCommand.Parameters.AddWithValue("@RefDocCustomGroupId", obj.ShipmentDocumentScheduleList[0].RefDocCustomGroupId);
				sqlCommand.Parameters.AddWithValue("@RefDocLockStatus", obj.ShipmentDocumentScheduleList[0].RefDocLockStatus);
				sqlCommand.ExecuteNonQuery();
			}
			foreach (Architecture.Model.SDT.ShipmentDocumentSchedule item2 in obj.ShipmentDocumentScheduleList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, ProcName));
			}
			if (obj.DetailAttachmentsList != null && obj.DetailAttachmentsList.Count > 0)
			{
				if (IsVpsAttachmentsServiceOn)
				{
					Sc = CommonServices.GetServerCredentials();
				}
				else
				{
					AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
					if (!Directory.Exists(AttachmentFolderPath))
					{
						throw new Exception("Attachment Folder Path Not Configure Properly");
					}
				}
				if (obj.DetailAttachmentsList != null && obj.DetailAttachmentsList.Count > 0)
				{
					obj.DetailAttachmentsList = CommonServices.CommonMethodForAttachmentsToPhysicallySave(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DetailAttachmentsList, copiedFiles);
				}
				int i = 0;
				if (obj.DetailAttachmentsList != null && obj.DetailAttachmentsList.Count > 0)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "Sp_DMSAttachments_GetAllMethod";
					sqlCommand2.Parameters.AddWithValue("@Id", obj.ShipmentDocumentScheduleList[0].refDocumentId);
					sqlCommand2.Parameters.AddWithValue("@ScreenName", obj.DetailAttachmentsList[0].ScreenName);
					sqlCommand2.Parameters.AddWithValue("@Activity", "DetailDeleteById");
					sqlCommand2.ExecuteNonQuery();
					foreach (DMSAttachments item in obj.DetailAttachmentsList.ToList())
					{
						item.RefDocumentTypeId = obj.ShipmentDocumentScheduleList[0].referenceDocumentTypeId;
						item.RefAccountId = obj.ShipmentDocumentScheduleList[0].refDocumentId;
						item.RefDocumentNo = obj.ShipmentDocumentScheduleList[0].refDocumentId;
						item.LineId = obj.DetailAttachmentsList[i].LineId;
						item.EntryDate = DateTime.Now;
						item.ModifyDate = DateTime.Now;
						item.EntryUser = obj.EntryUserId;
						item.ModifyUser = obj.ModifyUserId;
						item.OrganizationId = obj.ShipmentDocumentScheduleList[0].OrganizationId;
						item.CompanyId = obj.ShipmentDocumentScheduleList[0].CompanyId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
						i++;
					}
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

	public void UpdateRefDocument(int RefDocumentTypeId, int RefDocId, int RefDocCustomGroupId, bool RefDocLockStatus)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (RefDocumentTypeId > 0)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "[sdt].[USP_ShipmentDocumentSchedule_UpdateRefDocument]";
				sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", RefDocumentTypeId);
				sqlCommand.Parameters.AddWithValue("@RefDocId", RefDocId);
				sqlCommand.Parameters.AddWithValue("@RefDocCustomGroupId", RefDocCustomGroupId);
				sqlCommand.Parameters.AddWithValue("@RefDocLockStatus", RefDocLockStatus);
				sqlCommand.ExecuteNonQuery();
			}
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public void AutoSaveByRefDocumentType(int RefDocumentTypeId, int RefDocId, int CustomGroupId, string ChartOfDocumentIds)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (RefDocumentTypeId > 0 && RefDocId > 0)
			{
				SqlCommand sqlCommandAutoInsert = new SqlCommand
				{
					Connection = sqlcon,
					Transaction = sqlTrn,
					CommandType = CommandType.StoredProcedure,
					CommandText = "[sdt].[USP_ShipmentDocumentSchedule_AutoInsert]"
				};
				sqlCommandAutoInsert.Parameters.AddWithValue("@RefDocumentTypeId", RefDocumentTypeId);
				sqlCommandAutoInsert.Parameters.AddWithValue("@RefDocId", RefDocId);
				sqlCommandAutoInsert.Parameters.AddWithValue("@CustomGroupId", CustomGroupId);
				if (!string.IsNullOrEmpty(ChartOfDocumentIds))
				{
					sqlCommandAutoInsert.Parameters.AddWithValue("@ChartOfDocumentIds", ChartOfDocumentIds);
				}
				sqlCommandAutoInsert.ExecuteNonQuery();
			}
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public void DeleteData(string Ids, string CODIds, int RefDocumentTypeId, int EntryUserId)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (!string.IsNullOrEmpty(Ids))
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "[sdt].[USP_ShipmentDocumentSchedule_GetAllMethod]";
				sqlCommand.Parameters.AddWithValue("@Activity", "DeleteByIds");
				sqlCommand.Parameters.AddWithValue("@Ids", Ids);
				sqlCommand.Parameters.AddWithValue("@CODIds", CODIds);
				sqlCommand.Parameters.AddWithValue("@RefDocumentTypeId", RefDocumentTypeId);
				sqlCommand.Parameters.AddWithValue("@EntryUserId", EntryUserId);
				sqlCommand.ExecuteNonQuery();
			}
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.SDT.ShipmentDocumentSchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SDT.ShipmentDocumentSchedule> lst = new List<Architecture.Model.SDT.ShipmentDocumentSchedule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.SDT.ShipmentDocumentSchedule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
