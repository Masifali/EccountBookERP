using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class SupplierCustomerAllocateToDiscounType
{
	public int SetData(Architecture.Model.SupplierCustomerAllocateToDiscounType obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.SupplierCustomerAllocateToDiscounType item in obj.SupplierCustomerAllocateToDiscounTypeList)
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

	public List<Architecture.Model.SupplierCustomerAllocateToDiscounType> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.SupplierCustomerAllocateToDiscounType> lst = new List<Architecture.Model.SupplierCustomerAllocateToDiscounType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.SupplierCustomerAllocateToDiscounType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
