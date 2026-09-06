using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Lab;
using Architecture.Model.Lab;

namespace Architecture.BLL.Lab;

public class InvLabAnalysisStandardDeductionPolicyHeader
{
	public static int Save(Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.EntryDate = DateTime.Now;
				return new Architecture.DAL.Lab.InvLabAnalysisStandardDeductionPolicyHeader().SetData(obj, "Sp_InvLabAnalysisStandardDeductionPolicyHeader_Insert");
			}
			obj.ModifyDate = DateTime.Now;
			return new Architecture.DAL.Lab.InvLabAnalysisStandardDeductionPolicyHeader().SetData(obj, "Sp_InvLabAnalysisStandardDeductionPolicyHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader>("Sp_InvLabAnalysisStandardDeductionPolicyHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader GetById(int Id)
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
			new List<Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader>();
			return new Architecture.DAL.Lab.InvLabAnalysisStandardDeductionPolicyHeader().GetData("Sp_InvLabAnalysisStandardDeductionPolicyHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByItemId(Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader obj)
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
				ParameterName = "@PolicyApplyOn",
				Value = obj.PolicyApplyOn
			});
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EffectiveFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.EffectiveFrom
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByItemId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader>("Sp_InvLabAnalysisStandardDeductionPolicyHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
