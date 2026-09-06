using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.DAW;
using Architecture.Model.DAW;

namespace Architecture.BLL.DAW;

public class DocumentApprovalLevel
{
	public static int Save(Architecture.Model.DAW.DocumentApprovalLevel obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.DAW.DocumentApprovalLevel().SetData(obj, "DAW.USp_ApprovalLevel_Insert");
			}
			return new Architecture.DAL.DAW.DocumentApprovalLevel().SetData(obj, "DAW.USp_ApprovalLevel_Update");
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
			return GenericProvider.GetDataTableProc<Architecture.Model.DAW.DocumentApprovalLevel>("DAW.USp_ApprovalLevel_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.DAW.DocumentApprovalLevel GetByID(int Id)
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
			new List<Architecture.Model.DAW.DocumentApprovalLevel>();
			return new Architecture.DAL.DAW.DocumentApprovalLevel().GetAll("DAW.USp_ApprovalLevel_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
