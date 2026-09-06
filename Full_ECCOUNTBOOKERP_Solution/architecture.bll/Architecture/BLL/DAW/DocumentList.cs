using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.DAW;
using Architecture.Model.DAW;

namespace Architecture.BLL.DAW;

public class DocumentList
{
	public static int Save(Architecture.Model.DAW.DocumentList obj)
	{
		try
		{
			return new Architecture.DAL.DAW.DocumentList().SetData(obj, "DAW.USP_DocumentList_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.DAW.DocumentList>("DAW.USP_DocumentList_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.DAW.DocumentList GetByID(int Id)
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
			new List<Architecture.Model.DAW.DocumentList>();
			return new Architecture.DAL.DAW.DocumentList().GetAll("DAW.USP_DocumentList_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
