using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Lab;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Lab;

public class ProcessStep
{
	public static int Save(Architecture.Model.Inventory.ProcessStep obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Lab.ProcessStep().SetData(obj, "Sp_LabInProcessAnalysisStep_Insert");
			}
			return new Architecture.DAL.Lab.ProcessStep().SetData(obj, "Sp_LabInProcessAnalysisStep_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.ProcessStep GetByID(int Id)
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
			new List<Architecture.Model.Inventory.ProcessStep>();
			return new Architecture.DAL.Lab.ProcessStep().GetData("Sp_LabInProcessAnalysisStep_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Inventory.ProcessStep obj)
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
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ProcessStep>("Sp_LabInProcessAnalysisStep_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Inventory.ProcessStep obj)
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
				Value = "GenerateSortNo"
			});
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ProcessStep>("Sp_LabInProcessAnalysisStep_GetAllMethod", p).Rows[0]["SortNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
