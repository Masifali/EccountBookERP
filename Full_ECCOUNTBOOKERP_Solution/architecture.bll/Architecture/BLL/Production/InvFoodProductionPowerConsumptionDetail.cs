using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Production;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class InvFoodProductionPowerConsumptionDetail
{
	public static int Save(Architecture.Model.Production.InvFoodProductionPowerConsumptionDetail obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Production.InvFoodProductionPowerConsumptionDetail().SetDate(obj, "Sp_InvFoodProductionPowerConsumptionDetail_Insert");
			}
			return new Architecture.DAL.Production.InvFoodProductionPowerConsumptionDetail().SetDate(obj, "Sp_InvFoodProductionPowerConsumptionDetail_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Production.InvFoodProductionPowerConsumptionDetail obj)
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
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.InvFoodProductionPowerConsumptionDetail>("Sp_InvFoodProductionPowerConsumptionDetail_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Production.InvFoodProductionPowerConsumptionDetail GetByID(int Id)
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
			new List<Architecture.Model.Production.InvFoodProductionPowerConsumptionDetail>();
			return new Architecture.DAL.Production.InvFoodProductionPowerConsumptionDetail().GetDate("Sp_InvFoodProductionPowerConsumptionDetail_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
