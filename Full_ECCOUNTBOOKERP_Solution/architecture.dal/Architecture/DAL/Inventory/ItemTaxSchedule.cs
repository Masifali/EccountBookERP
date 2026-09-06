using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Inventory;

namespace Architecture.DAL.Inventory;

public class ItemTaxSchedule : DALBase
{
	public int SetData(Architecture.Model.Inventory.ItemTaxSchedule obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (obj.ItemTaxScheduleList == null || obj.ItemTaxScheduleList.Count == 0)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			}
			else
			{
				foreach (Architecture.Model.Inventory.ItemTaxSchedule item in obj.ItemTaxScheduleList)
				{
					returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_ItemTaxSchedule_Insert"));
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

	public List<Architecture.Model.Inventory.ItemTaxSchedule> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.ItemTaxSchedule> lst = new List<Architecture.Model.Inventory.ItemTaxSchedule>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Inventory.ItemTaxSchedule>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
