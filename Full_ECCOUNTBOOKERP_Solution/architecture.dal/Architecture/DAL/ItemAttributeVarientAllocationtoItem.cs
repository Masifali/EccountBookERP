using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class ItemAttributeVarientAllocationtoItem
{
	public int SetData(Architecture.Model.ItemAttributeVarientAllocationtoItem obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.ItemAttributeVarientAllocationtoItem item in obj.ItemAttributeVarientAllocationtoItemList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
			}
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
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

	public List<Architecture.Model.ItemAttributeVarientAllocationtoItem> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.ItemAttributeVarientAllocationtoItem> lst = new List<Architecture.Model.ItemAttributeVarientAllocationtoItem>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.ItemAttributeVarientAllocationtoItem>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
