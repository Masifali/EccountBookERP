using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class PdcBankName
{
	public static int Save(Architecture.Model.PdcBankName obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.PdcBankName().SetData(obj, "Sp_PdcBankName_Insert");
			}
			return new Architecture.DAL.PdcBankName().SetData(obj, "Sp_PdcBankName_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.PdcBankName GetByID(int Id)
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
			new List<Architecture.Model.PdcBankName>();
			return new Architecture.DAL.PdcBankName().GetData("Sp_PdcBankName_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.PdcBankName obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.PdcBankName>("Sp_PdcBankName_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
