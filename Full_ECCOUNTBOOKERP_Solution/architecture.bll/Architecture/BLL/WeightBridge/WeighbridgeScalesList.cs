using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.WeightBridge;
using Architecture.Model.WeightBridge;

namespace Architecture.BLL.WeightBridge;

public class WeighbridgeScalesList
{
	public static int Save(Architecture.Model.WeightBridge.WeighbridgeScalesList obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.WeightBridge.WeighbridgeScalesList().SetDate(obj, "Sp_WeighbridgeScalesList_Insert");
			}
			return new Architecture.DAL.WeightBridge.WeighbridgeScalesList().SetDate(obj, "Sp_WeighbridgeScalesList_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.WeightBridge.WeighbridgeScalesList GetByID(int Id)
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
				Value = "ReadById"
			});
			new List<Architecture.Model.WeightBridge.WeighbridgeScalesList>();
			return new Architecture.DAL.WeightBridge.WeighbridgeScalesList().GetAll("Sp_WeighbridgeScalesList_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.WeightBridge.WeighbridgeScalesList obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@MacAddress",
				Value = obj.MacAddress
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.WeightBridge.WeighbridgeScalesList>("Sp_WeighbridgeScalesList_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
