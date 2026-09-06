using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class RevExpAcAllocationToProductAndDiscountType
{
	public int SetData(Architecture.Model.RevExpAcAllocationToProductAndDiscountType obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.RevExpAcAllocationToProductAndDiscountType item in obj.RevExpAcAllocationToProductAndDiscountTypeList)
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

	public List<Architecture.Model.RevExpAcAllocationToProductAndDiscountType> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.RevExpAcAllocationToProductAndDiscountType> lst = new List<Architecture.Model.RevExpAcAllocationToProductAndDiscountType>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.RevExpAcAllocationToProductAndDiscountType>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
