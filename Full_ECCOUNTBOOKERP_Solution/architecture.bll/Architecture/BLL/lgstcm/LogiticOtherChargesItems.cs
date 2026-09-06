using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.lgstcm;
using Architecture.Model.lgstcm;

namespace Architecture.BLL.lgstcm;

public class LogiticOtherChargesItems
{
	public static int Save(Architecture.Model.lgstcm.LogiticOtherChargesItems obj)
	{
		try
		{
			return new Architecture.DAL.lgstcm.LogiticOtherChargesItems().SetData(obj, "[lgstcm].[USP_LogiticOtherChargesItems_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.lgstcm.LogiticOtherChargesItems ReadById(int Id)
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
			new List<Architecture.Model.lgstcm.LogiticOtherChargesItems>();
			return new Architecture.DAL.lgstcm.LogiticOtherChargesItems().GetAll("[lgstcm].[USP_LogiticOtherChargesItems_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.lgstcm.LogiticOtherChargesItems>("[lgstcm].[USP_LogiticOtherChargesItems_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
