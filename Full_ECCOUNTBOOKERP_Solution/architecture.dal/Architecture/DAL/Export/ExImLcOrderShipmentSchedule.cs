using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.VPS;

namespace Architecture.DAL.Export;

public class ExImLcOrderShipmentSchedule : DALBase
{
	public int SetData(Architecture.Model.Export.ExImLcOrderShipmentSchedule obj)
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
			if (obj.ExImLcOrderShipmentScheduleList != null && obj.ExImLcOrderShipmentScheduleList.Count > 0)
			{
				if (IsVpsAttachmentsServiceOn)
				{
					Sc = CommonServices.GetServerCredentials();
				}
				else
				{
					AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.ExImLcOrderShipmentScheduleList[0].OrganizationId, obj.ExImLcOrderShipmentScheduleList[0].CompanyId, "Attachment Folder Path");
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
				foreach (Architecture.Model.Export.ExImLcOrderShipmentSchedule item3 in obj.ExImLcOrderShipmentScheduleList)
				{
					if (item3.Id > 0)
					{
						item3.AttachmentsValues = obj.AttachmentsValues;
						item3.CustomAttachmentsValues = obj.CustomAttachmentsValues;
						returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_ExImLcOrderShipmentSchedule_Update"));
					}
					else
					{
						item3.AttachmentsValues = obj.AttachmentsValues;
						item3.CustomAttachmentsValues = obj.CustomAttachmentsValues;
						returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_ExImLcOrderShipmentSchedule_Insert"));
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
						Value = obj.ExImLcOrderShipmentScheduleList[0].ExImLcOrderId
					});
					p.Add(new SqlParameter
					{
						ParameterName = "@Activity",
						Value = "DeleteById"
					});
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments item2 in obj.AttachmentsList.ToList())
					{
						item2.RefAccountId = obj.ExImLcOrderShipmentScheduleList[0].ExImLcOrderId;
						item2.ScreenName = obj.AttachmentsList[0].ScreenName;
						item2.RefDocumentTypeId = obj.ExImLcOrderShipmentScheduleList[0].DocumentTypeId;
						item2.RefDocumentNo = obj.ExImLcOrderShipmentScheduleList[0].ExImLcOrderId;
						item2.EntryDate = DateTime.Now;
						item2.ModifyDate = DateTime.Now;
						item2.EntryUser = obj.EntryUserId;
						item2.ModifyUser = obj.EntryUserId;
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
			throw new Exception("No new record found for save-update.Because either list is empty or all record is referred!");
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex;
		}
	}

	public List<Architecture.Model.Export.ExImLcOrderShipmentSchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImLcOrderShipmentSchedule> lst = new List<Architecture.Model.Export.ExImLcOrderShipmentSchedule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.Export.ExImLcOrderShipmentSchedule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public int SetDataForPackingDetail(ExImLcOrderShipmentSchedulePackingDetail obj, string Proc)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.ExImLcOrderShipmentSchedulePackingDetailList.Count == 0)
			{
				throw new Exception("No new detail record found. Because either list is empty or record is referred");
			}
			foreach (ExImLcOrderShipmentSchedulePackingDetail item3 in obj.ExImLcOrderShipmentSchedulePackingDetailList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, Proc));
			}
			if (obj.ExImLcOrderShipmentSchedulePackingDetailList.Count > 0)
			{
				var distlist = obj.ExImLcOrderShipmentSchedulePackingDetailList.Select((ExImLcOrderShipmentSchedulePackingDetail x) => new { x.ExImLcOrderShipmentScheduleId, x.ExImLcOrderId }).Distinct().ToList();
				foreach (var item2 in distlist)
				{
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "[dbo].[usp_PackingMaterialRequirementSchedule_PlanningAutoInsert]";
					sqlCommand.Parameters.AddWithValue("@ContractScheduleId", item2.ExImLcOrderShipmentScheduleId);
					sqlCommand.Parameters.AddWithValue("@ContractId", item2.ExImLcOrderId);
					sqlCommand.ExecuteNonQuery();
				}
				foreach (var item in distlist)
				{
					SqlCommand sqlCommand2 = new SqlCommand();
					sqlCommand2.Connection = sqlcon;
					sqlCommand2.Transaction = sqlTrn;
					sqlCommand2.CommandType = CommandType.StoredProcedure;
					sqlCommand2.CommandText = "[dbo].[USP_ExImScheduleDetailWithPm_UpdateStatus]";
					sqlCommand2.Parameters.AddWithValue("@ContractScheduleId", item.ExImLcOrderShipmentScheduleId);
					sqlCommand2.Parameters.AddWithValue("@ContractId", item.ExImLcOrderId);
					sqlCommand2.ExecuteNonQuery();
				}
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<ExImLcOrderShipmentSchedulePackingDetail> GetDataForPackingDetail(string ProcName, List<SqlParameter> p = null)
	{
		List<ExImLcOrderShipmentSchedulePackingDetail> lst = new List<ExImLcOrderShipmentSchedulePackingDetail>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<ExImLcOrderShipmentSchedulePackingDetail>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public void PackingMaterialRequirementSchedule_PlanningAutoInsert(int ScheduleId, int ContractId)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "[dbo].[usp_PackingMaterialRequirementSchedule_PlanningAutoInsert]";
			sqlCommand.Parameters.AddWithValue("@ContractScheduleId", ScheduleId);
			sqlCommand.Parameters.AddWithValue("@ContractId", ContractId);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
