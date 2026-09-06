using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Import;

namespace Architecture.DAL.Import;

public class ImLcOrderScheduleHeader : DALBase
{
	public int SetData(Architecture.Model.Import.ImLcOrderScheduleHeader obj)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.ImLcOrderScheduleHeaderList != null)
			{
				foreach (Architecture.Model.Import.ImLcOrderScheduleHeader item2 in obj.ImLcOrderScheduleHeaderList)
				{
					returnID = ((item2.Id <= 0) ? Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[dbo].[USP_ImLcOrderScheduleHeader_Insert]")) : Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[dbo].[USP_ImLcOrderScheduleHeader_Update]")));
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
						Value = obj.ImLcOrderScheduleHeaderList[0].ImLcOrderId
					});
					p.Add(new SqlParameter
					{
						ParameterName = "@Activity",
						Value = "DeleteById"
					});
					new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
					foreach (DMSAttachments item in obj.AttachmentsList.ToList())
					{
						item.RefAccountId = obj.ImLcOrderScheduleHeaderList[0].ImLcOrderId;
						item.ScreenName = obj.AttachmentsList[0].ScreenName;
						item.RefDocumentNo = obj.ImLcOrderScheduleHeaderList[0].ImLcOrderId;
						item.EntryDate = DateTime.Now;
						item.ModifyDate = DateTime.Now;
						item.EntryUser = clsGlobalVariables.UserAccount.ID;
						item.ModifyUser = clsGlobalVariables.UserAccount.ID;
						item.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
						item.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
					}
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

	public List<Architecture.Model.Import.ImLcOrderScheduleHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Import.ImLcOrderScheduleHeader> lst = new List<Architecture.Model.Import.ImLcOrderScheduleHeader>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.Import.ImLcOrderScheduleHeader>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public int SetDataForPackingDetail(ImLcOrderSchedulePackingDetail obj, string Proc)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (ImLcOrderSchedulePackingDetail item in obj.ImLcOrderSchedulePackingDetailList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, Proc));
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

	public List<ImLcOrderSchedulePackingDetail> GetDataForPackingDetail(string ProcName, List<SqlParameter> p = null)
	{
		List<ImLcOrderSchedulePackingDetail> lst = new List<ImLcOrderSchedulePackingDetail>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<ImLcOrderSchedulePackingDetail>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
