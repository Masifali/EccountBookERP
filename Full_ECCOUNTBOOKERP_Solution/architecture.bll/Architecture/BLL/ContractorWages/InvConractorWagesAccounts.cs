using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.ContractorWages;
using Architecture.Model;
using Architecture.Model.ContractorWages;

namespace Architecture.BLL.ContractorWages;

public class InvConractorWagesAccounts
{
	public static int Save(Architecture.Model.ContractorWages.InvConractorWagesAccounts obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.ContractorWages.InvConractorWagesAccounts().SetDate(obj, "Sp_InvConractorWagesAccounts_Insert");
			}
			return new Architecture.DAL.ContractorWages.InvConractorWagesAccounts().SetDate(obj, "Sp_InvConractorWagesAccounts_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(ReportsParameters obj)
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvConractorWagesAccounts>("Sp_InvConractorWagesAccounts_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.ContractorWages.InvConractorWagesAccounts GetByID(int Id)
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
			new List<Architecture.Model.ContractorWages.InvConractorWagesAccounts>();
			return new Architecture.DAL.ContractorWages.InvConractorWagesAccounts().GetData("Sp_InvConractorWagesAccounts_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWagesItemsByWagesTypeIds(ReportsParameters obj)
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
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WagesLookupIds",
					Value = obj.Ids
				});
			}
			if (obj.WagesActivityId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WagesActivityId",
					Value = obj.WagesActivityId
				});
			}
			if (obj.ActionId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetWagesItemsByWagesTypeIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvConractorWagesAccounts>("Sp_InvConractorWagesAccounts_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
