using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.FinanceManagment;
using Architecture.Model.FinanceManagment;

namespace Architecture.BLL.FinanceManagment;

public class FmFinanceType
{
	public static int Save(Architecture.Model.FinanceManagment.FmFinanceType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.FinanceManagment.FmFinanceType().SetData(obj, "Sp_FmFinanceType_Insert");
			}
			return new Architecture.DAL.FinanceManagment.FmFinanceType().SetData(obj, "Sp_FmFinanceType_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.FinanceManagment.FmFinanceType GetByID(int Id)
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
			new List<Architecture.Model.FinanceManagment.FmFinanceType>();
			return new Architecture.DAL.FinanceManagment.FmFinanceType().GetDate("Sp_FmFinanceType_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.FinanceManagment.FmFinanceType obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.FinanceManagment.FmFinanceType>("Sp_FmFinanceType_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByFundType(Architecture.Model.FinanceManagment.FmFinanceType obj)
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
				ParameterName = "@FundNonFund",
				Value = obj.FundNonFund
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByFundType"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FinanceManagment.FmFinanceType>("Sp_FmFinanceType_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
