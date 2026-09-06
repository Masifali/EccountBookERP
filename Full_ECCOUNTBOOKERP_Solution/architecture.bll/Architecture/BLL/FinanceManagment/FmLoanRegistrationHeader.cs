using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.FinanceManagment;
using Architecture.Model.FinanceManagment;

namespace Architecture.BLL.FinanceManagment;

public class FmLoanRegistrationHeader
{
	public static int Save(Architecture.Model.FinanceManagment.FmLoanRegistrationHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.FinanceManagment.FmLoanRegistrationHeader().SetData(obj, "Sp_FmLoanRegistrationHeader_Insert");
			}
			return new Architecture.DAL.FinanceManagment.FmLoanRegistrationHeader().SetData(obj, "Sp_FmLoanRegistrationHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.FinanceManagment.FmLoanRegistrationHeader obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.FinanceManagment.FmLoanRegistrationHeader>("Sp_FmLoanRegistrationHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.FinanceManagment.FmLoanRegistrationHeader GetByID(int Id)
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
			new List<Architecture.Model.FinanceManagment.FmLoanRegistrationHeader>();
			return new Architecture.DAL.FinanceManagment.FmLoanRegistrationHeader().GetDate("Sp_FmLoanRegistrationHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
