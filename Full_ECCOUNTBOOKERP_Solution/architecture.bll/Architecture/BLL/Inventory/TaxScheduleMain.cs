using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class TaxScheduleMain
{
	public static int Save(Architecture.Model.Inventory.TaxScheduleMain obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.TaxScheduleMain().SetData(obj, "Sp_TaxScheduleMain_Insert");
			}
			return new Architecture.DAL.Inventory.TaxScheduleMain().SetData(obj, "Sp_TaxScheduleMain_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.TaxScheduleMain GetByID(int Id)
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
			new List<Architecture.Model.Inventory.TaxScheduleMain>();
			return new Architecture.DAL.Inventory.TaxScheduleMain().GetAll("Sp_TaxScheduleMain_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Inventory.TaxScheduleMain obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = clsGlobalVariables.UserAccount.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = clsGlobalVariables.UserAccount.CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.TaxScheduleMain>("Sp_TaxScheduleMain_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadTaxSchedule(Architecture.Model.Inventory.TaxScheduleMain obj)
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
				ParameterName = "@EffectedDate",
				Value = obj.EffectedDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@TaxNameId",
				Value = obj.TaxNameId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetTaxPercentInTaxSchedule"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.TaxScheduleMain>("Sp_TaxSchedule_GetAllMehtod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
