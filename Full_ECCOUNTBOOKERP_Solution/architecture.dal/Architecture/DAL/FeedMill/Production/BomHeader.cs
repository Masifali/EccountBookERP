using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.FeedMill.Production;

namespace Architecture.DAL.FeedMill.Production;

public class BomHeader
{
	public int SetData(Architecture.Model.FeedMill.Production.BomHeader obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (BomInputDetail item4 in obj.BomInputDetailList)
			{
				item4.BomHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[fed].[USP_BomInputDetail_Insert]"));
			}
			foreach (BomFohDetail item3 in obj.BomFohDetailList)
			{
				item3.BomHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[fed].[USP_BomFohDetail_Insert]"));
			}
			foreach (BomConsumptionDetail item2 in obj.BomConsumptionDetailList)
			{
				item2.BomHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[fed].[USP_BomConsumptionDetail_Insert]"));
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = obj.EntryUserId;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = obj.EntryUserId;
					item.ModifyUser = obj.ModifyUserId;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
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

	public List<Architecture.Model.FeedMill.Production.BomHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.FeedMill.Production.BomHeader> lst = new List<Architecture.Model.FeedMill.Production.BomHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.FeedMill.Production.BomHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Production.BomHeader item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomInputDetail"));
						item3.BomInputDetailList = GenericProvider.GetAllDetail<BomInputDetail>(sqlConnection2, "[fed].[USP_BomHeader_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.FeedMill.Production.BomHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomByConsumptionDetail"));
						item2.BomConsumptionDetailList = GenericProvider.GetAllDetail<BomConsumptionDetail>(sqlConnection3, "[fed].[USP_BomHeader_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.FeedMill.Production.BomHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomFohDetail"));
					item.BomFohDetailList = GenericProvider.GetAllDetail<BomFohDetail>(sqlcon, "[fed].[USP_BomHeader_GetAllMethod]", lstPrm);
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
