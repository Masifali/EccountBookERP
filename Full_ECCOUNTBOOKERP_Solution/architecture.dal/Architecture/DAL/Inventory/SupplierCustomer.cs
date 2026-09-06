using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class SupplierCustomer : DALBase
{
	public int SetData(Architecture.Model.Inventory.SupplierCustomer obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID == 0)
			{
				returnID = obj.Id;
			}
			using (SqlCommand sqlCmd = new SqlCommand())
			{
				sqlCmd.Connection = sqlcon;
				sqlCmd.Transaction = sqlTrn;
				sqlCmd.CommandType = CommandType.StoredProcedure;
				sqlCmd.CommandText = "[dbo].[USP_SupplierCustomerTaxSchedule_SyncFromMapping]";
				sqlCmd.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				sqlCmd.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				sqlCmd.ExecuteNonQuery();
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

	public List<Architecture.Model.Inventory.SupplierCustomer> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.SupplierCustomer> lst = new List<Architecture.Model.Inventory.SupplierCustomer>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetAllDetail<Architecture.Model.Inventory.SupplierCustomer>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
