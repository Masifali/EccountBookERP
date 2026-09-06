using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.ContractorWages;
using Architecture.Model.ContractorWages;

namespace Architecture.BLL.ContractorWages;

public class InvWagesSceduleForAtuoCalculationHeader
{
	public static int Save(Architecture.Model.ContractorWages.InvWagesSceduleForAtuoCalculationHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.ContractorWages.InvWagesSceduleForAtuoCalculationHeader().SetData(obj, "Sp_InvWagesSceduleForAtuoCalculationHeader_Insert");
			}
			return new Architecture.DAL.ContractorWages.InvWagesSceduleForAtuoCalculationHeader().SetData(obj, "Sp_InvWagesSceduleForAtuoCalculationHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.ContractorWages.InvWagesSceduleForAtuoCalculationHeader GetByID(int Id)
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
			new List<Architecture.Model.ContractorWages.InvWagesSceduleForAtuoCalculationHeader>();
			return new Architecture.DAL.ContractorWages.InvWagesSceduleForAtuoCalculationHeader().GetData("Sp_InvWagesSceduleForAtuoCalculation_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.ContractorWages.InvWagesSceduleForAtuoCalculationHeader obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvWagesSceduleForAtuoCalculationHeader>("Sp_InvWagesSceduleForAtuoCalculation_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
