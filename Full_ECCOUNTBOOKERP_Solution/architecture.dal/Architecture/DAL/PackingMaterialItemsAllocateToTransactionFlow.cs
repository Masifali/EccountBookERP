using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class PackingMaterialItemsAllocateToTransactionFlow
{
	public int SetData(Architecture.Model.PackingMaterialItemsAllocateToTransactionFlow obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.PackingMaterialItemsAllocateToTransactionFlow item in obj.PackingMaterialItemsAllocateToTransactionFlowList)
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

	public List<Architecture.Model.PackingMaterialItemsAllocateToTransactionFlow> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.PackingMaterialItemsAllocateToTransactionFlow> lst = new List<Architecture.Model.PackingMaterialItemsAllocateToTransactionFlow>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.PackingMaterialItemsAllocateToTransactionFlow>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}
}
