using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class WareHouseAllocationToPlants
{
	public int SetData(Architecture.Model.WareHouseAllocationToPlants obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.WareHouseAllocationToPlants item in obj.WareHouseAllocationToPlantsList)
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

	public List<Architecture.Model.WareHouseAllocationToPlants> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.WareHouseAllocationToPlants> lst = new List<Architecture.Model.WareHouseAllocationToPlants>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.WareHouseAllocationToPlants>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
