using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.PartyProcessing;
using Architecture.Model.VPS;

namespace Architecture.DAL.PartyProcessing;

public class InvStockOpeningBalancePartyProcessing
{
	public int SetData(Architecture.Model.PartyProcessing.InvStockOpeningBalancePartyProcessing obj, string ProcName)
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
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
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
					item.RefAccountId = obj.StockPartyId;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = obj.Id;
					item.ScreenName = obj.ScreenName;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					item.EntryUser = obj.EntryUserId;
					item.ModifyUser = obj.EntryUserId;
					item.EntryDate = obj.EntryDate;
					item.ModifyDate = obj.ModifyDate;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
				}
			}
			InventoryTransactionsPartyProcessing transactions = new InventoryTransactionsPartyProcessing();
			transactions.OrganizationId = obj.OrganizationId;
			transactions.CompanyId = obj.CompanyId;
			transactions.RefDocumentTypeId = obj.DocumentTypeId;
			transactions.RefDocIdNo = returnID;
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, transactions, "Sp_InventoryTransactionsPartyProcessing_Insert"));
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

	public List<Architecture.Model.PartyProcessing.InvStockOpeningBalancePartyProcessing> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.InvStockOpeningBalancePartyProcessing> lst = new List<Architecture.Model.PartyProcessing.InvStockOpeningBalancePartyProcessing>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.PartyProcessing.InvStockOpeningBalancePartyProcessing>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
