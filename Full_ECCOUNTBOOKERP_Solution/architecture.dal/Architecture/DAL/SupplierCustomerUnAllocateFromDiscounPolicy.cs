using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class SupplierCustomerUnAllocateFromDiscounPolicy
{
	public int SetData(Architecture.Model.SupplierCustomerUnAllocateFromDiscounPolicy obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.SupplierCustomerUnAllocateFromDiscounPolicy item in obj.SupplierCustomerUnAllocateFromDiscounPolicyList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.SupplierCustomerUnAllocateFromDiscounPolicy> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SupplierCustomerUnAllocateFromDiscounPolicy> lst = new List<Architecture.Model.SupplierCustomerUnAllocateFromDiscounPolicy>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.SupplierCustomerUnAllocateFromDiscounPolicy>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
