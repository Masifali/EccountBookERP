using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.PartyProcessing;
using Architecture.Model.VPS;

namespace Architecture.DAL.PartyProcessing;

public class ItemPartyProcessing
{
	public int SetData(Architecture.Model.PartyProcessing.ItemPartyProcessing obj, string ProcName)
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
				if (obj.ItemAllocationlist == null || obj.ItemAllocationlist.Count <= 0)
				{
					throw new Exception("ItemPartyProcessing Allocation List Empty");
				}
				foreach (ItemAllocation ItemPartyProcessing in obj.ItemAllocationlist)
				{
					ItemPartyProcessing.ItemId = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, ItemPartyProcessing, "Sp_ItemAllocation_Insert"));
					if (obj.UomGroupId <= 0)
					{
						continue;
					}
					DataTable lst = null;
					List<SqlParameter> p2 = new List<SqlParameter>();
					p2.Add(new SqlParameter
					{
						ParameterName = "@ItemGroupId",
						Value = obj.UomGroupId
					});
					p2.Add(new SqlParameter
					{
						ParameterName = "@Activity",
						Value = "ReadByItemGroupId"
					});
					lst = GenericProvider.GetDataTableProc<UOMSchedule>("Sp_UOMSchedule_GetAllMethod", p2);
					if (lst.Rows.Count > 0)
					{
						for (int i = 0; i < lst.Rows.Count; i++)
						{
							UOMSchedule cat = new UOMSchedule();
							cat.Equivalent = Conversion.ToSingle(lst.Rows[i]["Equivalent"]);
							cat.ScheduleUnitId = Conversion.ToInt(lst.Rows[i]["UOMId"]);
							cat.ItemId = Conversion.ToInt(returnID);
							cat.EntryDate = DateTime.Now;
							cat.ModifyDate = DateTime.Now;
							cat.EntryUser = obj.EntryUser;
							cat.OrganizationId = ItemPartyProcessing.OrganizationId;
							cat.CompanyId = ItemPartyProcessing.CompanyId;
							cat.Active = true;
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, cat, "Sp_UOMSchedule_Insert"));
						}
					}
				}
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
					item.RefAccountId = returnID;
					item.RefDocumentTypeId = 0;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = obj.EntryUser;
					item.ModifyUser = obj.ModifyUser;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
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

	public List<Architecture.Model.PartyProcessing.ItemPartyProcessing> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PartyProcessing.ItemPartyProcessing> lst = new List<Architecture.Model.PartyProcessing.ItemPartyProcessing>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.PartyProcessing.ItemPartyProcessing>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
