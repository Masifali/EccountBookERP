using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.pcc;
using Architecture.Model.pcc;

namespace Architecture.BLL.pcc;

public class ContractorWagesGroup
{
	public static int Save(Architecture.Model.pcc.ContractorWagesGroup obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.pcc.ContractorWagesGroup().SetData(obj, "[pcc].[USP_ContractorWagesGroup_Insert]");
			}
			return new Architecture.DAL.pcc.ContractorWagesGroup().SetData(obj, "[pcc].[USP_ContractorWagesGroup_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.pcc.ContractorWagesGroup ReadById(int Id)
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
			new List<Architecture.Model.pcc.ContractorWagesGroup>();
			return new Architecture.DAL.pcc.ContractorWagesGroup().GetData("[pcc].[USP_ContractorWagesGroup_GetAllMethod]", p)[0];
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.ContractorWagesGroup>("[pcc].[USP_ContractorWagesGroup_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int Id)
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
				Value = "DeleteById"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.pcc.ContractorWagesGroup>("[pcc].[USP_ContractorWagesGroup_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
