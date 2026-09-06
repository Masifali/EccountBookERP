using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.FinanceManagment;
using Architecture.Model;
using Architecture.Model.FinanceManagment;

namespace Architecture.BLL.FinanceManagment;

public class FmLoanAgreementHeader
{
	public static int Save(Architecture.Model.FinanceManagment.FmLoanAgreementHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.FinanceManagment.FmLoanAgreementHeader().SetData(obj, "Sp_FmLoanAgreementHeader_Insert");
			}
			return new Architecture.DAL.FinanceManagment.FmLoanAgreementHeader().SetData(obj, "Sp_FmLoanAgreementHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.FinanceManagment.FmLoanAgreementHeader obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.FinanceManagment.FmLoanAgreementHeader>("Sp_FmLoanAgreementHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllBankFromLoanAgreement(Architecture.Model.FinanceManagment.FmLoanAgreementHeader obj)
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
				Value = "GetAllBankFromLoanAgreement"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FinanceManagment.FmLoanAgreementHeader>("Sp_FmLoanAgreementHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.FinanceManagment.FmLoanAgreementHeader GetByID(int Id)
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
			new List<Architecture.Model.FinanceManagment.FmLoanAgreementHeader>();
			return new Architecture.DAL.FinanceManagment.FmLoanAgreementHeader().GetDate("Sp_FmLoanAgreementHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByAgreementId(Architecture.Model.FinanceManagment.FmLoanAgreementHeader obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAgreementDetailByHeaderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.FinanceManagment.FmLoanAgreementHeader>("Sp_FmLoanAgreementHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLoanAgreementBalanceAmountByCollectrelType(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
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
				ParameterName = "@LoanColletralType",
				Value = obj.ReqType
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetLoanAgreementBalanceAmount"
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_FmLoanAgreementHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
