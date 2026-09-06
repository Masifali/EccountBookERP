using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class MultiCurrency
{
	public static int save(Architecture.Model.MultiCurrency cty)
	{
		try
		{
			if (cty.Id == 0)
			{
				return new Architecture.DAL.MultiCurrency().SetData(cty, "Sp_MultiCurrency_Insert");
			}
			return new Architecture.DAL.MultiCurrency().SetData(cty, "Sp_MultiCurrency_Update");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetAll(Architecture.Model.MultiCurrency obj)
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
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.MultiCurrency>("Sp_MultiCurrency_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetMultiCurrencywithExchangeRate(int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.MultiCurrency>("[dbo].[usp_getMultiCurrencywithExchangeRate]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.MultiCurrency GetById(int id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = id
			});
			new List<Architecture.Model.MultiCurrency>();
			return new Architecture.DAL.MultiCurrency().GetDate("Sp_MultiCurrency_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable USP_GetBaseCurrencyInfo(Architecture.Model.MultiCurrency obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.MultiCurrency>("[dbo].[USP_GetBaseCurrencyInfo]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
