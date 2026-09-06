using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class ItemAllocateToProductType
{
	public int SetData(Architecture.Model.ItemAllocateToProductType obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.ItemAllocateToProductType item in obj.ItemAllocateToProductTypeList)
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

	public List<Architecture.Model.ItemAllocateToProductType> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ItemAllocateToProductType> lst = new List<Architecture.Model.ItemAllocateToProductType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ItemAllocateToProductType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
