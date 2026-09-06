using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Production;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class PowerConsumptionRateSchedule
{
	public static int Save(Architecture.Model.Production.PowerConsumptionRateSchedule obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Production.PowerConsumptionRateSchedule().SetDate(obj, "Sp_PowerConsumptionRateSchedule_Insert");
			}
			return new Architecture.DAL.Production.PowerConsumptionRateSchedule().SetDate(obj, "Sp_PowerConsumptionRateSchedule_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Production.PowerConsumptionRateSchedule obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.PowerConsumptionRateSchedule>("Sp_PowerConsumptionRateSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Production.PowerConsumptionRateSchedule GetByID(int Id)
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
			new List<Architecture.Model.Production.PowerConsumptionRateSchedule>();
			return new Architecture.DAL.Production.PowerConsumptionRateSchedule().GetDate("Sp_PowerConsumptionRateSchedule_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetRateSchedule(Architecture.Model.Production.PowerConsumptionRateSchedule obj)
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
				ParameterName = "@DocDate",
				Value = obj.EffectiveDateFrom
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetRateSchedule"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.PowerConsumptionRateSchedule>("Sp_PowerConsumptionRateSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
