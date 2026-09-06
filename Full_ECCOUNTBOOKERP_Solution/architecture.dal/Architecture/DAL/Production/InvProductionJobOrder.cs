using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Production;
using Architecture.Model.VPS;

namespace Architecture.DAL.Production;

public class InvProductionJobOrder
{
	public int SetData(Architecture.Model.Production.InvProductionJobOrder obj, string ProcName)
	{
		int returnID = 0;
		bool AutoJobLotCreateOnJobOrder = false;
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
			AutoJobLotCreateOnJobOrder = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "AutoJobLotCreateOnJobOrder"));
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (InvProductionJobOrderInput item9 in obj.invProductionJobOrderInput)
			{
				item9.InvProductionJobOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "Sp_InvProductionJobOrderInput_Insert"));
			}
			foreach (InvProductionJobOrderOutput item8 in obj.invProductionJobOrderOutput)
			{
				item8.InvProductionJobOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "Sp_InvProductionJobOrderOutput_Insert"));
			}
			foreach (InvProductionJobOrderPlant item7 in obj.InvProductionJobOrderPlantslist)
			{
				item7.InvProductionJobOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "Sp_InvProductionJobOrderPlant_Insert"));
			}
			foreach (InvProductionJobOrderOverHeads item6 in obj.InvProductionJobOrderOverHeadsList)
			{
				item6.InvProductionJobOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "USP_InvProductionJobOrderOverHeads_Insert"));
			}
			foreach (InvProductionJobOrderPackingMaterial item5 in obj.InvProductionJobOrderPackingMaterialList)
			{
				item5.InvProductionJobOrderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "USP_InvProductionJobOrderPackingMaterial_Insert"));
			}
			if (obj.JobOrderRateSchedulelist != null && obj.JobOrderRateSchedulelist.Count > 0)
			{
				foreach (JobOrderRateSchedule item4 in obj.JobOrderRateSchedulelist)
				{
					item4.JobOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "USP_JobOrderRateSchedule_Insert"));
				}
			}
			if (obj.InvProductionJobOrderAndOrderAllocationList != null && obj.InvProductionJobOrderAndOrderAllocationList.Count > 0)
			{
				foreach (InvProductionJobOrderAndOrderAllocation item3 in obj.InvProductionJobOrderAndOrderAllocationList)
				{
					item3.InvProductionJobOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_InvProductionJobOrderAndOrderAllocation_Insert_Update"));
				}
			}
			if (obj.InvProductionJobOrderLabStandardDetailList != null && obj.InvProductionJobOrderLabStandardDetailList.Count > 0)
			{
				foreach (InvProductionJobOrderLabStandardDetail item2 in obj.InvProductionJobOrderLabStandardDetailList)
				{
					item2.InvProductionJobOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "USP_InvProductionJobOrderLabStandardDetail_Insert"));
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
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = obj.EntryUser;
					item.ModifyUser = obj.EntryUser;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
				}
			}
			if (AutoJobLotCreateOnJobOrder && obj.ActionId == 1)
			{
				JobLot job = new JobLot();
				job.RefDocNoId = returnID;
				job.RefDocumentTypeId = obj.DocumentTypeId;
				job.JobLotCode = obj.RefInvoiceNo;
				job.JobLotDescription = obj.RefInvoiceNo;
				job.StartDate = obj.StartDate;
				job.EndDate = obj.EndDate;
				job.JobStatus = "InComplete";
				job.IsApproved = false;
				job.EntryDate = DateTime.Now;
				job.ModifyDate = DateTime.Now;
				job.EntryUser = obj.EntryUser;
				job.ModifyUser = obj.EntryUser;
				job.OrganizationId = obj.OrganizationId;
				job.CompanyId = obj.CompanyId;
				job.JobTypeId = 22;
				Conversion.ToInt(GenericProvider.SetProc(sqlTrn, job, "Proc_JobLot_Insert"));
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

	public List<Architecture.Model.Production.InvProductionJobOrder> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.InvProductionJobOrder> lst = new List<Architecture.Model.Production.InvProductionJobOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Production.InvProductionJobOrder>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvProductionJobOrder item8 in lst)
					{
						List<SqlParameter> lstPrm8 = new List<SqlParameter>();
						lstPrm8.Add(new SqlParameter("@Id", item8.Id));
						item8.invProductionJobOrderInput = GenericProvider.GetAllDetail<InvProductionJobOrderInput>(sqlConnection2, "Sp_InvProductionJobOrderInput_GetAllMethod", lstPrm8);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvProductionJobOrder item7 in lst)
					{
						List<SqlParameter> lstPrm7 = new List<SqlParameter>();
						lstPrm7.Add(new SqlParameter("@Id", item7.Id));
						item7.invProductionJobOrderOutput = GenericProvider.GetAllDetail<InvProductionJobOrderOutput>(sqlConnection3, "Sp_InvProductionJobOrderOutput_GetAllMethod", lstPrm7);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvProductionJobOrder item6 in lst)
					{
						List<SqlParameter> lstPrm6 = new List<SqlParameter>();
						lstPrm6.Add(new SqlParameter("@Id", item6.Id));
						lstPrm6.Add(new SqlParameter("@Activity", "GetPlantDetailByHeaderId"));
						item6.InvProductionJobOrderPlantslist = GenericProvider.GetAllDetail<InvProductionJobOrderPlant>(sqlConnection4, "Sp_InvProductionJobOrder_GetAllMethod", lstPrm6);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvProductionJobOrder item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.Id));
						lstPrm5.Add(new SqlParameter("@Activity", "ReadById"));
						item5.InvProductionJobOrderOverHeadsList = GenericProvider.GetAllDetail<InvProductionJobOrderOverHeads>(sqlConnection5, "[dbo].[USP_InvProductionJobOrderOverHeads_GetAllMethod]", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection6 = ConnectionObject.SQL())
			{
				sqlConnection6.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvProductionJobOrder item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadById"));
						item4.InvProductionJobOrderPackingMaterialList = GenericProvider.GetAllDetail<InvProductionJobOrderPackingMaterial>(sqlConnection6, "USP_InvProductionJobOrderPackingMaterial_GetAllMethod", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection7 = ConnectionObject.SQL())
			{
				sqlConnection7.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvProductionJobOrder item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "GetJobScheduleByHeaderId"));
						item3.JobOrderRateSchedulelist = GenericProvider.GetAllDetail<JobOrderRateSchedule>(sqlConnection7, "Sp_InvProductionJobOrder_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection8 = ConnectionObject.SQL())
			{
				sqlConnection8.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvProductionJobOrder item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadById"));
						item2.InvProductionJobOrderAndOrderAllocationList = GenericProvider.GetAllDetail<InvProductionJobOrderAndOrderAllocation>(sqlConnection8, "[dbo].[Sp_InvProductionJobOrderAndOrderAllocation_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Production.InvProductionJobOrder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadById_InvProductionJobOrderLabStandardDetail"));
					item.InvProductionJobOrderLabStandardDetailList = GenericProvider.GetAllDetail<InvProductionJobOrderLabStandardDetail>(sqlcon, "[dbo].[Sp_InvProductionJobOrder_GetAllMethod]", lstPrm);
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
