using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Steel.Production;
using Architecture.Model.VPS;

namespace Architecture.DAL.Steel.Production;

public class InvProductionJobOrder
{
	public int SetData(Architecture.Model.Steel.Production.InvProductionJobOrder obj, string ProcName)
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
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (InvProductionJobOrderInput item5 in obj.invProductionJobOrderInput)
			{
				item5.InvProductionJobOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[ST].[USP_InvProductionJobOrderInput_Insert]"));
			}
			foreach (InvProductionJobOrderOutput item4 in obj.invProductionJobOrderOutput)
			{
				item4.InvProductionJobOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[ST].[USP_InvProductionJobOrderOutput_Insert]"));
			}
			foreach (InvProductionJobOrderPlant item3 in obj.InvProductionJobOrderPlantslist)
			{
				item3.InvProductionJobOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[ST].[USP_InvProductionJobOrderPlant_Insert]"));
			}
			foreach (InvProductionJobOrderOverHeads item2 in obj.InvProductionJobOrderOverHeadsList)
			{
				item2.ProductionJobOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[ST].[USP_InvProductionJobOrderOverHeads_Insert]"));
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
					item.RefAccountId = 0;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = obj.EntryUserId;
					item.ModifyUser = obj.EntryUserId;
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

	public List<Architecture.Model.Steel.Production.InvProductionJobOrder> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Steel.Production.InvProductionJobOrder> lst = new List<Architecture.Model.Steel.Production.InvProductionJobOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Steel.Production.InvProductionJobOrder>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Steel.Production.InvProductionJobOrder item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadInputDetailByJobOrderId"));
						item4.invProductionJobOrderInput = GenericProvider.GetAllDetail<InvProductionJobOrderInput>(sqlConnection2, "[ST].[USP_ProductionJobOrder_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Steel.Production.InvProductionJobOrder item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadOutPutDetailByJobOrderId"));
						item3.invProductionJobOrderOutput = GenericProvider.GetAllDetail<InvProductionJobOrderOutput>(sqlConnection3, "[ST].[USP_ProductionJobOrder_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Steel.Production.InvProductionJobOrder item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadPlantDetailByJobOrderId"));
						item2.InvProductionJobOrderPlantslist = GenericProvider.GetAllDetail<InvProductionJobOrderPlant>(sqlConnection4, "[ST].[USP_ProductionJobOrder_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Steel.Production.InvProductionJobOrder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadOverHeadsDetailByJobOrderId"));
					item.InvProductionJobOrderOverHeadsList = GenericProvider.GetAllDetail<InvProductionJobOrderOverHeads>(sqlcon, "[ST].[USP_ProductionJobOrder_GetAllMethod]", lstPrm);
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
