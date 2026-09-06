using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.ChequePrinting;
using Architecture.DAL.Common;
using Architecture.Model.Accounts;
using Architecture.Model.ChequePrinting;

namespace Architecture.BLL.ChequePrinting;

public class Cheques
{
	public static int Save(Architecture.Model.ChequePrinting.Cheques obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.ChequePrinting.Cheques().SetData(obj, "[cqp].[Sp_Cheques_Insert]");
			}
			return new Architecture.DAL.ChequePrinting.Cheques().SetData(obj, "[cqp].[Sp_Cheques_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.ChequePrinting.Cheques ReadById(int Id)
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
			return new Architecture.DAL.ChequePrinting.Cheques().GetData("[cqp].[Sp_Cheques_ReadAll]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable History(int OrganizationId, int CompanyId)
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
				Value = "History"
			});
			return GenericProvider.GetDataTableProc<AcLookUps>("[cqp].[Sp_Cheques_ReadAll]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadParties(int OrganizationId, int CompanyId)
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
				Value = "ReadParties"
			});
			return GenericProvider.GetDataTableProc<AcLookUps>("[cqp].[Sp_Cheques_ReadAll]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
