using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class FinancialYear
{
	public static int Save(Architecture.Model.FinancialYear obj)
	{
		try
		{
			return new Architecture.DAL.FinancialYear().SetData(obj, "Proc_FinancialYear_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.FinancialYear ReadActiveByOrganizationIdNCompanyId(Architecture.Model.FinancialYear obj)
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
			List<Architecture.Model.FinancialYear> lst = new Architecture.DAL.FinancialYear().GetData("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", p);
			if (lst.Count == 0)
			{
				throw new Exception("Financial Year Not Found");
			}
			return lst[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.FinancialYear ReadById(int Id)
	{
		List<SqlParameter> p = new List<SqlParameter>();
		p.Add(new SqlParameter
		{
			ParameterName = "@Id",
			Value = Id
		});
		return new Architecture.DAL.FinancialYear().GetData("USP_FinancialYear_ReadById", p)[0];
	}

	public static List<Architecture.Model.FinancialYear> GetFinancialYearlist(Architecture.Model.FinancialYear obj)
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
		return new Architecture.DAL.FinancialYear().GetData("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", p);
	}

	public static DataTable FormHistory(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Branches>("USP_FinancialYear_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getFinancialStartAndEndDate(int CompanyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Branches>("usp_getFinancialStartAndEndDate", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
