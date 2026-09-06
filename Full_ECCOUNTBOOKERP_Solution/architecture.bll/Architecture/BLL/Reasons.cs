using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class Reasons
{
	public static int Save(Architecture.Model.Reasons obj)
	{
		try
		{
			if (obj.ReasonId == 0)
			{
				return new Architecture.DAL.Reasons().SetData(obj, "USP_Reasons_Insert");
			}
			return new Architecture.DAL.Reasons().SetData(obj, "USP_Reasons_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Reasons>("[dbo].[USP_Reasons_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Reasons GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ReasonId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			List<Architecture.Model.Reasons> lst = new List<Architecture.Model.Reasons>();
			lst = new Architecture.DAL.Reasons().GetAll("[dbo].[USP_Reasons_GetAllMethod]", p);
			return (lst != null && lst.Count > 0) ? lst[0] : null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByRefDocumentType(int RefDocumentTypeId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeId",
				Value = RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetByRefDocumentType"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Reasons>("[dbo].[USP_Reasons_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
