using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Pcc;

namespace Architecture.DAL.pcc.BomHeader;

public class BomHeader
{
	public int SetData(Architecture.Model.Pcc.BomHeader obj, string ProcName)
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
			foreach (BomInputDetail item6 in obj.BomInputDetailList)
			{
				item6.BomHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "[pcc].[USP_BomInputDetail_Insert]"));
			}
			foreach (BomFohDetail item5 in obj.BomFohDetailList)
			{
				item5.BomHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "[pcc].[USP_BomFohDetail_Insert]"));
			}
			foreach (BomByProductDetail item4 in obj.BomByProductDetailList)
			{
				item4.BomHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "[pcc].[USP_BomByProductDetail_Insert]"));
			}
			foreach (BomOperationDetail item3 in obj.BomOperationDetailList)
			{
				item3.BomHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "[pcc].[USP_BomOperationDetail_Insert]"));
			}
			foreach (BomWagesDetail item2 in obj.BomWagesDetailList)
			{
				item2.BomHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "[pcc].[USP_BomWagesDetail_Insert]"));
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

	public List<Architecture.Model.Pcc.BomHeader> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Pcc.BomHeader> lst = new List<Architecture.Model.Pcc.BomHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Pcc.BomHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Pcc.BomHeader item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@Id", item5.Id));
						lstPrm5.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomInputDetail"));
						item5.BomInputDetailList = GenericProvider.GetAllDetail<BomInputDetail>(sqlConnection2, "[pcc].[USP_BomHeader_GetAllMethod]", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Pcc.BomHeader item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomByProductDetail"));
						item4.BomByProductDetailList = GenericProvider.GetAllDetail<BomByProductDetail>(sqlConnection3, "[pcc].[USP_BomHeader_GetAllMethod]", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Pcc.BomHeader item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomFohDetail"));
						item3.BomFohDetailList = GenericProvider.GetAllDetail<BomFohDetail>(sqlConnection4, "[pcc].[USP_BomHeader_GetAllMethod]", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Pcc.BomHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomOperationDetail"));
						item2.BomOperationDetailList = GenericProvider.GetAllDetail<BomOperationDetail>(sqlConnection5, "[pcc].[USP_BomHeader_GetAllMethod]", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Pcc.BomHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId_BomWagesDetail"));
					item.BomWagesDetailList = GenericProvider.GetAllDetail<BomWagesDetail>(sqlcon, "[pcc].[USP_BomHeader_GetAllMethod]", lstPrm);
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
