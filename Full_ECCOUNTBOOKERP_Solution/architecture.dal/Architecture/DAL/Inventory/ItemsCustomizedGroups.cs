using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class ItemsCustomizedGroups : DALBase
{
	public int SetData(Architecture.Model.Inventory.ItemsCustomizedGroups obj, string ProcName)
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
				obj.CustomGroupId = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.CustomGroupId;
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.Inventory.ItemsCustomizedGroups> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.ItemsCustomizedGroups> lst = new List<Architecture.Model.Inventory.ItemsCustomizedGroups>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Inventory.ItemsCustomizedGroups>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
