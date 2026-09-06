using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Lab;
using Architecture.Model.Inventory;
using Architecture.Model.Lab;

namespace Architecture.BLL.Lab;

public class InvProcessAnalysisStepSchedule
{
	public static int Save(Architecture.Model.Lab.InvProcessAnalysisStepSchedule obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Lab.InvProcessAnalysisStepSchedule().SetDate(obj, "Sp_LabInProcessAnalysisStepAndParameterSchedule_Insert");
			}
			return new Architecture.DAL.Lab.InvProcessAnalysisStepSchedule().SetDate(obj, "Sp_LabInProcessAnalysisStepAndParameterSchedule_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Lab.InvProcessAnalysisStepSchedule obj)
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
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvProcessAnalysisStepSchedule>("Sp_LabInProcessAnalysisStepAndParameterSchedule_GetAllMethod", p).Rows[0]["SortNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllOrById(Architecture.Model.Lab.InvProcessAnalysisStepSchedule obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvProcessAnalysisStepSchedule>("Sp_LabInProcessAnalysisStepAndParameterSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable BindProcessStepName(Architecture.Model.Inventory.ProcessStep obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
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
				Value = "BindProcessStepName"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.ProcessStep>("Sp_LabInProcessAnalysisStepAndParameterSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
