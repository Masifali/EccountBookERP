using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Service;

namespace Architecture.DAL.Service;

public class ItemPricingSupplierCustomer : DALBase
{
	public int SetDate(Architecture.Model.Service.ItemPricingSupplierCustomer obj, string ProcName)
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
			foreach (ItemPricingSupplierCustomerDetail item in obj.ItemPricingDetail)
			{
				item.HeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_ItemPricingSupplierCustomerDetail_Insert"));
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

	public List<Architecture.Model.Service.ItemPricingSupplierCustomer> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Service.ItemPricingSupplierCustomer> lst = new List<Architecture.Model.Service.ItemPricingSupplierCustomer>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Service.ItemPricingSupplierCustomer>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Service.ItemPricingSupplierCustomer item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByHeaderId"));
					item.ItemPricingDetail = GenericProvider.Get<ItemPricingSupplierCustomerDetail>(sqlcon, "Sp_ItemPricingSupplierCustomer_GetAll", lstPrm);
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
