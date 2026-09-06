using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class InvFoodProductionDetail
{
	public static DataTable GetByJobOrderID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByProductionJobOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProductionDetail>("Sp_InvFoodProductionDetail_GetALlMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
