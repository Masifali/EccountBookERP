using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Import;

namespace Architecture.DAL.Import;

public class ImLcOrder : DALBase
{
	public int SetDate(Architecture.Model.Import.ImLcOrder obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			if (obj.ImLcOrderPackingDetailList != null)
			{
				foreach (ImLcOrderPackingDetail item3 in obj.ImLcOrderPackingDetailList)
				{
					item3.ImLcOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_ImLcOrderPackingDetail_Insert"));
				}
			}
			if (obj.ImLcOrderPaymnetTermList != null)
			{
				foreach (ImLcOrderPaymnetTerm item2 in obj.ImLcOrderPaymnetTermList)
				{
					item2.ImLcOrderId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "USP_ImLcOrderPaymnetTerm_Insert"));
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

	public List<Architecture.Model.Import.ImLcOrder> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Import.ImLcOrder> lst = new List<Architecture.Model.Import.ImLcOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Import.ImLcOrder>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					if (lst[0].DocumentTypeId == 231)
					{
						foreach (Architecture.Model.Import.ImLcOrder item2 in lst)
						{
							List<SqlParameter> lstPrm2 = new List<SqlParameter>();
							lstPrm2.Add(new SqlParameter("@Id", item2.Id));
							lstPrm2.Add(new SqlParameter("@Activity", "ReadByLcOrderHeaderId"));
							item2.ImLcOrderPackingDetailList = GenericProvider.Get<ImLcOrderPackingDetail>(sqlConnection2, "USP_ImLcOrder_GetAllMethod", lstPrm2);
						}
					}
					else
					{
						foreach (Architecture.Model.Import.ImLcOrder item3 in lst)
						{
							List<SqlParameter> lstPrm3 = new List<SqlParameter>();
							lstPrm3.Add(new SqlParameter("@Id", item3.Id));
							lstPrm3.Add(new SqlParameter("@Activity", "ReadByLcOrderHeaderIdForPurchaseContractExport"));
							item3.ImLcOrderPackingDetailList = GenericProvider.Get<ImLcOrderPackingDetail>(sqlConnection2, "USP_ImLcOrder_GetAllMethod", lstPrm3);
						}
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Import.ImLcOrder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadImLcOrderPaymnetTerm_ByLcOrderHeaderId"));
					item.ImLcOrderPaymnetTermList = GenericProvider.Get<ImLcOrderPaymnetTerm>(sqlcon, "USP_ImLcOrder_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public List<Architecture.Model.Import.ImLcOrder> GetDetailImLcOrderByImLcOrderId(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Import.ImLcOrder> lst = new List<Architecture.Model.Import.ImLcOrder>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Import.ImLcOrder>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Import.ImLcOrder item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "GetDetailImLcOrderByImLcOrderId"));
					item.ImLcOrderPackingDetailList = GenericProvider.Get<ImLcOrderPackingDetail>(sqlcon, "Sp_ImLcOrder_GetAllMethod", lstPrm);
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
