using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.PartyProcessing;
using Architecture.Model.VPS;

namespace Architecture.DAL.PartyProcessing;

public class GatePassPartyProcessing
{
	public int SetData(Architecture.Model.PartyProcessing.GatePassPartyProcessing obj, string ProcName)
	{
		int returnID = 0;
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
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
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
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = obj.SupplierCustomerId;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = obj.Id;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					item.EntryUser = obj.EntryUser;
					item.ModifyUser = obj.EntryUser;
					item.EntryDate = obj.EntryDate;
					item.ModifyDate = obj.ModifyDate;
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

	public List<Architecture.Model.PartyProcessing.GatePassPartyProcessing> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.GatePassPartyProcessing> lst = new List<Architecture.Model.PartyProcessing.GatePassPartyProcessing>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.PartyProcessing.GatePassPartyProcessing>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public void DeleteById(int Id, int EntryUserId)
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
			sqlCommand.CommandText = "Sp_GatePassPartyProcessing_GetAllMethod";
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.Parameters.AddWithValue("@EntryUser", EntryUserId);
			sqlCommand.Parameters.AddWithValue("@Activity", "DeleteById");
			sqlCommand.ExecuteScalar();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
