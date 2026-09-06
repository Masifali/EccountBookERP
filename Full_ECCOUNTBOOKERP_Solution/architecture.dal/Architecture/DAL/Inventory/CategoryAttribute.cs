using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class CategoryAttribute
{
	public long SetData(Architecture.Model.Inventory.CategoryAttribute obj, string ProcName)
	{
		long returnID = 0L;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Inventory.CategoryAttribute item in obj.CategoryAttributeList)
			{
				returnID = Conversion.ToInt64(GenericProvider.SetProc(sqlTrn, item, ProcName));
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

	public List<Architecture.Model.Inventory.CategoryAttribute> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.CategoryAttribute> lst = new List<Architecture.Model.Inventory.CategoryAttribute>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Inventory.CategoryAttribute>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
