using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model.Production;

namespace Architecture.DAL.Production;

public class InvManufacturingBOMProcessFlow : DALBase
{
	public int SetData(Architecture.Model.Production.InvManufacturingBOMProcessFlow obj, string ProcName)
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

	public List<Architecture.Model.Production.InvManufacturingBOMProcessFlow> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.InvManufacturingBOMProcessFlow> lst = new List<Architecture.Model.Production.InvManufacturingBOMProcessFlow>();
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			return GenericProvider.GetProc<Architecture.Model.Production.InvManufacturingBOMProcessFlow>(sqlcon, ProcName, p);
		}
		catch
		{
			throw;
		}
	}

	public int DeleteByID(int Id)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_InvManufacturingBOMProcessFlow_DeleteById");
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.BigInt, Id);
			return Conversion.ToInt(sqlDataBase.ExecuteNonQuery(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
